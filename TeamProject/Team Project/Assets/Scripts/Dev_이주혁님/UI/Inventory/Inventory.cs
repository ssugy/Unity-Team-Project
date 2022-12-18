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

    // �������� �κ��丮�� �߰��ϴ� �ڵ�. �κ��丮�� ���� á�ٸ� ������ ȹ�� �Ұ�.
    // �������� �߰��ϴ� ���� �����ߴٸ� true ��ȯ �� �κ��丮 UI�� ������.
    public bool AddItem(Item _item, bool _setOption = true)
    {        
        // �߰��Ǵ� �������� �̹� �κ��丮�� �����ϴ� �Һ�, ��� �������� ���. (�Һ�/��� �������� ������)
        if ((int)_item.type >= 2)
        {
            //���� ������ �������� �ִ��� �˻�     
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
            // �ش� �������� �߰� �ɼ��� �����Ѵ�. ���Ϳ��� ����ǰų�, ���۵� ��� �����ۿ��� �߰� �ɼ� ����.
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
