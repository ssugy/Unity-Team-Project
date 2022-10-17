using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/WeaponUnequip")]
public class WeaponUnequip : ItemEffect
{
    public override void ExecuteRole(Item _item)
    {
        if (Player.instance.playerStat.equiped.TryGetValue(EquipPart.WEAPON, out Item _val))
        {
            _val.equipedState = EquipState.UNEQUIPED;
            Player.instance.playerStat.equiped.Remove(EquipPart.WEAPON);
        }
        if (Player.instance.rWeaponDummy.GetComponentInChildren<Weapon>() != null)
        {
            Destroy(Player.instance.rWeaponDummy.GetComponentInChildren<Weapon>().gameObject);
        }        
        Inventory.instance.onChangeItem();
        InventoryUI.instance.weaponIcon.sprite = null;
        InventoryUI.instance.weaponIcon.gameObject.SetActive(false);
    }    
}
