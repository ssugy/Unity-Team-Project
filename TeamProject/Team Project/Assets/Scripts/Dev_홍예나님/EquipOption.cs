using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class EquipOption
{
    private EquipType part;
    private Dictionary<EquipAttrib, float> options = new Dictionary<EquipAttrib, float>();
    private List<EquipAttrib> optionList = new List<EquipAttrib>();
    private const int MAX_OPTIONS = 3;
    public enum EquipType
    {
        EquipTypeWeapon,
        EquipTypeArmor,
        EquipTypeShield
    }

    public enum EquipAttrib
    {
        // 무기 속성
        AttribAtkSpeed,         // 공속 증가 (5퍼센트 ~ 20퍼센트사이 랜덤)
        AttribAtkPower,         // 공격력 추가 (1에서부터 ~ 최대 40)
        AtrribAtkCritical,      // 치명타 확률 증가 (1~40퍼센트) +연산입니다.
        AtrribAtkHP,            // 체력 1 ~ 20까지 증가
        AtrribAtkStrength,      // 근력 1 ~ 20까지 증가
        AtrribAtkStamina,       // 스태미나 1 ~ 20까지 증가
        AtrribAtkDex,           // 민첩 1 ~ 20까지 증가

        // 옷 속성
        AttribArmorDef,         // 방어력 증가 (1에서부터 ~ 최대 40)
        AttribArmorHPMax,       // 체력 최대치 1 ~ 20까지 증가
        AttribArmorStaminaMax,  // 스태미나 최대치 1 ~ 20까지 증가
        AttribArmorHP,          // 체력 1 ~ 20까지 증가
        AtrribArmorStrength,    // 근력 1 ~ 20까지 증가
        AttribArmorStamina,     //스태미나 1 ~ 20까지 증가
        AtrribArmorDex,         // 민첩 1 ~ 20까지 증가
        AtrribArmorRecover,     // 생명력 물약이 회복시키는 회복량 100~500%증가
        AtrribArmorAvoid,       // 회피 확률 %증가 (1~40프로)

        //방패 속성
        AttribShieldDef,        // 방어시 추가로 데미지 1~20% 감소
        
        AttribNone
    }
    // 무기용 속성표
    private EquipAttrib[] weaponOptions = 
    {
        EquipAttrib.AttribAtkSpeed,
        EquipAttrib.AttribAtkPower,
        EquipAttrib.AtrribAtkCritical,      
        EquipAttrib.AtrribAtkHP,            
        EquipAttrib.AtrribAtkStrength,      
        EquipAttrib.AtrribAtkStamina,       
        EquipAttrib.AtrribAtkDex
    };
    private EquipAttrib[] clothOptions =
    {
        EquipAttrib.AttribArmorDef,
        EquipAttrib.AttribArmorHPMax,
        EquipAttrib.AttribArmorStaminaMax,
        EquipAttrib.AttribArmorHP,
        EquipAttrib.AtrribArmorStrength,
        EquipAttrib.AttribArmorStamina,
        EquipAttrib.AtrribArmorDex,
        EquipAttrib.AtrribArmorRecover,
        EquipAttrib.AtrribArmorAvoid
    };
    private EquipAttrib[] shieldOptions =
    {
        // 옷과 동일
        EquipAttrib.AttribArmorDef,
        EquipAttrib.AttribArmorHPMax,
        EquipAttrib.AttribArmorStaminaMax,
        EquipAttrib.AttribArmorHP,
        EquipAttrib.AtrribArmorStrength,
        EquipAttrib.AttribArmorStamina,
        EquipAttrib.AtrribArmorDex,
        EquipAttrib.AtrribArmorRecover,
        EquipAttrib.AtrribArmorAvoid,
        // 방패 추가 사양
        EquipAttrib.AttribShieldDef
    };
    public EquipOption(EquipType _part)
    {
        this.part = _part;
        switch(_part)
        {
            case EquipType.EquipTypeWeapon:
                EquipOptionWeapon();
                break;
            case EquipType.EquipTypeArmor:
                EquipOptionCloth();
                break;
            case EquipType.EquipTypeShield:
                EquipOptionShield();
                break;
        }
    }

    private void MakeList()
    {
        optionList = new List<EquipAttrib>();
        foreach (KeyValuePair<EquipAttrib, float> entry in options)
        {
            optionList.Add(entry.Key);
        }
    }

    private void EquipOptionWeapon()
    {
        int num = Random.Range(1, MAX_OPTIONS+1);
        for (int i = 0; i < num; i++)
        {
            int idx = Random.Range(0,weaponOptions.Length);
            float value = Random.Range(5f, 20f);
            if (!options.ContainsKey(weaponOptions[idx]))
            {
                options.Add(weaponOptions[idx], value);
            }
        }
        MakeList();
    }

    private void EquipOptionCloth()
    {
        int num = Random.Range(1, MAX_OPTIONS + 1);
        for (int i = 0; i < num; i++)
        {
            int idx = Random.Range(0, clothOptions.Length);
            float value = Random.Range(5f, 20f);
            if (!options.ContainsKey(clothOptions[idx]))
            {
                options.Add(clothOptions[idx], value);
            }
        }
        MakeList();
    }

    private void EquipOptionShield()
    {
        int num = Random.Range(1, MAX_OPTIONS + 1);
        for (int i = 0; i < num; i++)
        {
            int idx = Random.Range(0, shieldOptions.Length);
            float value = Random.Range(5f, 20f);
            if (!options.ContainsKey(shieldOptions[idx]))
            {
                options.Add(shieldOptions[idx], value);
            }
        }
        MakeList();
    }
    public bool GetOptionValue(int idx, out EquipAttrib attrib, out float value)
    {
        if (idx < optionList.Count)
        {
            attrib = optionList[idx];
            value = options[attrib];
            return true;
        }
        else
        {
            attrib = EquipAttrib.AttribNone;
            value = 0;
        }
        return false;
    }
}
