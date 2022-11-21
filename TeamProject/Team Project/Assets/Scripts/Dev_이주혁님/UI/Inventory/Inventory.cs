using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //public static Inventory instance;
    public List<Item> items = new List<Item>();
    private Dictionary<int, Item> itemMap = new Dictionary<int, Item>();
    public delegate void OnChangeItem();        // 아이템이 변경되면 인벤토리 UI를 갱신하는 델리게이트.
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
        // 월드 씬과 던전 씬이 시작되었을 때, 인벤토리의 아이템 설명/아이콘/이펙트를 불러옴.
        // 인벤토리를 로드할 때, 아이템의 이름과 타입, 착용 정보만을 불러오기 때문.    
        // 로비 씬에서는 캐릭터를 선택할 때 ListSwap 스크립트에서 로드하기 때문에 필요없음.
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
    //디버그 전용 나중에 지우면 됨.
#if UNITY_EDITOR
    private void Update()
    {
        bool isSave = false;
        // 살라만드라 불덩이
        if (Input.GetKeyDown(KeyCode.F1))
        {
            AddItem(ItemDatabase.s_instance.itemDB[20].Copy(), 20);
            isSave = true;
        }
        // 소형 포션
        if (Input.GetKeyDown(KeyCode.F2))
        {
            AddItem(ItemDatabase.s_instance.itemDB[16].Copy(), 16);
            isSave = true;
        }
        // 원형 방패
        if (Input.GetKeyDown(KeyCode.F3))
        {
            AddItem(ItemDatabase.s_instance.itemDB[14].Copy(), 14);
            isSave = true;
        }
        // 널빤지 방패
        if (Input.GetKeyDown(KeyCode.F4))
        {
            AddItem(ItemDatabase.s_instance.itemDB[15].Copy(), 15);
            isSave = true;
        }
        // 철제 도끼
        if (Input.GetKeyDown(KeyCode.F5))
        {
            AddItem(ItemDatabase.s_instance.itemDB[0].Copy(), 0);
            isSave = true;
        }
        // 가벼운 갑옷
        if (Input.GetKeyDown(KeyCode.F6))
        {
            AddItem(ItemDatabase.s_instance.itemDB[23].Copy(), 23);
            isSave = true;
        }
        // 경갑 하의
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
    // 아이템을 인벤토리에 추가하는 코드. 인벤토리가 가득 찼다면 아이템 획득 불가.
    // 아이템을 추가하는 데에 성공했다면 true 반환 후 인벤토리 UI를 갱신함.
    public bool AddItem(Item _item, int itemID = -1)
    {
        if (itemID >= 0)
        {
            _item.SetID(itemID);
        }
        if (_item.type == ItemType.CONSUMABLE || _item.type == ItemType.INGREDIENTS)
        {
            //먼저 동일한 아이템이 있는지 찾아야 한다.
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
            // 해당 아이템의 Option을 지정한다.
            _item.SetOption();
            if (onChangeItem != null)
                onChangeItem();            
            return true;
        }
        return false;
    }
    // 아이템을 인벤토리에서 삭제하는 코드. 삭제한 후에 인벤토리 UI를 갱신함.
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

    // 필드에 있는 아이템과 골드를 줍는 코드.
    private void OnTriggerEnter(Collider other)
    {
        bool isSave = false;
        if (other.CompareTag("Item"))
        {            
            FieldItem fieldItem = other.GetComponent<FieldItem>();
            // 필드 아이템을 인벤토리에 넣음. 인벤토리가 가득 찼으면 얻을 수 없음.
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
            // 골드 획득 사운드 변경 필요.
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
