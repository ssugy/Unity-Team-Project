using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{    

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
    public GameObject portal;
    private enum BossState {STATE_IDLE, STATE_GUN, STATE_SWORD, STATE_WALK, STATE_RUN, STATE_DEATH, STATE_SKILL1, STATE_SKILL2, STATE_SKILL3 }
    private enum BossAttackState { STATE_NORMAL, STATE_70, STATE_30}
    private string[] animNames = { "Idle", "shoots gun_2", "sword attack", "Walking", "Run", "Death" };
    private BossState currentState;    
    private int bossState;

    public bool secondCutScenePlay;
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
    }

    /*
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

    
    public virtual void IsAttacked(int _damage)
    {
        if (curHealth > 0)
        {
            curHealth -= _damage;
            if ((float)curHealth < maxHealth * 0.3f)
            {
                bossState = (int)BossAttackState.STATE_30;
            }
            else if ((float)curHealth < maxHealth * 0.7f)
            {
                bossState = (int)BossAttackState.STATE_70;
            }
            else
            {
                bossState = (int)BossAttackState.STATE_NORMAL;
            }            
        }
    }
    */     
}
