using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���ſ� �Ǹ� ����� �����.
public class Shop : MonoBehaviour
{
    public enum ShopType
    {
        NONE,
        BUY,
        SELL
    }

    public static Shop shop;
    public Item selected;

    private ShopType type;

    public int[] sellList;  // �������� �Ǹ��� �������� �ε��� ���.
    public Slot_Shop[] slots;
    public RectTransform scroll;

    // ������ ������ ǥ���� �г�.
    public GameObject infoPanel;
    public GameObject infoEquiped;
    public Image infoIcon;
    public Text infoName;
    public Text infoType;
    public Text infoLevel;
    public Text infoExplain;

    public Button interact;
    public Slot_Shop firstList; // ���� ��� ù��° ������.
    public Text goldText;

    public GameObject title_Buy;
    public GameObject title_Sell;

    private Inventory mine;

    private void Awake()
    {
        shop = this;
    }

    private void OnEnable()
    {
        // ���� �г��� �� ������ �����.
        goldText.text = JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold.ToString();
        shop ??= this;               
    }

    private void OnDisable()
    {
        shop = null;
    }

    public void SetType(int _num)
    {
        // ����/�Ǹ� �������� �� ������ ��ũ�� �� ����.
        scroll.anchoredPosition = Vector2.zero;

        type = (ShopType)_num;

        infoPanel.SetActive(false);
        title_Buy.SetActive(false);
        title_Sell.SetActive(false);
        // �켱 ������ ��� ��Ȱ��ȭ.
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false);
        }

        switch (type)
        {
            case ShopType.BUY:
                {
                    title_Buy.SetActive(true);

                    for (int i = 0; i < slots.Length; i++)
                    {
                        if (i == sellList.Length)
                            break;
                        SetSlotToBuy(slots[i], sellList[i]);
                        slots[i].gameObject.SetActive(true);
                    }

                    interact.onClick.RemoveAllListeners();
                    interact.onClick.AddListener(() => BuyItem());
                    Text tmp = interact.GetComponentInChildren<Text>();
                    tmp.text = "����";

                    // ���� ���� �г��� ���� �� ù��° �������� ���õ� ���·� �����ϰԲ� ��.
                    firstList.OnClick();
                    break;
                }
            case ShopType.SELL:
                {
                    title_Sell.SetActive(true);

                    mine ??= JY_CharacterListManager.s_instance.invenList[0];
                    for (int i = 0; i < mine.items.Count; i++)
                    {                        
                        SetSlotToSell(slots[i], i);
                        slots[i].gameObject.SetActive(true);
                    }
                    interact.onClick.RemoveAllListeners();
                    interact.onClick.AddListener(() => SellItem());
                    Text tmp = interact.GetComponentInChildren<Text>();
                    tmp.text = "�Ǹ�";
                    break;
                }
            default:
                {
                    Debug.Log("���� ������ ���� ����");
                    return;
                }
        }

        
    }

    // ������ �ε���(DB �� ������ ��ȣ)�� ���� ������ ������ �ε���.
    void SetSlotToBuy(Slot_Shop _slot, int _index)
    {
        _slot.item = ItemDatabase.s_instance.itemDB[_index].Copy();

        _slot.icon.sprite = _slot.item.image;
        _slot.nameText.text = _slot.item.name;
        _slot.priceText.text = _slot.item.price.ToString();
    }

    // �κ��丮 ���� �ѹ��� ���� �Ǹ��� ������ �ε���.
    void SetSlotToSell(Slot_Shop _slot, int _num)
    {
        _slot.item = mine.items[_num];

        _slot.icon.sprite = _slot.item.image;
        _slot.nameText.text = _slot.item.name;
        _slot.priceText.text = ((int)(_slot.item.price * 0.8)).ToString();
    }

    public void UpdatePanel()
    {
        infoPanel.SetActive(true);
        infoEquiped.SetActive(false);
        infoIcon.sprite = selected.image;
        infoName.text = selected.name;
        interact.interactable = true;

        switch (selected.type)
        {
            case ItemType.EQUIPMENT:
                {
                    infoType.text = "���";
                    if (selected.equipedState.Equals(EquipState.EQUIPED))
                    {
                        infoEquiped.SetActive(true);
                        interact.interactable = false;
                    }                        
                    break;
                }
            case ItemType.CONSUMABLE:
                {
                    infoType.text = "�Һ�";
                    break;
                }
            case ItemType.INGREDIENTS:
                {
                    infoType.text = "���";
                    break;
                }
        }
        infoLevel.text = "Lv. " + selected.level.ToString();
        infoExplain.text = selected.explanation;
    }

    // ���� ��ư�� �Ҵ�� �޼ҵ�.
    public void BuyItem()
    {
        // �÷��̾��� �������� ������ ���ݺ��� ���� �� üũ. �������� ���ٸ� ����.
        if (JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold < selected.price)
        {
            Debug.Log("���� ����");
            return;
        }
            

        // ������ �߰��� ���������� (�÷��̾��� �κ��丮�� ���� ���� �ʾҴٸ�)
        if (JY_CharacterListManager.s_instance.invenList[0].AddItem(selected, false))
        {
            Debug.Log("���� ����");
            JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold -= selected.price;
            goldText.text = JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold.ToString();
            return;
        }
        Debug.Log("���� ����");
    }

    // �Ǹ� ��ư�� �Ҵ�� �޼ҵ�.
    public void SellItem()
    {
        // ������ ���� �г��� ����.
        infoPanel.SetActive(false);

        // �������� �κ��丮���� ����.
        mine.RemoveItem(selected);
        // ������ ���ݸ�ŭ ���� �÷���. �ǸŰ��� �� �� ������ 80%
        JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold 
            += (int)(selected.price * 0.8);
        goldText.text = JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold.ToString();

        // ����Ʈ�� �ٽ� ����.
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < mine.items.Count; i++)
        {
            SetSlotToSell(slots[i], i);
            slots[i].gameObject.SetActive(true);
        }        
    }
}
