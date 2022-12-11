using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/StaffUnequip")]
public class StaffUnequip : ItemEffect
{
    public override void ExecuteRole(Item _item)
    {        
        if (JY_CharacterListManager.s_instance.playerList[0].playerStat.equiped.TryGetValue(EquipPart.SHIELD, out Item _val))
        {
            _val.equipedState = EquipState.UNEQUIPED;
            JY_CharacterListManager.s_instance.playerList[0].playerStat.equiped.Remove(EquipPart.SHIELD);
        }

        Staff staffComp;
        if ((staffComp = JY_CharacterListManager.s_instance.playerList[0].lWeaponDummy.GetComponentInChildren<Staff>()) != null) 
        {
            staffComp.ReturnOptions(_item);
            DestroyImmediate(JY_CharacterListManager.s_instance.playerList[0].lWeaponDummy.GetComponentInChildren<Staff>().gameObject);
        }        

        if (JY_CharacterListManager.s_instance.invenList[0].onChangeItem != null)
            JY_CharacterListManager.s_instance.invenList[0].onChangeItem();              
    }
}
