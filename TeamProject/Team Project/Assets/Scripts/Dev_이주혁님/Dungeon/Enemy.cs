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
    public int dropExp;               // ���Ͱ� ����ϴ� ����ġ.
    public int dropGold;              // ���Ͱ� ����ϴ� ���.
    //public ItemData[] dropItem;       // ���Ͱ� ����ϴ� ������.
    [Space(10f)]
    [Header("�ν� ���� ���� ������Ƽ")]
    public float targetRadius;  // ������ �ν� ����.
    public float targetRange;   // ������ �ν� �Ÿ�
    public float attackDistance;// ���Ͱ� ������ �ϴ� �Ÿ�.
    protected Transform target;        // Ÿ��. (�÷��̾�)                    
    protected Vector3 originPos;      // ������ �ʱ� ��ġ.    
    protected Rigidbody rigid;
    protected Collider hitbox;
    //private Material mat;
    protected NavMeshAgent nav;
    protected Animator anim;
    protected float atkTime;      // ���� ��Ÿ��. Unirx�� ��ü����.
    private void Awake()
    {        
        rigid = GetComponent<Rigidbody>();
        hitbox = GetComponent<Collider>();
        //mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        originPos = transform.position;                
        atkTime = 0f;        
    }
    protected void Start()
    {
        StartCoroutine(Targeting());      
    }
    private void FixedUpdate()
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
    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Attack(other);
            Debug.Log("����");
        }
    }
    protected void FreezeEnemy ()
    {
        nav.isStopped = true;    
    }           // ���Ͱ� �������� ���ϰ� ��.
    protected void UnfreezeEnemy()
    {
        nav.isStopped = false;
    }          // ���Ͱ� ������ �� �ְ� ��.
    protected void WakeUp()
    {
        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position, 4f, transform.forward, 0f, LayerMask.GetMask("Enemy"));
        if (rayHits.Length > 0)
        {
            for (int i = 0; i < rayHits.Length; ++i)
            {
                Enemy other = rayHits[i].transform.GetComponent<Enemy>();
                other.target = this.target;
            }

        }
    }                 // �ֺ��� �ִ� ���͸� ����.
    protected IEnumerator Targeting()
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
    }       // ������ ���� ������ �÷��̾ ������ Ÿ������ ������.    
    public virtual void Attack(Collider _player)
    {
        Player player = _player.GetComponent<Player>();
        if (player != null)
        {
            int damage = Mathf.CeilToInt(atkPoint * atkMag * (1 - player.playerStat.defMag) * Random.Range(0.95f, 1.05f));
            player.IsAttacked(damage);
        }
    }
    public virtual void IsAttacked(int _damage)
    {
        curHealth -= _damage;
        Vector3 reactVec = transform.position - Player.instance.transform.position; // �˹� �Ÿ�.
        StartCoroutine(OnDamage(reactVec));        
    }   
    protected IEnumerator OnDamage(Vector3 reactVec)
    {        
        yield return new WaitForSeconds(0.1f);
        if(curHealth > 0)
        {            
            anim.SetTrigger("isAttacked");
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec*5, ForceMode.Impulse);
        }
        else
        {
            hitbox.enabled = false;
            anim.SetTrigger("isDead");
            nav.enabled = false;
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            DropExp();
            Destroy(gameObject, 4);
        }       
    }

    protected void LookTarget()
    {
        if (target != null)
        {            
            transform.LookAt(target);
        }              
    }
    protected void DropExp()
    {
        if (target != null)
        {
            Player player = target.GetComponent<Player>();
            if (player != null)
            {
                player.playerStat.curExp += dropExp;
            }
        }
    }
}