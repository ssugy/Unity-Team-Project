using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/WeaponUnequip")]
public class WeaponUnequip : ItemEffect
{
    public override void ExecuteRole(Item _item)
    {        
        if (JY_CharacterListManager.s_instance.playerList[0].playerStat.equiped.TryGetValue(EquipPart.WEAPON, out Item _val))
        {
            _val.equipedState = EquipState.UNEQUIPED;
            JY_CharacterListManager.s_instance.playerList[0].playerStat.equiped.Remove(EquipPart.WEAPON);
        }
        while (JY_CharacterListManager.s_instance.playerList[0].rWeaponDummy.GetComponentInChildren<Weapon>() != null)
        {
            DestroyImmediate(JY_CharacterListManager.s_instance.playerList[0].rWeaponDummy.GetComponentInChildren<Weapon>().gameObject);
        }
        if (JY_CharacterListManager.s_instance.invenList[0].onChangeItem != null)
            JY_CharacterListManager.s_instance.invenList[0].onChangeItem();
        if (InventoryUI.instance != null)
        {
            InventoryUI.instance.weaponIcon.sprite = null;
            InventoryUI.instance.weaponIcon.gameObject.SetActive(false);
        }
    }    
}
