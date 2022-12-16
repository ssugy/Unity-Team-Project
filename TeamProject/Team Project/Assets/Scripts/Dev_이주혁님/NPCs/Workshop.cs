using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// 아이템의 제조법을 구성하는 구조체.
[System.Serializable]
public struct ItemMethod
{
    // 만들고자 하는 아이템의 인덱스.
    public int objective;

    // 재료 아이템의 인덱스와 개수는 배열로 관리함. 크기가 같아야 함.
    public int[] index;
    public int[] ammount;

    // 제작에 필요한 골드.
    public int gold;
}

// 아이템 제작 및 강화 기능을 담당할 스크립트.
public class Workshop : MonoBehaviour
{
    public enum WorkshopType
    {
        NONE,
        PRODUCE,
        ENHANCE
    }

    public static Workshop workshop;
    public ItemMethod selectedMethod;         // 제작에 사용됨.
    public Item selectedItem;                   // 강화에 사용됨.

    private WorkshopType type;

    // 제작 아이템 목록.
    public List<ItemMethod> methods;

    public Slot_Workshop[] slots;
    public RectTransform scroll;

    // 아이템 정보를 표시할 패널.
    public GameObject infoPanel;    
    public Image infoIcon;
    public Text infoName;
    public Text infoType;
    public Text infoLevel;
    public Text infoGold;

    // 재료들.
    public Image[] icons;
    public Text[] texts;

    public Button interact;
    public Slot_Workshop firstList; // 상점 목록 첫번째 아이템.
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
        // 우선 슬롯을 모두 비활성화.
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
                    tmp.text = "제작";

                    // 아이템 제작 패널이 열릴 때 첫번째 아이템이 선택된 상태로 시작하게끔 함.
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
                        // 장착 중이지 않으며, 강화 단계가 9 이하인 장비만 강화 가능.
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
                    tmp.text = "강화";
                    break;
                }
            default:
                {
                    Debug.Log("워크샵 페이지 열기 실패");
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
                                infoType.text = "장비";
                                break;
                            }
                        case ItemType.CONSUMABLE:
                            {
                                infoType.text = "소비";
                                break;
                            }
                        case ItemType.INGREDIENTS:
                            {
                                infoType.text = "제료";
                                break;
                            }
                        default:
                            {
                                Debug.Log("정보 패널 열기 실패");
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
                    infoType.text = "장비";
                    infoLevel.text = "Lv. " + selectedItem.level.ToString();
                    infoGold.text = ((selectedItem.enhanced + 2) * 200).ToString();

                    // 장비 강화 단계에 따라 다른 강화 재료를 사용.
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
                    Debug.Log("정보 패널 열기 실패");
                    return;
                }
        }
    }

    // 아이템 인덱스(DB 상 아이템 번호)를 통해 제작 가능한 아이템을 로드함.
    void SetSlotToProduce(Slot_Workshop _slot, ItemMethod _method)
    {
        _slot.method.objective = _method.objective;
        _slot.method.index = (int[])_method.index.Clone();
        _slot.method.ammount = (int[])_method.ammount.Clone();
        _slot.method.gold = _method.gold;

        _slot.icon.sprite = ItemDatabase.s_instance.itemDB[_method.objective].image;
        _slot.nameText.text = ItemDatabase.s_instance.itemDB[_method.objective].name;
    }

    // 인벤토리 슬롯 넘버를 통해 강화할 물건을 로드함.
    void SetSlotToEnhance(Slot_Workshop _slot, Item _item)
    {
        _slot.item = _item;

        _slot.icon.sprite = _slot.item.image;
        _slot.nameText.text = _slot.item.name;
    }
    void ProduceItem()
    {
        // 재료를 다 갖고 있는지 체크.
        for (int i = 0; i < selectedMethod.index.Length; i++)
        {
            string name = ItemDatabase.s_instance.itemDB[selectedMethod.index[i]].name;
            if (FindIngredientNum(name) < selectedMethod.ammount[i])
            {
                Debug.Log("재료 부족으로 제작 실패");
                return;
            }               
        }

        // 소지 골드가 부족하지 않은지 체크.
        if (JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold < selectedMethod.gold) 
        {
            Debug.Log("소지금 부족으로 제작 실패");
            return;
        }

        Item item = ItemDatabase.s_instance.itemDB[selectedMethod.objective].Copy();
        // 아이템 추가에 성공했으면 (플레이어의 인벤토리가 가득 차지 않았다면)
        // 아이템 제작으로 얻는 장비는 추가옵션이 붙은 상태로 획득됨.
        if (mine.AddItem(item, true)) 
        {
            Debug.Log("제작 성공");
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
            Debug.Log("인벤토리가 가득 차 제작 실패");
    }
    void EnhanceItem()
    {

    }

    // 찾으려는 재료 아이템을 검색하여 개수를 반환함. 이름으로 검색.
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
