using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

[System.Serializable]
public class EquipOption
{
    public EquipType part;
    public Dictionary<EquipAttrib, float> options = new();
    public List<EquipAttrib> optionList = new();
    public List<float> valueList = new();
    private const int MAX_OPTIONS = 3;
    public enum EquipType
    {
        EquipTypeWeapon,
        EquipTypeArmor,
        EquipTypeShield
    }

    [System.Serializable]
    public enum EquipAttrib
    {
        // 무기 속성
        AttribAtkSpeed,         // 공속 증가 (5퍼센트 ~ 20퍼센트사이 랜덤)        
        AtrribAtkCriPro,        // 치명타 확률 증가 (5~20퍼센트) +연산입니다.
        AttribAtkPoint,         // 공격력 추가 (5에서부터 ~ 최대 20)
        AtrribAtkHealth,        // 체력 5 ~ 20까지 증가
        AtrribAtkStamina,       // 지구력 5 ~ 20까지 증가
        AtrribAtkStrength,      // 근력 5 ~ 20까지 증가        
        AtrribAtkDexterity,     // 민첩 5 ~ 20까지 증가

        // 옷 속성
        AttribArmorDef,         // 방어력 증가 (5 ~ 20)
        AttribArmorHP,          // HP 5 ~ 20까지 증가
        AttribArmorSP,          // SP 5 ~ 20까지 증가
        AttribArmorHealth,      // 체력 5 ~ 20까지 증가
        AttribArmorStamina,     // 지구력 5 ~ 20까지 증가
        AtrribArmorStrength,    // 근력 5 ~ 20까지 증가        
        AtrribArmorDexterity,   // 민첩 5 ~ 20까지 증가
        AtrribArmorRecover,     // 생명력 물약이 회복시키는 회복량 5~20% 증가
        AtrribArmorAvoid,       // 회피 확률 %증가 (5~20프로)

        //방패 속성
        AttribShieldDef,        // 방어시 추가로 데미지 5~20% 감소

        AttribNone
    }

    // 무기용 속성표
    private EquipAttrib[] weaponOptions = 
    {
        EquipAttrib.AttribAtkSpeed,
        EquipAttrib.AttribAtkPoint,
        EquipAttrib.AtrribAtkCriPro,      
        EquipAttrib.AtrribAtkHealth,            
        EquipAttrib.AtrribAtkStrength,      
        EquipAttrib.AtrribAtkStamina,       
        EquipAttrib.AtrribAtkDexterity
    };
    private EquipAttrib[] clothOptions =
    {
        EquipAttrib.AttribArmorDef,
        EquipAttrib.AttribArmorHP,
        EquipAttrib.AttribArmorSP,
        EquipAttrib.AttribArmorHealth,
        EquipAttrib.AtrribArmorStrength,
        EquipAttrib.AttribArmorStamina,
        EquipAttrib.AtrribArmorDexterity,
        EquipAttrib.AtrribArmorRecover,
        EquipAttrib.AtrribArmorAvoid
    };
    private EquipAttrib[] shieldOptions =
    {
        // 옷과 동일
        EquipAttrib.AttribArmorDef,
        EquipAttrib.AttribArmorHP,
        EquipAttrib.AttribArmorSP,
        EquipAttrib.AttribArmorHealth,
        EquipAttrib.AtrribArmorStrength,
        EquipAttrib.AttribArmorStamina,
        EquipAttrib.AtrribArmorDexterity,
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
        valueList = new List<float>();
        foreach (KeyValuePair<EquipAttrib, float> entry in options)
        {
            optionList.Add(entry.Key);
            valueList.Add(entry.Value);
        }
    }

    public void MakeDictionary()
    {
        options = new Dictionary<EquipAttrib, float>();
        if (optionList != null && valueList != null)
            for(int i = 0; i < optionList.Count; i++)
            {
                options.Add(optionList[i], valueList[i]);
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
        if (optionList == null || idx >= optionList.Count)
        {
            attrib = EquipAttrib.AttribNone;
            value = 0;
            return false;
        }
        else
        {
            attrib = optionList[idx];
            value = options[attrib];
            return true;
        }        
    }
}
