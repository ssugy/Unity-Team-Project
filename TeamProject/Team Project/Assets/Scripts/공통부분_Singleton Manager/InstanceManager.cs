using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceManager : MonoBehaviour
{
    static InstanceManager instance;
    public static InstanceManager s_instance { get => instance; }
    List<GameObject> SkillEffectList;
    List<GameObject> PlayerEffectList;
    List<GameObject> BossSkillEffectList;
    [Header("플레이어 이펙트")]
    public GameObject Normal_Attack_Effect;
    public GameObject Normal_Attack_Effect2;
    public GameObject Normal_Attack_Effect3;
    public GameObject Skill_1_Effect;
    public GameObject Skill_1_Effect2;
    public GameObject Skill_2_Effect;
    public GameObject Skill_2_Effect2;
    public GameObject Skill_2_Effect3;
    public GameObject LevelUpEffect;
    [Header("보스 스킬 이펙트")]
    public GameObject Boss_Skill_Effect;
    public GameObject Boss_Skill_Effect3;
    public GameObject Boss_Skill2_Effect;
    public GameObject Boss_Skill3_Effect;
    private void Awake()
    {
        instance = this;
        SkillEffectList = new List<GameObject>();
        PlayerEffectList = new List<GameObject>();
        BossSkillEffectList = new List<GameObject>();
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

    public void NormalAttackEffect(string EffectName)
    {
        StartCoroutine(NormalAttackEffectCreate(EffectName));
        StartCoroutine(EffectOnDisable(EffectName, 1f, SkillEffectList));
    }
    public void PlaySkillEffect(string EffectName, float delay)
    {
        StartCoroutine(SkillEffectCreate(EffectName,delay));
        StartCoroutine(EffectOnDisable(EffectName, 1.5f,SkillEffectList));
    }
    public void PlayPlayerEffect(string EffectName)
    {
        PlayerEffectCreate(EffectName);
        StartCoroutine(EffectOnDisable(EffectName, 2.5f, PlayerEffectList));
    }
    public void PlayBossSkillEffect(string EffectName, float delay, Transform boss)
    {
        StartCoroutine(BossSkillEffectCreate(EffectName, delay, boss));
        StartCoroutine(EffectOnDisable(EffectName,3.3f, BossSkillEffectList));
    }
    void PlayerEffectCreate(string EffectName)
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
                effect = Instantiate<GameObject>(LevelUpEffect, Player.instance.transform);
            effect.transform.localPosition = Vector3.forward;
            effect.gameObject.name = EffectName;
            PlayerEffectList.Add(effect);
        }

    }

    IEnumerator SkillEffectCreate(string EffectName,float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject effect = FindEffect(EffectName,SkillEffectList);
        if (effect != null)
        {
            effect.SetActive(true);
        }
        else
        {
            if (EffectName.Equals("Skill_1_Effect"))
                effect = Instantiate<GameObject>(Skill_1_Effect, Player.instance.transform);
            else if (EffectName.Equals("Skill_1_Effect2"))
                effect = Instantiate<GameObject>(Skill_1_Effect2, Player.instance.transform);
            else if (EffectName.Equals("Skill_2_Effect"))
                effect = Instantiate<GameObject>(Skill_2_Effect, Player.instance.transform);
            else if (EffectName.Equals("Skill_2_Effect2"))
                effect = Instantiate<GameObject>(Skill_2_Effect2, Player.instance.transform);
            else if (EffectName.Equals("Skill_2_Effect3"))
                effect = Instantiate<GameObject>(Skill_2_Effect3, Player.instance.transform);
            effect.transform.localPosition =  Vector3.forward;
            if (EffectName.Equals("Skill_1_Effect2"))
                effect.transform.localPosition = new Vector3(0, 0, 2);
            else if (EffectName.Equals("Skill_2_Effect1") || EffectName.Equals("Skill_2_Effect3"))
                effect.transform.localPosition = new Vector3(0, 2, 1);
            effect.gameObject.name = EffectName;
            SkillEffectList.Add(effect);
        }
    }

    IEnumerator BossSkillEffectCreate(string EffectName, float delay, Transform boss)
    {
        yield return new WaitForSeconds(delay);
        GameObject effect = FindEffect(EffectName, BossSkillEffectList);
        if (effect != null)
        {
            effect.SetActive(true);
        }
        else
        {
            if (EffectName.Equals("Boss_Skill_Effect")|| EffectName.Equals("Boss_Skill_Effect2"))
                effect = Instantiate<GameObject>(Boss_Skill_Effect,boss);
            else if (EffectName.Equals("Boss_Skill_Effect3"))
                effect = Instantiate<GameObject>(Boss_Skill_Effect3, boss);
            else if (EffectName.Equals("Boss_Skill2_Effect"))
                effect = Instantiate<GameObject>(Boss_Skill2_Effect, boss);
            else if (EffectName.Equals("Boss_Skill3_Effect")|| EffectName.Equals("Boss_Skill3_Effect2")|| EffectName.Equals("Boss_Skill3_Effect3")|| EffectName.Equals("Boss_Skill3_Effect4"))
                effect = Instantiate<GameObject>(Boss_Skill3_Effect, boss);
            effect.transform.localPosition = new Vector3(0, 0,0);
            effect.gameObject.name = EffectName;
            BossSkillEffectList.Add(effect);
        }
    }
    IEnumerator NormalAttackEffectCreate(string EffectName)
    {
        yield return new WaitForSeconds(0.5f);
        GameObject effect = FindEffect(EffectName, SkillEffectList);
        if (effect != null)
        {
            effect.SetActive(true);
        }
        else
        {
            if (EffectName.Equals("Normal_Attack_Effect"))
                effect = Instantiate<GameObject>(Normal_Attack_Effect, Player.instance.transform);
            else if (EffectName.Equals("Normal_Attack_Effect2"))
                effect = Instantiate<GameObject>(Normal_Attack_Effect2, Player.instance.transform);
            else if (EffectName.Equals("Normal_Attack_Effect3"))
                effect = Instantiate<GameObject>(Normal_Attack_Effect3, Player.instance.transform);
            effect.transform.localPosition = new Vector3(0, 1f, 1f);
            effect.gameObject.name = EffectName;
            SkillEffectList.Add(effect);
        }
    }

    IEnumerator EffectOnDisable(string EffectName, float elapsed, List<GameObject> targetEffectList)
    {
        yield return new WaitForSeconds(elapsed);
        foreach(GameObject one in targetEffectList)
        {
            if (one.name.Equals(EffectName))
                one.SetActive(false);
        }    
    }

    public void StopAllSkillEffect()
    {
        foreach (GameObject one in SkillEffectList)
            one.SetActive(false);
    }
    public void ClearList()
    {
        SkillEffectList.Clear();
        PlayerEffectList.Clear();
        BossSkillEffectList.Clear();
    }
}
