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

    // 장비의 타입. 무기/방어구/방패로 나뉨.
    public enum EquipType
    {
        EquipTypeWeapon,
        EquipTypeArmor,
        EquipTypeShield
    }
    // 장비 옵션의 티어. 숫자가 높을 수록 상급.
    public enum EquipTier
    {
        Nihil,
        Unus,   // 티어 1
        Duo,        // 티어 2
        Tres        // 티어 3
    }

    [System.Serializable]
    public enum EquipAttrib
    {
        None,

        // Random.Range(1, 9);
        AtkSpeed,         // 공속 증가 (5퍼센트 ~ 40퍼센트사이 랜덤)
        AtkPoint,         // 공격력 추가 (10에서부터 ~ 최대 80)

        // Random.Range(1, 7);
        CriPro,           // 치명타 확률 증가 (5~30퍼센트)
        DefPoint,         // 방어력 증가 (5 ~ 30)

        // Random.Range(1, 11);
        Health,        // 체력 1 ~ 10까지 증가
        Stamina,       // 지구력 1 ~ 10까지 증가
        Strength,      // 근력 1 ~ 10까지 증가        
        Dexterity,     // 민첩 1 ~ 10까지 증가
        HP,            // HP 50 ~ 500까지 증가
        PotionRecover, // 생명력 물약이 회복시키는 회복량 10~100% 증가        

        // Random.Range(1, 5);
        SP,            // SP 5 ~ 20까지 증가               
        AvoidPro,      // 회피 확률 %증가 (5~20프로)
        ShieldDef      // 방어시 추가로 데미지 5~20% 감소        
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
        // 옷과 동일
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
        // 매개변수로 입력된 장비 부위에 따라 어떤 옵션 리스트를 사용할 것인지 결정.
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
                    Debug.Log("잘못된 부위 입력");
                    return;
                }                
        }

        // 장비에 붙는 옵션의 개수. 0 ~ 3개 사이 랜덤.
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
            // 옵션 수치의 티어를 결정. 티어3: 10%, 티어2: 40%, 티어1: 50%
            
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

            // 어떤 옵션을 붙일 것인지 결정. 한 아이템에 중복된 옵션은 붙을 수 없음.
            // 중복된 옵션이 있다면 다시 뽑음.
            int idx = Random.Range(0, _options.Length);            
            while (options.ContainsKey(_options[idx]))
            {
                idx = Random.Range(0, _options.Length);
            }

            // 옵션 종류에 따른 값을 결정.
            float value;          // 옵션 수치.
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
                        Debug.Log("잘못된 인덱스 입력");
                        return;
                    }
            }

            // 티어에 따라 옵션 수치를 보정.
            // 티어3: 81~100%, 티어2: 31~80%, 티어1: 0~30%
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
                        Debug.Log("잘못된 티어 입력");
                        return;
                    }
            }

            // 1은 기본값. Random.Range가 0이면 value는 1이 됨.
            value += 1f;
            value *= 5f;

            // 공격력과 물약 효율 옵션의 경우에는 다른 옵션보다 수치가 2배 큼.
            if ((int)_options[idx] == 2 || (int)_options[idx] == 10)
                value *= 2f;
            // HP 옵션의 경우에는 다른 옵션보다 수치가 10배 큼.
            else if ((int)_options[idx] == 9)
                value *= 10f;
            // 스탯 옵션의 경우에는 다른 옵션보다 수치가 5배 작음.
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
