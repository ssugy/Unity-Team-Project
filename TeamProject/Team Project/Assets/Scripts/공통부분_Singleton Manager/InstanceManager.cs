using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceManager : MonoBehaviour
{
    #region �̱��� ����
    private static InstanceManager instance;
    public static InstanceManager s_instance { get => instance; }    
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public List<PlayerEffect> PlayerEffectList;    
    public List<BossEffect> BossEffectList;

    [Header("�÷��̾� ���� ����Ʈ")]
    public GameObject Normal_Attack_Effect;
    public GameObject Normal_Attack_Effect2;
    public GameObject Normal_Attack_Effect3;
    public GameObject Skill_1_Effect;
    public GameObject Skill_1_Effect2;
    public GameObject Skill_2_Effect;
    public GameObject Skill_2_Effect2;
    public GameObject Skill_2_Effect3;

    [Header("�÷��̾� ��Ÿ ����Ʈ")]
    public GameObject LevelUpEffect;
    public GameObject HealingEffect;

    [Header("���� ��ų ����Ʈ")]
    public GameObject Boss_Skill_Effect;
    public GameObject Boss_Skill_Effect2;
    public GameObject Boss_Dead_Effect;


    private void Start()
    {
        PlayerEffectList = new();        
        BossEffectList = new();
    }


    public void ExtraEffectCreate(string EffectName)
    {
        GameObject effect;
        switch (EffectName)
        {
            case "LevelUpEffect":
                effect = Instantiate(LevelUpEffect, JY_CharacterListManager.s_instance.playerList[0].transform);
                effect.transform.localPosition = Vector3.forward;
                break;
            case "HealingEffect":
                effect = Instantiate(HealingEffect, JY_CharacterListManager.s_instance.playerList[0].transform);
                break;          
            default:
                return;
        }             
    }
    public void BossEffectCreate(string _name, Transform _parent)
    {
        GameObject effect;
        switch (_name)
        {
            case "Boss_Skill_Effect":
                effect = Instantiate(Boss_Skill_Effect, _parent);
                break;
            case "Boss_Skill2_Effect":
                effect = Instantiate(Boss_Skill_Effect2, _parent);
                effect.transform.localPosition = Vector3.up;
                break;
            case "Boss_Dead_Effect":
                effect = Instantiate(Boss_Dead_Effect, _parent);
                effect.transform.localPosition = Vector3.forward;
                break;
            default:
                return;
        }

        BossEffectList.Add(effect.GetComponent<BossEffect>());
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

        PlayerEffectList.Add(effect.GetComponent<PlayerEffect>());
    }

    public void SkillEffectCreate(int _effectNum, Transform _parent)
    {        
        GameObject effect;
        switch (_effectNum)
        {
            // ��ų1 Power Strike ����Ʈ
            case 0:
                effect = Instantiate(Skill_1_Effect, _parent);
                effect.transform.localPosition = Vector3.forward;
                break;
            case 1:
                effect = Instantiate(Skill_1_Effect2, _parent);
                effect.transform.localPosition = new Vector3(0, 0, 2);
                break;

            // ��ų2 Turn Attack ����Ʈ
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
        
        PlayerEffectList.Add(effect.GetComponent<PlayerEffect>());
    }       
    
    // �ǰ� ��, ��� �ÿ��� ������. �������� ����Ʈ���� ��� Off�ϰ� ����Ʈ�� Ŭ����.
    public void StopAllSkillEffect()
    {
        PlayerEffectList.ForEach(e => e.EffectOff());        
        PlayerEffectList.Clear();
    }
    public void StopAllBossEffect()
    {
        BossEffectList.ForEach(e => e.EffectOff());
        BossEffectList.Clear();
    }
    public void ClearList()
    {
        PlayerEffectList.Clear();        
        BossEffectList.Clear();
    }
}
