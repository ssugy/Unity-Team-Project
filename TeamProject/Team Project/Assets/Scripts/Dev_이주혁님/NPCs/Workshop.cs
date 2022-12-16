using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// �������� �������� �����ϴ� ����ü.
[System.Serializable]
public struct ItemMethod
{
    // ������� �ϴ� �������� �ε���.
    public int objective;

    // ��� �������� �ε����� ������ �迭�� ������. ũ�Ⱑ ���ƾ� ��.
    public int[] index;
    public int[] ammount;

    // ���ۿ� �ʿ��� ���.
    public int gold;
}

// ������ ���� �� ��ȭ ����� ����� ��ũ��Ʈ.
public class Workshop : MonoBehaviour
{
    public enum WorkshopType
    {
        NONE,
        PRODUCE,
        ENHANCE
    }

    public static Workshop workshop;
    public ItemMethod selectedMethod;         // ���ۿ� ����.
    public Item selectedItem;                   // ��ȭ�� ����.

    private WorkshopType type;

    // ���� ������ ���.
    public List<ItemMethod> methods;

    public Slot_Workshop[] slots;
    public RectTransform scroll;

    // ������ ������ ǥ���� �г�.
    public GameObject infoPanel;    
    public Image infoIcon;
    public Text infoName;
    public Text infoType;
    public Text infoLevel;
    public Text infoGold;

    // ����.
    public Image[] icons;
    public Text[] texts;

    public Button interact;
    public Slot_Workshop firstList; // ���� ��� ù��° ������.
    public Text goldText;

    public GameObject title_Produce;
    public GameObject title_Enhance;

    private Inventory mine;
    private void Awake()
    {
        workshop = this;
    }

    private void OnEnable()
    {
        goldText.text = JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold.ToString();
        workshop ??= this;
    }

    private void OnDisable()
    {
        workshop = null;
    }

    public void SetType(int _num)
    {
        scroll.anchoredPosition = Vector2.zero;

        type = (WorkshopType)_num;

        infoPanel.SetActive(false);
        title_Produce.SetActive(false);
        title_Enhance.SetActive(false);
        // �켱 ������ ��� ��Ȱ��ȭ.
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false);
        }

        switch (type)
        {
            case WorkshopType.PRODUCE:
                {
                    title_Produce.SetActive(true);

                    //mine ??= JY_CharacterListManager.s_instance.invenList[0];
                    for (int i = 0; i < slots.Length; i++)
                    {
                        if (i == methods.Count)
                            break;
                        SetSlotToProduce(slots[i], methods[i]);
                        slots[i].gameObject.SetActive(true);
                    }

                    interact.onClick.RemoveAllListeners();
                    interact.onClick.AddListener(() => ProduceItem());
                    Text tmp = interact.GetComponentInChildren<Text>();
                    tmp.text = "����";

                    // ������ ���� �г��� ���� �� ù��° �������� ���õ� ���·� �����ϰԲ� ��.
                    firstList.OnClick();
                    break;
                }
            case WorkshopType.ENHANCE:
                {
                    title_Enhance.SetActive(true);

                    mine ??= JY_CharacterListManager.s_instance.invenList[0];

                    int i = 0;
                    mine.items.ForEach(e =>
                    {
                        // ���� ������ ������, ��ȭ �ܰ谡 9 ������ ��� ��ȭ ����.
                        if (e.type.Equals(ItemType.EQUIPMENT)
                        && e.equipedState.Equals(EquipState.UNEQUIPED)
                        && e.enhanced < 10) 
                        {
                            SetSlotToEnhance(slots[i], e);
                            slots[i++].gameObject.SetActive(true);
                        }
                    });

                    interact.onClick.RemoveAllListeners();
                    interact.onClick.AddListener(() => EnhanceItem());
                    Text tmp = interact.GetComponentInChildren<Text>();
                    tmp.text = "��ȭ";
                    break;
                }
            default:
                {
                    Debug.Log("��ũ�� ������ ���� ����");
                    return;
                }
        }
    }
    public void UpdatePanel()
    {
        infoPanel.SetActive(true);        

        Array.ForEach(icons, e => e.gameObject.SetActive(false));
        Array.ForEach(texts, e => e.gameObject.SetActive(false));

        switch (type)
        {
            case WorkshopType.PRODUCE:
                {
                    infoIcon.sprite = ItemDatabase.s_instance.itemDB[selectedMethod.objective].image;                    
                    infoName.text = ItemDatabase.s_instance.itemDB[selectedMethod.objective].name;
                    switch (ItemDatabase.s_instance.itemDB[selectedMethod.objective].type)
                    {
                        case ItemType.EQUIPMENT:
                            {
                                infoType.text = "���";
                                break;
                            }
                        case ItemType.CONSUMABLE:
                            {
                                infoType.text = "�Һ�";
                                break;
                            }
                        case ItemType.INGREDIENTS:
                            {
                                infoType.text = "����";
                                break;
                            }
                        default:
                            {
                                Debug.Log("���� �г� ���� ����");
                                return;
                            }
                    }
                    infoLevel.text = "Lv. "
                        + ItemDatabase.s_instance.itemDB[selectedMethod.objective].level.ToString();
                    infoGold.text = selectedMethod.gold.ToString();

                    for (int i = 0; i < selectedMethod.index.Length; i++)
                    {
                        icons[i].sprite = ItemDatabase.s_instance.itemDB[selectedMethod.index[i]].image;
                        texts[i].text
                            = FindIngredientNum(ItemDatabase.s_instance.itemDB[selectedMethod.index[i]].name).ToString()
                            + " / " + selectedMethod.ammount[i];
                        icons[i].gameObject.SetActive(true);
                        texts[i].gameObject.SetActive(true);
                    }
                    break;
                }
            case WorkshopType.ENHANCE:
                {
                    infoIcon.sprite = selectedItem.image;
                    infoName.text = $"+{selectedItem.enhanced} {selectedItem.name}";                    
                    infoType.text = "���";
                    infoLevel.text = "Lv. " + selectedItem.level.ToString();
                    infoGold.text = ((selectedItem.enhanced + 2) * 200).ToString();

                    // ��� ��ȭ �ܰ迡 ���� �ٸ� ��ȭ ��Ḧ ���.
                    icons[0].sprite = selectedItem.enhanced < 5 ?
                        ItemDatabase.s_instance.itemDB[37].image
                        : ItemDatabase.s_instance.itemDB[38].image;
                    texts[0].text = selectedItem.enhanced < 5 ?
                            FindIngredientNum(ItemDatabase.s_instance.itemDB[37].name).ToString()
                            : FindIngredientNum(ItemDatabase.s_instance.itemDB[38].name).ToString();
                    texts[0].text += " / " + ((selectedItem.enhanced + 1) * 2).ToString();

                    icons[0].gameObject.SetActive(true);
                    texts[0].gameObject.SetActive(true);
                    break;
                }
            default:
                {
                    Debug.Log("���� �г� ���� ����");
                    return;
                }
        }
    }

    // ������ �ε���(DB �� ������ ��ȣ)�� ���� ���� ������ �������� �ε���.
    void SetSlotToProduce(Slot_Workshop _slot, ItemMethod _method)
    {
        _slot.method.objective = _method.objective;
        _slot.method.index = (int[])_method.index.Clone();
        _slot.method.ammount = (int[])_method.ammount.Clone();
        _slot.method.gold = _method.gold;

        _slot.icon.sprite = ItemDatabase.s_instance.itemDB[_method.objective].image;
        _slot.nameText.text = ItemDatabase.s_instance.itemDB[_method.objective].name;
    }

    // �κ��丮 ���� �ѹ��� ���� ��ȭ�� ������ �ε���.
    void SetSlotToEnhance(Slot_Workshop _slot, Item _item)
    {
        _slot.item = _item;

        _slot.icon.sprite = _slot.item.image;
        _slot.nameText.text = _slot.item.name;
    }
    void ProduceItem()
    {
        // ��Ḧ �� ���� �ִ��� üũ.
        for (int i = 0; i < selectedMethod.index.Length; i++)
        {
            string name = ItemDatabase.s_instance.itemDB[selectedMethod.index[i]].name;
            if (FindIngredientNum(name) < selectedMethod.ammount[i])
            {
                Debug.Log("��� �������� ���� ����");
                return;
            }               
        }

        // ���� ��尡 �������� ������ üũ.
        if (JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold < selectedMethod.gold) 
        {
            Debug.Log("������ �������� ���� ����");
            return;
        }

        Item item = ItemDatabase.s_instance.itemDB[selectedMethod.objective].Copy();
        // ������ �߰��� ���������� (�÷��̾��� �κ��丮�� ���� ���� �ʾҴٸ�)
        // ������ �������� ��� ���� �߰��ɼ��� ���� ���·� ȹ���.
        if (mine.AddItem(item, true)) 
        {
            Debug.Log("���� ����");
            JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold -= selectedMethod.gold;
            goldText.text = JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold.ToString();

            for (int i = 0; i < selectedMethod.index.Length; i++)
            {
                string name = ItemDatabase.s_instance.itemDB[selectedMethod.index[i]].name;
                Item tmp = FindIngredientItem(name);
                mine.RemoveItem(tmp, selectedMethod.ammount[i]);
            }
            UpdatePanel();
            return;
        }
        else
            Debug.Log("�κ��丮�� ���� �� ���� ����");
    }
    void EnhanceItem()
    {

    }

    // ã������ ��� �������� �˻��Ͽ� ������ ��ȯ��. �̸����� �˻�.
    int FindIngredientNum(string _name)
    {
        mine ??= JY_CharacterListManager.s_instance.invenList[0];
        foreach(var item in mine.items)
        {
            if (item.name.Equals(_name) && !item.equipedState.Equals(EquipState.EQUIPED))
            {
                return item.itemCount;
            }
        }       
        return 0;
    }
    Item FindIngredientItem(string _name)
    {
        mine ??= JY_CharacterListManager.s_instance.invenList[0];
        foreach (var item in mine.items)
        {
            if (item.name.Equals(_name))
            {
                return item;
            }
        }
        return null;
    }
}
