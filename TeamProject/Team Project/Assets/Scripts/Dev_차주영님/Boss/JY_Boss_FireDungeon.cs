using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class JY_Boss_FireDungeon : Enemy
{
    public static JY_Boss_FireDungeon instacne;
    public static JY_Boss_FireDungeon s_instance { get { return instacne; } }
    Vector3 tauntVec;
    public bool isLook;
    public bool isAwake;
    bool isDead;
    bool isStun;
    [HideInInspector]public bool isJump;
    bool isKick;
    [HideInInspector]public bool isAttack;
    int partCnt;
    [Header("보스 공격 범위 콜라이더")]
    public BoxCollider JumpAttackArea;
    public BoxCollider MeleeAttackArea;
    public Transform BossWeapon;
    [Header("보스 관련 인스턴스")]
    public GameObject BossWeaponFire;
    public GameObject FieldFire;
    public GameObject Fireball;
    public GameObject BossRoomPortal;
    [HideInInspector] public int HitSkillNum;
    [Header("부위파괴 콜라이더 및 파츠")]
    public GameObject HeadPart;
    public GameObject SholderPart;
    public Collider SholderHitBox;
    public Collider HeadHitBox;

    // Start is called before the first frame update
    void Awake()
    {
        instacne = this;
        rigid = GetComponentInChildren<Rigidbody>();
        hitbox = GetComponent<CapsuleCollider>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        originPos = transform.position;
        originRotateion = transform.rotation;
        isDead = false;
        HitSkillNum = -1;
        partCnt = 2;
        curHealth = maxHealth;
    }
    private new void Start()
    {
        target = JY_CharacterListManager.s_instance.playerList[0].transform;
        isLook = true;
        isAwake = false;
        isStun = false;
        isJump = false;
        isKick = false;
    }

    private void FixedUpdate()
    {
        atkTime += Time.fixedDeltaTime;
        if (isAwake && !isDead)
        {
            if (target != null)
            {
                if (nav.velocity != Vector3.zero)
                {
                    anim.SetBool("isWalk", true);
                }
                else
                {
                    anim.SetBool("isWalk", false);
                }

                nav.SetDestination(target.position);
                float distance = Vector3.Distance(transform.position, target.position);
                if (!isStop)
                {
                    Vector3 dir = target.transform.position - this.transform.position;
                    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime*3f);
                    anim.SetBool("isWalk", true);
                }

                if (distance <= attackDistance)
                {
                    totalTime = 0f;
                    if (distance <= 2f && !isAttack)
                        FreezeEnemy();
                    else if (distance > 2f && !isAttack)
                        UnfreezeEnemy();

                    if (atkTime >= attackCool)
                    {
                        isAttack = true;
                        atkTime = 0f;
                        isStop = true;
                        Vector3 dir = target.transform.position - this.transform.position;
                        this.transform.rotation = Quaternion.LookRotation(dir);
                        FreezeEnemy();
                        int ranAction = Random.Range(0, 3);
                        StartCoroutine(BossPattern(ranAction));
                    }
                }
                else if(distance > 10f && atkTime >= attackCool)
                {
                    atkTime = 0f;
                    isStop = true;
                    Vector3 dir = target.transform.position - this.transform.position;
                    this.transform.rotation = Quaternion.LookRotation(dir);
                    FreezeEnemy();
                    StartCoroutine(BossPattern(3));
                }
                else if(distance > 8f && isKick)
                {
                    StopAllCoroutines();
                    InstanceManager.s_instance.StopAllBossEffect();
                    Vector3 dir = target.transform.position - this.transform.position;
                    this.transform.rotation = Quaternion.LookRotation(dir);
                    StartCoroutine(JumpAttack());
                    isKick = false;
                }


                if (isJump)
                {
                    transform.position = Vector3.MoveTowards(transform.position, tauntVec, Time.deltaTime * 10f);
                }

                if (JY_CharacterListManager.s_instance.playerList[0].CompareTag("Dead"))
                {
                    nav.SetDestination(originPos);
                    transform.rotation = originRotateion;
                    UnfreezeEnemy();
                    target = null;
                    isAwake = false;
                    anim.SetBool("isWalk", false);
                }
            }
        }
    }

    IEnumerator BossPattern(int patternNum)
    {
        yield return new WaitForSeconds(0.1f);
        switch (patternNum)
        {
            case 0:
                StartCoroutine(NormalAttack());
                break;
            case 1:
                StartCoroutine(WhirlAttack());
                break;
            case 2:
                StartCoroutine(Kick());
                break;
            case 3:
                StartCoroutine(JumpAttack());
                break;
        }
    }

    IEnumerator NormalAttack()
    {
        anim.SetTrigger("NoramlAttack");
        yield return new WaitForSeconds(0.2f);
        MeleeAttackArea.gameObject.SetActive(true);
    }
    IEnumerator WhirlAttack()
    {
        anim.SetTrigger("WhirlAttack");
        yield return new WaitForSeconds(0.5f);
        ShootFire();
        yield return new WaitForSeconds(1f);
        ShootFire();
    }
    IEnumerator JumpAttack()
    {
        anim.SetTrigger("JumpAttack");
        yield return new WaitForSeconds(0.5f);
        isJump = true;
        tauntVec = target.position;
        hitbox.enabled = false;
        yield return new WaitForSeconds(1.3f);
        JumpAttackArea.gameObject.SetActive(true);
        for (int i = 0; i < 5; i++)
            FieldFireCreate();
        yield return new WaitForSeconds(0.5f);
        isJump = false;
        JumpAttackArea.gameObject.SetActive(false);
        hitbox.enabled = true;

    }
    IEnumerator Kick()
    {
        isKick = true;
        anim.SetTrigger("KickAttack");
        yield return new WaitForSeconds(0.5f);
        meleeInitialize();
        yield return new WaitForSeconds(0.5f);
        meleeInitialize();
        yield return new WaitForSeconds(1f);
        meleeInitialize();
    }
    void meleeInitialize()
    {
        MeleeAttackArea.gameObject.SetActive(true);
        StartCoroutine(meleeOffDelay());
    }    

    IEnumerator meleeOffDelay()
    {
        yield return new WaitForSeconds(0.2f);
        MeleeAttackArea.gameObject.SetActive(false);
    }
    public override void IsAttacked(int _damage, Vector3 _player)
    {
        photonView.RPC("IsAttacked_Do", RpcTarget.All, _damage, _player);
    }
    [PunRPC]
    public override void IsAttacked_Do(int _damage, Vector3 _player)
    {
        curHealth -= _damage;
        Vector3 reactVec = transform.position - _player; // 넉백 거리.
        StartCoroutine(OnDamage(reactVec * 0.2f));
        hpbar = Enemy_HP_UI.GetObject();
        hpbar.Recognize(this);
        if (EffectManager.Instance != null)
            EffectManager.Instance.PlayHitEffect(transform.position + offset, transform.rotation.eulerAngles, transform);
    }

    protected new IEnumerator OnDamage(Vector3 reactVec)
    {
        yield return new WaitForSeconds(0.1f);
        if (curHealth > 0)
        {
            if (HitSkillNum != -1)
            {
                if(!isStun)
                    isAttackedAnimPlay(HitSkillNum);
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;
                StartCoroutine(KnockBack(reactVec));
            }
        }
        else
        {
            StopAllCoroutines();
            hitbox.enabled = false;
            target = null;
            isDead = true;
            anim.SetTrigger("isDead");
            FreezeEnemy();

            questProgress();
            DropExp();
            DropGold();
            DropItem();
            portalCreate();
            Destroy(gameObject,10f);
        }
    }

    /// <summary>
    /// 스킬에 따른 다른 피격모션 재생
    /// </summary>
    /// <param name="playerSkill">-1:Idle상태, 평타 1,2타, 0:플레이어 평타 3번째, 1:스킬1, 2:스킬2</param>
    void isAttackedAnimPlay(int playerSkill)
    {
        Debug.Log(HitSkillNum);
        anim.SetTrigger("isAttacked");
        anim.SetInteger("HitNum",playerSkill);
    }
    public void WeaponEffectOnOff(bool state)
    {
        BossWeaponFire.SetActive(state);
    }
    void FieldFireCreate()
    {
        GameObject tmp = Instantiate<GameObject>(FieldFire);
        tmp.transform.position = transform.position + new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
    }
    public void MeleeAreaDisEnable()
    {
        MeleeAttackArea.gameObject.SetActive(false);
        JumpAttackArea.gameObject.SetActive(false);
    }
    void ShootFire()
    {
        Instantiate(Fireball, BossWeapon.position, transform.rotation);
    }
    public void PartDestruction(string partName)
    {
        isStun = true;
        if (partName.Equals("BossSholderHitBox"))
        {
            SholderHitBox.enabled = false;
            SholderPart.SetActive(false);
        }
        else
        {
            HeadHitBox.enabled = false;
            HeadPart.SetActive(false);
        }

        partCnt--;
        switch (partCnt)
        {
            case 0:
                defMag = 0.3f;
                break;
            case 1:
                defMag = 0.4f;
                break;
            case 2:
                break;
        }
        Debug.Log(partCnt);
        anim.SetTrigger("Stun");
    }
    public void UnfreezeBoss()
    {
        UnfreezeEnemy();
    }
    public void BossRotate()
    {
        isStop = false;
    }

    void portalCreate()
    {
        BossRoomPortal.SetActive(true);
    }
    public void HitIntermission(float intermission)
    {
        atkTime = intermission;
    }
}
