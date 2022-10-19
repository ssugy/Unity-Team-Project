using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/ShieldEquip")]
public class ShieldEquip : ItemEffect
{
    public override void ExecuteRole(Item _item)
    {
        Player player = Inventory.instance.transform.GetComponent<Player>();
        GameObject shieldSrc = Resources.Load<GameObject>("Item/Weapon/" + _item.image.name);
        Instantiate<GameObject>(shieldSrc, player.lWeaponDummy);

        if (!player.playerStat.equiped.TryGetValue(EquipPart.SHIELD, out Item _tmp))
        {
            player.playerStat.equiped.Add(EquipPart.SHIELD, _item);
        }
        
        if (Inventory.instance.onChangeItem != null)
        {
            Inventory.instance.onChangeItem();
        }
        if (InventoryUI.instance != null)
        {
            InventoryUI.instance.shieldIcon.sprite = _item.image;
            InventoryUI.instance.shieldIcon.gameObject.SetActive(true);
        }                       
    }
}
