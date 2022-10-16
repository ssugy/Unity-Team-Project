using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;
    public static InventoryUI instance;

    public Image weaponIcon;
    public Image shieldIcon;
    public Image helmetIcon;
    public Image chestIcon;
    public Image legIcon;

    public GameObject inventoryPanel;
    bool activeInventory = false;
    
    public Slot[] slots;
    public Transform slotHolder;
    
    private void RedrawSlotUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveSlot();

        }
        for (int i = 0; i < inventory.items.Count; i++)
        {
            slots[i].item = inventory.items[i];
            slots[i].isEmpty = false;
            slots[i].UpdateSlotUI();

        }
    }    
    
    void Start()
    {
        inventory = Inventory.instance;
        instance = this;
        slots = slotHolder.GetComponentsInChildren<Slot>();        
        inventory.onChangeItem += RedrawSlotUI;
        this.gameObject.SetActive(false);      
    }
    
}
