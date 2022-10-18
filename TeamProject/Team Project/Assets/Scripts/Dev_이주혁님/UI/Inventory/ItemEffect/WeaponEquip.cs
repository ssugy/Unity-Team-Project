using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/WeaponEquip")]
public class WeaponEquip : ItemEffect
{
    public override void ExecuteRole(Item _item)
    {
        Player player = Inventory.instance.transform.GetComponent<Player>();
        GameObject weaponSrc = Resources.Load<GameObject>("Item/Weapon/" + _item.image.name);
        Instantiate<GameObject>(weaponSrc, player.rWeaponDummy);
        
        player.playerStat.equiped.Add(EquipPart.WEAPON, _item);
        if (Inventory.instance.onChangeItem != null)
        {
            Inventory.instance.onChangeItem();
        }   
        if (InventoryUI.instance != null)
        {
            InventoryUI.instance.weaponIcon.sprite = _item.image;
            InventoryUI.instance.weaponIcon.gameObject.SetActive(true);
        }          
    }
}
