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
    public Text gold;
    public GameObject inventoryPanel;        
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
    public void UpdateGold()
    {
        gold.text = string.Format("{0:#,0}", Player.instance.playerStat.Gold);
    }

    void Start()
    {
        inventory = Inventory.instance;
        instance = this;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inventory.onChangeItem += RedrawSlotUI;
        if (Player.instance != null)
        {
            UpdateGold();
        }
        // 씬이 시작되었을 때, 인벤토리의 아이템 설명/아이콘/이펙트를 불러옴.
        // 인벤토리를 로드할 때, 아이템의 이름과 타입, 착용 정보만을 불러오기 때문.
        if (inventory.items.Count >= 1)
        {
            for (int i = 0; i < inventory.items.Count; i++)
            {
                inventory.items[i].ShallowCopy();
            }
        }
        if (inventory != null && inventory.onChangeItem != null)
        {
            inventory.onChangeItem();
        }
        this.gameObject.SetActive(false);        
    }    
}
