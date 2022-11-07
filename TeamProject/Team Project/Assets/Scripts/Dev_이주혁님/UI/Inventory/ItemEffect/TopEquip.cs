using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/TopEquip")]
public class TopEquip : ItemEffect
{
    public int def;
    public int num;
    public override void ExecuteRole(Item _item)
    {
        Player player = Inventory.instance.transform.GetComponent<Player>();        
        player.playerStat.defPoint += def;
        if (Player.instance.enabled)
        {
            player.playerStat.customized[1] = num;
            player.AvatarSet();
        }
        
        if(!player.playerStat.equiped.TryGetValue(EquipPart.CHEST, out Item _tmp))
        {
            player.playerStat.equiped.Add(EquipPart.CHEST, _item);
        } 
        
        if (Inventory.instance.onChangeItem != null)
        {
            Inventory.instance.onChangeItem();
        }                
    }
}
