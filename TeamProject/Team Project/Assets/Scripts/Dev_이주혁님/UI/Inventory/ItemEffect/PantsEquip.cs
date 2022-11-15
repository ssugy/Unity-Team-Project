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
        Player player = JY_CharacterListManager.s_instance.playerList[0];

        if (player != null)
        {
            if (player.playerStat.equiped.TryGetValue(EquipPart.LEG, out Item _tmp))
            {
                _tmp.effects[1].ExecuteRole(_tmp);
                player.playerStat.equiped.Add(EquipPart.LEG, _item);
            }
            else
            {
                player.playerStat.equiped.Add(EquipPart.LEG, _item);
            }
            _item.equipedState = EquipState.EQUIPED;
            player.playerStat.customized[0] = num;
            player.AvatarSet();
            player.playerStat.defPoint += def;
            player.SetState();
        }             

        if (JY_CharacterListManager.s_instance.invenList[0].onChangeItem != null)
        {
            JY_CharacterListManager.s_instance.invenList[0].onChangeItem();
        }
        if (InventoryUI.instance != null)
        {
            InventoryUI.instance.legIcon.sprite = _item.image;
            InventoryUI.instance.legIcon.gameObject.SetActive(true);
        }
    }
    public override int GetType()
    {
        return (int)EquipOption.EquipType.EquipTypeArmor;
    }
}
