using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/WeaponEquip")]
public class WeaponEquip : ItemEffect
{
    public override void ExecuteRole(Item _item)
    {
        GameObject weaponSrc = Resources.Load<GameObject>("Item/Weapon/" + _item.image.name);
        Instantiate<GameObject>(weaponSrc, Player.instance.rWeaponDummy);
        Player.instance.playerStat.equiped.Add(EquipPart.WEAPON, _item);
        Inventory.instance.onChangeItem();
        InventoryUI.instance.weaponIcon.sprite = _item.image;
        InventoryUI.instance.weaponIcon.gameObject.SetActive(true);        
    }
}
