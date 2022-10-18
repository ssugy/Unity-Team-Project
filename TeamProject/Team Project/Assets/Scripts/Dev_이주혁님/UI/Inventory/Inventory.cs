using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<Item> items = new List<Item>();
    
    public delegate void OnChangeItem();        // �������� ����Ǹ� �κ��丮 UI�� �����ϴ� ��������Ʈ.
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
    
    // �������� �κ��丮�� �߰��ϴ� �ڵ�. �κ��丮�� ���� á�ٸ� ������ ȹ�� �Ұ�.
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
    
    // �ʵ忡 �ִ� �������� �ݴ� �ڵ�.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Debug.Log("������ ����");
            FieldItem fieldItem = other.GetComponent<FieldItem>();
            // �ʵ� �������� �κ��丮�� ����. �κ��丮�� ���� á���� ���� �� ����.
            if (AddItem(fieldItem.GetItem()))
            {
                fieldItem.DestroyItem();
            }
        }
        
    }
}
