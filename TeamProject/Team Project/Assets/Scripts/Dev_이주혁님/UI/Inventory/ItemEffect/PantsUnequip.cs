using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/PantsUnequip")]
public class PantsUnequip : ItemEffect
{
    public int def;
    public override void ExecuteRole(Item _item)
    {
        Player player = Inventory.instance.transform.GetComponent<Player>();
        player.playerStat.defPoint -= def;

        if (Player.instance.enabled)
        {
            player.playerStat.customized[0] = 0;
            player.AvatarSet();
        }

        if (Player.instance.playerStat.equiped.TryGetValue(EquipPart.LEG, out Item _val))
        {
            _val.equipedState = EquipState.UNEQUIPED;
            Player.instance.playerStat.equiped.Remove(EquipPart.LEG);
        }
        if (Inventory.instance.onChangeItem != null)
        {
            Inventory.instance.onChangeItem();
        }
    }    
}
