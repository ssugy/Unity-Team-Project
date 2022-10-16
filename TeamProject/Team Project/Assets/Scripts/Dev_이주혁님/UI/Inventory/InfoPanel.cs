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
                typeText.text = "���";
                if (_item.equipedState == EquipState.UNEQUIPED)
                {
                    useButtonText.text = "����";
                    useButton.onClick.AddListener(() => _item.Equip());                    
                }
                else if (_item.equipedState == EquipState.EQUIPED) 
                {
                    useButtonText.text = "����";
                    useButton.onClick.AddListener(() => _item.Equip());                    
                }                
                break;
            case ItemType.CONSUMABLE:
                typeText.text = "�Һ�";
                useButtonText.text = "���";
                useButton.onClick.AddListener(()=>_item.Use());     // ��� ��ư�� ������ �Һ� �������� ȿ���� �ߵ�.
                useButton.onClick.AddListener(() => Inventory.instance.RemoveItem(_item));      // ��� ��ư�� ������ �������� �����.
                break;
            case ItemType.INGREDIENTS:
                typeText.text = "���";
                useButton.gameObject.SetActive(false);
                break;
            default:
                typeText.text = "<����>";
                break;
        }                
        explanationText.text = _item.explanation;
        destroyButton.onClick.AddListener(() => Inventory.instance.RemoveItem(_item));
    }

    public void UseItem()
    {

    }
   
}
