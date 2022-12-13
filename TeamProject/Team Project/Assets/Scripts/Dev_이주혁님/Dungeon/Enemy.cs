using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Enemy : MonoBehaviourPun, IPunObservable
{
    [Header("���� ���� ���� ������Ƽ")]
    public int maxHealth;             // �ִ� ü��.
    protected int curHealth;             // ���� ü��.
    public int CurHealth
    {
        get => curHealth;
        set => curHealth = value;                 
    }
    
    public float defMag;              // �����.
    public int atkPoint;              // ������ ���ݷ�.
    public float atkMag;              // ������ ���� ����.
    public Vector3 offset;
    [Header("���� ��� ���� ������Ƽ")]
    public int dropExp;               // ���Ͱ� ����ϴ� ����ġ.
    public int dropGold;              // ���Ͱ� ����ϴ� ���.    
    // �� ����Ʈ�� ũ�Ⱑ ���ƾ� ��.
    public List<int> dropItem;       // ���Ͱ� ����ϴ� ������ ID.
    public List<float> dropPro;        // ���Ͱ� ����ϴ� �������� ��� Ȯ��.
    public GameObject fieldItem;       // ���Ͱ� ����ϴ� ������ ������.   
    public GameObject fieldGold;    // ��ȭ ������.
    [Header("�ν� ���� ���� ������Ƽ")]
    public float targetRadius;  // ������ �ν� ����.
    public float targetRange;   // ������ �ν� �Ÿ�
    public float attackDistance;// ���Ͱ� ������ �ϴ� �Ÿ�.
    [Header("��Ÿ�� ����")]
    public float attackCool;    

    public Transform target;        // Ÿ��. (�÷��̾�)                    
    public Vector3 originPos;      // ������ �ʱ� ��ġ.    
    public Quaternion originRotateion;
    protected Rigidbody rigid;
    protected Collider hitbox;    
    protected NavMeshAgent nav;
    protected Animator anim;
    protected float atkTime;      // ���� ��Ÿ��. Unirx�� ��ü����.
    protected HP_Bar hpbar;         // HP ��.
    protected float attackTime;    // ���Ͱ� ������ �ϴ� �ð�
    
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
    }       // ������ ���� ������ �÷��̾ ������ Ÿ������ ������.    
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
        Vector3 reactVec = transform.position - _player; // �˹� �Ÿ�.
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

    // ����ġ�� ��带 ���. �������� �÷��̾��� ���ȿ� ���� �ݿ��Ǵ� ���� �ƴ� �ʵ忡 ����ǹǷ� ������ �޼ҵ带 ���.
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
                // FieldItem �������� �ν��Ͻ�ȭ�ϰ� ������ �����ͺ��̽����� �������� ������.
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


    // �ִϸ��̼� �̺�Ʈ �Լ���.
    protected void FreezeEnemy()
    {
        nav.isStopped = true;
    }            // ���Ͱ� �������� ���ϰ� ��. nav.isStopped
    protected void UnfreezeEnemy()
    {
        nav.isStopped = false;
    }          // ���Ͱ� ������ �� �ְ� ��.
    protected void LookTarget()
    {
        if (target != null)
        {
            transform.LookAt(target);
        }
    }             // ���Ͱ� Ÿ���� �ٶ�.    
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