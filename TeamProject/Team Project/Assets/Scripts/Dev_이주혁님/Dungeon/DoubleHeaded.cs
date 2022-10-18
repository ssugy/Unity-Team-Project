using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoubleHeaded : Enemy
{    
    public float skillCool;
    [Header("��ų ���� ����")]
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
    // ������ Ÿ���� ����� ������� ����. �����뿡 ���� ���� �����ϴ� �ݶ��̴��� target�� ����.
    // �����뿡 �÷��̾ ������ ����ĥ �� �����Ƿ� target�� null�� �ٲ���� ���� ������ �ʿ����.
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
