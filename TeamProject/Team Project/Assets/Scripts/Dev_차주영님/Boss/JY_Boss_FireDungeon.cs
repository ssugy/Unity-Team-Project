using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class JY_Boss_FireDungeon : Enemy
{
    public override int CurHealth
    {
        get => base.CurHealth; 
        set
        {
            curHealth = value;
            if (curHealth >= maxHealth)
                curHealth = maxHealth;
            else if (curHealth <= 0 && !isDead)
            {
                StopAllCoroutines();
                hitbox.enabled = false;
                target = null;
                isDead = true;
                isAwake = false;
                anim.SetTrigger("isDead");
                FreezeEnemy();

                DungeonManager.instance.DungeonProgress(4);
                DungeonManager.instance.SetDungeonGuide(4);
                JY_QuestManager.s_instance.QuestProgress(1);
                AudioManager.s_instance.SoundFadeInOut(AudioManager.s_instance.nowplayName, 0f, 0.3f);
                DropExp();
                DropGold();
                DropItem();
                portalCreate();
                Destroy(gameObject, 10f);
            }

        }
    }
        

    public static JY_Boss_FireDungeon instacne;
    public static JY_Boss_FireDungeon s_instance { get { return instacne; } }
    Vector3 tauntVec;
    public bool isLook;
    public bool isAwake;
    bool isDead;
    bool isStun;
    [HideInInspector] public bool isJump;
    bool isKick;
    [HideInInspector] public bool isAttack;
    int partCnt;
    float angleRange;
    [Header("보스 공격 범위 콜라이더")]
    public BoxCollider JumpAttackArea;
    public BoxCollider MeleeAttackArea;
    public BoxCollider KickAttackArea;
    public Transform BossWeapon;
    [Header("보스 관련 인스턴스")]
    public GameObject BossWeaponFire;
    public GameObject FieldFire;
    public GameObject Fireball;
    public GameObject BossRoomPortal;
    [HideInInspector] public int HitSkillNum;
    [Header("부위파괴 콜라이더 및 파츠")]
    public GameObject HeadPart;
    public GameObject SholderPart;
    public Collider SholderHitBox;
    public Collider HeadHitBox;

    // Start is called before the first frame update
    new void Awake()
    {
        instacne = this;        
        hitbox = GetComponent<CapsuleCollider>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        originPos = transform.position;
        originRot = transform.rotation;
        isDead = false;
        HitSkillNum = -1;
        partCnt = 2;
        curHealth = maxHealth;
        angleRange = 30f;
    }
    private void Start()
    {
        //target = JY_CharacterListManager.s_instance.playerList[0].transform;
        isLook = true;
        isAwake = false;
        isStun = false;
        isJump = false;
        isKick = false;
        StartCoroutine(Targeting());
    }

    private void FixedUpdate()
    {
        atkTime += Time.fixedDeltaTime;
        if (isAwake && !isDead)
        {
            if (target != null)
            {
                nav.SetDestination(target.position);
                float distance = Vector3.Distance(transform.position, target.position);
                if (nav.velocity != Vector3.zero && !isAttack)
                {
                    Vector3 dir = target.transform.position - this.transform.position;
                    if (distance > 2f) 
                        anim.SetBool("isWalk", true);
                }
                else
                {
                    anim.SetBool("isWalk", false);
                }


                if (!isStop)
                {
                    Vector3 dir = target.transform.position - this.transform.position;
                    if (CheckInsight(dir))
                        anim.SetBool("isWalk", true);
                }

                if (distance <= attackDistance)
                {
                    //totalTime = 0f;
                    if (distance <= 2f && !isAttack)
                        FreezeEnemy();
                    /*else if (distance > 2f && !isAttack)
                        UnfreezeEnemy();*/

                    if (atkTime >= attackCool)
                    {
                        isAttack = true;
                        atkTime = 0f;
                        isStop = true;
                        Vector3 dir = target.transform.position - this.transform.position;
                        this.transform.rotation = Quaternion.LookRotation(dir.normalized);
                        FreezeEnemy();
                        int ranAction = Random.Range(0, 3);
                        StartCoroutine(BossPattern(ranAction));
                    }
                }
                else if (distance > 10f && atkTime >= attackCool)
                {
                    atkTime = 0f;
                    isStop = true;
                    isAttack = true;
                    Vector3 dir = target.transform.position - this.transform.position;
                    this.transform.rotation = Quaternion.LookRotation(dir.normalized);
                    FreezeEnemy();
                    StartCoroutine(BossPattern(3));
                }
                else if (distance > 8f && isKick)
                {
                    StopAllCoroutines();
                    atkTime = 0f;
                    InstanceManager.s_instance.StopAllBossEffect();
                    isAttack = true;
                    Vector3 dir = target.transform.position - this.transform.position;
                    this.transform.rotation = Quaternion.LookRotation(dir.normalized);
                    FreezeEnemy();
                    KickOff();
                    StartCoroutine(BossPattern(3));
                }


                if (isJump)
                {
                    transform.position = Vector3.MoveTowards(transform.position, tauntVec, Time.deltaTime * 15f);
                }

                if (JY_CharacterListManager.s_instance.playerList[0].CompareTag("Dead"))
                {
                    nav.SetDestination(originPos);
                    transform.rotation = originRot;
                    UnfreezeEnemy();
                    target = null;
                    StartCoroutine(Targeting());

                    if(target = null)
                    {
                        isAwake = false;
                        if (transform.position == originPos)
                            anim.SetBool("isWalk", true);
                        else
                            anim.SetBool("isWalk", false);
                    }
                }
            }
        }
    }

    // 전방에 플레이어가 있는지를 체크하는 함수 -> 없으면 걷는모션, 있으면 공격
    bool CheckInsight(Vector3 dir)
    {
        float t = Mathf.Clamp(Time.deltaTime * 3f, 0f, 0.99f);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), t);
        float dot = Vector3.Dot(dir, transform.forward);
        float theta = Mathf.Acos(dot);
        float degree = Mathf.Rad2Deg * theta;
        bool result = degree <= angleRange / 2f ? true : false;
        return result;
    }

    IEnumerator BossPattern(int patternNum)
    {
        yield return new WaitForSeconds(0.1f);
        switch (patternNum)
        {
            case 0:
                NormalAttack();
                break;
            case 1:
                WhirlAttack();
                break;
            case 2:
                Kick();
                break;
            case 3:
                JumpAttack();
                break;
        }
    }

    // 일반공격 몽둥이를 위에서 아래로 한번 내리치는 것.
    void NormalAttack()
    {
        anim.SetTrigger("NoramlAttack");
    }
    public void SwingSound()
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BOSS_SWING);
    }
    // 회전하면서 파이어볼 쓰는 공격
    void WhirlAttack()
    {
        anim.SetTrigger("WhirlAttack");
    }

    // 멀리서부터 돌진해서 찍고, 불덩어리가 바닥에 뿌려지는 공격
    void JumpAttack()
    {
        anim.SetTrigger("JumpAttack");
    }
    public void JumpAttackStart()
    {
        isJump = true;
        tauntVec = target.position;
        hitbox.enabled = false;
    }
    public void JumpAttackFinish()
    {
        isJump = false;
        hitbox.enabled = true;
    }
    public void JumpAttackAreaOn()
    {
        JumpAttackArea.gameObject.SetActive(true);
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Boss_JUMP);
        InstanceManager.s_instance.BossEffectCreate("Boss_Skill_Effect", this.transform);
    }
    public void JumpAttackAreaOff()
    {
        JumpAttackArea.gameObject.SetActive(false);
    }
    public void FieldFireGenerate(int num)
    {
        for (int i = 0; i < num; i++)
            FieldFireCreate();
    }
    // 발차리 계속하는 공격
    void Kick()
    {
        isKick = true;
        anim.SetTrigger("KickAttack");
    }
    public void KickOff()
    {
        isKick = false;
    }
    public void KickEffect(string EffectName)
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BOSS_KICK);
        InstanceManager.s_instance.BossEffectCreate(EffectName, this.transform);
    }
    public void KickEffectOff(string EffectName)
    {
        InstanceManager.s_instance.BossEffectOff(EffectName);
    }
    public void BossDieEffect()
    {
        InstanceManager.s_instance.BossEffectCreate("Boss_Dead_Effect", this.transform);
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BOSS_DEAD, false, 1f);
    }
    public void StopAllEffect()
    {
        InstanceManager.s_instance.StopAllBossEffect();
    }
    // 보스가 플레이어한테 공격 받았으면에 대한 판정
    public override void OnDamage(int _damage, Vector3 _attacker)
    {
        // 마스터(호스트)에서만 연산을 함.
        if (!PhotonNetwork.IsMasterClient)
            return;

        // 데미지와 넉백 연산. 피격 애니메이션 트리거를 Set
        CurHealth -= _damage;
        if (_damage >= (0.2f * maxHealth))
            anim.SetTrigger("isAttacked");

        Vector3 reactVec = transform.position - _attacker; // 넉백 거리.        
        if (HitSkillNum != -1)
        {
            if (!isStun)
                isAttackedAnimPlay(HitSkillNum);
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            StartCoroutine(KnockBack(reactVec));
        }

        // 피격에 따른 이펙트 출력과 UI 활성화는 모든 클라이언트에게.
        //photonView.RPC("HitEffect", RpcTarget.All);       
    }    
    
    /// <summary>
    /// 스킬에 따른 다른 피격모션 재생, 플레이어가 어떤 스킬로 보스에게 공격했냐에 따라서 보스의 피격 애니매이션을 결정하는 함수.
    /// </summary>
    /// <param name="playerSkill">-1:Idle상태, 평타 1,2타, 0:플레이어 평타 3번째, 1:스킬1, 2:스킬2</param>
    void isAttackedAnimPlay(int playerSkill)
    {
        anim.SetTrigger("isAttacked");
        anim.SetInteger("HitNum",playerSkill);
    }

    // 보스 무기에 나오는 불덩어리 - 외부에서 참조, 애니매이션 behaviour에서 제어
    public void WeaponEffectOnOff(bool state)
    {
        BossWeaponFire.SetActive(state);
    }

    // 바닥에 불덩이(점프 공격 시 불덩어리 나오는 것)
    void FieldFireCreate()
    {
        GameObject tmp = Instantiate<GameObject>(FieldFire);
        tmp.transform.position = transform.position + new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
    }

    // 공격중 피격 모션이 발생할 때 사용하는 함수. 공격판정 콜라이더 인스턴스를 전부 비활성화 시킴
    public void MeleeAreaDisEnable()
    {
        MeleeAttackArea.gameObject.SetActive(false);
        KickAttackArea.gameObject.SetActive(false);
        JumpAttackArea.gameObject.SetActive(false);
    }

    // 파이어볼 - 봉에서 날라가는 파이어볼
    void ShootFire()
    {
        Instantiate(Fireball, BossWeapon.position, transform.rotation);
    }

    // 부위파괴 함수
    public void PartDestruction(string partName)
    {
        isStop = true;
        isStun = true;
        if (partName.Equals("BossSholderHitBox"))
        {
            SholderHitBox.enabled = false;
            SholderPart.SetActive(false);
        }
        else
        {
            HeadHitBox.enabled = false;
            HeadPart.SetActive(false);
        }

        partCnt--;
        switch (partCnt)
        {
            case 0:
                defMag = 0.3f;
                break;
            case 1:
                defMag = 0.4f;
                break;
            case 2:
                break;
        }
        anim.SetTrigger("Stun");
    }
    
    // 보스 네비게이션 메시 관련해서, 공격 할 때 계속 따라가지 않고 제자리에서 공격하는 함수 - 애니매이션에서 쓰고있음.
    public void UnfreezeBoss()
    {
        UnfreezeEnemy();
    }

    // 보스 회전 - 애니매이션에서 사용
    public void BossRotate()
    {
        isStop = false;
    }

    // 포탈 만드는 것
    void portalCreate()
    {
        BossRoomPortal.SetActive(true);
    }

    // 피격 애니매이션 출력되었을 때 바로 공격하도록 공격 쿨타임 조정하는 함수
    public void SetAtkTime(float tmp)
    {
        atkTime = tmp;
    }

    // 부위파괴 그로기상태 들어갔다가 일어날 때 사용하는 함수.
    public void StunWakeUp()
    {
        anim.SetTrigger("StunWakeUP");
    }
    public void StunReset()
    {
        isStun = false;
        isStop = false;
        isAwake = true;
    }

    /// <summary>
    /// 근접 공격 콜라이더 On/Off
    /// </summary>
    public void MeleeColliderOn()
    {
        MeleeAttackArea.gameObject.SetActive(true);
    }
    public void MeleeColliderOff()
    {
        MeleeAttackArea.gameObject.SetActive(false);
    }
    public void KickColliderOn()
    {
        KickAttackArea.gameObject.SetActive(true);
    }
    public void KickColliderOff()
    {
        KickAttackArea.gameObject.SetActive(false);
    }
}
