using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/PantsUnequip")]
public class PantsUnequip : ItemEffect
{
    public int def;
    public override void ExecuteRole(Item _item)
    {
        if (JY_CharacterListManager.s_instance.playerList[0].playerStat.equiped.TryGetValue(EquipPart.LEG, out Item _val))
        {
            _val.equipedState = EquipState.UNEQUIPED;
            JY_CharacterListManager.s_instance.playerList[0].playerStat.equiped.Remove(EquipPart.LEG);
        }
        if (JY_CharacterListManager.s_instance.playerList[0].enabled)
        {
            Player _player = JY_CharacterListManager.s_instance.playerList[0];
            _player.playerStat.customized[0] = 0;
            _player.AvatarSet();
            _player.playerStat.defPoint -= def;
            _player.SetState();
        }

        if (JY_CharacterListManager.s_instance.invenList[0].onChangeItem != null)
            JY_CharacterListManager.s_instance.invenList[0].onChangeItem();        
    }    
}
