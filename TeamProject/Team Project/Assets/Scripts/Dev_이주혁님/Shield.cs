using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float defPro;                    // ���� ��� �� ����� �����.    
    public Player player; 
    
    private void OnEnable()
    {        
        player = GetComponentInParent<Player>();
        if (player != null)
        {
            player.lWeapon = this;
            player.SetState();
        }
        
    }
    private void OnDisable()
    {
        if (player != null)
        {
            player.lWeapon = null;
            player.SetState();
        }
    }
    public void ApplyOptions(Item _item)
    {
        float value = 0f;

        if (_item != null && _item.option != null && _item.option.optionList != null)
        {
            //player.playerStat.CopyToTemp();
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
                    case EquipOption.EquipAttrib.AtrribAtkHP:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.tmpHealth += (int)value;
                        }
                        break;
                    // �ٷ� ����
                    case EquipOption.EquipAttrib.AtrribAtkStrength:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.tmpStrength += (int)value;
                        }
                        break;
                    // ���¹̳� ����
                    case EquipOption.EquipAttrib.AtrribAtkStamina:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.tmpStamina += (int)value;
                        }
                        break;
                    // ��ø ����
                    case EquipOption.EquipAttrib.AtrribAtkDex:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.tmpDexterity += (int)value;
                        }
                        break;
                    // ġ��Ÿ Ȯ�� ����
                    case EquipOption.EquipAttrib.AtrribAtkCritical:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.addedCriticalPro += value;
                        }
                        break;
                    // ������ �߰� ���� (1~20�ۼ�Ʈ ����)
                    case EquipOption.EquipAttrib.AttribShieldDef:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.addedShieldDef += value / 100.0f;
                        }
                        break;
                }
                // temp���� �̿��� �÷��̾� �ɷ�ġ�� ���� ���
                player.SetStateOption();
            }
        }
    }
}
