using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    // 플레이어의 인벤토리를 관리할 스크립트입니다.

    // 퀵슬롯에 대입될 변수들입니다.
    public ItemData quick_1;
    public ItemData quick_2;
    public ItemData quick_3;
    public ItemData quick_4;

    public GameObject empty;
    public GameObject inventoryUI;
    public GameObject quickSlotSet;

    public void QuickSlot_1()
    {
        if (quick_1 != null)
        {
            UseItem(quick_1);            
            return;
        }
        QuickSlotSet();
    }
    public void QuickSlot_2()
    {
        if (quick_2 != null)
        {
            UseItem(quick_2);            
            return;
        }
        QuickSlotSet();
    }
    public void QuickSlot_3()
    {
        if (quick_3 != null)
        {
            UseItem(quick_3);
            return;
        }
        QuickSlotSet();
    }
    public void QuickSlot_4()
    {
        if (quick_4 != null)
        {
            UseItem(quick_4);
            return;
        }
        QuickSlotSet();
    }
    public void QuickSlotSet()
    {        
        inventoryUI.SetActive(true);        
    }
    public void UseItem(ItemData _item)
    {
        if (_item.cnt != 0)
        {
            _item.cnt -= 1;
            CoolDown(_item);
        }
        else
        {
            empty.SetActive(true);
        }
    }    

    void Start()
    {       
        quick_1 = null;
        quick_2 = null;
        quick_3 = null;
        quick_4 = null;
    }

    void CoolDown(ItemData _item)
    {
        // 소비아이템의 쿨타임이 결정되면 쿨타임 동안 아이템을 사용할 수 없게 만들 함수.
        // ItemData에 쿨타임 값을 변수로 선언해야 할 듯.
    }

}
