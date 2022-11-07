using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;
    public static InventoryUI instance;

    public Image weaponIcon;
    public Image shieldIcon;
    public Image helmetIcon;
    public Image chestIcon;
    public Image legIcon;
    public Text gold;
    public GameObject inventoryPanel;        
    public Slot[] slots;
    public Transform slotHolder;
    
    private void RedrawSlotUI()
    {
        Array.ForEach(slots, e => e.RemoveSlot());               
        for (int i = 0; i < inventory.items.Count; i++)
        {
            slots[i].item = inventory.items[i];            
            slots[i].isEmpty = false;
            slots[i].UpdateSlotUI();
        }
    } 
    public void UpdateGold()
    {
        gold.text = string.Format("{0:#,0}", Player.instance.playerStat.Gold);
    }

    private void Awake()
    {
        instance = this;
        inventory = Inventory.instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inventory.onChangeItem += RedrawSlotUI;
    }
    
    private void OnEnable()
    {
        UpdateGold();
        RedrawSlotUI();
    }
}
