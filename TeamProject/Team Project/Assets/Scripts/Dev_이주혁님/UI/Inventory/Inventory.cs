using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{    
    public List<Item> items;    
    public delegate void OnChangeItem();        // 아이템이 변경되면 인벤토리 UI를 갱신하는 델리게이트.
    // onChangeItem 이벤트에는 RedrawSlotUI가 할당된다. 인벤토리 UI가 열려있을 때만 실행됨.
    // 인벤토리 UI가 열려있지 않을 때는 onChangeItem이 null이 되어 실행되지 않는다. (불필요한 함수 호출 X)
    public OnChangeItem onChangeItem { get; set; }
    public int SlotCnt { get; set; } = 36;    
    
    void Start()
    {
        if (JY_CharacterListManager.s_instance.invenList.Count > 0)
        {
            // invenList[0]은 사용자 캐릭터의 인벤토리임. 다른 사용자의 캐릭터라면 실행하지 않음.
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
            // 현재 인벤토리에서 아이템을 장착함.
            items.ForEach(e =>
            {
                if (e.equipedState.Equals(EquipState.EQUIPED))
                    e.effects[0].ExecuteRole(e);
            });
        }

        if (JY_CharacterListManager.s_instance.invenList.Count > 0)
        {
            // 장비 장착이 다 끝난 다음에 HP, SP를 셋팅해줘야 씬이 시작될 때 HP, SP가 가득 찬 상태로 시작.
            JY_CharacterListManager.s_instance.playerList[0].playerStat.CurHP
                = JY_CharacterListManager.s_instance.playerList[0].playerStat.HP;
            JY_CharacterListManager.s_instance.playerList[0].playerStat.CurSP
                = JY_CharacterListManager.s_instance.playerList[0].playerStat.SP;
        }        

        if (onChangeItem != null)
            onChangeItem();
    }    

    // 아이템을 인벤토리에 추가하는 코드. 인벤토리가 가득 찼다면 아이템 획득 불가.
    // 아이템을 추가하는 데에 성공했다면 true 반환 후 인벤토리 UI를 갱신함.
    public bool AddItem(Item _item, bool _setOption = true)
    {        
        // 추가되는 아이템이 이미 인벤토리에 존재하는 소비, 재료 아이템인 경우. (소비/재료 아이템은 겹쳐짐)
        if ((int)_item.type >= 2)
        {
            //먼저 동일한 아이템이 있는지 검색     
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

        // 장비 아이템의 경우. 혹은 최초로 습득하는 재료, 소비 아이템인 경우.
        if (items.Count < SlotCnt) 
        {
            _item.itemCount = 1;            
            items.Add(_item);
            // 해당 아이템의 추가 옵션을 생성한다. 몬스터에게 드랍되거나, 제작된 장비 아이템에만 추가 옵션 지정.
            if(_setOption)
                _item.SetOption();
            if (onChangeItem != null)
                onChangeItem();            
            return true;
        }

        // 아이템 추가에 실패한 경우.
        return false;
    }

    // 아이템을 인벤토리에서 삭제하는 코드. 삭제한 후에 인벤토리 UI를 갱신함.
    public bool RemoveItem(Item _item, int _num = 1)
    {
        // 삭제하려는 개수보다 소지 아이템 개수가 적다면 삭제 불가.
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
                    Debug.Log("아이템 타입 에러");
                    return false;
                }
        }        

        if (onChangeItem != null)
            onChangeItem();
        return true;
    }

    // 필드에 있는 아이템과 골드를 줍는 코드.
    private void OnTriggerEnter(Collider other)
    {
        Player player = GetComponent<Player>();
        // 아이템에 접촉한 캐릭터가 자신이 아니면 아이템을 습득하면 안됨.
        if (!player.photonView.IsMine) return;        

        bool isSave = false;
        if (other.CompareTag("Item"))
        {            
            FieldItem fieldItem = other.GetComponent<FieldItem>();
            // 필드 아이템을 인벤토리에 넣음. 인벤토리가 가득 찼으면 얻을 수 없음.
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
        // 아이템을 획득할 때마다 데이터 저장.
        if (isSave)
        {            
            player.SaveData();
            JY_CharacterListManager.s_instance.Save();
        }
    }
}
