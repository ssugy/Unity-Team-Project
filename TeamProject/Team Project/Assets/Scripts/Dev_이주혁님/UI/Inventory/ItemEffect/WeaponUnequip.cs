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
        while (Player.instance.rWeaponDummy.GetComponentInChildren<Weapon>() != null)
        {
            DestroyImmediate(Player.instance.rWeaponDummy.GetComponentInChildren<Weapon>().gameObject);
        }
        if (Inventory.instance.onChangeItem != null)
            Inventory.instance.onChangeItem();
        if (InventoryUI.instance != null)
        {
            InventoryUI.instance.weaponIcon.sprite = null;
            InventoryUI.instance.weaponIcon.gameObject.SetActive(false);
        }
    }    
}
