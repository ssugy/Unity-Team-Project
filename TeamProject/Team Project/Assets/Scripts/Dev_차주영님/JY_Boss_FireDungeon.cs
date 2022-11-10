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
    [Header("보스 공격 범위 콜라이더")]
    public BoxCollider JumpAttackArea;
    public BoxCollider KickAttackArea;
    public BoxCollider BossWeapon;

    [HideInInspector] public int HitSkillNum;

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
        HitSkillNum = -1;
    }
    private new void Start()
    {
        target = Player.instance.transform;
        isLook = true;
        isAwake = false;
    }

    private void FixedUpdate()
    {
        FreezeVelocity();
        if (isLook)
        {
            lookVec = Player.instance.movement;
            //transform.LookAt(target.position+lookVec);
            Vector3 dir = target.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime);
        }
        atkTime += Time.fixedDeltaTime;

        if (target != null && isAwake)
        {
            nav.SetDestination(target.position);
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance <= attackDistance && !DoAttack )
            {
                if (atkTime >= attackCool)
                {
                    atkTime = 0f;
                    FreezeEnemy();
                    FreezeBoss();
                    StartCoroutine(BossPattern(4));
                }

            }
        }
        if (nav.velocity != Vector3.zero && !isStop)
        {
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
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
        UnfreezeBoss();
        UnfreezeEnemy();
    }
    IEnumerator WhirlAttack()
    {
        anim.SetTrigger("WhirlAttack");
        yield return new WaitForSeconds(5f);
        UnfreezeBoss();
        UnfreezeEnemy();
    }
    IEnumerator JumpAttack()
    {

        anim.SetTrigger("JumpAttack");
        tauntVec = target.position + lookVec;
        nav.speed = 200f;
        nav.isStopped = false;
        nav.SetDestination(tauntVec);
        hitbox.enabled = false;
        yield return new WaitForSeconds(1.0f);
        JumpAttackArea.enabled = true;
        yield return new WaitForSeconds(0.5f);
        JumpAttackArea.enabled = false;
        nav.isStopped = true;
        hitbox.enabled = true;
        nav.speed = 6f;
        yield return new WaitForSeconds(2f);
        UnfreezeBoss();
        UnfreezeEnemy();
    }
    IEnumerator Kick()
    {
        anim.SetTrigger("KickAttack");
        yield return new WaitForSeconds(5f);
        UnfreezeBoss();
        UnfreezeEnemy();
    }
    public override void IsAttacked(int _damage)
    {
        curHealth -= _damage;
        Vector3 reactVec = transform.position - Player.instance.transform.position; // 넉백 거리.
        StartCoroutine(OnDamage(reactVec*0.2f));
        hpbar = Enemy_HP_UI.GetObject();
        hpbar.Recognize(this);
        EffectManager.Instance.PlayHitEffect(transform.position + offset, transform.rotation.eulerAngles, transform);

    }
    protected new IEnumerator OnDamage(Vector3 reactVec)
    {
        yield return new WaitForSeconds(0.1f);
        if (curHealth > 0)
        {
            if (HitSkillNum != -1)
            {
                isAttackedAnimPlay(HitSkillNum);
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;
                StartCoroutine(KnockBack(reactVec));
            }
        }
        else
        {
            hitbox.enabled = false;
            anim.SetTrigger("isDead");
            FreezeEnemy();
            questProgress();
            DropExp();
            DropGold();
            DropItem();
            Destroy(gameObject, 4);
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
        isLook = false;
    }
    void UnfreezeBoss()
    {
        isStop = false;
        DoAttack = false;
        isLook = true;
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
}
