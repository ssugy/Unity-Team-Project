using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class JY_Boss_FireDungeon : Enemy
{
    public override int CurHealth
    {
        get => base.CurHealth; 
        set
        {
            curHealth = value;
            if (curHealth >= maxHealth)
                curHealth = maxHealth;
            else if (curHealth <= 0 && !isDead)
            {
                StopAllCoroutines();
                isDead = true;
                hitbox.enabled = false;
                anim.SetBool("isDead", true);

                isAwake = false;                

                FreezeEnemy();

                DungeonManager.instance.DungeonProgress(4);
                DungeonManager.instance.SetDungeonGuide(4);
                JY_QuestManager.s_instance.QuestProgress(1);
                AudioManager.s_instance.SoundFadeInOut(AudioManager.s_instance.nowplayName, 0f, 0.3f);

                DropExp();
                DropGold();
                DropItem();

                BossRoomPortal.SetActive(true);

                Destroy(gameObject, 10f);
            }
        }
    }
        

    public static JY_Boss_FireDungeon instacne;
    public static JY_Boss_FireDungeon s_instance { get { return instacne; } }

    public bool isAwake = false;    // ������ �ƽ� ���� �ൿ ���ϰ� �ϴ� ����. true�� �� ������.
    private bool isStun = false;       

    [Header("���� ���� ���� �ݶ��̴�")]
    public BoxCollider JumpAttackArea;       

    [Header("���� ���� �ν��Ͻ�")]    
    public GameObject FieldFire;
    
    public GameObject BossRoomPortal;
    public Transform shooter;

    [Header("�����ı� �ݶ��̴� �� ����")]
    public GameObject HeadPart;
    public GameObject SholderPart;
    public Collider SholderHitBox;
    public Collider HeadHitBox;

    private float hitPoint;
    public float HitPoint
    {
        get => hitPoint;
        set
        {
            hitPoint = value;
            if (hitPoint < 0f)
                hitPoint = 0f;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        
        instacne = this;             
        HitPoint = 0f;       
    }
    public void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        StartCoroutine(Targeting());
    }

    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        HitPoint -= Time.deltaTime * 0.5f;
        atkTime += Time.fixedDeltaTime;

       
        if (!isAwake)
        {
            anim.SetBool("isWalk", false);
            return;
        }
            
        if (isDead)
            return;

        // free unfreeze������ �����.
        if (!isStop)
        {           
            Move();
            Rotate();
            Attack();
        }
        else
        {
            // isstop�� true�ΰ�� ���� = FreezeEnemy�λ��� = idle���� ó������ ���� ���� = ��ų�� ������λ���
            //anim.SetBool("isWalk", false);
        }
    }

    protected override void Attack()
    {
        if (target != null)
        {            
            float distance = Vector3.Distance(transform.position, target.position);
            // ��Ÿ�� 3�� �߿� �������� �����.
            if (distance <= attackDistance && atkTime >= attackCool)
            {
                atkTime = 0f;
                RandomAttack(Random.Range(0, 4));
            }            
            // 10���� �̻� �־����� ���� ���� ���. ��Ÿ���� 2�� �� ��.
            else if (distance >= 10f && atkTime >= (attackCool + 1f)) 
            {
                atkTime = 0f;
                anim.SetTrigger("JumpAttack");
            }                      
        }
    }

    void RandomAttack(int _num)
    {        
        switch (_num)
        {
            case 0: // �Ϲ� ����.
            case 1:                
                anim.SetTrigger("NoramlAttack");
                break;
            case 2: // �� ����.
                anim.SetTrigger("WhirlAttack");
                break;
            case 3: // ������.
                anim.SetTrigger("KickAttack");
                break;            
        }
    }

    void SetAtkMag(float _value)
    {
        atkMag = _value;
    }
    
    public void SwingSound()
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BOSS_SWING);
    }
    public void WalkSound()
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BOSS_WALK);
    }
    public void JumpSound()
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Boss_JUMP);
    }

    public void HitboxOff()
    {                
        hitbox.enabled = false;
    }
    public void HitboxOn()
    {        
        hitbox.enabled = true;
    }
    public void Jump()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        StartCoroutine(JumpMove(target.position));
    }
    private IEnumerator JumpMove(Vector3 _target)
    {
        float t = 0f;
        while (t <= 0.6f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime *10f);
            yield return null;
        }        
    }          

    public void FieldFireGenerate(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject tmp = Instantiate(FieldFire);
            tmp.transform.position = transform.position + new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
        }            
    }

    public void AttacakEffect(string EffectName)
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BOSS_KICK);
        InstanceManager.s_instance.BossEffectCreate(EffectName, transform);
    }
    
    public void BossDieEffect()
    {
        InstanceManager.s_instance.BossEffectCreate("Boss_Dead_Effect", transform);
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BOSS_DEAD, false, 1f);
    }

    protected void HitSound()
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BOSS_HIT);
    }

    public void StopAllEffect()
    {
        InstanceManager.s_instance.StopAllBossEffect();
    }

    // ������ �÷��̾����� ���� �޾����鿡 ���� ����
    public override void OnDamage(int _damage, Vector3 _attacker)
    {
        // ������(ȣ��Ʈ)������ ������ ��.
        if (!PhotonNetwork.IsMasterClient)
            return;
        
        CurHealth -= _damage;

        // ���� ���� �ƴ� ����, ���� ��ġ�� ������Ű�� ���� �̻��� �Ǹ� ������ ����Ŵ.
        if (!isStun)
        {
            HitPoint += ((float)_damage * 100) / maxHealth;

            if (HitPoint >= 5f)
            {
                HitPoint = 0f;
                anim.SetInteger("HitNum", Random.Range(1, 4));
                anim.SetTrigger("isAttacked");                
            }

            Vector3 reactVec = transform.position - _attacker; // �˹� �Ÿ�.
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            StartCoroutine(KnockBack(reactVec));
        }
        
        photonView.RPC("HitEffect", RpcTarget.All);       
    }           
              

    // ���̾ - ������ ���󰡴� ���̾
    void ShootFire()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        BossFireball tmp = PhotonNetwork.Instantiate("Monster/BossFireball",
            shooter.position, transform.rotation).GetComponent<BossFireball>();
        tmp.target = target;        
    }

    // �����ı� �Լ�
    public void PartDestruction(string partName)
    {
        // ������(ȣ��Ʈ)������ ������ ��.
        if (!PhotonNetwork.IsMasterClient)
            return;
        
        isStun = true;
        anim.SetTrigger("isStun");
        if (partName.Equals("BossSholderHitBox"))        
            photonView.RPC("ShoulderDestruct", RpcTarget.All);
        else        
            photonView.RPC("CrownDestruct", RpcTarget.All);
        

        // ���� �ı��� �߻��� ������ ���� ����.
        defMag -= 0.12f;        
    }
    [PunRPC]
    public void ShoulderDestruct()
    {
        SholderHitBox.enabled = false;
        SholderPart.SetActive(false);
    }
    [PunRPC]
    public void CrownDestruct()
    {
        HeadHitBox.enabled = false;
        HeadPart.SetActive(false);
    }

    public void UnStunned()
    {
        isStun = false;
    }         
}
