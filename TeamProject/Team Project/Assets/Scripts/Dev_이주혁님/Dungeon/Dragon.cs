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

    [Header("��ų ���� ����")]
    public float skillCool;
    public float skillDistance;

    [Header("���൵ ����")]
    public bool ProgressionMonster;
    public int explanationNum;

    public Transform shooter;       // ȭ������ �߻��� ��ġ.
    private GameObject fireballPrefab;

    protected override void Awake()
    {
        base.Awake();
        // ������ Enemy�� ����.
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
            // �Ÿ��� �ʹ� �־����� Ÿ���� Ǯ��.
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

            // 10�� �̻� ������ ���ϸ� Ÿ���� Ǯ��.
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
        // ������(ȣ��Ʈ)������ ������ ��.
        if (!PhotonNetwork.IsMasterClient)
            return;        

        // �������� �˹� ����. �ǰ� �ִϸ��̼� Ʈ���Ÿ� Set
        CurHealth -= _damage;
        if (_damage >= (0.2f * maxHealth))        
            anim.SetTrigger("isAttacked");
        
        Vector3 reactVec = transform.position - _attacker; // �˹� �Ÿ�.        
        reactVec = reactVec.normalized;
        reactVec += Vector3.up;
        StartCoroutine(KnockBack(reactVec));        

        // �ǰݿ� ���� ����Ʈ ��°� UI Ȱ��ȭ�� ��� Ŭ���̾�Ʈ����.
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
