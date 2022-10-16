using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<Item> items = new List<Item>();
    
    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    private int slotCnt;
    public int SlotCnt
    {
        get => slotCnt;
        set
        {
            slotCnt = value;            
        }
    }
    

    private void Awake()
    {        
        instance = this;
        //items = new List<Item>(36);
    }
    void Start()
    {
        SlotCnt = 36;
    }
    
    public bool AddItem(Item _item)
    {
        if (items.Count < SlotCnt)
        {
            items.Add(_item);
            if (onChangeItem != null)
            {
                onChangeItem();
            }            
            return true;
        }
        return false;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Debug.Log("¾ÆÀÌÅÛ ½Àµæ");
            FieldItem fieldItem = other.GetComponent<FieldItem>();
            if (AddItem(fieldItem.GetItem()))
            {
                fieldItem.DestroyItem();
            }
        }
        
    }
}
