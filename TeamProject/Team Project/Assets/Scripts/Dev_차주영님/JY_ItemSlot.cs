using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JY_ItemSlot : MonoBehaviour
{
    public JY_Equipment equipment;
    
    public string itemName;
    public string itemType;
    public int itemTypeNum;
    public int equipLevel;
    public int statusAttack;
    public int statusDefence;
    public bool isEmpty;
    public bool isEquiped;
    public int itemNum;

    public Image slotImage;
    public Sprite itemImage;

    public void openInfo()
    {
        if (!isEmpty)
        {
            equipment.itemName = itemName;
            equipment.itemType = itemType;
            equipment.itemTypeNum = itemTypeNum;
            equipment.equipLevel = equipLevel;
            equipment.statusAttack = statusAttack;
            equipment.statusDefence = statusDefence;

            equipment.itemSelected = this;
            equipment.infoRenew();
            equipment.gameObject.SetActive(true);
        }
    }
}
