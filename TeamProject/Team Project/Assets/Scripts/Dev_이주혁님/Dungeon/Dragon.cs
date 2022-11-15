using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dragon : Enemy
{    
    public float skillCool;    
    [Header("스킬 공격 관련")]
    public float skillDistance;
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
        isStop = false;
    }
    
    void FixedUpdate()
    {
        if (nav.velocity != Vector3.zero)
        {
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }

        atkTime += Time.fixedDeltaTime;
        if (nav.enabled)
        {
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
                    FreezeEnemy();
                    if (atkTime >= attackCool)
                    {
                        anim.SetTrigger("isAttack");
                        atkTime = 0f;
                        isStop = true;
                    }
                }
                else if (distance >= 20f)
                {
                    target = null;
                    StartCoroutine(Targeting());
                }
                else if (distance > skillDistance && atkTime >= skillCool) 
                {
                    anim.SetTrigger("isSkill");
                    atkTime = 0f;
                    isStop = true;
                }
            }
            else
            {
                nav.SetDestination(originPos);
                UnfreezeEnemy();
            }
        }
               
    }

    public override void IsAttacked(int _damage, Vector3 _player)
    {
        curHealth -= _damage;
        Vector3 reactVec = transform.position - _player; // 넉백 거리.
        StartCoroutine(OnDamage(reactVec, _damage));
        hpbar = Enemy_HP_UI.GetObject();
        hpbar.Recognize(this);
        EffectManager.Instance.PlayHitEffect(transform.position + offset, transform.rotation.eulerAngles, transform);
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
            rigid.AddForce(reactVec * 10, ForceMode.Impulse);
        }
        else
        {
            hitbox.enabled = false;
            anim.SetTrigger("isDead");                                
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 10, ForceMode.Impulse);
            DropExp();
            DropGold();
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
    void SoundGrowl()
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.MONSTER_GROWL);
    }
}
