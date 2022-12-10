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
        // ���� �Ӽ�
        AttribAtkSpeed,         // ���� ���� (5�ۼ�Ʈ ~ 20�ۼ�Ʈ���� ����)        
        AtrribAtkCriPro,        // ġ��Ÿ Ȯ�� ���� (5~20�ۼ�Ʈ) +�����Դϴ�.
        AttribAtkPoint,         // ���ݷ� �߰� (5�������� ~ �ִ� 20)
        AtrribAtkHealth,        // ü�� 5 ~ 20���� ����
        AtrribAtkStamina,       // ������ 5 ~ 20���� ����
        AtrribAtkStrength,      // �ٷ� 5 ~ 20���� ����        
        AtrribAtkDexterity,     // ��ø 5 ~ 20���� ����

        // �� �Ӽ�
        AttribArmorDef,         // ���� ���� (5 ~ 20)
        AttribArmorHP,          // HP 5 ~ 20���� ����
        AttribArmorSP,          // SP 5 ~ 20���� ����
        AttribArmorHealth,      // ü�� 5 ~ 20���� ����
        AttribArmorStamina,     // ������ 5 ~ 20���� ����
        AtrribArmorStrength,    // �ٷ� 5 ~ 20���� ����        
        AtrribArmorDexterity,   // ��ø 5 ~ 20���� ����
        AtrribArmorRecover,     // ����� ������ ȸ����Ű�� ȸ���� 5~20% ����
        AtrribArmorAvoid,       // ȸ�� Ȯ�� %���� (5~20����)

        //���� �Ӽ�
        AttribShieldDef,        // ���� �߰��� ������ 5~20% ����

        AttribNone
    }

    // ����� �Ӽ�ǥ
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
        // �ʰ� ����
        EquipAttrib.AttribArmorDef,
        EquipAttrib.AttribArmorHP,
        EquipAttrib.AttribArmorSP,
        EquipAttrib.AttribArmorHealth,
        EquipAttrib.AtrribArmorStrength,
        EquipAttrib.AttribArmorStamina,
        EquipAttrib.AtrribArmorDexterity,
        EquipAttrib.AtrribArmorRecover,
        EquipAttrib.AtrribArmorAvoid,
        // ���� �߰� ���
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
