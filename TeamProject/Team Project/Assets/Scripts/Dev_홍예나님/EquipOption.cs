using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

[System.Serializable]
public class EquipOption
{   
    public EquipType part;
    public EquipTier[] tiers = new EquipTier[3];
    public Dictionary<EquipAttrib, float> options = new();
    public List<EquipAttrib> optionList = new();
    public List<float> valueList = new();    

    // ����� Ÿ��. ����/��/���з� ����.
    public enum EquipType
    {
        EquipTypeWeapon,
        EquipTypeArmor,
        EquipTypeShield
    }
    // ��� �ɼ��� Ƽ��. ���ڰ� ���� ���� ���.
    public enum EquipTier
    {
        Nihil,
        Unus,   // Ƽ�� 1
        Duo,        // Ƽ�� 2
        Tres        // Ƽ�� 3
    }

    [System.Serializable]
    public enum EquipAttrib
    {
        None,

        // Random.Range(1, 9);
        AtkSpeed,         // ���� ���� (5�ۼ�Ʈ ~ 40�ۼ�Ʈ���� ����)
        AtkPoint,         // ���ݷ� �߰� (10�������� ~ �ִ� 80)

        // Random.Range(1, 7);
        CriPro,           // ġ��Ÿ Ȯ�� ���� (5~30�ۼ�Ʈ)
        DefPoint,         // ���� ���� (5 ~ 30)

        // Random.Range(1, 11);
        Health,        // ü�� 1 ~ 10���� ����
        Stamina,       // ������ 1 ~ 10���� ����
        Strength,      // �ٷ� 1 ~ 10���� ����        
        Dexterity,     // ��ø 1 ~ 10���� ����
        HP,            // HP 50 ~ 500���� ����
        PotionRecover, // ����� ������ ȸ����Ű�� ȸ���� 10~100% ����        

        // Random.Range(1, 5);
        SP,            // SP 5 ~ 20���� ����               
        AvoidPro,      // ȸ�� Ȯ�� %���� (5~20����)
        ShieldDef      // ���� �߰��� ������ 5~20% ����        
    }
    
    private EquipAttrib[] weaponOptions = 
    {
        EquipAttrib.AtkSpeed,
        EquipAttrib.AtkPoint,

        EquipAttrib.CriPro,    
        
        EquipAttrib.Health,            
        EquipAttrib.Strength,      
        EquipAttrib.Stamina,       
        EquipAttrib.Dexterity
    };
    private EquipAttrib[] armourOptions =
    {
        EquipAttrib.DefPoint,        
        
        EquipAttrib.Health,
        EquipAttrib.Strength,
        EquipAttrib.Stamina,
        EquipAttrib.Dexterity,
        EquipAttrib.HP,
        EquipAttrib.PotionRecover,

        EquipAttrib.SP,        
        EquipAttrib.AvoidPro
    };
    private EquipAttrib[] shieldOptions =
    {
        // �ʰ� ����
        EquipAttrib.DefPoint,
        
        EquipAttrib.Health,
        EquipAttrib.Strength,
        EquipAttrib.Stamina,
        EquipAttrib.Dexterity,
        EquipAttrib.HP,
        EquipAttrib.PotionRecover,

        EquipAttrib.SP,        
        EquipAttrib.AvoidPro,        
        EquipAttrib.ShieldDef
    };
    public EquipOption(EquipType _part)
    {
        this.part = _part;
        SetOption(_part);        
    }

    private void MakeList()
    {
        optionList = new List<EquipAttrib>();
        valueList = new();
        foreach (KeyValuePair<EquipAttrib, float> entry in options)
        {
            optionList.Add(entry.Key);
            valueList.Add(entry.Value);
        }
    }

    public void MakeDictionary()
    {
        options = new();
        if (optionList != null && valueList != null)
            for(int i = 0; i < optionList.Count; i++)
            {
                options.Add(optionList[i], valueList[i]);
            }
    }

    private void SetOption(EquipType _part)
    {
        // �Ű������� �Էµ� ��� ������ ���� � �ɼ� ����Ʈ�� ����� ������ ����.
        EquipAttrib[] _options;        
        switch (_part)
        {
            case EquipType.EquipTypeWeapon:
                {
                    _options = weaponOptions;
                    break;
                }                
            case EquipType.EquipTypeArmor:
                {
                    _options = armourOptions;
                    break;
                }
            case EquipType.EquipTypeShield:
                {
                    _options = shieldOptions;
                    break;
                }
            default:
                {
                    Debug.Log("�߸��� ���� �Է�");
                    return;
                }                
        }

        // ��� �ٴ� �ɼ��� ����. 0 ~ 3�� ���� ����.
        int num;
        switch (Random.Range(1, 9))
        {
            case 8:
                {
                    num = 3;
                    break;
                }
            case 5:
            case 6:
            case 7:
                {
                    num = 2;
                    break;
                }
            case 2:
            case 3:
            case 4:
                {
                    num = 1;
                    break;
                }
            default:
                {
                    num = 0;
                    break;
                }
        }

        for (int i = 0; i < num; i++)
        {
            // �ɼ� ��ġ�� Ƽ� ����. Ƽ��3: 10%, Ƽ��2: 40%, Ƽ��1: 50%
            
            switch(Random.Range(0, 10))
            {
                case int n when (n < 1):
                    {
                        tiers[i] = EquipTier.Tres;
                        break;
                    }
                case int n when (n < 5):
                    {
                        tiers[i] = EquipTier.Duo;
                        break;
                    }
                default:
                    {
                        tiers[i] = EquipTier.Unus;
                        break;
                    }
            }

            // � �ɼ��� ���� ������ ����. �� �����ۿ� �ߺ��� �ɼ��� ���� �� ����.
            // �ߺ��� �ɼ��� �ִٸ� �ٽ� ����.
            int idx = Random.Range(0, _options.Length);            
            while (options.ContainsKey(_options[idx]))
            {
                idx = Random.Range(0, _options.Length);
            }

            // �ɼ� ������ ���� ���� ����.
            float value;          // �ɼ� ��ġ.
            switch ((int)_options[idx])
            {
                case 1:
                case 2:
                    {
                        value = 7f;
                        break;
                    }
                case 3:
                case 4:
                    {
                        value = 5f;
                        break;
                    }
                case int n when (n >= 5 && n <= 10):                
                    {
                        value = 9f;
                        break;
                    }
                case int n when (n >= 11):
                    {
                        value = 3f;
                        break;
                    }
                default:
                    {
                        Debug.Log("�߸��� �ε��� �Է�");
                        return;
                    }
            }

            // Ƽ� ���� �ɼ� ��ġ�� ����.
            // Ƽ��3: 81~100%, Ƽ��2: 31~80%, Ƽ��1: 0~30%
            switch (tiers[i])
            {
                case EquipTier.Unus:
                    {
                        value *= Random.Range(0f, 0.3f);
                        break;
                    }
                case EquipTier.Duo:
                    {
                        value *= Random.Range(0.31f, 0.8f);                        
                        break;
                    }
                case EquipTier.Tres:
                    {
                        value *= Random.Range(0.81f, 1f);                        
                        break;
                    }
                default:
                    {
                        Debug.Log("�߸��� Ƽ�� �Է�");
                        return;
                    }
            }

            // 1�� �⺻��. Random.Range�� 0�̸� value�� 1�� ��.
            value += 1f;
            value *= 5f;

            // ���ݷ°� ���� ȿ�� �ɼ��� ��쿡�� �ٸ� �ɼǺ��� ��ġ�� 2�� ŭ.
            if ((int)_options[idx] == 2 || (int)_options[idx] == 10)
                value *= 2f;
            // HP �ɼ��� ��쿡�� �ٸ� �ɼǺ��� ��ġ�� 10�� ŭ.
            else if ((int)_options[idx] == 9)
                value *= 10f;
            // ���� �ɼ��� ��쿡�� �ٸ� �ɼǺ��� ��ġ�� 5�� ����.
            else if ((int)_options[idx] >= 5 && (int)_options[idx] <= 8)
                value /= 5f;

            options.Add(_options[idx], value);
        }
        MakeList();
    }

    public bool GetOptionValue(int idx, out EquipAttrib attrib, out float value)
    {
        if (optionList == null || idx >= optionList.Count)
        {
            attrib = EquipAttrib.None;
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
