using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;


public class Enemy : MonoBehaviour
{
    [Header("���� ����")]
    public int maxHealth;             // �ִ� ü��.
    public int curHealth;             // ���� ü��.
    public float defMag;              // �����.
    public int atkPoint;              // ������ ���ݷ�.
    public float atkMag;              // ������ ���� ����.
    [Space(10f)]
    [Header("�ν� ���� ���� ������Ƽ")]
    public float targetRadius; // ������ �ν� ����.
    public float targetRange;  // ������ �ν� �Ÿ�
    private Transform target;        // Ÿ��. (�÷��̾�)                    
    private Vector3 originPos;      // ������ �ʱ� ��ġ.    
    private Rigidbody rigid;
    private BoxCollider hitbox;
    //private Material mat;
    private NavMeshAgent nav;
    private Animator anim;
    private float atkTime;      // Unirx�� ��ü����.
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
        Vector3 reactVec = transform.position - Player.instance.transform.position; // �˹� �Ÿ�.
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