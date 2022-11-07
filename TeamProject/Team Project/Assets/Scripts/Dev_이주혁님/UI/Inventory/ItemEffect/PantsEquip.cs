using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/PantsEquip")]
public class PantsEquip : ItemEffect
{
    public int def;
    public int num;
    public override void ExecuteRole(Item _item)
    {
        Player player = Inventory.instance.transform.GetComponent<Player>();
        player.playerStat.defPoint += def;
        if (Player.instance.enabled)
        {
            player.playerStat.customized[0] = num;
            player.AvatarSet();
        }

        if (!player.playerStat.equiped.TryGetValue(EquipPart.LEG, out Item _tmp))
        {
            player.playerStat.equiped.Add(EquipPart.LEG, _item);
        }

        if (Inventory.instance.onChangeItem != null)
        {
            Inventory.instance.onChangeItem();
        }                        
    }
}
