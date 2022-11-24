using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class JY_Boss_FireDungeon : Enemy
{
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
    void Awake()
    {
        instacne = this;
        rigid = GetComponentInChildren<Rigidbody>();
        hitbox = GetComponent<CapsuleCollider>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        originPos = transform.position;
        originRotateion = transform.rotation;
        isDead = false;
        HitSkillNum = -1;
        partCnt = 2;
        curHealth = maxHealth;
        angleRange = 30f;
    }
    private new void Start()
    {
        target = JY_CharacterListManager.s_instance.playerList[0].transform;
        isLook = true;
        isAwake = false;
        isStun = false;
        isJump = false;
        isKick = false;
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
                    totalTime = 0f;
                    if (distance <= 2f && !isAttack)
                        FreezeEnemy();
                    else if (distance > 2f && !isAttack)
                        UnfreezeEnemy();

                    if (atkTime >= attackCool)
                    {
                        isAttack = true;
                        atkTime = 0f;
                        isStop = true;
                        Vector3 dir = target.transform.position - this.transform.position;
                        this.transform.rotation = Quaternion.LookRotation(dir.normalized);
                        FreezeEnemy();
                        //int ranAction = Random.Range(0, 3);
                        //StartCoroutine(BossPattern(ranAction));
                        StartCoroutine(BossPattern(0));
                    }
                }
                else if (distance > 10f && atkTime >= attackCool)
                {
                    atkTime = 0f;
                    isStop = true;
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
                    Vector3 dir = target.transform.position - this.transform.position;
                    this.transform.rotation = Quaternion.LookRotation(dir.normalized);
                    StartCoroutine(JumpAttack());
                    isKick = false;
                }


                if (isJump)
                {
                    transform.position = Vector3.MoveTowards(transform.position, tauntVec, Time.deltaTime * 15f);
                }

                if (JY_CharacterListManager.s_instance.playerList[0].CompareTag("Dead"))
                {
                    nav.SetDestination(originPos);
                    transform.rotation = originRotateion;
                    UnfreezeEnemy();
                    target = null;
                    isAwake = false;
                    if(transform.position == originPos)
                        anim.SetBool("isWalk", true);
                    else
                        anim.SetBool("isWalk", false);
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
                StartCoroutine(NormalAttack());
                break;
            case 1:
                StartCoroutine(WhirlAttack());
                break;
            case 2:
                StartCoroutine(Kick());
                break;
            case 3:
                StartCoroutine(JumpAttack());
                break;
        }
    }

    // 일반공격 몽둥이를 위에서 아래로 한번 내리치는 것.
    IEnumerator NormalAttack()
    {
        anim.SetTrigger("NoramlAttack");
        yield return new WaitForSeconds(0.2f);
        //MeleeAttackArea.gameObject.SetActive(true);
    }

    // 회전하면서 파이어볼 쓰는 공격
    IEnumerator WhirlAttack()
    {
        anim.SetTrigger("WhirlAttack");
        yield return new WaitForSeconds(0.5f);
        ShootFire();
        yield return new WaitForSeconds(1f);
        ShootFire();
    }

    // 멀리서부터 돌진해서 찍고, 불덩어리가 바닥에 뿌려지는 공격
    IEnumerator JumpAttack()
    {
        anim.SetTrigger("JumpAttack");
        yield return new WaitForSeconds(0.5f);
        isJump = true;
        tauntVec = target.position;
        hitbox.enabled = false;
        yield return new WaitForSeconds(0.5f);
        JumpAttackArea.gameObject.SetActive(true);
        for (int i = 0; i < 5; i++)
            FieldFireCreate();
        yield return new WaitForSeconds(0.5f);
        isJump = false;
        JumpAttackArea.gameObject.SetActive(false);
        hitbox.enabled = true;

    }

    // 발차리 계속하는 공격
    IEnumerator Kick()
    {
        isKick = true;
        anim.SetTrigger("KickAttack");
        yield return new WaitForSeconds(0.5f);
        meleeInitialize();
        yield return new WaitForSeconds(0.5f);
        meleeInitialize();
        yield return new WaitForSeconds(1f);
        meleeInitialize();
    }

    // 원래는 무기에 콜라이더를 달았는데, 잘 안맞아서 보스 전방에 근접공격 콜라이더를 달아서 처리하는 구문
    // 0.2초의 판정을 유지한다.
    void meleeInitialize()
    {
        MeleeAttackArea.gameObject.SetActive(true);
        StartCoroutine(meleeOffDelay());
    }    

    IEnumerator meleeOffDelay()
    {
        yield return new WaitForSeconds(0.2f);
        MeleeAttackArea.gameObject.SetActive(false);
    }

    // 보스가 플레이어한테 공격 받았으면에 대한 판정
    public override void IsAttacked(int _damage, Vector3 _player)
    {
        photonView.RPC("IsAttacked_Do", RpcTarget.All, _damage, _player);
    }
    [PunRPC]
    public override void IsAttacked_Do(int _damage, Vector3 _player)
    {
        curHealth -= _damage;
        Vector3 reactVec = transform.position - _player; // 넉백 거리.
        StartCoroutine(OnDamage(reactVec * 0.2f));
        hpbar = Enemy_HP_UI.GetObject();
        hpbar.Recognize(this);
        if (EffectManager.Instance != null)
            EffectManager.Instance.PlayHitEffect(transform.position + offset, transform.rotation.eulerAngles, transform);
    }

    // 어택에서 이어져서, 데미지를 처리하는 구문, 사망했는지 여부도 처리
    protected new IEnumerator OnDamage(Vector3 reactVec)
    {
        yield return new WaitForSeconds(0.1f);
        if (curHealth > 0)
        {
            if (HitSkillNum != -1)
            {
                if(!isStun)
                    isAttackedAnimPlay(HitSkillNum);
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;
                StartCoroutine(KnockBack(reactVec));
            }
        }
        else
        {
            StopAllCoroutines();
            AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BOSS_DEAD,false,1f);
            hitbox.enabled = false;
            target = null;
            isDead = true;
            isAwake = false;
            anim.SetTrigger("isDead");
            FreezeEnemy();
            InstanceManager.s_instance.PlayBossSkillEffect("Boss_Dead_Effect", 0f, this.transform);

            questProgress();
            DropExp();
            DropGold();
            DropItem();
            portalCreate();
            Destroy(gameObject,10f);
        }
    }

    /// <summary>
    /// 스킬에 따른 다른 피격모션 재생, 플레이어가 어떤 스킬로 보스에게 공격했냐에 따라서 보스의 피격 애니매이션을 결정하는 함수.
    /// </summary>
    /// <param name="playerSkill">-1:Idle상태, 평타 1,2타, 0:플레이어 평타 3번째, 1:스킬1, 2:스킬2</param>
    void isAttackedAnimPlay(int playerSkill)
    {
        Debug.Log(HitSkillNum);
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

    // 근접공격과 점프공격이 동시에 나가는 것, 한번에 쓰도록 하는 함수(콜라이더가 달라서 2개를 같이 활성화 시키거나 비활성화 시킴)
    public void MeleeAreaDisEnable()
    {
        MeleeAttackArea.gameObject.SetActive(false);
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
        Debug.Log(partCnt);
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
    public void stunWakeUp()
    {
        anim.SetTrigger("StunWakeUP");
        isStun = false;
        isAwake = true;
    }

    /// <summary>
    /// 근접 공격 콜라이더 On/Off
    /// </summary>
    public void MeleeColliderOn()
    {
        Debug.Log("on");
        MeleeAttackArea.gameObject.SetActive(true);
    }
    public void MeleeColliderOff()
    {
        Debug.Log("off");
        MeleeAttackArea.gameObject.SetActive(false);
    }

    public void eventTest()
    {
        Debug.Log("normal");
    }
}
