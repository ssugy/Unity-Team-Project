using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;

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

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();        
        inventory.onChangeItem += RedrawSlotUI;
        this.gameObject.SetActive(false);
        

    }
    /*
    private void OnEnable()
    {
        SetInven();
    }
    private void SetInven()
    {
        
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].item = inventory.items[i];
            slots[i].icon.sprite = slots[i].item.image;
            if (slots[i].item != null)
            {
                slots[i].isEmpty = false;
                slots[i].icon.gameObject.SetActive(true);
            }
            else
            {
                slots[i].isEmpty = true;
                slots[i].icon.gameObject.SetActive(false);
            }
        }
    }
    */

    // Update is called once per frame
    void Update()
    {
        
    }    
}
