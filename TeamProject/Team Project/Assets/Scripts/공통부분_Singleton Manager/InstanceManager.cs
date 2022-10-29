using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceManager : MonoBehaviour
{
    static InstanceManager instance;
    public static InstanceManager s_instance { get => instance; }
    List<GameObject> SkillEffectList;
    List<GameObject> PlayerEffectList;

    public GameObject Skill_1_Effect;
    public GameObject Skill_2_Effect;
    public GameObject LevelUpEffect;
    private void Awake()
    {
        instance = this;
        SkillEffectList = new List<GameObject>();
        PlayerEffectList = new List<GameObject>();
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
    public void PlaySkillEffect(string EffectName)
    {
        StartCoroutine(SkillEffectCreate(EffectName));
        StartCoroutine(EffectOnDisable(EffectName, 2f,SkillEffectList));
    }
    public void PlayPlayerEffect(string EffectName)
    {
        PlayerEffectCreate(EffectName);
        StartCoroutine(EffectOnDisable(EffectName, 2.5f, PlayerEffectList));
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
    IEnumerator SkillEffectCreate(string EffectName)
    {
        yield return new WaitForSeconds(0.5f);
        GameObject effect = FindEffect(EffectName,SkillEffectList);
        if (effect != null)
        {
            effect.SetActive(true);
            effect.transform.SetParent(Player.instance.transform);
            effect.transform.localPosition = Vector3.forward;
            effect.transform.SetParent(instance.transform);
        }
        else
        {
            if (EffectName.Equals("Skill_1_Effect"))
                effect = Instantiate<GameObject>(Skill_1_Effect,instance.transform);
            else if (EffectName.Equals("Skill_2_Effect"))
                effect = Instantiate<GameObject>(Skill_2_Effect, instance.transform);
            effect.transform.position = Player.instance.transform.position + Vector3.forward;
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
    public void ClearList()
    {
        SkillEffectList.Clear();
        PlayerEffectList.Clear();
    }
}
