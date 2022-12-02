using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 아이템 정보 패널을 담당하는 컴포넌트.
// 인벤토리의 아이템을 선택하면 아이템 정보 패널이 열리고, 사용/장착/해제/파괴 기능을 사용할 수 있다.
public class InfoPanel : MonoBehaviour
{    
    [Header("패널에 표시될 정보")]
    public Image icon;              // 아이템 아이콘.
    public Text nameText;           // 아이템 이름.
    public Text typeText;           // 아이템 타입.
    public Text explanationText;    // 아이템 설명.
    public GameObject QuickSlot;    // 아이템 퀵슬롯 등록 버튼
    [Space(32)]
    [Header("사용/파괴 버튼")]
    public Button useButton;
    public Text useButtonText;
    public Button destroyButton;
    public Text destroyButtonText;
    [Header("인벤토리 UI")]
    public InventoryUI iUi;
    [Header("추가옵션 UI")]
    public List<Text> options;
    public List<string> optionNames;

    // 아이템 패널을 출력하는 메소드. 인벤토리의 아이템을 클릭하면 해당 슬롯의 아이템 정보를 받아와 출력한다.
    public void SetInformation(Item _item, Slot _slot)
    {        
        useButton.onClick.RemoveAllListeners();         // 사용 버튼에 할당된 메소드를 초기화.
        destroyButton.onClick.RemoveAllListeners();     // 파괴 버튼에 할당된 메소드를 초기화.
        icon.sprite = _item.image;                      // 선택된 아이템의 이미지로 아이콘을 교체.
        nameText.text = _item.name;                     // 선택된 아이템의 이름으로 텍스트를 교체.
        explanationText.text = _item.explanation;       // 선택된 아이템의 설명으로 텍스트를 교체.
        useButton.gameObject.SetActive(true);           // 사용 버튼을 활성화함. (재료 아이템은 사용이 비활성화.)
        destroyButton.gameObject.SetActive(true);       // 파괴 버튼을 활성화함. (장착된 장비 아이템은 파괴가 비활성화.)
        QuickSlot.SetActive(false);
        switch (_item.type)                             // 아이템 타입에 따라 다른 기능을 수행.
        {
            case ItemType.EQUIPMENT:
                typeText.text = "장비";
                if (_item.equipedState == EquipState.UNEQUIPED)         // 장착 해제 상태면.
                {
                    useButtonText.text = "장착";
                    //useButton.onClick.AddListener(() => _item.Equip());
                    useButton.onClick.AddListener(() => iUi.Equip(_item, _slot));
                }
                else if (_item.equipedState == EquipState.EQUIPED)      // 장착 상태면.
                {
                    destroyButton.gameObject.SetActive(false);
                    useButtonText.text = "해제";
                    //useButton.onClick.AddListener(() => _item.Unequip());
                    useButton.onClick.AddListener(() => iUi.Unequip(_item, _slot));
                }
                break;
            case ItemType.CONSUMABLE:
                QuickSlot.SetActive(true);
                typeText.text = "소비";
                useButtonText.text = "사용";
                useButton.onClick.AddListener(()=>_item.Use());     // 사용 버튼을 누르면 소비 아이템의 효과가 발동.
                useButton.onClick.AddListener(() => JY_CharacterListManager.s_instance.invenList[0].RemoveItem(_item));      // 사용 버튼을 누르면 아이템이 사라짐.
                break;
            case ItemType.INGREDIENTS:
                typeText.text = "재료";
                useButton.gameObject.SetActive(false);
                break;
            default:
                typeText.text = "<오류>";
                break;
        }
        // 추가 옵션 값 표시
        ShowOptions(_item);

        destroyButton.onClick.AddListener(() => //Inventory.instance.RemoveItem(_item));
        iUi.DestroyItem(_item, _slot));
    }   

    private void ShowOptions(Item _item)
    {
        if (_item.type != ItemType.EQUIPMENT)
        {
            for (int i = 0; i < options.Count; i++)
            {
                options[i].gameObject.SetActive(false);
            }
            return;
        }
        if (_item.option != null && options.Count > 0)
        {
            for(int i = 0; i < options.Count; i++)
            {
                EquipOption.EquipAttrib attrib;
                float val;
                bool valid = _item.option.GetOptionValue(i, out attrib, out val);
                if (valid)
                {
                    options[i].gameObject.SetActive(true);
                    options[i].text = string.Format("{0}: {1:F2}%", optionNames[(int)attrib], val);
                }
                else
                {
                    options[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
