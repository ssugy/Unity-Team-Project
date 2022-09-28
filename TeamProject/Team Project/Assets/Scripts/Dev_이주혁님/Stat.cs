using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CharacterClass
{
    NULL,
    warrior,
    thief,
    explorer,
    sorcerer,
    enemy
}
public enum Sex
{
    NULL,
    male,
    female
}
public enum Adjustable
{
    health,
    stamina,
    strength,
    dexterity
}
[System.Serializable]
public class PlayerStat
{
    [Header("Adjustable")]
    public int health;
    public int stamina;
    public int strength;
    public int dexterity;

    [Header("Statistic")]
    public CharacterClass characterClass;
    public Sex sex;
    public int level;       
    public int[] customized;
    public List<int> equiped;
    public int Exp;
    public int curExp;    
    public int HP;
    public int curHP;
    public int SP;
    public int curSP;
    public float criPro;
    public const float criMag = 1.5f;
    public int defPoint;
    public float defMag;
    public int statPoint;
    public int atkPoint;    
    public int gold;
    public bool isDead;
    
    public void InitialStat(CharacterClass _class)  
    {
        switch (_class)
        {
            case CharacterClass.warrior:
                health = 7;
                stamina = 6;
                strength = 10;
                dexterity = 5;
                break;
            case CharacterClass.thief:
                health = 5;
                stamina = 8;
                strength = 6;
                dexterity = 9;
                break;
            case CharacterClass.explorer:
                health = 7;
                stamina = 7;
                strength = 7;
                dexterity = 7;
                break;
            case CharacterClass.sorcerer:
                health = 5;
                stamina = 9;
                strength = 5;
                dexterity = 9;
                break;           
        }
    }
    // 캐릭터 생성 시, 혹은 스탯 초기화 시 할당할 클래스별 초기스탯.
}
[System.Serializable]
public class EnemyStat
{
    public int HP;
    public int curHP;
    public float defMag;
}
