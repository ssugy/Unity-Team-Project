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
    protected Vector3 originPos;      // ������ �ʱ� ��ġ.    
    protected Quaternion originRot;   // ������ �ʱ� ȸ��.
       
    protected Collider hitbox;    
    protected NavMeshAgent nav;
    protected Animator anim;
   
    protected HP_Bar hpbar;         // HP ��.
    protected float attackTime;    // ���Ͱ� ������ �ϴ� �ð�    
    
    protected bool isStop = false;
    protected bool isDead = false;
    protected float atkTime = 0f;     // ���� ��Ÿ��. Unirx�� ��ü����.
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
        // Ÿ���� ���� �� Ÿ���� ���� �̵��� Ÿ�� ������ ���� ���� �Ÿ��� �޶���.
        // (Ÿ���� ������ ���ڸ��� ���� �ϹǷ� ���� �Ÿ��� 0)
        nav.stoppingDistance = target ? stoppingDist : 0f;
        nav.SetDestination(target ? target.position : originPos);

        // ���� nav�� �ӵ��� 0�� �ƴϸ� isWalk = true
        anim.SetBool("isWalk", (nav.velocity != Vector3.zero));               
    }

    protected virtual void Rotate()
    {
        // Ÿ���� ���� ��� Ÿ���� �ٶ󺸵��� ��.
        if (target != null)
        {
            Vector3 dir = target.transform.position - this.transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, 
                Quaternion.LookRotation(dir), Time.deltaTime);
        }
        // Ÿ���� ���� ��� ���ڸ��� �ǵ��ư� �� ���� ������ �ٶ󺸵��� ��.
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
            // �Ÿ��� �ʹ� �־����� Ÿ���� Ǯ��.
            else if (distance >= 15f)
            {
                atkTime = 0f;
                target = null;                
                curHealth = maxHealth;
                StartCoroutine(ReTargeting(2f));
            }

            // 8�� �̻� ������ ���ϸ� Ÿ���� Ǯ��.
            if (atkTime > 8f)
            {
                atkTime = 0f;                
                target = null;
                curHealth = maxHealth;
                StartCoroutine(ReTargeting(2f));              
            }
        }        
    }
    // 0.5�� �������� ���濡 �÷��̾ ������ Ÿ������ ������.    
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
        // ������(ȣ��Ʈ)������ ������ ��.
        if (!PhotonNetwork.IsMasterClient)
            return;

        // �������� �˹� ����. �ǰ� �ִϸ��̼� Ʈ���Ÿ� Set
        CurHealth -= _damage;
        anim.SetTrigger("isAttacked");

        Vector3 reactVec = transform.position - _attacker; // �˹� �Ÿ�.        
        reactVec = reactVec.normalized;
        reactVec += Vector3.up;
        StartCoroutine(KnockBack(reactVec));
        //rigid.AddForce(reactVec * 10, ForceMode.Impulse);

        // �ǰݿ� ���� ����Ʈ ��°� UI Ȱ��ȭ�� ��� Ŭ���̾�Ʈ����.
        photonView.RPC("HitEffect", RpcTarget.All);        
    }    
    
    [PunRPC]
    public virtual void HitEffect()
    {
        hpbar = Enemy_HP_UI.GetObject();
        hpbar.Recognize(this);
        EffectManager.Instance.PlayHitEffect(transform.position + offset, transform.rotation, transform);
    }

    // ����ġ�� ��带 ���. �������� �÷��̾��� ���ȿ� ���� �ݿ��Ǵ� ���� �ƴ� �ʵ忡 ����ǹǷ� ������ �޼ҵ带 ���.
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
    protected void FreezeEnemy() => isStop = true;               
    protected void UnfreezeEnemy() => isStop = false;

    // ���Ͱ� Ÿ���� �ٶ�.  
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
    }                 // �ֺ��� �ִ� ���͸� ����.    
        
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