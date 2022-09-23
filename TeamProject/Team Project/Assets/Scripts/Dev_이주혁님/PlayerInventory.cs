using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    // �÷��̾��� �κ��丮�� ������ ��ũ��Ʈ�Դϴ�.

    // �����Կ� ���Ե� �������Դϴ�.
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
        // �Һ�������� ��Ÿ���� �����Ǹ� ��Ÿ�� ���� �������� ����� �� ���� ���� �Լ�.
        // ItemData�� ��Ÿ�� ���� ������ �����ؾ� �� ��.
    }

}
