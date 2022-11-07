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
        _item.equipedState = EquipState.EQUIPED;

        Player player = Inventory.instance.transform.GetComponent<Player>();

        if (player.playerStat.equiped.TryGetValue(EquipPart.LEG, out Item _tmp))
        {
            _tmp.effects[1].ExecuteRole(_tmp);
            player.playerStat.equiped.Add(EquipPart.LEG, _item);
        }
        else
        {
            player.playerStat.equiped.Add(EquipPart.LEG, _item);
        }

        if (Player.instance.enabled)
        {
            player.playerStat.customized[0] = num;
            player.AvatarSet();
            player.playerStat.defPoint += def;
            player.SetState();
        }        

        if (Inventory.instance.onChangeItem != null)
        {
            Inventory.instance.onChangeItem();
        }                        
    }
}
