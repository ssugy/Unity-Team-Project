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
        Player player = JY_CharacterListManager.s_instance.playerList[0];

        if (player != null)
        {
            if (player.playerStat.equiped.TryGetValue(EquipPart.CHEST, out Item _tmp))
            {
                _tmp.effects[1].ExecuteRole(_tmp);
                player.playerStat.equiped.Add(EquipPart.CHEST, _item);
            }
            else
            {
                player.playerStat.equiped.Add(EquipPart.CHEST, _item);
            }
            _item.equipedState = EquipState.EQUIPED;
            player.playerStat.customized[1] = num;
            player.AvatarSet();
            player.playerStat.defPoint += def;

            ApplyOptions(player, _item);
            player.SetState();
        }        
                               
        
        if (JY_CharacterListManager.s_instance.invenList[0].onChangeItem != null)
        {
            JY_CharacterListManager.s_instance.invenList[0].onChangeItem();
        }        
    }
    public override int GetType()
    {
        return (int)EquipOption.EquipType.EquipTypeArmor;
    }
    private void ApplyOptions(Player player, Item _item)
    {
        float value;

        if (_item.option != null && _item.option.optionList != null)
        {
            foreach (var e in _item.option.optionList)
            {
                switch (e)
                {
                    // 방어력 증가
                    case EquipOption.EquipAttrib.AttribArmorDef:
                        value = _item.option.options[e];
                        player.playerStat.addedDefPoint += (int)value;
                        break;
                    // 최대 체력(Max HP) 증가
                    case EquipOption.EquipAttrib.AttribArmorHP:
                        value = _item.option.options[e];
                        player.playerStat.addedHP += (int)value;
                        break;
                    // 최대 스태미나(Max SP) 증가
                    case EquipOption.EquipAttrib.AttribArmorSP:
                        value = _item.option.options[e];
                        player.playerStat.addedSP += (int)value;
                        break;
                    // 체력(Health) 증가
                    case EquipOption.EquipAttrib.AttribArmorHealth:
                        value = _item.option.options[e];
                        player.playerStat.AddedHealth += (int)value;
                        break;
                    // 근력 증가
                    case EquipOption.EquipAttrib.AtrribArmorStrength:
                        value = _item.option.options[e];
                        player.playerStat.AddedStrength += (int)value;
                        break;
                    // 스태미나 증가
                    case EquipOption.EquipAttrib.AttribArmorStamina:
                        value = _item.option.options[e];
                        player.playerStat.AddedStamina += (int)value;
                        break;
                    // 민첩 증가
                    case EquipOption.EquipAttrib.AtrribArmorDexterity:
                        value = _item.option.options[e];
                        player.playerStat.AddedDexterity += (int)value;
                        break;
                    // 회피 확률 증가
                    case EquipOption.EquipAttrib.AtrribArmorAvoid:
                        value = _item.option.options[e];
                        player.playerStat.addedAvoid += (int)value;
                        break;
                    // 회복약 효율 증가
                    case EquipOption.EquipAttrib.AtrribArmorRecover:
                        value = _item.option.options[e];
                        player.playerStat.addedRecover += (int)value;
                        break;
                }                
            }
        }
    }
}
