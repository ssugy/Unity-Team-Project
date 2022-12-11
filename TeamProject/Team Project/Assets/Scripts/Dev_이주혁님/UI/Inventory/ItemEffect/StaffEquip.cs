using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/StaffEquip")]
public class StaffEquip : ItemEffect
{
    public override void ExecuteRole(Item _item)
    {
        Player player = JY_CharacterListManager.s_instance.playerList[0];

        if (player != null)
        {            
            if (player.playerStat.equiped.TryGetValue(EquipPart.SHIELD, out Item _tmp))
            {
                _tmp.effects[1].ExecuteRole(_tmp);
                player.playerStat.equiped.Add(EquipPart.SHIELD, _item);
            }
            else
            {
                player.playerStat.equiped.Add(EquipPart.SHIELD, _item);
            }
            _item.equipedState = EquipState.EQUIPED;
            // 이미 장착한 아이템이 있는 경우 _tmp.effects[1].ExecuteRole(_tmp);에서 현재 장착한 아이템을 파괴하기 때문에, 무기/방패 인스턴스 생성은 그 다음에 해주어야 한다.
            GameObject staffSrc = Resources.Load<GameObject>("Item/Shield/" + _item.image.name);
            GameObject staff = Instantiate(staffSrc, player.lWeaponDummy);            
            Staff staffComp = staff.GetComponent<Staff>();
            if (staffComp != null)
                staffComp.ApplyOptions(_item);                      
            
            staff.name = string.Copy(staffSrc.name);
        }                

        if (JY_CharacterListManager.s_instance.invenList[0].onChangeItem != null)
        {
            JY_CharacterListManager.s_instance.invenList[0].onChangeItem();
        }                              
    }
    public override int GetType()
    {
        return (int)EquipOption.EquipType.Staff;
    }
}
