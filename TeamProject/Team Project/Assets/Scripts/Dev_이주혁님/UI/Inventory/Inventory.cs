using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{    
    public List<Item> items;    
    public delegate void OnChangeItem();        // �������� ����Ǹ� �κ��丮 UI�� �����ϴ� ��������Ʈ.
    // onChangeItem �̺�Ʈ���� RedrawSlotUI�� �Ҵ�ȴ�. �κ��丮 UI�� �������� ���� �����.
    // �κ��丮 UI�� �������� ���� ���� onChangeItem�� null�� �Ǿ� ������� �ʴ´�. (���ʿ��� �Լ� ȣ�� X)
    public OnChangeItem onChangeItem { get; set; }
    public int SlotCnt { get; set; } = 36;    
    
    void Start()
    {
        if (JY_CharacterListManager.s_instance.invenList.Count > 0)
        {
            // invenList[0]�� ����� ĳ������ �κ��丮��. �ٸ� ������� ĳ���Ͷ�� �������� ����.
            if (JY_CharacterListManager.s_instance.invenList[0].Equals(this))
            {
                if (JY_CharacterListManager.s_instance.selectNum >= 0)
                {
                    JY_CharacterListManager.s_instance.CopyInventoryDataToScript(items);
                }
            }
        }       
        
        if (items != null)
        {
            // ���� �κ��丮���� �������� ������.
            items.ForEach(e =>
            {
                if (e.equipedState.Equals(EquipState.EQUIPED))
                    e.effects[0].ExecuteRole(e);
            });
        }

        if (JY_CharacterListManager.s_instance.invenList.Count > 0)
        {
            // ��� ������ �� ���� ������ HP, SP�� ��������� ���� ���۵� �� HP, SP�� ���� �� ���·� ����.
            JY_CharacterListManager.s_instance.playerList[0].playerStat.CurHP
                = JY_CharacterListManager.s_instance.playerList[0].playerStat.HP;
            JY_CharacterListManager.s_instance.playerList[0].playerStat.CurSP
                = JY_CharacterListManager.s_instance.playerList[0].playerStat.SP;
        }        

        if (onChangeItem != null)
            onChangeItem();
    }
    /*
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
    */

    // �������� �κ��丮�� �߰��ϴ� �ڵ�. �κ��丮�� ���� á�ٸ� ������ ȹ�� �Ұ�.
    // �������� �߰��ϴ� ���� �����ߴٸ� true ��ȯ �� �κ��丮 UI�� ������.
    public bool AddItem(Item _item, bool _setOption = true)
    {        
        // �߰��Ǵ� �������� �̹� �κ��丮�� �����ϴ� �Һ�, ��� �������� ���. (�������� ������)
        if ((int)_item.type >= 2)
        {
            //���� ������ �������� �ִ��� ã�ƾ� �Ѵ�.
            // itemMap ��� �̸����� �˻��ϵ��� ��. (ȿ�������� ������ ������ �߻����� ����)
            foreach (var item in items) 
            {
                if (item.name.Equals(_item.name))
                {
                    item.itemCount++;
                    if (onChangeItem != null)
                        onChangeItem();
                    return true;
                }
            }            
        }

        // ��� �������� ���. Ȥ�� ���ʷ� �����ϴ� ���, �Һ� �������� ���.
        if (items.Count < SlotCnt) 
        {
            _item.itemCount = 1;            
            items.Add(_item);
            // �ش� �������� Option�� �����Ѵ�. ���Ϳ��� ����� �����۸� �ɼ� ����.
            if(_setOption)
                _item.SetOption();
            if (onChangeItem != null)
                onChangeItem();            
            return true;
        }

        // ������ �߰��� ������ ���.
        return false;
    }

    // �������� �κ��丮���� �����ϴ� �ڵ�. ������ �Ŀ� �κ��丮 UI�� ������.
    public bool RemoveItem(Item _item, int _num = 1)
    {
        // �����Ϸ��� �������� ���� ������ ������ ���ٸ� ���� �Ұ�.
        if (_item.itemCount < _num)
            return false;

        switch (_item.type)
        {
            case ItemType.CONSUMABLE:
            case ItemType.INGREDIENTS:
                {
                    _item.itemCount -= _num;
                    if (_item.itemCount <= 0)                    
                        items.Remove(_item);
                    break;
                }
            case ItemType.EQUIPMENT:
                {
                    items.Remove(_item);
                    break;
                }
            default:
                {
                    Debug.Log("������ Ÿ�� ����");
                    return false;
                }
        }        

        if (onChangeItem != null)
            onChangeItem();
        return true;
    }

    // �ʵ忡 �ִ� �����۰� ��带 �ݴ� �ڵ�.
    private void OnTriggerEnter(Collider other)
    {
        Player player = GetComponent<Player>();
        // �����ۿ� ������ ĳ���Ͱ� �ڽ��� �ƴϸ� �������� �����ϸ� �ȵ�.
        if (!player.photonView.IsMine) return;        

        bool isSave = false;
        if (other.CompareTag("Item"))
        {            
            FieldItem fieldItem = other.GetComponent<FieldItem>();
            // �ʵ� �������� �κ��丮�� ����. �κ��丮�� ���� á���� ���� �� ����.
            if (AddItem(fieldItem.item))
            {
                fieldItem.DestroyItem();
                isSave = true;
            }
        }
        else if (other.CompareTag("Gold"))
        {
            FieldGold fieldGold = other.GetComponent<FieldGold>();
            JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold += fieldGold.ammount;            
            AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Get_Gold);
            Destroy(fieldGold.gameObject);
            isSave = true;
        }
        // �������� ȹ���� ������ ������ ����.
        if (isSave)
        {            
            player.SaveData();
            JY_CharacterListManager.s_instance.Save();
        }
    }
}
