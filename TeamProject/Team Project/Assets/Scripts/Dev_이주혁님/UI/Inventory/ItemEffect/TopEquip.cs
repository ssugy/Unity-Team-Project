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
        if (InventoryUI.instance != null)
        {
            InventoryUI.instance.chestIcon.sprite = _item.image;
            InventoryUI.instance.chestIcon.gameObject.SetActive(true);
        }
    }
    public override int GetType()
    {
        return (int)EquipOption.EquipType.EquipTypeArmor;
    }
    private void ApplyOptions(Player player, Item _item)
    {
        float value = 0f;

        if (_item != null && _item.option != null && _item.option.optionList != null)
        {
            foreach (var e in _item.option.optionList)
            {
                switch (e)
                {
                    // ���� ����
                    case EquipOption.EquipAttrib.AttribArmorDef:
                        value = _item.option.options[e];
                        player.playerStat.addedDefPoint += (int)value;
                        break;
                    // �ִ� ü��(Max HP) ����
                    case EquipOption.EquipAttrib.AttribArmorHPMax:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.addedHP += (int)value;
                        }
                        break;
                    // �ִ� ���¹̳�(Max SP) ����
                    case EquipOption.EquipAttrib.AttribArmorStaminaMax:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.addedSP += (int)value;
                        }
                        break;
                    // ü��(Health) ����
                    case EquipOption.EquipAttrib.AttribArmorHP:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.tmpHealth += (int)value;
                        }
                        break;
                    // �ٷ� ����
                    case EquipOption.EquipAttrib.AtrribArmorStrength:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.tmpStrength += (int)value;
                        }
                        break;
                    // ���¹̳� ����
                    case EquipOption.EquipAttrib.AttribArmorStamina:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.tmpStamina += (int)value;
                        }
                        break;
                    // ��ø ����
                    case EquipOption.EquipAttrib.AtrribArmorDex:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.tmpDexterity += (int)value;
                        }
                        break;
                    // ȸ�� Ȯ�� ����
                    case EquipOption.EquipAttrib.AtrribArmorAvoid:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.addedCriticalPro += value;
                        }
                        break;
                    // ȸ���� ȿ�� ����
                    case EquipOption.EquipAttrib.AtrribArmorRecover:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.addedRecover += (int)value;
                        }
                        break;
                }
                // temp���� �̿��� �÷��̾� �ɷ�ġ�� ���� ���
                player.SetStateOption();
            }
        }
    }
}
