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
    bool isLook;
    public bool isAwake;
    bool DoAttack;
    [Header("보스 공격 범위 콜라이더")]
    public BoxCollider JumpAttackArea;
    public BoxCollider BossWeapon;


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
    }
    private void Start()
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
            transform.LookAt(target.position+lookVec);
        }
        if (target != null && isAwake)
        {
            nav.SetDestination(target.position);
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance <= attackDistance && !DoAttack)
            {
                FreezeEnemy();
                FreezeBoss();
                StartCoroutine(BossPattern(3));
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
}
