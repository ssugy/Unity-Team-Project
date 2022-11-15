using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JY_Boss_FireDungeon : Enemy
{
    public static JY_Boss_FireDungeon instacne;
    public static JY_Boss_FireDungeon s_instance { get { return instacne; } }
    Vector3 lookVec;
    Vector3 tauntVec;
    public bool isLook;
    public bool isAwake;
    bool DoAttack;
    bool isDead;
    bool isStun;
    int partCnt;
    [Header("보스 공격 범위 콜라이더")]
    public BoxCollider JumpAttackArea;
    public BoxCollider KickAttackArea;
    public BoxCollider BossWeapon;
    public GameObject BossWeaponFire;
    public GameObject FieldFire;
    public GameObject Fireball;
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
    }
    private new void Start()
    {
        target = JY_CharacterListManager.s_instance.playerList[0].transform;
        isLook = true;
        isAwake = false;
        isStun = false;
    }

    private void FixedUpdate()
    {/*
        if (!isDead && isAwake)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (target != null)
            {
                FreezeVelocity();
                BossRotate();
                nav.SetDestination(target.position);
                atkTime += Time.fixedDeltaTime;

                if (distance <= attackDistance && !DoAttack)
                {
                    if (atkTime >= attackCool)
                    {
                        atkTime = 0f;
                        FreezeEnemy();
                        FreezeBoss();
                        StartCoroutine(BossPattern(4));
                    }

                }

                if (distance >= attackDistance && nav.velocity != Vector3.zero && !isStop)
                {
                    anim.SetBool("isWalk", true);
                }
                else
                {
                    anim.SetBool("isWalk", false);
                }
            }
        }*/


        if (nav.velocity != Vector3.zero)
        {
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }

        atkTime += Time.fixedDeltaTime;
        if (target != null)
        {

            nav.SetDestination(target.position);
            float distance = Vector3.Distance(transform.position, target.position);
            if (!isStop)
            {
                Vector3 dir = target.transform.position - this.transform.position;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime);
                anim.SetBool("isWalk", true);
            }

            if (distance <= attackDistance)
            {
                totalTime = 0f;
                FreezeEnemy();
                if (atkTime >= attackCool)

                {
                    atkTime = 0f;
                    isStop = true;
                    FreezeEnemy();
                    FreezeBoss();
                    StartCoroutine(BossPattern(4)); 
                }
            }
        }
    }

    IEnumerator BossPattern(int patternNum)
    {
        yield return new WaitForSeconds(0.1f);
        int ranAction = Random.Range(0, patternNum);
        switch (ranAction)
        {
            case 0:
                StartCoroutine(NormalAttack());
                break;
            case 1:
                StartCoroutine(JumpAttack());
                break;
            case 2:
                StartCoroutine(WhirlAttack());
                break;
            case 3:
                StartCoroutine(Kick());
                break;
        }
    }

    IEnumerator NormalAttack()
    {
        anim.SetTrigger("NoramlAttack");
        yield return new WaitForSeconds(5f);
        UnFreezeAll();
    }
    IEnumerator WhirlAttack()
    {
        anim.SetTrigger("WhirlAttack");
        yield return new WaitForSeconds(1f);
        ShootFire();
        yield return new WaitForSeconds(1f);
        ShootFire();
        yield return new WaitForSeconds(3f);
        UnFreezeAll();
    }
    IEnumerator JumpAttack()
    {
        anim.SetTrigger("JumpAttack");
        tauntVec = target.position;
        yield return new WaitForSeconds(0.5f);
        while(transform.position != tauntVec)
            transform.position = Vector3.MoveTowards(transform.position, tauntVec,Time.deltaTime);
        yield return new WaitForSeconds(1.0f);
        JumpAttackArea.enabled = true;
        for (int i = 0; i < 3; i++)
            FieldFireCreate();
        yield return new WaitForSeconds(0.5f);
        JumpAttackArea.enabled = false;
        hitbox.enabled = true;
        yield return new WaitForSeconds(3f);
        UnFreezeAll();
    }
    IEnumerator Kick()
    {
        anim.SetTrigger("KickAttack");
        yield return new WaitForSeconds(1f);
        StartCoroutine("kickAttackAreaOnOff");
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("kickAttackAreaOnOff");
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("kickAttackAreaOnOff");
        yield return new WaitForSeconds(1f);
        StartCoroutine("kickAttackAreaOnOff");
        yield return new WaitForSeconds(2f);
        UnFreezeAll();
    }
    IEnumerator kickAttackAreaOnOff()
    {
        KickAttackArea.enabled = true;
        yield return new WaitForSeconds(0.3f);
        KickAttackArea.enabled = false;
    }
    public override void IsAttacked(int _damage, Vector3 _player)
    {
        curHealth -= _damage;
        Vector3 reactVec = transform.position - _player; // 넉백 거리.
        StartCoroutine(OnDamage(reactVec*0.2f));
        hpbar = Enemy_HP_UI.GetObject();
        hpbar.Recognize(this);
        if(EffectManager.Instance != null)
            EffectManager.Instance.PlayHitEffect(transform.position + offset, transform.rotation.eulerAngles, transform);

    }
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
            hitbox.enabled = false;
            target = null;
            isDead = true;
            anim.SetTrigger("isDead");
            FreezeEnemy();

            questProgress();
            DropExp();
            DropGold();
            DropItem();
            Destroy(gameObject,10f);
        }
    }

    void FreezeVelocity()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    void FreezeBoss()
    {
        isStop = true;
        DoAttack = true;
    }
    void UnfreezeBoss()
    {
        isStop = false;
        DoAttack = false;
    }
    /// <summary>
    /// 스킬에 따른 다른 피격모션 재생
    /// </summary>
    /// <param name="playerSkill">-1:Idle상태, 평타 1,2타, 0:플레이어 평타 3번째, 1:스킬1, 2:스킬2</param>
    void isAttackedAnimPlay(int playerSkill)
    {
        Debug.Log(HitSkillNum);
        anim.SetTrigger("isAttacked");
        anim.SetInteger("HitNum",playerSkill);
    }
    public void WeaponEffectOnOff(bool state)
    {
        BossWeaponFire.SetActive(state);
    }
    void FieldFireCreate()
    {
        GameObject tmp = Instantiate<GameObject>(FieldFire);
        tmp.transform.position = transform.position + new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
    }
    public void ClearAttackCool()
    {
        FreezeEnemy();
        FreezeBoss();
        atkTime = attackCool - 3f;
        Invoke("UnFreezeAll", 3f);
    }
    public void MeleeAreaDisEnable()
    {
        KickAttackArea.enabled = false;
        JumpAttackArea.enabled = false;
        BossWeapon.enabled = false;
    }
    void UnFreezeAll()
    {
        UnfreezeBoss();
        UnfreezeEnemy();
    }
    void ShootFire()
    {
        Instantiate(Fireball, BossWeapon.transform.position, transform.rotation);
    }
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
    void BossAttackIntermission()
    {
        Invoke("UnfreezeEnemy",2f);
    }
    public void stunWakeUp()
    {
        anim.SetTrigger("StunWakeUP");
        Invoke("BossAwake", 3f);
        isStun = false;
    }
    void BossAwake()
    {
        isAwake = true;
    }
}
