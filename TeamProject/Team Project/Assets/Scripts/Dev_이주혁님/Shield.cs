using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float defPro;                    // 막기 사용 시 적용될 방어율.    
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
                    // 방어력 증가
                    case EquipOption.EquipAttrib.AttribArmorDef:
                        value = _item.option.options[e];
                        player.playerStat.addedDefPoint += (int)value;
                        break;
                    // 최대 체력(Max HP) 증가
                    case EquipOption.EquipAttrib.AttribArmorHPMax:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.addedHP += (int)value;
                        }
                        break;
                    // 최대 스태미나(Max SP) 증가
                    case EquipOption.EquipAttrib.AttribArmorStaminaMax:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.addedSP += (int)value;
                        }
                        break;
                    // 체력(Health) 증가
                    case EquipOption.EquipAttrib.AtrribAtkHP:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.tmpHealth += (int)value;
                        }
                        break;
                    // 근력 증가
                    case EquipOption.EquipAttrib.AtrribAtkStrength:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.tmpStrength += (int)value;
                        }
                        break;
                    // 스태미나 증가
                    case EquipOption.EquipAttrib.AtrribAtkStamina:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.tmpStamina += (int)value;
                        }
                        break;
                    // 민첩 증가
                    case EquipOption.EquipAttrib.AtrribAtkDex:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.tmpDexterity += (int)value;
                        }
                        break;
                    // 치명타 확률 증가
                    case EquipOption.EquipAttrib.AtrribAtkCritical:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.addedCriticalPro += value;
                        }
                        break;
                    // 데미지 추가 감소 (1~20퍼센트 사이)
                    case EquipOption.EquipAttrib.AttribShieldDef:
                        value = _item.option.options[e];
                        if (player != null && player.playerStat != null)
                        {
                            player.playerStat.addedShieldDef += value / 100.0f;
                        }
                        break;
                }
                // temp값을 이용해 플레이어 능력치를 새로 계산
                player.SetStateOption();
            }
        }
    }
}
