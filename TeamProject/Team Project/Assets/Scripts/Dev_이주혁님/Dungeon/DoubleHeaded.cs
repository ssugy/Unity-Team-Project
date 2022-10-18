using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoubleHeaded : Enemy
{    
    public float skillCool;
    [Header("스킬 공격 관련")]
    public float skillDistance;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        hitbox = GetComponent<Collider>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        originPos = transform.position;
        atkTime = 0f;
    }
    // 보스는 타겟팅 기능을 사용하지 않음. 보스룸에 들어온 것을 감지하는 콜라이더로 target을 설정.
    // 보스룸에 플레이어가 들어오면 도망칠 수 없으므로 target이 null로 바뀌었을 때의 조건은 필요없음.
    protected new void Start()
    {

    }
    private void FixedUpdate()
    {
        atkTime += Time.fixedDeltaTime;
        if (target != null)
        {
            nav.SetDestination(target.position);
            float distance = (transform.position - target.position).magnitude;
            if (distance <= attackDistance)
            {
                FreezeEnemy();
                if (atkTime >= 5f)
                {
                    anim.SetTrigger("isAttack");
                    atkTime = 0f;
                }
            }
            else if (distance > skillDistance && atkTime >= skillCool)
            {
                anim.SetTrigger("isSkill");
                atkTime = 0f;
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
        if (curHealth <= 0)
        {
            hitbox.enabled = false;
            anim.SetTrigger("isDead");
            FreezeEnemy();            
            DropExpAndGold();
            DropItem();
            questProgress();
            Destroy(gameObject, 4);
        }       
    }    
}
