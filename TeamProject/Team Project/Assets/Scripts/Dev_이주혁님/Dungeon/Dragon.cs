using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dragon : Enemy
{
    [Space(10f)]
    [Header("원거리 공격 관련")]
    public Transform shooter;       // 화염구를 발사할 위치.
    private GameObject fireballPrefab;    
    

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        hitbox = GetComponent<BoxCollider>();        
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        originPos = transform.position;
        atkTime = 0f;
        fireballPrefab = Resources.Load<GameObject>("Monster\\Fireball");
    }
    
    void FixedUpdate()
    {
        atkTime += Time.fixedDeltaTime;
        if (nav.enabled)
        {
            if (target != null)
            {
                nav.SetDestination(target.position);
                float distance = (transform.position - target.position).magnitude;
                if (distance <= attackDistance)
                {
                    FreezeEnemy();
                    if (atkTime >= 2.5f)
                    {
                        anim.SetTrigger("isAttack");
                        atkTime = 0f;
                    }
                }
                else if (distance >= 20f)
                {
                    target = null;
                    StartCoroutine(Targeting());
                }
                else if (distance > 5f && atkTime >= 2.5f) 
                {
                    anim.SetTrigger("isSkill");
                    atkTime = 0f;
                }
            }
            else
            {
                nav.SetDestination(originPos);
                UnfreezeEnemy();
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
        Vector3 reactVec = transform.position - Player.instance.transform.position; // 넉백 거리.
        StartCoroutine(OnDamage(reactVec, _damage));
    }
    protected IEnumerator OnDamage(Vector3 reactVec, int _damage)
    {        
        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            if (_damage >= (0.2f * maxHealth))
            {
                anim.SetTrigger("isAttacked");
            }
            else
            {
                anim.SetTrigger("isWakeUp");
            }            
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);
        }
        else
        {
            hitbox.enabled = false;
            anim.SetTrigger("isDead");                                
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            DropExpAndGold();
            DropItem();
            Destroy(gameObject, 4);
        }
    }
    void StopNav()
    {
        nav.enabled = false;
    }
    void StartNav()
    {
        nav.enabled = true;
    }
    void ShootFire()
    {
        Instantiate(fireballPrefab, shooter.position, transform.rotation);
    }
}
