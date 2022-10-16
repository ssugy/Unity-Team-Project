using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    Item item;
    public Image icon;
    public Text nameText;
    public Text type;
    public Text explanation;

    public void SetInformation(Item _item)
    {
        icon.sprite = _item.image;
        nameText.text = _item.name;
        switch (_item.type)
        {
            case ItemType.EQUIPMENT:
                type.text = "���";
                break;
            case ItemType.CONSUMABLE:
                type.text = "�Һ�";
                break;
            case ItemType.STUFF:
                type.text = "���";
                break;
            default:
                type.text = "<����>";
                break;
        }                
        explanation.text = _item.explanation;
    }

   
}
