using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/ShieldUnequip")]
public class ShieldUnequip : ItemEffect
{
    public override void ExecuteRole(Item _item)
    {
        if (Player.instance.playerStat.equiped.TryGetValue(EquipPart.SHIELD, out Item _val))
        {
            _val.equipedState = EquipState.UNEQUIPED;
            Player.instance.playerStat.equiped.Remove(EquipPart.SHIELD);
        }
        if (Player.instance.lWeaponDummy.GetComponentInChildren<Shield>() != null)
        {
            Destroy(Player.instance.lWeaponDummy.GetComponentInChildren<Shield>().gameObject);
        }
        if (Player.instance.lWeaponDummy.GetComponentInChildren<Staff>() != null)
        {
            Destroy(Player.instance.lWeaponDummy.GetComponentInChildren<Staff>().gameObject);
        }
        if (Inventory.instance.onChangeItem != null)
            Inventory.instance.onChangeItem();
        if (InventoryUI.instance != null)
        {
            InventoryUI.instance.shieldIcon.sprite = null;
            InventoryUI.instance.shieldIcon.gameObject.SetActive(false);
        }        
    }
}
