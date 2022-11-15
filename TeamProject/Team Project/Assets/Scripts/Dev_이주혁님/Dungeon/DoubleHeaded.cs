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
    public GameObject flame;
    public GameObject poison;
    public GameObject earthquake;
    private float skillTime;

    // ���� ���� ȿ������ ���.
    public BossAudioManager audioManager;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        hitbox = GetComponent<Collider>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        originPos = transform.position;
        originRotateion = transform.rotation;
        atkTime = 0f;
        skillTime = 0f;
    }
    // ������ Ÿ���� ����� ������� ����. �����뿡 ���� ���� �����ϴ� �ݶ��̴��� target�� ����.
    // �����뿡 �÷��̾ ������ ����ĥ �� �����Ƿ� target�� null�� �ٲ���� ���� ������ �ʿ����.
    protected new void Start()
    {
    }
    private void FixedUpdate()
    {
        atkTime += Time.fixedDeltaTime;
        skillTime += Time.fixedDeltaTime;
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
            else if (distance > skillMinimumDistance && distance<skillMaximumDistance && skillTime >= skillCool)
            {
                switch(Random.Range(0, 4))
                {
                    case 0:
                        anim.SetTrigger("isFlame");
                        skillTime = 0f;
                        break;
                    case 1:
                        anim.SetTrigger("isFart");
                        skillTime = 0f;
                        break;
                    case 2:
                        anim.SetTrigger("isEarthquake");
                        skillTime = 0f;
                        break;
                    default:
                        anim.SetTrigger("isShoot");
                        skillTime = 0f;
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
    public override void IsAttacked(int _damage, Vector3 _player)
    {
        curHealth -= _damage;       
        if (curHealth <= 0)
        {
            hitbox.enabled = false;
            anim.SetTrigger("isDead");
            FreezeEnemy();            
            DropExp();
            DropGold();
            DropItem();
            questProgress();

            Destroy(gameObject, 8);
        }
        /*else if (curHealth <= maxHealth * 0.3f && bossManager.secondCutScenePlay == false)
        {
            JY_CutScenePlay.instance.PlayCutScene2();
            bossManager.secondCutScenePlay = true;
        }*/
    }
    protected override void questProgress()
    {
        if (JY_QuestManager.s_instance != null &&
            JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress2[2] == 1 &&
            JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress2[3] == 0)
            JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress2[1]++;
    }

    // �ִϸ��̼� �̺�Ʈ �Լ�.
    void NormalAttack()
    {
        atkMag = 1.1f;
        Collider[] player = Physics.OverlapSphere(transform.position + transform.forward * 1.2f,
            1.5f, LayerMask.GetMask("Player"));
        if (player != null)
        {
            foreach (Collider col in player)
            {
                Attack(col);
            }
        }
    }
    void FlameThrower()
    {        
        atkMag = 0.8f;
        Collider[] player = Physics.OverlapCapsule(transform.position + transform.forward, 
            transform.position + transform.forward * 7, 0.3f, LayerMask.GetMask("Player"));        
        if (player != null)
        {
            foreach (Collider col in player)
            {
                Attack(col);
            }
        }
    }
    void PoisonGas()
    {
        atkMag = 1f;
        Collider[] player = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("Player"));
        if (player != null)
        {
            foreach (Collider col in player)
            {
                Attack(col);
            }
        }
    }
    void Earthquake()
    {
        atkMag = 2f;
        Collider[] player = Physics.OverlapCapsule(transform.position + transform.forward,
            transform.position + transform.forward * 11, 0.2f, LayerMask.GetMask("Player"));
        if (player != null)
        {
            foreach (Collider col in player)
            {
                Attack(col);
            }
        }
    }
    void ShootGun()
    {
        atkMag = 2.2f;
        Collider[] player = Physics.OverlapSphere(transform.position + transform.forward * 2,
            1.5f, LayerMask.GetMask("Player"));
        if (player != null)
        {
            foreach (Collider col in player)
            {
                Attack(col);
            }
        }
    }
    void FlameEffectOn()
    {
        flame.SetActive(true);
    }
    void PoisonEffectOn()
    {
        poison.SetActive(true);
    }
    void EarthEffectOn()
    {
        earthquake.SetActive(true);
    }
    void FlameEffectOff()
    {
        flame.SetActive(false);
    }
    void PoisonEffectOff()
    {
        poison.SetActive(false);
    }
    void EarthEffectOff()
    {
        earthquake.SetActive(false);
    }
    void Audio_1()
    {
        audioManager.PlaySound(0);
    }
    void Audio_2()
    {
        audioManager.PlaySound(1);
    }
    void Audio_3()
    {
        audioManager.PlaySound(2);
    }
    void SoundWalk()
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BOSS_WALK);
    }
    void SoundSwing()
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BOSS_WALK);
    }
}
