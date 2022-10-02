using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int maxHealth;           // �ִ� ü��.
    public int curHealth;           // ���� ü��.
    public float defMag;            // �����.
    public Transform target;        // Ÿ��. (�÷��̾�)
    public BoxCollider meleeArea;   // ������ ���� ��Ʈ�ڽ�.
    public bool isChase;
    public bool isAttack;
    public int atkPoint;            // ������ ���ݷ�.
    public float atkMag;              // ������ ���� ����.
    
    Vector3 originPos;
    [Header("ü�¹�")]
    [SerializeField] Transform hp;
    [SerializeField] Camera cam;
    [SerializeField] Slider hp_slider;

    Rigidbody rigid;
    BoxCollider enemyHitbox;
    Material mat;
    NavMeshAgent nav;
    Animator anim;
    void Awake()
    {
        target = Player.instance.transform;
        rigid = GetComponent<Rigidbody>();
        enemyHitbox = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();        
        cam = Camera.main;
        hp_slider.value = hp_slider.maxValue;
        

        Invoke("ChaseStart", 2);
        originPos = transform.position;
    }
    
    void Update()
    {
        Quaternion q_hp = Quaternion.LookRotation(hp.position - cam.transform.position);

        Vector3 hp_angle = Quaternion.RotateTowards(hp.rotation, q_hp, 200).eulerAngles;

        hp.rotation = Quaternion.Euler(0, hp_angle.y, 0);                     
    }
    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }
   

    void FreezeVelocity ()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
        

    }

    void Targeting()
    {
        float targetRadius = 1.8f;
        float targetRange = 0.1f;

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
                                  targetRadius,
                                  transform.forward,
                                  targetRange,
                                  LayerMask.GetMask("Player"));
        if(rayHits.Length > 0 && !isAttack)
        {
            StartCoroutine(Attak());
        }
    }

    IEnumerator Attak()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack", true);
       
        yield return new WaitForSeconds(0.2f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(1f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(1f);

        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }
    void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();

        float distance = (transform.position - target.position).magnitude;
        if (nav.enabled)
        {
            if (distance <= 10f)
            {
                nav.SetDestination(target.position);
                nav.isStopped = !isChase;
            }
            else
            {
                nav.SetDestination(originPos);
            }
        }        
    }

    public void IsAttacked(int _damage)
    {
        curHealth -= _damage;
        Vector3 reactVec = transform.position - Player.instance.transform.position; //������ �˹�
        StartCoroutine(OnDamage(reactVec));
        hp_slider.value = (float)curHealth / maxHealth;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Attack(other);
        }
        /*if (other.tag == "Melee")               //�������� ��������
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position; //������ �˹�
            StartCoroutine(OnDamage(reactVec));
            
        }*/
        /*else if (other.tag == "Magic")         //�������� ��������
        {
            Magic magic = other.GetComponent<Magic>();
            curHealth -= magic.damage;
            Vector3 reactVec = transform.position - other.transform.position; //������ �˹�
            Destory(other.gameObject);   ������ ������ ������� �������� 

            StartCoroutine(OnDamage(reactVec));
           
        }*/
    }

    IEnumerator OnDamage(Vector3 reactVec)
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if(curHealth > 0)
        {
            mat.color = Color.white;
        }
        else
        {
            enemyHitbox.enabled = false;
            nav.enabled = false;
            mat.color = Color.gray;
            gameObject.layer = 11;
            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("doDie");
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);

            Destroy(gameObject, 4);
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
}