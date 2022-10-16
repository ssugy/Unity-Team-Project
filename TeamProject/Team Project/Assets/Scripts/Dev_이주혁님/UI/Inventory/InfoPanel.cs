using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    //private Item item;
    public Image icon;
    public Text nameText;
    public Text typeText;
    public Text explanationText;

    public Button useButton;
    public Text useButtonText;
    public Button destroyButton;
    public Text destroyButtonText;


    public void SetInformation(Item _item, Slot _slot)
    {
        useButton.onClick.RemoveAllListeners();
        destroyButton.onClick.RemoveAllListeners();
        icon.sprite = _item.image;
        nameText.text = _item.name;
        useButton.gameObject.SetActive(true);
        switch (_item.type)
        {
            case ItemType.EQUIPMENT:
                typeText.text = "장비";
                if (_item.equipedState == EquipState.UNEQUIPED)
                {
                    useButtonText.text = "장착";
                    useButton.onClick.AddListener(() => _item.Equip());                    
                }
                else if (_item.equipedState == EquipState.EQUIPED) 
                {
                    useButtonText.text = "해제";
                    useButton.onClick.AddListener(() => _item.Equip());                    
                }                
                break;
            case ItemType.CONSUMABLE:
                typeText.text = "소비";
                useButtonText.text = "사용";
                useButton.onClick.AddListener(()=>_item.Use());     // 사용 버튼을 누르면 소비 아이템의 효과가 발동.
                useButton.onClick.AddListener(() => Inventory.instance.RemoveItem(_item));      // 사용 버튼을 누르면 아이템이 사라짐.
                break;
            case ItemType.INGREDIENTS:
                typeText.text = "재료";
                useButton.gameObject.SetActive(false);
                break;
            default:
                typeText.text = "<오류>";
                break;
        }                
        explanationText.text = _item.explanation;
        destroyButton.onClick.AddListener(() => Inventory.instance.RemoveItem(_item));
    }

    public void UseItem()
    {

    }
   
}
