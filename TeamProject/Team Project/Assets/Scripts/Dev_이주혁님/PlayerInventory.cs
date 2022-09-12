using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // 플레이어의 인벤토리를 관리할 스크립트입니다.

    // 퀵슬롯에 대입될 변수들입니다.
    public Item quick_1;
    public Item quick_2;
    public Item quick_3;
    public Item quick_4;

    public GameObject inventoryUI;
    public GameObject quickSlotSet;

    public void QuickSlot_1()
    {
        if (quick_1 != null)
        {
            UseItem(quick_1);
            return;
        }
        quick_1 = QuickSlotSet();
    }
    public void QuickSlot_2()
    {
        if (quick_2 != null)
        {
            UseItem(quick_2);
            return;
        }
        quick_2 = QuickSlotSet();
    }
    public void QuickSlot_3()
    {
        if (quick_3 != null)
        {
            UseItem(quick_3);
            return;
        }
        quick_3 = QuickSlotSet();
    }
    public void QuickSlot_4()
    {
        if (quick_4 != null)
        {
            UseItem(quick_4);
            return;
        }
        quick_4 = QuickSlotSet();
    }
    public Item QuickSlotSet()
    {        
        quickSlotSet.SetActive(true);
        return null;
    }
    public void UseItem(Item _item)
    {

    }
    void Cooltime()
    {

    }

    void Start()
    {
        
        quick_1 = null;
        quick_2 = null;
        quick_3 = null;
        quick_4 = null;
    }

}
