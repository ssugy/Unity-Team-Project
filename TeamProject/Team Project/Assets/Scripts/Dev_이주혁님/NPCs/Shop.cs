using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 구매와 판매 기능을 담당함.
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

    public int[] sellList;  // 상점에서 판매할 아이템의 인덱스 목록.
    public Slot_Shop[] slots;
    public RectTransform scroll;

    // 아이템 정보를 표시할 패널.
    public GameObject infoPanel;
    public GameObject infoEquiped;
    public Image infoIcon;
    public Text infoName;
    public Text infoType;
    public Text infoLevel;
    public Text infoExplain;

    public Button interact;
    public Slot_Shop firstList; // 상점 목록 첫번째 아이템.
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
        // 상점 패널을 열 때마다 실행됨.
        goldText.text = JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold.ToString();
        shop ??= this;               
    }

    private void OnDisable()
    {
        shop = null;
    }

    public void SetType(int _num)
    {
        // 구매/판매 페이지를 열 때마다 스크롤 맨 위로.
        scroll.anchoredPosition = Vector2.zero;

        type = (ShopType)_num;

        infoPanel.SetActive(false);
        title_Buy.SetActive(false);
        title_Sell.SetActive(false);
        // 우선 슬롯을 모두 비활성화.
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
                    tmp.text = "구매";

                    // 상점 구매 패널이 열릴 때 첫번째 아이템이 선택된 상태로 시작하게끔 함.
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
                    tmp.text = "판매";
                    break;
                }
            default:
                {
                    Debug.Log("상점 페이지 열기 실패");
                    return;
                }
        }

        
    }

    // 아이템 인덱스(DB 상 아이템 번호)를 통해 구매할 물건을 로드함.
    void SetSlotToBuy(Slot_Shop _slot, int _index)
    {
        _slot.item = ItemDatabase.s_instance.itemDB[_index].Copy();

        _slot.icon.sprite = _slot.item.image;
        _slot.nameText.text = _slot.item.name;
        _slot.priceText.text = _slot.item.price.ToString();
    }

    // 인벤토리 슬롯 넘버를 통해 판매할 물건을 로드함.
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
                    infoType.text = "장비";
                    if (selected.equipedState.Equals(EquipState.EQUIPED))
                    {
                        infoEquiped.SetActive(true);
                        interact.interactable = false;
                    }                        
                    break;
                }
            case ItemType.CONSUMABLE:
                {
                    infoType.text = "소비";
                    break;
                }
            case ItemType.INGREDIENTS:
                {
                    infoType.text = "재료";
                    break;
                }
        }
        infoLevel.text = "Lv. " + selected.level.ToString();
        infoExplain.text = selected.explanation;
    }

    // 구매 버튼에 할당될 메소드.
    public void BuyItem()
    {
        // 플레이어의 소지금이 아이템 가격보다 적은 지 체크. 소지금이 적다면 리턴.
        if (JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold < selected.price)
        {
            Debug.Log("구매 실패");
            return;
        }
            

        // 아이템 추가에 성공했으면 (플레이어의 인벤토리가 가득 차지 않았다면)
        if (JY_CharacterListManager.s_instance.invenList[0].AddItem(selected, false))
        {
            Debug.Log("구매 성공");
            JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold -= selected.price;
            goldText.text = JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold.ToString();
            return;
        }
        Debug.Log("구매 실패");
    }

    // 판매 버튼에 할당될 메소드.
    public void SellItem()
    {
        // 아이템 정보 패널은 닫힘.
        infoPanel.SetActive(false);

        // 아이템을 인벤토리에서 삭제.
        mine.RemoveItem(selected);
        // 아이템 가격만큼 돈을 늘려줌. 판매가는 살 때 가격의 80%
        JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold 
            += (int)(selected.price * 0.8);
        goldText.text = JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold.ToString();

        // 리스트를 다시 갱신.
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
