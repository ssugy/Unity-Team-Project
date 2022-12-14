using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Dragon : Enemy
{
    public override int CurHealth
    {
        get => curHealth;
        set
        {
            curHealth = value;
            if (curHealth >= maxHealth)
                curHealth = maxHealth;
            else if (curHealth <= 0 && !isDead)
            {
                isDead = true;
                hitbox.enabled = false;
                anim.SetBool("isDead", true);

                FreezeEnemy();                

                DropExp();
                DropGold();
                DropItem();

                Destroy(gameObject, 4);                               
                if (ProgressionMonster)
                {
                    if (DungeonManager.instance != null)
                    {
                        if (DungeonManager.instance.NOWPROGRESS != 0)
                        {
                            DungeonManager.instance.DungeonProgress(explanationNum);
                            DungeonManager.instance.SetDungeonGuide(explanationNum);
                        }
                    }
                }
            }
        }
    }

    [Header("스킬 공격 관련")]
    public float skillCool;
    public float skillDistance;

    [Header("진행도 관련")]
    public bool ProgressionMonster;
    public int explanationNum;

    public Transform shooter;       // 화염구를 발사할 위치.
    private GameObject fireballPrefab;

    protected override void Awake()
    {
        base.Awake();
        // 위까진 Enemy와 공통.
        fireballPrefab = Resources.Load<GameObject>("Monster\\Fireball");
    }

    void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        atkTime += Time.fixedDeltaTime;

        anim.SetBool("isWalk", false);
        if (!isStop)
        {
            Move();
            Rotate();
            Attack();
        }               
    }

    protected override void Attack()
    {
        if (target != null)
        {
            
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance <= attackDistance && atkTime >= attackCool)
            {
                atkTime = 0f;
                anim.SetTrigger("isAttack");
            }
            // 거리가 너무 멀어지면 타겟이 풀림.
            else if (distance >= 20f)
            {
                atkTime = 0f;
                target = null;
                curHealth = maxHealth;
                StartCoroutine(ReTargeting(2f));
            }
            else if (distance > skillDistance && atkTime >= skillCool)
            {
                atkTime = 0f;
                anim.SetTrigger("isSkill");                
            }

            // 10초 이상 공격을 못하면 타겟이 풀림.
            if (atkTime > 10f)
            {
                atkTime = 0f;
                target = null;
                curHealth = maxHealth;
                StartCoroutine(ReTargeting(2f));
            }
        }
    }

    [PunRPC]
    public override void OnDamage(int _damage, Vector3 _attacker)
    {
        // 마스터(호스트)에서만 연산을 함.
        if (!PhotonNetwork.IsMasterClient)
            return;        

        // 데미지와 넉백 연산. 피격 애니메이션 트리거를 Set
        CurHealth -= _damage;
        if (_damage >= (0.2f * maxHealth))        
            anim.SetTrigger("isAttacked");
        
        Vector3 reactVec = transform.position - _attacker; // 넉백 거리.        
        reactVec = reactVec.normalized;
        reactVec += Vector3.up;
        StartCoroutine(KnockBack(reactVec));        

        // 피격에 따른 이펙트 출력과 UI 활성화는 모든 클라이언트에게.
        photonView.RPC("HitEffect", RpcTarget.All);

        anim.SetTrigger("isWakeUp");
    }   
    
    void ShootFire()
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Dragon_Fire);
        GameObject fireball = Instantiate(fireballPrefab, shooter.position, Quaternion.identity);
        fireball.transform.LookAt(target.position + new Vector3(0, 1f, 0f));
    }
    void SoundGrowl()
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.MONSTER_GROWL);
    }
}
