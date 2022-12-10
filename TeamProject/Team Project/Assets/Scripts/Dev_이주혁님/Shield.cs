using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public int level;                       // 방패 장착 가능 유저 레벨
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
        if (player == null)
            return;

        if (_item != null && _item.option != null && _item.option.optionList != null)
        {
            float value;            
            foreach (var e in _item.option.optionList)
            {
                switch (e)
                {
                    // 방어력 증가
                    case EquipOption.EquipAttrib.AttribArmorDef:
                        value = _item.option.options[e];
                        player.playerStat.addedDefPoint += (int)value;
                        break;
                    // HP 증가
                    case EquipOption.EquipAttrib.AttribArmorHP:
                        value = _item.option.options[e];
                        player.playerStat.addedHP += (int)value;
                        break;
                    // SP 증가
                    case EquipOption.EquipAttrib.AttribArmorSP:
                        value = _item.option.options[e];
                        player.playerStat.addedSP += (int)value;
                        break;
                    // 체력 증가
                    case EquipOption.EquipAttrib.AttribArmorHealth:
                        value = _item.option.options[e];
                        player.playerStat.AddedHealth += (int)value;
                        break;
                    // 근력 증가
                    case EquipOption.EquipAttrib.AtrribArmorStrength:
                        value = _item.option.options[e];
                        player.playerStat.AddedStrength += (int)value;
                        break;
                    // 지구력 증가
                    case EquipOption.EquipAttrib.AttribArmorStamina:
                        value = _item.option.options[e];
                        player.playerStat.AddedStamina += (int)value;
                        break;
                    // 민첩 증가
                    case EquipOption.EquipAttrib.AtrribArmorDexterity:
                        value = _item.option.options[e];
                        player.playerStat.AddedDexterity += (int)value;
                        break;
                    // 물약 효율 증가
                    case EquipOption.EquipAttrib.AtrribArmorRecover:
                        value = _item.option.options[e];
                        player.playerStat.addedRecover += (int)value;
                        break;
                    // 회피율 증가
                    case EquipOption.EquipAttrib.AtrribArmorAvoid:
                        value = _item.option.options[e];
                        player.playerStat.addedAvoid += (int)value;
                        break;
                    // 방어 시 데미지 추가 감소 (1~20퍼센트 사이)
                    case EquipOption.EquipAttrib.AttribShieldDef:
                        value = _item.option.options[e];
                        defPro += value / 100.0f;
                        break;
                }
                
            }            
            player.SetState();
        }
    }
    public void ReturnOptions(Item _item)
    {
        if (player == null)
            return;

        if (_item != null && _item.option != null && _item.option.optionList != null)
        {
            float value;
            foreach (var e in _item.option.optionList)
            {
                switch (e)
                {                    
                    case EquipOption.EquipAttrib.AttribArmorDef:
                        value = _item.option.options[e];
                        player.playerStat.addedDefPoint -= (int)value;
                        break;
                    // HP 증가
                    case EquipOption.EquipAttrib.AttribArmorHP:
                        value = _item.option.options[e];
                        player.playerStat.addedHP -= (int)value;
                        break;
                    // SP 증가
                    case EquipOption.EquipAttrib.AttribArmorSP:
                        value = _item.option.options[e];
                        player.playerStat.addedSP -= (int)value;
                        break;
                    // 체력 증가
                    case EquipOption.EquipAttrib.AttribArmorHealth:
                        value = _item.option.options[e];
                        player.playerStat.AddedHealth -= (int)value;
                        break;
                    // 근력 증가
                    case EquipOption.EquipAttrib.AtrribArmorStrength:
                        value = _item.option.options[e];
                        player.playerStat.AddedStrength -= (int)value;
                        break;
                    // 지구력 증가
                    case EquipOption.EquipAttrib.AttribArmorStamina:
                        value = _item.option.options[e];
                        player.playerStat.AddedStamina -= (int)value;
                        break;
                    // 민첩 증가
                    case EquipOption.EquipAttrib.AtrribArmorDexterity:
                        value = _item.option.options[e];
                        player.playerStat.AddedDexterity -= (int)value;
                        break;
                    // 물약 효율 증가
                    case EquipOption.EquipAttrib.AtrribArmorRecover:
                        value = _item.option.options[e];
                        player.playerStat.addedRecover -= (int)value;
                        break;
                    // 회피율 증가
                    case EquipOption.EquipAttrib.AtrribArmorAvoid:
                        value = _item.option.options[e];
                        player.playerStat.addedAvoid -= (int)value;
                        break;                    
                }

            }
            player.SetState();
        }
    }
}
