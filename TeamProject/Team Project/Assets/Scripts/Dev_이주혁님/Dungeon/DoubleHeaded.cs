using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoubleHeaded : Enemy
{    
    public float skillCool;
    [Header("��ų ���� ����")]
    public float skillMinimumDistance;
    public float skillMaximumDistance;

    private BossManager bossManager;
    public new HP_Bar_Boss hpbar;         // HP ��.

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        hitbox = GetComponent<Collider>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        originPos = transform.position;
        atkTime = 0f;
    }
    // ������ Ÿ���� ����� ������� ����. �����뿡 ���� ���� �����ϴ� �ݶ��̴��� target�� ����.
    // �����뿡 �÷��̾ ������ ����ĥ �� �����Ƿ� target�� null�� �ٲ���� ���� ������ �ʿ����.
    protected new void Start()
    {
        bossManager = BossManager.instance;
    }
    private void FixedUpdate()
    {
        atkTime += Time.fixedDeltaTime;
        if (target != null)
        {
            nav.SetDestination(target.position);
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance <= attackDistance)
            {
                FreezeEnemy();
                if (atkTime >= 5f)
                {
                    anim.SetTrigger("isAttack");
                    atkTime = 0f;
                }
            }
            else if (distance > skillMinimumDistance && distance<skillMaximumDistance && atkTime >= skillCool)
            {
                switch(Random.Range(0, 4))
                {
                    case 0:
                        anim.SetTrigger("isFlame");
                        atkTime = 0f;
                        break;
                    case 1:
                        anim.SetTrigger("isFart");
                        atkTime = 0f;
                        break;
                    case 2:
                        anim.SetTrigger("isEarthquake");
                        atkTime = 0f;
                        break;
                    default:
                        anim.SetTrigger("isShoot");
                        atkTime = 0f;
                        break;
                }                
            }
        }        
        if (nav.velocity != Vector3.zero)
        {
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }
    }
    public override void IsAttacked(int _damage)
    {
        curHealth -= _damage;        
        hpbar.Recognize(this);
        if (curHealth <= 0)
        {
            hitbox.enabled = false;
            anim.SetTrigger("isDead");
            FreezeEnemy();            
            DropExp();
            DropGold();
            DropItem();
            questProgress();
            if (bossManager != null)
            {
                bossManager.portal.SetActive(true);
            }
            Destroy(gameObject, 8);
        }
        else if (curHealth <= maxHealth * 0.3f && bossManager.secondCutScenePlay == false)
        {
            JY_CutScenePlay.instance.PlayCutScene2();
            bossManager.secondCutScenePlay = true;
        }
    }
    protected override void questProgress()
    {
        if (JY_QuestManager.s_instance != null &&
            JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress2[2] == 1 &&
            JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress2[3] == 0)
            JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress2[1]++;
    }
}
