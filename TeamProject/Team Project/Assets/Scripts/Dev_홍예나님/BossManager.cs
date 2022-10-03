using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    const float DIST_ATTACK = 2.0f;
    const float SPEED_FOLLOW = 1.0f;
    static public BossManager instance = null;
    public GameObject boss;
    private BossControl control;
    private bool isPlayerNear = false;
    private bool isFollow = false;
    private bool isAttack = false;
    public TrailRenderer trailRenderer;
    private enum BossState {STATE_IDLE, STATE_GUN, STATE_SWORD, STATE_WALK, STATE_RUN, STATE_DEATH }
    private string[] animNames = { "Idle", "shoots gun_2", "sword attack", "Walking", "Run", "Death" };

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
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        control = boss.GetComponent<BossControl>();
        trailRenderer.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNear)
        {
            float dist = control.GetPlayerDistance();
            if (dist <= DIST_ATTACK && !control.GetAnim(animNames[(int)BossState.STATE_SWORD]))
            {
                // 가까이 근접했고, 공격중이 아니면 공격
                control.OnClickAnim(animNames[(int)BossState.STATE_SWORD]);

                isFollow = false;
                isAttack = true;
                trailRenderer.gameObject.SetActive(true);
            }
            else if (control.GetAnim(animNames[(int)BossState.STATE_IDLE]))
            {
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

    public void SetNear(bool isNear)
    {
        isPlayerNear = isNear;
        if (isPlayerNear)
        {
            //control.OnClickAnim(animNames[(int)BossState.STATE_SWORD]);
        }
        else
        {
            control.OnClickAnim(animNames[(int)BossState.STATE_GUN]);
            isFollow = false;
        }
    }
}
