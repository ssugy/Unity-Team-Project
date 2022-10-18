using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<Item> items = new List<Item>();
    
    public delegate void OnChangeItem();        // 아이템이 변경되면 인벤토리 UI를 갱신하는 델리게이트.
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
        items.Clear();
        items = JY_CharacterListManager.s_instance.characterInventoryData.InventoryJDataList[JY_CharacterListManager.s_instance.selectNum].itemList;
        
    }
    
    // 아이템을 인벤토리에 추가하는 코드. 인벤토리가 가득 찼다면 아이템 획득 불가.
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
    public void RemoveItem(Item _item)
    {
        items.Remove(_item);
        if (onChangeItem != null)
        {
            onChangeItem();
        }
    }
    
    // 필드에 있는 아이템을 줍는 코드.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Debug.Log("아이템 습득");
            FieldItem fieldItem = other.GetComponent<FieldItem>();
            // 필드 아이템을 인벤토리에 넣음. 인벤토리가 가득 찼으면 얻을 수 없음.
            if (AddItem(fieldItem.GetItem()))
            {
                fieldItem.DestroyItem();
            }
        }
        
    }
}
