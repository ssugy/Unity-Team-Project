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
                // ������ ����� �������� ���ܾ� �ϹǷ� slots[i].item.effect[0] �� role�� �����Ѵ�.
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
        // ù��° RedrawSlotUI�� ȣ���Ͽ� ������ Icon���� �ʱ�ȭ ��ŵ�ϴ�.
        // ����, OnChangeItem �̺�Ʈ�� �����մϴ�. Awake���� ���� ����ϸ� ����ȣ�⿡ �ɸ��ϴ�.
        //inventory.onChangeItem += RedrawSlotUI;
    }
    private void OnDisable()
    {
        instance = null;
        // �̺�Ʈ�� �����մϴ�. �ٽõ��ö� �ٽ� �����մϴ�.
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
