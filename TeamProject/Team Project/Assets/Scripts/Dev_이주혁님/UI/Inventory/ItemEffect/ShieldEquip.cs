using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/ShieldEquip")]
public class ShieldEquip : ItemEffect
{
    public override void ExecuteRole(Item _item)
    {
        GameObject shieldSrc = Resources.Load<GameObject>("Item/Weapon/" + _item.image.name);
        Instantiate<GameObject>(shieldSrc, Player.instance.lWeaponDummy);
        Player.instance.playerStat.equiped.Add(EquipPart.SHIELD, _item);
        Inventory.instance.onChangeItem();
        InventoryUI.instance.shieldIcon.sprite = _item.image;
        InventoryUI.instance.shieldIcon.gameObject.SetActive(true);
    }
}
