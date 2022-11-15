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
            if (slots[i].item.type == ItemType.EQUIPMENT)
            {
                // 장착된 장비의 아이콘이 생겨야 하므로 slots[i].item.effect[0] 의 role을 실행한다.
                //slots[i].item.effects[0].ExecuteRole(slots[i].item);
                if (slots[i].item.equipedState == EquipState.EQUIPED)
                {
                    slots[i].item.Equip();
                }                
            }
            slots[i].UpdateSlotUI();
        }
    } 
    public void UpdateGold()
    {
        gold.text = JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold.ToString();
    }

    private void Awake()
    {
        instance ??= this;
        inventory = JY_CharacterListManager.s_instance.invenList[0];
        slots = slotHolder.GetComponentsInChildren<Slot>();

    }
    
    private void OnEnable()
    {
        instance ??= this;
        UpdateGold();
        RedrawSlotUI();
        // 첫번째 RedrawSlotUI를 호출하여 장착된 Icon들을 초기화 시킵니다.
        // 이후, OnChangeItem 이벤트를 설정합니다. Awake에서 먼저 등록하면 무한호출에 걸립니다.
        //inventory.onChangeItem += RedrawSlotUI;
    }
    private void OnDisable()
    {
        instance = null;
        // 이벤트를 해제합니다. 다시들어올때 다시 설정합니다.
        //inventory.onChangeItem -= RedrawSlotUI;
    }
    public void Equip(Item _item, Slot _slot)
    {
        _item.Equip();
        //_slot.UpdateSlotUI();
        RedrawSlotUI();
    }
    public void Unequip(Item _item, Slot _slot)
    {
        _item.Unequip();
        _slot.UpdateSlotUI();
    }
    public void DestroyItem(Item _item, Slot _slot)
    {
        JY_CharacterListManager.s_instance.invenList[0].RemoveItem(_item);
        RedrawSlotUI();
    }
}
