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
    public virtual int CurHealth
    {
        get => curHealth;
        set
        {
            curHealth = value;
            if (curHealth >= maxHealth)
                curHealth = maxHealth;
            else if(curHealth <= 0 && !isDead)
            {
                isDead = true;
                hitbox.enabled = false;
                anim.SetBool("isDead", true);

                FreezeEnemy();
                questProgress();

                DropExp();
                DropGold();
                DropItem();

                Destroy(gameObject, 4);
            }            
        }                
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
    protected Vector3 originPos;      // 몬스터의 초기 위치.    
    protected Quaternion originRot;   // 몬스터의 초기 회전.
       
    protected Collider hitbox;    
    protected NavMeshAgent nav;
    protected Animator anim;
   
    protected HP_Bar hpbar;         // HP 바.
    protected float attackTime;    // 몬스터가 공격을 하는 시간    
    
    protected bool isStop = false;
    protected bool isDead = false;
    protected float atkTime = 0f;     // 공격 쿨타임. Unirx로 교체예정.
    protected float stoppingDist;

    protected virtual void Awake()
    {               
        hitbox = GetComponent<Collider>();        
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        originPos = transform.position;
        originRot = transform.rotation;
        
        CurHealth = maxHealth;
        stoppingDist = nav.stoppingDistance;
    }

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)                   
            return;        
            
        StartCoroutine(Targeting());        
    }
    

    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        anim.SetBool("isWalk", false);
        if (!isStop)
        {
            Move();
            Rotate();
            Attack();
        }               
    }

    protected virtual void Move()
    {
        // 타겟이 있을 때 타겟을 향해 이동함 타겟 유무에 따라 정지 거리가 달라짐.
        // (타겟이 없으면 제자리로 가야 하므로 정지 거리가 0)
        nav.stoppingDistance = target ? stoppingDist : 0f;
        nav.SetDestination(target ? target.position : originPos);

        // 현재 nav의 속도가 0이 아니면 isWalk = true
        anim.SetBool("isWalk", (nav.velocity != Vector3.zero));               
    }

    protected virtual void Rotate()
    {
        // 타겟이 있을 경우 타겟을 바라보도록 함.
        if (target != null)
        {
            Vector3 dir = target.transform.position - this.transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, 
                Quaternion.LookRotation(dir), Time.deltaTime);
        }
        // 타겟이 없을 경우 제자리로 되돌아간 후 원래 방향을 바라보도록 함.
        else
        {            
            if (nav.remainingDistance <= 0.2f)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    originRot, Time.deltaTime * 5);
            }
        }
    }

    protected virtual void Attack()
    {                
        if (target != null)
        {
            atkTime += Time.fixedDeltaTime;
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance <= attackDistance && atkTime >= attackCool)
            {
                atkTime = 0f;
                anim.SetTrigger("isAttack");
            }
            // 거리가 너무 멀어지면 타겟이 풀림.
            else if (distance >= 15f)
            {
                atkTime = 0f;
                target = null;                
                curHealth = maxHealth;
                StartCoroutine(ReTargeting(2f));
            }

            // 8초 이상 공격을 못하면 타겟이 풀림.
            if (atkTime > 8f)
            {
                atkTime = 0f;                
                target = null;
                curHealth = maxHealth;
                StartCoroutine(ReTargeting(2f));              
            }
        }        
    }
    // 0.5초 간격으로 전방에 플레이어가 있으면 타겟으로 설정함.    
    protected IEnumerator Targeting()
    {
        yield return new WaitForSeconds(0.5f);
        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
                                  targetRadius,
                                  transform.forward,
                                  targetRange,
                                  LayerMask.GetMask("Player"));
        if (rayHits.Length > 0)
        {
            target = rayHits[0].transform;
            atkTime = 1.5f;
            yield break;
        }
        StartCoroutine(Targeting());
    }
    protected IEnumerator ReTargeting(float _time)
    {
        yield return new WaitForSeconds(_time);
        StartCoroutine(Targeting());
    }   

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Attack(other);            
        }
    }  
   
         
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

    [PunRPC]
    public virtual void OnDamage(int _damage, Vector3 _attacker)
    {
        // 마스터(호스트)에서만 연산을 함.
        if (!PhotonNetwork.IsMasterClient)
            return;

        // 데미지와 넉백 연산. 피격 애니메이션 트리거를 Set
        CurHealth -= _damage;
        anim.SetTrigger("isAttacked");

        Vector3 reactVec = transform.position - _attacker; // 넉백 거리.        
        reactVec = reactVec.normalized;
        reactVec += Vector3.up;
        StartCoroutine(KnockBack(reactVec));
        //rigid.AddForce(reactVec * 10, ForceMode.Impulse);

        // 피격에 따른 이펙트 출력과 UI 활성화는 모든 클라이언트에게.
        photonView.RPC("HitEffect", RpcTarget.All);        
    }    
    
    [PunRPC]
    public virtual void HitEffect()
    {
        hpbar = Enemy_HP_UI.GetObject();
        hpbar.Recognize(this);
        EffectManager.Instance.PlayHitEffect(transform.position + offset, transform.rotation, transform);
    }

    // 경험치와 골드를 드랍. 아이템은 플레이어의 스탯에 직접 반영되는 것이 아닌 필드에 드랍되므로 별도의 메소드를 사용.
    protected void DropExp()
    {
        Player player = JY_CharacterListManager.s_instance.playerList[0];
        player.playerStat.CurExp += dropExp;
        player.SaveData();
        JY_CharacterListManager.s_instance.Save();
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
    protected void FreezeEnemy() => isStop = true;               
    protected void UnfreezeEnemy() => isStop = false;

    // 몬스터가 타겟을 바라봄.  
    protected void LookTarget()
    {
        if (target != null)        
            transform.LookAt(target, Vector3.up);        
    }               
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
        
    public IEnumerator KnockBack(Vector3 _dir)
    {
        for (int i = 0; i < 20; i++)
        {
            nav.Move(_dir * 0.02f);
            yield return null;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {        
        if (stream.IsWriting)        
            stream.SendNext(curHealth);        
        else        
            CurHealth = (int)stream.ReceiveNext();                
    }
}