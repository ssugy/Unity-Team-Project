using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;


public class Enemy : MonoBehaviour
{
    [Header("몬스터 스탯")]
    public int maxHealth;             // 최대 체력.
    public int curHealth;             // 현재 체력.
    public float defMag;              // 방어율.
    public int atkPoint;              // 몬스터의 공격력.
    public float atkMag;              // 몬스터의 공격 배율.
    [Space(10f)]
    [Header("인식 범위 관련 프로퍼티")]
    public float targetRadius; // 몬스터의 인식 범위.
    public float targetRange;  // 몬스터의 인식 거리
    private Transform target;        // 타겟. (플레이어)                    
    private Vector3 originPos;      // 몬스터의 초기 위치.    
    private Rigidbody rigid;
    private BoxCollider hitbox;
    //private Material mat;
    private NavMeshAgent nav;
    private Animator anim;
    private float atkTime;      // Unirx로 교체예정.
    void Awake()
    {        
        rigid = GetComponent<Rigidbody>();
        hitbox = GetComponent<BoxCollider>();
        //mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        originPos = transform.position;
                
        atkTime = 0f;
        
    }
    private void Start()
    {
        StartCoroutine(Targeting());      
    }       

    void FreezeEnemy ()
    {
        nav.isStopped = true;    
    }    
    void UnfreezeEnemy()
    {
        nav.isStopped = false;
    }

    IEnumerator Targeting()
    {
        yield return new WaitForSeconds(0.5f);       
        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
                                  targetRadius,
                                  transform.forward,
                                  targetRange,
                                  LayerMask.GetMask("Player"));
        if(rayHits.Length > 0)
        {
            target = rayHits[0].transform;            
            yield break;
        }        
        StartCoroutine(Targeting());
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
                if (distance <= 1.7f)
                {
                    FreezeEnemy();
                    if (atkTime >= 2.5f)
                    {
                        anim.SetTrigger("isAttack");
                        atkTime = 0f;
                    }                                      
                }
                else if (distance >= 15f)
                {
                    target = null;                    
                    StartCoroutine(Targeting());
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
    public void Attack(Collider _player)
    {
        Player player = _player.GetComponent<Player>();
        if (player != null)
        {
            int damage = Mathf.CeilToInt(atkPoint * atkMag * (1 - player.playerStat.defMag) * Random.Range(0.95f, 1.05f));
            player.IsAttacked(damage);
        }
    }
    public void IsAttacked(int _damage)
    {
        curHealth -= _damage;
        Vector3 reactVec = transform.position - Player.instance.transform.position; // 넉백 거리.
        StartCoroutine(OnDamage(reactVec));        
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Attack(other);
        }        
    }

    IEnumerator OnDamage(Vector3 reactVec)
    {
        //mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if(curHealth > 0)
        {
            //mat.color = Color.white;
            anim.SetTrigger("isAttacked");
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec*5, ForceMode.Impulse);
        }
        else
        {
            hitbox.enabled = false;
            nav.enabled = false;
            //mat.color = Color.gray;                     
            nav.enabled = false;
            anim.SetTrigger("isDead");
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            Destroy(gameObject, 4);
        }

       
    }
    
}