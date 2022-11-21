using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //public static Inventory instance;
    public List<Item> items = new List<Item>();
    private Dictionary<int, Item> itemMap = new Dictionary<int, Item>();
    public delegate void OnChangeItem();        // �������� ����Ǹ� �κ��丮 UI�� �����ϴ� ��������Ʈ.
    public OnChangeItem onChangeItem;
    private int slotCnt;
    public int SlotCnt
    {
        get => slotCnt;
        set => slotCnt = value;
    }

    private void OnEnable()
    {
        //instance = this;
        
        
        
    }
    private void OnDisable()
    {
        //instance = null;        
        items.Clear();       
    }
    void Start()
    {
        if (JY_CharacterListManager.s_instance.invenList.Count > 0)
        {
            if (JY_CharacterListManager.s_instance.invenList[0].Equals(this))
            {
                if (JY_CharacterListManager.s_instance.selectNum >= 0)
                {
                    JY_CharacterListManager.s_instance.CopyInventoryDataToScript(items);
                }
            }
        }
        SlotCnt = 36;
        // ���� ���� ���� ���� ���۵Ǿ��� ��, �κ��丮�� ������ ����/������/����Ʈ�� �ҷ���.
        // �κ��丮�� �ε��� ��, �������� �̸��� Ÿ��, ���� �������� �ҷ����� ����.    
        // �κ� �������� ĳ���͸� ������ �� ListSwap ��ũ��Ʈ���� �ε��ϱ� ������ �ʿ����.
        if (items != null)
        {
            for (int i = 0; i < this.items.Count; i++)
            {
                this.items[i].ShallowCopy();
            }
            foreach (Item one in items)
            {
                if (one.equipedState.Equals(EquipState.EQUIPED))
                    one.effects[0].ExecuteRole(one);
            }
        }

        if (onChangeItem != null)
            onChangeItem();
    }
    //����� ���� ���߿� ����� ��.
#if UNITY_EDITOR
    private void Update()
    {
        bool isSave = false;
        // ��󸸵�� �ҵ���
        if (Input.GetKeyDown(KeyCode.F1))
        {
            AddItem(ItemDatabase.s_instance.itemDB[20].Copy(), 20);
            isSave = true;
        }
        // ���� ����
        if (Input.GetKeyDown(KeyCode.F2))
        {
            AddItem(ItemDatabase.s_instance.itemDB[16].Copy(), 16);
            isSave = true;
        }
        // ���� ����
        if (Input.GetKeyDown(KeyCode.F3))
        {
            AddItem(ItemDatabase.s_instance.itemDB[14].Copy(), 14);
            isSave = true;
        }
        // �κ��� ����
        if (Input.GetKeyDown(KeyCode.F4))
        {
            AddItem(ItemDatabase.s_instance.itemDB[15].Copy(), 15);
            isSave = true;
        }
        // ö�� ����
        if (Input.GetKeyDown(KeyCode.F5))
        {
            AddItem(ItemDatabase.s_instance.itemDB[0].Copy(), 0);
            isSave = true;
        }
        // ������ ����
        if (Input.GetKeyDown(KeyCode.F6))
        {
            AddItem(ItemDatabase.s_instance.itemDB[23].Copy(), 23);
            isSave = true;
        }
        // �氩 ����
        if (Input.GetKeyDown(KeyCode.F7))
        {
            AddItem(ItemDatabase.s_instance.itemDB[26].Copy(), 26);
            isSave = true;
        }
        if(isSave)
        {
            Player player = JY_CharacterListManager.s_instance.playerList[0];
            player.SaveData();
            JY_CharacterListManager.s_instance.Save();
        }
    }
#endif
    // �������� �κ��丮�� �߰��ϴ� �ڵ�. �κ��丮�� ���� á�ٸ� ������ ȹ�� �Ұ�.
    // �������� �߰��ϴ� ���� �����ߴٸ� true ��ȯ �� �κ��丮 UI�� ������.
    public bool AddItem(Item _item, int itemID = -1)
    {
        if (itemID >= 0)
        {
            _item.SetID(itemID);
        }
        if (_item.type == ItemType.CONSUMABLE || _item.type == ItemType.INGREDIENTS)
        {
            //���� ������ �������� �ִ��� ã�ƾ� �Ѵ�.
            Item found;
            if (itemMap.TryGetValue(_item.GetID(), out found))
            //if (itemMap[_item.GetID()] != null)
            {
                found.itemCount++;
                if (onChangeItem != null)
                    onChangeItem();
                return true;
            }
        }
        if (items.Count < SlotCnt)
        {
            _item.itemCount = 1;
            itemMap[_item.GetID()] = _item;
            items.Add(_item);
            // �ش� �������� Option�� �����Ѵ�.
            _item.SetOption();
            if (onChangeItem != null)
                onChangeItem();            
            return true;
        }
        return false;
    }
    // �������� �κ��丮���� �����ϴ� �ڵ�. ������ �Ŀ� �κ��丮 UI�� ������.
    public void RemoveItem(Item _item)
    {
        if (_item.type == ItemType.CONSUMABLE || _item.type == ItemType.INGREDIENTS)
        {
            _item.itemCount--;
            if (_item.itemCount <= 0)
            {
                items.Remove(_item);
                itemMap.Remove(_item.GetID());
            }
        }
        else
        {
            items.Remove(_item);
        }

        if (onChangeItem != null)
            onChangeItem();
    }

    // �ʵ忡 �ִ� �����۰� ��带 �ݴ� �ڵ�.
    private void OnTriggerEnter(Collider other)
    {
        bool isSave = false;
        if (other.CompareTag("Item"))
        {            
            FieldItem fieldItem = other.GetComponent<FieldItem>();
            // �ʵ� �������� �κ��丮�� ����. �κ��丮�� ���� á���� ���� �� ����.
            if (AddItem(fieldItem.GetItem()))
            {
                fieldItem.DestroyItem();
                isSave = true;
            }
        }
        else if (other.CompareTag("Gold"))
        {
            FieldGold fieldGold = other.GetComponent<FieldGold>();
            JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold += fieldGold.ammount;
            // ��� ȹ�� ���� ���� �ʿ�.
            AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Get_Gold);
            Destroy(fieldGold.gameObject);
            isSave = true;
        }
        if (isSave)
        {
            Player player = JY_CharacterListManager.s_instance.playerList[0];
            player.SaveData();
            JY_CharacterListManager.s_instance.Save();
        }
    }
}
