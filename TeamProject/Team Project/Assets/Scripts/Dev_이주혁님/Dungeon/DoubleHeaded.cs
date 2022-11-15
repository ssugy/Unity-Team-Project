using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoubleHeaded : Enemy
{    
    

    public float skillCool;
    [Header("스킬 공격 관련")]
    public float skillMinimumDistance;
    public float skillMaximumDistance;
    public GameObject flame;
    public GameObject poison;
    public GameObject earthquake;
    private float skillTime;

    // 보스 관련 효과음을 담당.
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
    // 보스는 타겟팅 기능을 사용하지 않음. 보스룸에 들어온 것을 감지하는 콜라이더로 target을 설정.
    // 보스룸에 플레이어가 들어오면 도망칠 수 없으므로 target이 null로 바뀌었을 때의 조건은 필요없음.
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

    // 애니메이션 이벤트 함수.
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
