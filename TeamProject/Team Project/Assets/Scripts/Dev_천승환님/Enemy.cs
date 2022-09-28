using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public int maxHealth;
    public int curHealth;
    public int def;
    public Transform target;
    public BoxCollider meleeArea;
    public bool isChase;
    public bool isAttack;
    public Transform player;
    Vector3 originPos;

    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;
    NavMeshAgent nav;
    Animator anim;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        //mat = GetComponentInChildren<MeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        

        Invoke("ChaseStart", 2);
        originPos = transform.position;
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




    void OnTriggerEnter(Collider other)
    {
        /*if (other.tag == "Melee")               //근접공격 당했을떄
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position; //죽을때 넉배
            StartCoroutine(OnDamage(reactVec));
            
        }*/
        /*else if (other.tag == "Magic")         //마법공격 당했을때
        {
            Magic magic = other.GetComponent<Magic>();
            curHealth -= magic.damage;
            Vector3 reactVec = transform.position - other.transform.position; //죽을때 넉배
            Destory(other.gameObject);   적에게 마법이 닿았을때 마법삭제 

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
}