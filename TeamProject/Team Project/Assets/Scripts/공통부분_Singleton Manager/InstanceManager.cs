using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceManager : MonoBehaviour
{
    static InstanceManager instance;
    public static InstanceManager s_instance { get => instance; }
    List<GameObject> SkillEffectList;
    List<GameObject> UIEffectList;

    public GameObject Skill_1_Effect;
    public GameObject Skill_2_Effect;
    private void Awake()
    {
        instance = this;
        SkillEffectList = new List<GameObject>();
        UIEffectList = new List<GameObject>();
    }

    public void PlaySkillEffect(string EffectName)
    {
        StartCoroutine(EffectCreate(EffectName));
        StartCoroutine(EffectOnDisable(EffectName, 2f));
    }
    IEnumerator EffectCreate(string EffectName)
    {
        yield return new WaitForSeconds(0.5f);
        GameObject effect = FindEffect(EffectName);
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

    GameObject FindEffect(string EffectName)
    {
        foreach (GameObject one in SkillEffectList)
        {
            if (one.name.Equals(EffectName))
                return one;
        }
        return null;
    }
    IEnumerator EffectOnDisable(string EffectName, float elapsed)
    {
        yield return new WaitForSeconds(elapsed);
        foreach(GameObject one in SkillEffectList)
        {
            if (one.name.Equals(EffectName))
                one.SetActive(false);
        }
            
    }
    public void ClearList()
    {
        SkillEffectList.Clear();
        UIEffectList.Clear();
    }
}
