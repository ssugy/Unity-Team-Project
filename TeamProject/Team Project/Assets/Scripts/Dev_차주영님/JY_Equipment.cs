using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JY_Equipment : MonoBehaviour
{
    public JY_ItemSlot itemSelected;
    public JY_ItemSlot ChangeTargetSlot;

    public JY_ItemSlot weaponL;
    public JY_ItemSlot weaponR;
    public JY_ItemSlot helmet;
    public JY_ItemSlot top;
    public JY_ItemSlot bottom;
    public JY_ItemSlot gloves;
    public JY_ItemSlot boots;

    public string itemName;
    public string itemType;
    public int itemTypeNum;
    public int equipLevel;
    public int statusAttack;
    public int statusDefence;
    public bool isEmpty;
    public int itemNum;

    public Image itemImageUI;
    public Text itemNameUI;
    public Text itemStatusUI;
    public Text itemTypeUI;
    public Text itemExplainUI;

    public void infoRenew()
    {

        itemImageUI.sprite = itemSelected.itemImage;
        itemNameUI.text = itemName;
        itemStatusUI.text = string.Empty;
        if (statusAttack != 0)
            itemStatusUI.text = "공격력 : " + statusAttack.ToString() + "\n";
        if(statusDefence !=0 )
            itemStatusUI.text = "방어력 : " + statusDefence.ToString() + "\n";
        itemTypeUI.text = itemType;

        itemExplainUI.text = string.Empty;
        itemExplainUI.text += "착용 레벨 : " + equipLevel.ToString();
    }

    public void equipItem()
    {
        switch (itemTypeNum)
        {
            case 0:
                ChangeTargetSlot = weaponL;
                break;
        }

        ChangeTargetSlot.itemName = itemName;
        ChangeTargetSlot.itemType = itemType;
        ChangeTargetSlot.equipLevel = equipLevel;
        ChangeTargetSlot.statusAttack = statusAttack;
        ChangeTargetSlot.statusDefence = statusDefence;
        ChangeTargetSlot.isEquiped = true;
        itemSelected.isEquiped = true;
        ChangeTargetSlot.slotImage.sprite = itemSelected.itemImage;
        ChangeTargetSlot.slotImage.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void clearEquipItem()
    {
        switch (itemTypeNum)
        {
            case 0:
                ChangeTargetSlot = weaponL;
                break;
        }

        ChangeTargetSlot.itemName = null;
        ChangeTargetSlot.itemType = null;
        ChangeTargetSlot.equipLevel = 0;
        ChangeTargetSlot.statusAttack = 0;
        ChangeTargetSlot.statusDefence = 0;
        ChangeTargetSlot.isEquiped = false;
        ChangeTargetSlot.slotImage.gameObject.SetActive(false);
    }

    public void destoryItem()
    {
        if (itemSelected.isEquiped)
        {
            clearEquipItem();
        }

        itemSelected.itemName = null;
        itemSelected.itemType = null;
        itemSelected.equipLevel = 0;
        itemSelected.statusAttack = 0;
        itemSelected.statusDefence = 0;
        itemSelected.isEquiped = false;
        itemSelected.isEmpty = true;
        itemSelected.slotImage.gameObject.SetActive(false);
        itemSelected.itemImage = null;
        this.gameObject.SetActive(false);
    }
}
