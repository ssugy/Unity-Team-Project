using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Enemy : MonoBehaviourPun, IPunObservable
{
    [Header("몬스터 스탯 관련 프로퍼티")]
    public int maxHealth;             // 최대 체력.
    protected int curHealth;             // 현재 체력.
    public int CurHealth
    {
        get => curHealth;
        set => curHealth = value;                 
    }
    
    public float defMag;              // 방어율.
    public int atkPoint;              // 몬스터의 공격력.
    public float atkMag;              // 몬스터의 공격 배율.
    public Vector3 offset;
    [Header("몬스터 드랍 관련 프로퍼티")]
    public int dropExp;               // 몬스터가 드랍하는 경험치.
    public int dropGold;              // 몬스터가 드랍하는 골드.    
    // 두 리스트는 크기가 같아야 함.
    public List<int> dropItem;       // 몬스터가 드랍하는 아이템 ID.
    public List<float> dropPro;        // 몬스터가 드랍하는 아이템의 드랍 확률.
    public GameObject fieldItem;       // 몬스터가 드랍하는 아이템 프리팹.   
    public GameObject fieldGold;    // 금화 프리팹.
    [Header("인식 범위 관련 프로퍼티")]
    public float targetRadius;  // 몬스터의 인식 범위.
    public float targetRange;   // 몬스터의 인식 거리
    public float attackDistance;// 몬스터가 공격을 하는 거리.
    [Header("쿨타임 관련")]
    public float attackCool;    

    public Transform target;        // 타겟. (플레이어)                    
    public Vector3 originPos;      // 몬스터의 초기 위치.    
    public Quaternion originRotateion;
    protected Rigidbody rigid;
    protected Collider hitbox;    
    protected NavMeshAgent nav;
    protected Animator anim;
    protected float atkTime;      // 공격 쿨타임. Unirx로 교체예정.
    protected HP_Bar hpbar;         // HP 바.
    protected float attackTime;    // 몬스터가 공격을 하는 시간
    
    bool isBorder;
    protected bool isStop;
    public float totalTime = 0f;

    protected float stoppingDist;


    private void Awake()
    {        
        rigid = GetComponent<Rigidbody>();
        hitbox = GetComponent<Collider>();        
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        originPos = transform.position;
        originRotateion = transform.rotation;
        atkTime = 0f;
        isStop = false;
        CurHealth = maxHealth;
        stoppingDist = nav.stoppingDistance;
    }
    protected void Start()
    {        
        StartCoroutine(Targeting());        
    }

    
    void StopToWall()
    {
        RaycastHit[] hit;

        hit = Physics.SphereCastAll(transform.position, 1f ,transform.forward, 1f, LayerMask.GetMask("Wall"));
        if(hit.Length > 0 )        
            isBorder = true;        
        else        
            isBorder = false;                      
    }

    private void FixedUpdate()
    {
        if (nav.velocity != Vector3.zero)        
            anim.SetBool("isWalk", true);        
        else        
            anim.SetBool("isWalk", false);
        

        atkTime += Time.fixedDeltaTime;        
        if (target != null)
        {            
            nav.SetDestination(target.position);
            nav.stoppingDistance = stoppingDist;

            float distance = Vector3.Distance(transform.position, target.position);
            if (!isStop)
            {
                Vector3 dir = target.transform.position - this.transform.position;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime);
                anim.SetBool("isWalk", true);
            }            
            
            if (distance <= attackDistance)
            {
                totalTime = 0f;
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
                curHealth = maxHealth;
            }
            
            StopToWall();            
            totalTime = isBorder ? totalTime + Time.deltaTime : 0f;
            
            if (totalTime>=5f)
            {
                totalTime = 0f;
                target = null;
                Invoke("ReStartTarget", 2f);
                curHealth = maxHealth;
            }           
        }
        else
        {
            nav.SetDestination(originPos);
            nav.stoppingDistance = 0f;
            UnfreezeEnemy();
        }
      
        

        if (!nav.pathPending && target == null) 
        {            
            if (nav.remainingDistance <= nav.stoppingDistance)
            {                
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, originRotateion, Time.deltaTime * 5);
            }
        }
    }

    
    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Attack(other);            
        }
    }  
   
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
            float damage = atkPoint * atkMag * (1 - player.playerStat.defMag) 
                * Random.Range(0.95f, 1.05f);
            if (damage > 0)
            {                               
                player.IsAttacked(Mathf.CeilToInt(damage), hitbox);
            }
        }
    }
    public virtual void IsAttacked(int _damage, Vector3 _player)
    {
        //photonView.RPC("IsAttacked_Do", RpcTarget.All, _damage, _player);
        IsAttacked_Do(_damage, _player);
    }   
    [PunRPC]
    public virtual void IsAttacked_Do(int _damage, Vector3 _player)
    {
        curHealth -= _damage;
        Vector3 reactVec = transform.position - _player; // 넉백 거리.
        StartCoroutine(OnDamage(reactVec));
        hpbar = Enemy_HP_UI.GetObject();
        hpbar.Recognize(this);
        EffectManager.Instance.PlayHitEffect(transform.position + offset, transform.rotation, transform);
    }
    protected IEnumerator OnDamage(Vector3 reactVec)
    {        
        yield return new WaitForSeconds(0.1f);
        if(curHealth > 0)
        {            
            anim.SetTrigger("isAttacked");
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            StartCoroutine(KnockBack(reactVec));
            
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

    // 경험치와 골드를 드랍. 아이템은 플레이어의 스탯에 직접 반영되는 것이 아닌 필드에 드랍되므로 별도의 메소드를 사용.
    protected void DropExp()
    {
        Player player = JY_CharacterListManager.s_instance.playerList[0];
        if (player != null) 
        {
            player.playerStat.CurExp += dropExp;
            player.SaveData();
            JY_CharacterListManager.s_instance.Save();
        }                   
    }
    protected void DropGold()
    {
        FieldGold tmp = Instantiate(fieldGold, transform.position, Quaternion.identity).GetComponent<FieldGold>();
        tmp.ammount = Random.Range(dropGold, (int)(dropGold * 1.15f));
    }
    protected void DropItem()
    {
        for (int i = 0; i < dropItem.Count; i++)
        {
            if (Random.Range(0f, 1f) <= dropPro[i])
            {
                // FieldItem 프리팹을 인스턴스화하고 아이템 데이터베이스에서 아이템을 복사함.
                FieldItem tmp = Instantiate(fieldItem, transform.position, Quaternion.identity).GetComponent<FieldItem>();                
                tmp.item = ItemDatabase.s_instance.itemDB[dropItem[i]].Copy();
            }
            
        }        
    }
    protected virtual void questProgress()
    {
        if (JY_QuestManager.s_instance != null && this.gameObject.name == JY_QuestManager.s_instance.QuestData[0][3])
            JY_QuestManager.s_instance.QuestProgress(0);
    }


    // 애니메이션 이벤트 함수들.
    protected void FreezeEnemy()
    {
        nav.isStopped = true;
    }            // 몬스터가 움직이지 못하게 함. nav.isStopped
    protected void UnfreezeEnemy()
    {
        nav.isStopped = false;
    }          // 몬스터가 움직일 수 있게 함.
    protected void LookTarget()
    {
        if (target != null)
        {
            transform.LookAt(target);
        }
    }             // 몬스터가 타겟을 바라봄.    
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
    protected void DragunReturn()
    {
        isStop = false;
    }
    public void ReStartTarget()
    {
        StartCoroutine(Targeting());
    }
    public IEnumerator KnockBack(Vector3 _dir)
    {
        for (int i = 0; i < 20; i++)
        {
            nav.Move(_dir * 0.03f);
            yield return null;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(curHealth);
        }
        else
        {
            CurHealth = (int)stream.ReceiveNext();
        }
    }
}