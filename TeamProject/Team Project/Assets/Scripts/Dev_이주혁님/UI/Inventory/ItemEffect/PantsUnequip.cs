using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/PantsUnequip")]
public class PantsUnequip : ItemEffect
{
    public int def;
    public override void ExecuteRole(Item _item)
    {
        if (Player.instance.playerStat.equiped.TryGetValue(EquipPart.LEG, out Item _val))
        {
            _val.equipedState = EquipState.UNEQUIPED;
            Player.instance.playerStat.equiped.Remove(EquipPart.LEG);
        }
        if (Player.instance.enabled)
        {
            Player _player = Player.instance;
            _player.playerStat.customized[0] = 0;
            _player.AvatarSet();
            _player.playerStat.defPoint -= def;            
            _player.SetState();
        }

        if (Inventory.instance.onChangeItem != null)
            Inventory.instance.onChangeItem();
        if (InventoryUI.instance != null)
        {
            InventoryUI.instance.legIcon.sprite = null;
            InventoryUI.instance.legIcon.gameObject.SetActive(false);
        }
    }    
}
