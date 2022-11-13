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
        // ���� �Ӽ�
        AttribAtkSpeed,         // ���� ���� (5�ۼ�Ʈ ~ 20�ۼ�Ʈ���� ����)
        AttribAtkPower,         // ���ݷ� �߰� (1�������� ~ �ִ� 40)
        AtrribAtkCritical,      // ġ��Ÿ Ȯ�� ���� (1~40�ۼ�Ʈ) +�����Դϴ�.
        AtrribAtkHP,            // ü�� 1 ~ 20���� ����
        AtrribAtkStrength,      // �ٷ� 1 ~ 20���� ����
        AtrribAtkStamina,       // ���¹̳� 1 ~ 20���� ����
        AtrribAtkDex,           // ��ø 1 ~ 20���� ����

        // �� �Ӽ�
        AttribArmorDef,         // ���� ���� (1�������� ~ �ִ� 40)
        AttribArmorHPMax,       // ü�� �ִ�ġ 1 ~ 20���� ����
        AttribArmorStaminaMax,  // ���¹̳� �ִ�ġ 1 ~ 20���� ����
        AttribArmorHP,          // ü�� 1 ~ 20���� ����
        AtrribArmorStrength,    // �ٷ� 1 ~ 20���� ����
        AttribArmorStamina,     //���¹̳� 1 ~ 20���� ����
        AtrribArmorDex,         // ��ø 1 ~ 20���� ����
        AtrribArmorRecover,     // ����� ������ ȸ����Ű�� ȸ���� 100~500%����
        AtrribArmorAvoid,       // ȸ�� Ȯ�� %���� (1~40����)

        //���� �Ӽ�
        AttribShieldDef,        // ���� �߰��� ������ 1~20% ����
        
        AttribNone
    }
    // ����� �Ӽ�ǥ
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
        // �ʰ� ����
        EquipAttrib.AttribArmorDef,
        EquipAttrib.AttribArmorHPMax,
        EquipAttrib.AttribArmorStaminaMax,
        EquipAttrib.AttribArmorHP,
        EquipAttrib.AtrribArmorStrength,
        EquipAttrib.AttribArmorStamina,
        EquipAttrib.AtrribArmorDex,
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
