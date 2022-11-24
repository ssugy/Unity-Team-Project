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
    [HideInInspector] public bool isJump;
    bool isKick;
    [HideInInspector] public bool isAttack;
    int partCnt;
    float angleRange;
    [Header("���� ���� ���� �ݶ��̴�")]
    public BoxCollider JumpAttackArea;
    public BoxCollider MeleeAttackArea;
    public Transform BossWeapon;
    [Header("���� ���� �ν��Ͻ�")]
    public GameObject BossWeaponFire;
    public GameObject FieldFire;
    public GameObject Fireball;
    public GameObject BossRoomPortal;
    [HideInInspector] public int HitSkillNum;
    [Header("�����ı� �ݶ��̴� �� ����")]
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
        angleRange = 30f;
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
                nav.SetDestination(target.position);
                float distance = Vector3.Distance(transform.position, target.position);
                if (nav.velocity != Vector3.zero && !isAttack)
                {
                    Vector3 dir = target.transform.position - this.transform.position;
                    if (distance > 2f) 
                        anim.SetBool("isWalk", true);
                }
                else
                {
                    anim.SetBool("isWalk", false);
                }


                if (!isStop)
                {
                    Vector3 dir = target.transform.position - this.transform.position;
                    if (CheckInsight(dir))
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
                        this.transform.rotation = Quaternion.LookRotation(dir.normalized);
                        FreezeEnemy();
                        //int ranAction = Random.Range(0, 3);
                        //StartCoroutine(BossPattern(ranAction));
                        StartCoroutine(BossPattern(0));
                    }
                }
                else if (distance > 10f && atkTime >= attackCool)
                {
                    atkTime = 0f;
                    isStop = true;
                    Vector3 dir = target.transform.position - this.transform.position;
                    this.transform.rotation = Quaternion.LookRotation(dir.normalized);
                    FreezeEnemy();
                    StartCoroutine(BossPattern(3));
                }
                else if (distance > 8f && isKick)
                {
                    StopAllCoroutines();
                    atkTime = 0f;
                    InstanceManager.s_instance.StopAllBossEffect();
                    Vector3 dir = target.transform.position - this.transform.position;
                    this.transform.rotation = Quaternion.LookRotation(dir.normalized);
                    StartCoroutine(JumpAttack());
                    isKick = false;
                }


                if (isJump)
                {
                    transform.position = Vector3.MoveTowards(transform.position, tauntVec, Time.deltaTime * 15f);
                }

                if (JY_CharacterListManager.s_instance.playerList[0].CompareTag("Dead"))
                {
                    nav.SetDestination(originPos);
                    transform.rotation = originRotateion;
                    UnfreezeEnemy();
                    target = null;
                    isAwake = false;
                    if(transform.position == originPos)
                        anim.SetBool("isWalk", true);
                    else
                        anim.SetBool("isWalk", false);
                }
            }
        }
    }

    // ���濡 �÷��̾ �ִ����� üũ�ϴ� �Լ� -> ������ �ȴ¸��, ������ ����
    bool CheckInsight(Vector3 dir)
    {
        float t = Mathf.Clamp(Time.deltaTime * 3f, 0f, 0.99f);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), t);
        float dot = Vector3.Dot(dir, transform.forward);
        float theta = Mathf.Acos(dot);
        float degree = Mathf.Rad2Deg * theta;
        bool result = degree <= angleRange / 2f ? true : false;
        return result;
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

    // �Ϲݰ��� �����̸� ������ �Ʒ��� �ѹ� ����ġ�� ��.
    IEnumerator NormalAttack()
    {
        anim.SetTrigger("NoramlAttack");
        yield return new WaitForSeconds(0.2f);
        //MeleeAttackArea.gameObject.SetActive(true);
    }

    // ȸ���ϸ鼭 ���̾ ���� ����
    IEnumerator WhirlAttack()
    {
        anim.SetTrigger("WhirlAttack");
        yield return new WaitForSeconds(0.5f);
        ShootFire();
        yield return new WaitForSeconds(1f);
        ShootFire();
    }

    // �ָ������� �����ؼ� ���, �ҵ���� �ٴڿ� �ѷ����� ����
    IEnumerator JumpAttack()
    {
        anim.SetTrigger("JumpAttack");
        yield return new WaitForSeconds(0.5f);
        isJump = true;
        tauntVec = target.position;
        hitbox.enabled = false;
        yield return new WaitForSeconds(0.5f);
        JumpAttackArea.gameObject.SetActive(true);
        for (int i = 0; i < 5; i++)
            FieldFireCreate();
        yield return new WaitForSeconds(0.5f);
        isJump = false;
        JumpAttackArea.gameObject.SetActive(false);
        hitbox.enabled = true;

    }

    // ������ ����ϴ� ����
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

    // ������ ���⿡ �ݶ��̴��� �޾Ҵµ�, �� �ȸ¾Ƽ� ���� ���濡 �������� �ݶ��̴��� �޾Ƽ� ó���ϴ� ����
    // 0.2���� ������ �����Ѵ�.
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

    // ������ �÷��̾����� ���� �޾����鿡 ���� ����
    public override void IsAttacked(int _damage, Vector3 _player)
    {
        photonView.RPC("IsAttacked_Do", RpcTarget.All, _damage, _player);
    }
    [PunRPC]
    public override void IsAttacked_Do(int _damage, Vector3 _player)
    {
        curHealth -= _damage;
        Vector3 reactVec = transform.position - _player; // �˹� �Ÿ�.
        StartCoroutine(OnDamage(reactVec * 0.2f));
        hpbar = Enemy_HP_UI.GetObject();
        hpbar.Recognize(this);
        if (EffectManager.Instance != null)
            EffectManager.Instance.PlayHitEffect(transform.position + offset, transform.rotation.eulerAngles, transform);
    }

    // ���ÿ��� �̾�����, �������� ó���ϴ� ����, ����ߴ��� ���ε� ó��
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
            AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BOSS_DEAD,false,1f);
            hitbox.enabled = false;
            target = null;
            isDead = true;
            isAwake = false;
            anim.SetTrigger("isDead");
            FreezeEnemy();
            InstanceManager.s_instance.PlayBossSkillEffect("Boss_Dead_Effect", 0f, this.transform);

            questProgress();
            DropExp();
            DropGold();
            DropItem();
            portalCreate();
            Destroy(gameObject,10f);
        }
    }

    /// <summary>
    /// ��ų�� ���� �ٸ� �ǰݸ�� ���, �÷��̾ � ��ų�� �������� �����߳Ŀ� ���� ������ �ǰ� �ִϸ��̼��� �����ϴ� �Լ�.
    /// </summary>
    /// <param name="playerSkill">-1:Idle����, ��Ÿ 1,2Ÿ, 0:�÷��̾� ��Ÿ 3��°, 1:��ų1, 2:��ų2</param>
    void isAttackedAnimPlay(int playerSkill)
    {
        Debug.Log(HitSkillNum);
        anim.SetTrigger("isAttacked");
        anim.SetInteger("HitNum",playerSkill);
    }

    // ���� ���⿡ ������ �ҵ�� - �ܺο��� ����, �ִϸ��̼� behaviour���� ����
    public void WeaponEffectOnOff(bool state)
    {
        BossWeaponFire.SetActive(state);
    }

    // �ٴڿ� �ҵ���(���� ���� �� �ҵ�� ������ ��)
    void FieldFireCreate()
    {
        GameObject tmp = Instantiate<GameObject>(FieldFire);
        tmp.transform.position = transform.position + new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
    }

    // �������ݰ� ���������� ���ÿ� ������ ��, �ѹ��� ������ �ϴ� �Լ�(�ݶ��̴��� �޶� 2���� ���� Ȱ��ȭ ��Ű�ų� ��Ȱ��ȭ ��Ŵ)
    public void MeleeAreaDisEnable()
    {
        MeleeAttackArea.gameObject.SetActive(false);
        JumpAttackArea.gameObject.SetActive(false);
    }

    // ���̾ - ������ ���󰡴� ���̾
    void ShootFire()
    {
        Instantiate(Fireball, BossWeapon.position, transform.rotation);
    }

    // �����ı� �Լ�
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
    
    // ���� �׺���̼� �޽� �����ؼ�, ���� �� �� ��� ������ �ʰ� ���ڸ����� �����ϴ� �Լ� - �ִϸ��̼ǿ��� ��������.
    public void UnfreezeBoss()
    {
        UnfreezeEnemy();
    }

    // ���� ȸ�� - �ִϸ��̼ǿ��� ���
    public void BossRotate()
    {
        isStop = false;
    }

    // ��Ż ����� ��
    void portalCreate()
    {
        BossRoomPortal.SetActive(true);
    }

    // �ǰ� �ִϸ��̼� ��µǾ��� �� �ٷ� �����ϵ��� ���� ��Ÿ�� �����ϴ� �Լ�
    public void SetAtkTime(float tmp)
    {
        atkTime = tmp;
    }

    // �����ı� �׷α���� ���ٰ� �Ͼ �� ����ϴ� �Լ�.
    public void stunWakeUp()
    {
        anim.SetTrigger("StunWakeUP");
        isStun = false;
        isAwake = true;
    }

    /// <summary>
    /// ���� ���� �ݶ��̴� On/Off
    /// </summary>
    public void MeleeColliderOn()
    {
        Debug.Log("on");
        MeleeAttackArea.gameObject.SetActive(true);
    }
    public void MeleeColliderOff()
    {
        Debug.Log("off");
        MeleeAttackArea.gameObject.SetActive(false);
    }

    public void eventTest()
    {
        Debug.Log("normal");
    }
}
