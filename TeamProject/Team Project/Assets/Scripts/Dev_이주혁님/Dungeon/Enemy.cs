using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;


public class Enemy : MonoBehaviour
{
    [Header("몬스터 스탯 관련 프로퍼티")]
    public int maxHealth;             // 최대 체력.
    public int curHealth;             // 현재 체력.
    public float defMag;              // 방어율.
    public int atkPoint;              // 몬스터의 공격력.
    public float atkMag;              // 몬스터의 공격 배율.
    [Header("몬스터 드랍 관련 프로퍼티")]
    public int dropExp;               // 몬스터가 드랍하는 경험치.
    public int dropGold;              // 몬스터가 드랍하는 골드.    
    // 두 리스트는 크기가 같아야 함.
    public List<int> dropItem;       // 몬스터가 드랍하는 아이템 ID.
    public List<float> dropPro;        // 몬스터가 드랍하는 아이템의 드랍 확률.
    [Space(10f)]
    [Header("인식 범위 관련 프로퍼티")]
    public float targetRadius;  // 몬스터의 인식 범위.
    public float targetRange;   // 몬스터의 인식 거리
    public float attackDistance;// 몬스터가 공격을 하는 거리.
    protected Transform target;        // 타겟. (플레이어)                    
    protected Vector3 originPos;      // 몬스터의 초기 위치.    
    protected Rigidbody rigid;
    protected Collider hitbox;
    //private Material mat;
    protected NavMeshAgent nav;
    protected Animator anim;
    protected float atkTime;      // 공격 쿨타임. Unirx로 교체예정.
    public GameObject fieldItem;
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
            Debug.Log("공격");
        }
    }
    protected void FreezeEnemy ()
    {
        nav.isStopped = true;    
    }           // 몬스터가 움직이지 못하게 함.
    protected void UnfreezeEnemy()
    {
        nav.isStopped = false;
    }          // 몬스터가 움직일 수 있게 함.
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
    }                 // 주변에 있는 몬스터를 깨움.
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
            atkTime = 1.5f;
            yield break;
        }        
        StartCoroutine(Targeting());
    }       // 설정된 전방 범위에 플레이어가 있으면 타겟으로 설정함.    
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
        Vector3 reactVec = transform.position - Player.instance.transform.position; // 넉백 거리.
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
            FreezeEnemy();
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            DropExpAndGold();
            DropItem();
            questProgress();
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

    // 경험치와 골드를 드랍. 아이템은 플레이어의 스탯에 직접 반영되는 것이 아닌 필드에 드랍되므로 별도의 메소드를 사용.
    protected void DropExpAndGold()
    {
        if (target != null)
        {
            Player player = target.GetComponent<Player>();
            if (player != null)
            {
                player.playerStat.CurExp += dropExp;
                player.playerStat.Gold += dropGold;
                JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].exp = player.playerStat.CurExp;
                JY_CharacterListManager.s_instance.saveListData();
            }
        }
    }
    protected void DropItem()
    {
        for (int i = 0; i < dropItem.Count; i++)
        {
            if (Random.Range(0f, 1f) <= dropPro[i])
            {
                FieldItem tmp = Instantiate<GameObject>(fieldItem, transform.position, Quaternion.identity).GetComponent<FieldItem>();
                tmp.itemID = dropItem[i];
            }
            
        }
        
    }
    void questProgress()
    {
        if (JY_QuestManager.s_instance != null && this.gameObject.name == JY_QuestManager.s_instance.QuestData[0][3])
            JY_QuestManager.s_instance.QuestProgress(0);
    }
}