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
    
    private void OnEnable()
    {
        instance = this;
        
        if (JY_CharacterListManager.s_instance.selectNum >= 0)
        {
            items = JY_CharacterListManager.s_instance.characterInventoryData.InventoryJDataList[JY_CharacterListManager.s_instance.selectNum].itemList;
            
        }        
    }
    private void OnDisable()
    {        
        
        instance = null;
        items = null;
        
    }
    void Start()
    {
        SlotCnt = 36;
        // 씬이 시작되었을 때, 인벤토리의 아이템 설명/아이콘/이펙트를 불러옴.
        // 인벤토리를 로드할 때, 아이템의 이름과 타입, 착용 정보만을 불러오기 때문.    
        if (items != null)
        {
            for (int i = 0; i < this.items.Count; i++)
            {
                this.items[i].ShallowCopy();
            }
            foreach (Item one in items)
            {
                if (one.equipedState.Equals(EquipState.EQUIPED))
                {
                    one.effects[0].ExecuteRole(one);
                }
            }
        }
        if (onChangeItem != null)
        {
            onChangeItem();
        }
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
