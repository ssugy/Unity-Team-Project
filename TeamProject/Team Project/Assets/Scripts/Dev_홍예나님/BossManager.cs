using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Header("���� ����")]
    public int maxHealth;             // �ִ� ü��.
    public int curHealth;             // ���� ü��.
    public float defMag;              // �����.
    public int atkPoint;              // ������ ���ݷ�.
    public float atkMag;              // ������ ���� ����.
    public int dropExp;               // ���Ͱ� ����ϴ� ����ġ.
    public int dropGold;              // ���Ͱ� ����ϴ� ���.

    [Header("��ų")]
    public GameObject skillFlame;     // skill1 ȭ�����
    public GameObject skillGas;       // skill2 ������
    public GameObject skillEarth;     // skill3 ��������

    const float DIST_ATTACK = 1.8f;
    const float SPEED_FOLLOW = 1.0f;
    static public BossManager instance = null;
    public GameObject boss;
    private BossControl control;
    private bool isPlayerNear = false;
    private bool isFollow = false;
    private bool isAttack = false;
    private int countAttack;
    private float skillTimer;
    public TrailRenderer trailRenderer;
    private enum BossState {STATE_IDLE, STATE_GUN, STATE_SWORD, STATE_WALK, STATE_RUN, STATE_DEATH, STATE_SKILL1, STATE_SKILL2, STATE_SKILL3 }
    private string[] animNames = { "Idle", "shoots gun_2", "sword attack", "Walking", "Run", "Death" };
    private BossState currentState;
    private BossAudioManager audioManager;

    static public BossManager GetInstance()
    {
        if (instance == null)
        {
            instance = new BossManager();
        }
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this);
        }
        else
        {
            //DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    private void SetAttackCount()
    {
        countAttack = Random.Range(2, 5);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = BossState.STATE_IDLE;
        control = boss.GetComponent<BossControl>();
        trailRenderer.gameObject.SetActive(false);
        SetAttackCount();
        skillFlame.SetActive(false);
        skillGas.SetActive(false);
        skillEarth.SetActive(false);
        audioManager = GetComponent<BossAudioManager>();
    }

    private void NormalAttack()
    {
        // ������ �����߰�, �������� �ƴϸ� ����
        countAttack--;
        if (countAttack <= 0)
        {
            // ��ų ���
            SetAttackCount();
            switch (Random.Range(0, 3))
            {
                case 0:
                    SkillAttack1();
                    break;
                case 1:
                    SkillAttack2();
                    break;
                default:
                    SkillAttack3();
                    break;
            }
            
        }
        else
        {
            // �Ϲ� ����
            control.OnClickAnim(animNames[(int)BossState.STATE_SWORD]);
            currentState = BossState.STATE_SWORD;
            isFollow = false;
            isAttack = true;
            trailRenderer.gameObject.SetActive(true);
        }
    }

    private void SkillAttack1()
    {
        currentState = BossState.STATE_SKILL1;
        control.OnClickAnim(animNames[(int)BossState.STATE_IDLE]);
        skillTimer = 0f;
        StartCoroutine(Skill1());
    }

    private void SkillAttack2()
    {
        currentState = BossState.STATE_SKILL2;
        control.OnClickAnim(animNames[(int)BossState.STATE_IDLE]);
        skillTimer = 0f;
        StartCoroutine(Skill2());
    }

    private void SkillAttack3()
    {
        currentState = BossState.STATE_SKILL3;
        control.OnClickAnim(animNames[(int)BossState.STATE_IDLE]);
        skillTimer = 0f;
        StartCoroutine(Skill3());
    }

    IEnumerator Skill1()
    {
        yield return new WaitForSeconds(0.8f);
        audioManager.PlaySound(0);
        skillFlame.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        skillFlame.SetActive(false);
        currentState = BossState.STATE_IDLE;

    }

    IEnumerator Skill2()
    {
        yield return new WaitForSeconds(0.8f);
        audioManager.PlaySound(1);
        skillGas.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        skillGas.SetActive(false);
        currentState = BossState.STATE_IDLE;
    }

    IEnumerator Skill3()
    {
        yield return new WaitForSeconds(0.8f);
        audioManager.PlaySound(2);
        skillEarth.SetActive(true);
        yield return new WaitForSeconds(3f);
        skillEarth.SetActive(false);
        currentState = BossState.STATE_IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        if (curHealth > 0)
        {
            if (isPlayerNear)
            {
                float dist = control.GetPlayerDistance();
                if (dist <= DIST_ATTACK) 
                {
                    switch(currentState)
                    {
                        case BossState.STATE_IDLE:
                            NormalAttack();
                            break;
                        case BossState.STATE_SWORD:
                            if (!control.GetAnim(animNames[(int)BossState.STATE_SWORD]))
                            {
                                currentState = BossState.STATE_IDLE;
                            }
                            break;
                    }

                    

                    
                }
                else if (control.GetAnim(animNames[(int)BossState.STATE_IDLE]))
                {
                    // �ֱ� ������ �Ѿư�
                    control.OnClickAnim(animNames[(int)BossState.STATE_WALK]);
                    isFollow = true;
                    isAttack=false;
                    trailRenderer.gameObject.SetActive(false);
                }
            }
            if (isFollow)
            {
                float dist = Time.deltaTime * SPEED_FOLLOW;
                float ratio = dist / control.GetPlayerDistance();
                Vector3 newPos = Vector3.Slerp(control.transform.position, control.GetPlayerPosition(), ratio);
                control.transform.position = newPos;
            }
        }
    }

    public void SetNear(bool isNear)
    {
        if (curHealth > 0)
        {
            isPlayerNear = isNear;

            if (!isPlayerNear)
            {
                // �ʹ� �־ ���� ��� ������
                control.OnClickAnim(animNames[(int)BossState.STATE_GUN]);
                isFollow = false;
            }
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
        if (curHealth > 0)
        {
            curHealth -= _damage;
            Vector3 reactVec = transform.position - Player.instance.transform.position; // �˹� �Ÿ�.
            StartCoroutine(OnDamage(reactVec));
        }
    }
    
    protected IEnumerator OnDamage(Vector3 reactVec)
    {
        yield return new WaitForSeconds(0.1f);
        if (curHealth > 0)
        {
            //anim.SetTrigger("isAttacked");
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            // rigid.AddForce(reactVec * 5, ForceMode.Impulse);
        }
        else
        {
            //hitbox.enabled = false;
            control.OnClickAnim("Death");
            //FreezeEnemy();
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            //rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            //DropExp();
            Destroy(control.gameObject, 4);
        }
    }
}
