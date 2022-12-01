using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceManager : MonoBehaviour
{
    #region 싱글톤 패턴
    private static InstanceManager instance;
    public static InstanceManager s_instance { get => instance; }    
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public List<PlayerEffect> SkillEffectList;
    public List<GameObject> PlayerEffectList;
    public List<GameObject> BossSkillEffectList;
    [Header("플레이어 공격 이펙트")]
    public GameObject Normal_Attack_Effect;
    public GameObject Normal_Attack_Effect2;
    public GameObject Normal_Attack_Effect3;
    public GameObject Skill_1_Effect;
    public GameObject Skill_1_Effect2;
    public GameObject Skill_2_Effect;
    public GameObject Skill_2_Effect2;
    public GameObject Skill_2_Effect3;
    [Header("플레이어 기타 이펙트")]
    public GameObject LevelUpEffect;
    [Header("보스 스킬 이펙트")]
    public GameObject Boss_Skill_Effect;
    public GameObject Boss_Skill_Effect2;
    public GameObject Boss_Dead_Effect;


    private void Start()
    {
        SkillEffectList = new();
        PlayerEffectList = new();
        BossSkillEffectList = new();
    }

    GameObject FindEffect(string EffectName, List<GameObject> targetList)
    {
        foreach (GameObject one in targetList)
        {
            if (one.name.Equals(EffectName))
                return one;
        }
        return null;
    }

    public void ExtraEffectCreate(string EffectName)
    {
        GameObject effect = FindEffect(EffectName,PlayerEffectList);
        if (effect != null)
        {
            effect.SetActive(true);
            effect.transform.localPosition = Vector3.forward;
        }
        else
        {
            if(EffectName.Equals("LevelUpEffect"))
                effect = Instantiate<GameObject>(LevelUpEffect, JY_CharacterListManager.s_instance.playerList[0].transform);
            effect.transform.localPosition = Vector3.forward;
            effect.gameObject.name = EffectName;
            PlayerEffectList.Add(effect);
        }

    }
    public void BossEffectCreate(string EffectName, Transform boss)
    {
        GameObject effect = FindEffect(EffectName, BossSkillEffectList);
        if (effect != null)
            effect.SetActive(true);
        else
        {
            if (EffectName.Equals("Boss_Skill_Effect"))
                effect = Instantiate<GameObject>(Boss_Skill_Effect, boss);
            else if (EffectName.Equals("Boss_Skill2_Effect") || EffectName.Equals("Boss_Skill2_Effect2") || EffectName.Equals("Boss_Skill2_Effect3"))
            {
                effect = Instantiate<GameObject>(Boss_Skill_Effect2, boss);
                effect.transform.localPosition = Vector3.up;
            }
            else if (EffectName.Equals("Boss_Dead_Effect"))
            {
                effect = Instantiate<GameObject>(Boss_Dead_Effect, boss);
                effect.transform.localPosition = Vector3.forward;
            }
            effect.name = EffectName;
            BossSkillEffectList.Add(effect);
        }
    }
    public void BossEffectOff(string EffectName)
    {
        foreach (GameObject one in BossSkillEffectList)
        {
            if(one.name.Equals(EffectName))
                one.SetActive(false);
        }
    }
    public void NormalAttackEffectCreate(int _effectNum, Transform _parent)
    {        
        GameObject effect;
        switch (_effectNum)
        {
            case 0:
                effect = Instantiate(Normal_Attack_Effect, _parent);
                break;
            case 1:
                effect = Instantiate(Normal_Attack_Effect2, _parent);
                break;
            case 2:
                effect = Instantiate(Normal_Attack_Effect3, _parent);
                break;
            default:
                return;
        }        

        effect.transform.localPosition = new Vector3(0, 1f, 1f);        

        SkillEffectList.Add(effect.GetComponent<PlayerEffect>());
    }

    public void SkillEffectCreate(int _effectNum, Transform _parent)
    {        
        GameObject effect;
        switch (_effectNum)
        {
            // 스킬1 Power Strike 이펙트
            case 0:
                effect = Instantiate(Skill_1_Effect, _parent);
                effect.transform.localPosition = Vector3.forward;
                break;
            case 1:
                effect = Instantiate(Skill_1_Effect2, _parent);
                effect.transform.localPosition = new Vector3(0, 0, 2);
                break;

            // 스킬2 Turn Attack 이펙트
            case 2:
                effect = Instantiate(Skill_2_Effect, _parent);
                effect.transform.localPosition = new Vector3(0, 2, 1);
                break;
            case 3:
                effect = Instantiate(Skill_2_Effect2, _parent);
                effect.transform.localPosition = Vector3.forward;
                break;
            case 4:
                effect = Instantiate(Skill_2_Effect3, _parent);
                effect.transform.localPosition = new Vector3(0, 2, 1);
                break;
            default:
                return;
        }       
        
        SkillEffectList.Add(effect.GetComponent<PlayerEffect>());
    }
    
    public void ExtraEffectOff(string EffectName)
    {
        foreach (GameObject one in PlayerEffectList)
        {
            if (one.name.Equals(EffectName))
            {
                one.SetActive(false);
            }
        }
    }
    
    // 피격 시, 사망 시에만 실행함. 실행중인 이펙트들을 모두 Off하고 리스트를 클리어.
    public void StopAllSkillEffect()
    {
        SkillEffectList.ForEach(e => e.EffectOff());        
        SkillEffectList.Clear();
    }
    public void StopAllBossEffect()
    {
        foreach (GameObject one in BossSkillEffectList)
            one.SetActive(false);
    }
    public void ClearList()
    {
        SkillEffectList.Clear();
        PlayerEffectList.Clear();
        BossSkillEffectList.Clear();
    }
}
