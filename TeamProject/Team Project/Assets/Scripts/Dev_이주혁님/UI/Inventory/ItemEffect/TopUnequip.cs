using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/TopUnequip")]
public class TopUnequip : ItemEffect
{
    public int def;
    public override void ExecuteRole(Item _item)
    {        
        if (JY_CharacterListManager.s_instance.playerList[0].playerStat.equiped.TryGetValue(EquipPart.CHEST, out Item _val))
        {
            _val.equipedState = EquipState.UNEQUIPED;
            JY_CharacterListManager.s_instance.playerList[0].playerStat.equiped.Remove(EquipPart.CHEST);
        }
        if (JY_CharacterListManager.s_instance.playerList[0].enabled)
        {
            Player _player = JY_CharacterListManager.s_instance.playerList[0];
            _player.playerStat.customized[1] = 0;
            _player.AvatarSet();
            _player.playerStat.defPoint -= def;

            ReturnOptions(_player, _item);
            _player.SetState();
        }

        if (JY_CharacterListManager.s_instance.invenList[0].onChangeItem != null)
            JY_CharacterListManager.s_instance.invenList[0].onChangeItem();
        
    }
    private void ReturnOptions(Player player, Item _item)
    {
        float value;

        if (_item.option != null && _item.option.optionList != null)
        {
            foreach (var e in _item.option.optionList)
            {
                switch (e)
                {
                    // ���� ����
                    case EquipOption.EquipAttrib.AttribArmorDef:
                        value = _item.option.options[e];
                        player.playerStat.addedDefPoint -= (int)value;
                        break;
                    // �ִ� ü��(Max HP) ����
                    case EquipOption.EquipAttrib.AttribArmorHP:
                        value = _item.option.options[e];
                        player.playerStat.addedHP -= (int)value;
                        break;
                    // �ִ� ���¹̳�(Max SP) ����
                    case EquipOption.EquipAttrib.AttribArmorSP:
                        value = _item.option.options[e];
                        player.playerStat.addedSP -= (int)value;
                        break;
                    // ü��(Health) ����
                    case EquipOption.EquipAttrib.AttribArmorHealth:
                        value = _item.option.options[e];
                        player.playerStat.AddedHealth -= (int)value;
                        break;
                    // �ٷ� ����
                    case EquipOption.EquipAttrib.AtrribArmorStrength:
                        value = _item.option.options[e];
                        player.playerStat.AddedStrength -= (int)value;
                        break;
                    // ���¹̳� ����
                    case EquipOption.EquipAttrib.AttribArmorStamina:
                        value = _item.option.options[e];
                        player.playerStat.AddedStamina -= (int)value;
                        break;
                    // ��ø ����
                    case EquipOption.EquipAttrib.AtrribArmorDexterity:
                        value = _item.option.options[e];
                        player.playerStat.AddedDexterity -= (int)value;
                        break;
                    // ȸ�� Ȯ�� ����
                    case EquipOption.EquipAttrib.AtrribArmorAvoid:
                        value = _item.option.options[e];
                        player.playerStat.addedAvoid -= (int)value;
                        break;
                    // ȸ���� ȿ�� ����
                    case EquipOption.EquipAttrib.AtrribArmorRecover:
                        value = _item.option.options[e];
                        player.playerStat.addedRecover -= (int)value;
                        break;
                }

            }
        }
    }
}
