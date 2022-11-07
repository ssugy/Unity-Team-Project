using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/TopUnequip")]
public class TopUnequip : ItemEffect
{
    public int def;
    public override void ExecuteRole(Item _item)
    {        
        if (Player.instance.playerStat.equiped.TryGetValue(EquipPart.CHEST, out Item _val))
        {
            _val.equipedState = EquipState.UNEQUIPED;
            Player.instance.playerStat.equiped.Remove(EquipPart.CHEST);
        }
        if (Player.instance.enabled)
        {
            Player _player = Player.instance;
            _player.playerStat.customized[1] = 0;
            _player.AvatarSet();
            _player.playerStat.defPoint -= def;            
            _player.SetState();
        }

        
        if (Inventory.instance.onChangeItem != null)
        {
            Inventory.instance.onChangeItem();
        }
    }    
}
