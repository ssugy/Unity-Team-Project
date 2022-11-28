using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceManager : MonoBehaviour
{
    static InstanceManager instance;
    public static InstanceManager s_instance { get => instance; }
    List<GameObject> SkillEffectList;
    List<GameObject> PlayerEffectList;
    public List<GameObject> BossSkillEffectList;
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
    public GameObject Boss_Skill_Effect2;
    public GameObject Boss_Dead_Effect;

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
    public void NormalAttackEffectCreate(string EffectName)
    {
        GameObject effect = FindEffect(EffectName, SkillEffectList);
        if (effect != null)
        {
            effect.SetActive(true);
        }
        else
        {
            if (EffectName.Equals("Normal_Attack_Effect"))
                effect = Instantiate<GameObject>(Normal_Attack_Effect, JY_CharacterListManager.s_instance.playerList[0].transform);
            else if (EffectName.Equals("Normal_Attack_Effect2"))
                effect = Instantiate<GameObject>(Normal_Attack_Effect2, JY_CharacterListManager.s_instance.playerList[0].transform);
            else if (EffectName.Equals("Normal_Attack_Effect3"))
                effect = Instantiate<GameObject>(Normal_Attack_Effect3, JY_CharacterListManager.s_instance.playerList[0].transform);
            effect.transform.localPosition = new Vector3(0, 1f, 1f);
            effect.gameObject.name = EffectName;
            SkillEffectList.Add(effect);
        }
    }
    public void SkillEffectCreate(string EffectName)
    {
        GameObject effect = FindEffect(EffectName, SkillEffectList);
        if (effect != null)
        {
            effect.SetActive(true);
        }
        else
        {
            if (EffectName.Equals("Skill_1_Effect"))
                effect = Instantiate<GameObject>(Skill_1_Effect, JY_CharacterListManager.s_instance.playerList[0].transform);
            else if (EffectName.Equals("Skill_1_Effect2"))
                effect = Instantiate<GameObject>(Skill_1_Effect2, JY_CharacterListManager.s_instance.playerList[0].transform);
            else if (EffectName.Equals("Skill_2_Effect"))
                effect = Instantiate<GameObject>(Skill_2_Effect, JY_CharacterListManager.s_instance.playerList[0].transform);
            else if (EffectName.Equals("Skill_2_Effect2"))
                effect = Instantiate<GameObject>(Skill_2_Effect2, JY_CharacterListManager.s_instance.playerList[0].transform);
            else if (EffectName.Equals("Skill_2_Effect3"))
                effect = Instantiate<GameObject>(Skill_2_Effect3, JY_CharacterListManager.s_instance.playerList[0].transform);
            effect.transform.localPosition = Vector3.forward;
            if (EffectName.Equals("Skill_1_Effect2"))
                effect.transform.localPosition = new Vector3(0, 0, 2);
            else if (EffectName.Equals("Skill_2_Effect1") || EffectName.Equals("Skill_2_Effect3"))
                effect.transform.localPosition = new Vector3(0, 2, 1);
            effect.gameObject.name = EffectName;
            SkillEffectList.Add(effect);
        }
    }

    public void PlayerEffectOff(string EffectName)
    {
        foreach (GameObject one in SkillEffectList)
        {
            if (one.name.Equals(EffectName))
            {
                one.SetActive(false);
            }
        }
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
    public void StopAllSkillEffect()
    {
        foreach (GameObject one in SkillEffectList)
            one.SetActive(false);
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
