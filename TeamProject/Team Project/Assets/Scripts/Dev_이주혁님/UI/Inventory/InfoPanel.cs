using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    private Item item;
    public Image icon;
    public Text nameText;
    public Text typeText;
    public Text explanationText;

    public Button useButton;
    public Text useButtonText;
    public Button destroyButton;
    public Text destroyButtonText;


    public void SetInformation(Item _item)
    {
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
                }
                else if (_item.equipedState == EquipState.EQUIPED) 
                {
                    useButtonText.text = "해제";
                }                
                break;
            case ItemType.CONSUMABLE:
                typeText.text = "소비";
                useButtonText.text = "사용";
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
    }

    public void UseItem()
    {

    }
   
}
