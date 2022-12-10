using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/ShieldUnequip")]
public class ShieldUnequip : ItemEffect
{
    public override void ExecuteRole(Item _item)
    {        
        if (JY_CharacterListManager.s_instance.playerList[0].playerStat.equiped.TryGetValue(EquipPart.SHIELD, out Item _val))
        {
            _val.equipedState = EquipState.UNEQUIPED;
            JY_CharacterListManager.s_instance.playerList[0].playerStat.equiped.Remove(EquipPart.SHIELD);
        }
        Shield shieldComp;
        if ((shieldComp = JY_CharacterListManager.s_instance.playerList[0].lWeaponDummy.GetComponentInChildren<Shield>()) != null) 
        {
            shieldComp.ReturnOptions(_item);
            DestroyImmediate(JY_CharacterListManager.s_instance.playerList[0].lWeaponDummy.GetComponentInChildren<Shield>().gameObject);
        }
        else if (JY_CharacterListManager.s_instance.playerList[0].lWeaponDummy.GetComponentInChildren<Staff>() != null)
        {
            Destroy(JY_CharacterListManager.s_instance.playerList[0].lWeaponDummy.GetComponentInChildren<Staff>().gameObject);
        }

        if (JY_CharacterListManager.s_instance.invenList[0].onChangeItem != null)
            JY_CharacterListManager.s_instance.invenList[0].onChangeItem();              
    }
}
