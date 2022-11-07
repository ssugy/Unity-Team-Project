using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/TopUnequip")]
public class TopUnequip : ItemEffect
{
    public int def;
    public override void ExecuteRole(Item _item)
    {
        Player player = Inventory.instance.transform.GetComponent<Player>();
        player.playerStat.defPoint -= def;
        if (Player.instance.enabled)
        {
            player.playerStat.customized[1] = 0;
            player.AvatarSet();
        }

        if (Player.instance.playerStat.equiped.TryGetValue(EquipPart.CHEST, out Item _val))
        {
            _val.equipedState = EquipState.UNEQUIPED;
            Player.instance.playerStat.equiped.Remove(EquipPart.CHEST);
        }
        if (Inventory.instance.onChangeItem != null)
        {
            Inventory.instance.onChangeItem();
        }
    }    
}
