using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;

public class Player : MonoBehaviour
{
    public static Player instance;
    public PlayerStat playerStat;
    public Transform camAxis;                     // ���� ī�޶� ��.      
    [Header("�÷��̾��� ������Ʈ")]
    public Animator playerAni;                    // �÷��̾��� �ִϸ��̼�.
    public FloatingJoystick playerJoysitck;          // ���̽�ƽ �Է��� �޾ƿ�.
    public CharacterController controller;        // �÷��̾��� ĳ���� ��Ʈ�ѷ�.
    
    [Header("�̵� ���� ����")]
    public float rotateSpeed;
    public float moveSpeed;
    [HideInInspector] public float gravity;
    [HideInInspector] public Vector3 movement;    // ���̽�ƽ �Է� �̵� ����.
    [HideInInspector] public bool enableMove;      // �̵� ���� ���θ� ǥ��.
    [HideInInspector] public bool enableAtk;       // ���� ���� ���� ǥ��.

    public Transform rWeaponDummy;              // ������ ���� ����.
    private TrailRenderer rWeaponEffect;        // ������ ���� ����Ʈ. (�˱�)
    [HideInInspector] public bool isGround;
    private HP_Bar hpbar;

    private void Awake()
    {        
        instance = this;        
    }

    private void Start()
    {        
        isGround = true;
        camAxis = Camera.main.transform.parent;
        playerAni = GetComponent<Animator>();
        playerJoysitck = FloatingJoystick.instance;
        controller = GetComponent<CharacterController>();
        enableMove = true;
        enableAtk = true;
        movement = Vector3.zero;
        rotateSpeed = 5f;
        moveSpeed = 8f;
        gravity = 0f;
        if (rWeaponDummy.childCount != 0)
        {
            rWeaponEffect = rWeaponDummy.GetChild(0).GetChild(2).GetComponent<TrailRenderer>();
        }
        SetState();
        controller.ObserveEveryValueChanged(_ => _.isGrounded).ThrottleFrame(30).Subscribe(_ => isGround = _);  
        // UniRx�� �̿��Ͽ� isGrounded ������Ƽ�� 0.3�� �̻� �����Ǿ�� ���°� ���̵ǰԲ� ��. isGrounded�� �������� �ʱ� ����.
    }

    void Move()
    {
        /** ���̽�ƽ �Է��� �����Ͽ� �÷��̾��� �̵� ������ ����.
         * x, z���� -1�� 1 ������ ������ ������. */
        movement = new Vector3(playerJoysitck.Horizontal, 0,
            playerJoysitck.Vertical);
        if (!enableMove)  // ���� ���� �� �̵��� ���ϰ� ��.
        {
            movement = Vector3.zero;
        }
        if (movement != Vector3.zero)   // �����Ʈ�� �����Ͱ� �ƴ� �� ĳ���� �̵�.
        {
            Quaternion target = Quaternion.Euler(new Vector3(0, camAxis.rotation.eulerAngles.y, 0))
                * Quaternion.LookRotation(movement);
            // �÷��̾��� ȸ���� ���� ī�޶� �ٶ󺸴� ���⿡�� ���̽�ƽ �Է°���ŭ ȸ���� ���� ���麸���� ��.
            transform.rotation = Quaternion.Slerp(transform.rotation,
                target, rotateSpeed * Time.deltaTime);
            /** movement�� 0�� �ƴ϶�� ���� �÷��̾ �����δٴ� ��.
             * ���� �÷��̾��� �ִϸ��̼� ���¸� ��ȯ��. */
            playerAni.SetFloat("isMove", movement.magnitude);
        }
        else
        {
            playerAni.SetFloat("isMove", 0f);
        }
        if (!controller.isGrounded)
        {
            gravity = -30f;
        }
        else
        {
            gravity = 0f;
        }
        controller.Move(transform.forward * moveSpeed * movement.magnitude *
            Time.deltaTime + new Vector3(0, gravity * Time.deltaTime, 0));
    }
    private void Update()
    {
        if (playerStat.curExp >= playerStat.Exp)
        {
            LevelUp();
        }
    }
    void FixedUpdate()
    {
        Move();

        if (!isGround)
        {
            playerAni.SetBool("isGround", false);
        }
        else
        {            
            playerAni.SetBool("isGround", true);
        }
    }

    public void Fall()
    {
        playerAni.SetBool("isGround", false);
    }

    public void SetRotate()
    {
        Vector3 tmp = new Vector3(playerJoysitck.Horizontal, 0,
            playerJoysitck.Vertical);
        transform.rotation *= Quaternion.Euler(tmp);
    }
    public void NormalAttack()
    {
        if (enableAtk)
        {
            SetRotate();
            playerAni.SetTrigger("isAttack");
        }              
    }
    public void PowerStrike()       // ��ų 1.
    {
        if (enableAtk)
        {
            SetRotate();
            playerAni.Play("Player Skill 1");
        }
    }
    public void TurnAttack()        // ��ų 2.
    {
        if (enableAtk)
        {
            SetRotate();
            playerAni.Play("Player Skill 2");
        }
              
    }
    public void JumpAttack()        // ��ų 3.
    {
        if (enableAtk)
        {
            SetRotate();
            playerAni.Play("Player Skill 3");
        }
    }
    public void Warcry()            // ��ų 4.
    {
        if (enableAtk)
        { 
            SetRotate();
            playerAni.Play("Player Skill 4");
        }
    }

    public void Roll()
    {
        playerAni.SetBool("isRoll", true);
        CancelInvoke("_Roll");
        Invoke("_Roll", 0.4f);
    }
    void _Roll()
    {
        playerAni.SetBool("isRoll", false);
    }
    public void RollMove()
    {
        controller.Move(transform.forward * 6f *
            Time.deltaTime + new Vector3(0, gravity * Time.deltaTime, 0));
    }

    public void LArmDown(PointerEventData data)
    {
        playerAni.SetBool("isLArm", true);
    }
    public void LArmUp(PointerEventData data)
    {
        playerAni.SetBool("isLArm", false);
    }
    public void WeaponEffectOn()
    {
        if (rWeaponEffect != null)
        {
            rWeaponEffect.emitting = true;
        }
    }
    public void WeaponEffectOff()
    {
        if (rWeaponEffect != null)
        {
            rWeaponEffect.emitting = false;
        }
    }
    public void Die()
    {
        DisableAtk();
        playerAni.SetTrigger("isDead");
        transform.tag = "Dead";
        Camera.main.GetComponent<MainCamController>().enabled = false;   // �÷��̾ ����ϸ� �� �̻� ī�޶� �������� �ʰ� ��.    
        Camera.main.GetComponent<WhenPlayerDie>().enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Die"))
        {
            Die();
        }
    }

    // �ִϸ��̼� �̺�Ʈ �Լ�.
    void SetEvasion()
    {
        transform.tag = "Evasion";
    }       // �÷��̾��� �±׸� Evasion���� �ٲ�.
    void ResetEvasion()
    {
        transform.tag = "Player";
    }     // �÷��̾��� �±׸� Player�� �ٲ�.
    void FreezePlayer()
    {
        enableMove = false;
    }     // �÷��̾ �̵��� �� ���� ��.
    void UnFreezePlayer()
    {
        enableMove = true;
    }   // �÷��̾ �̵��� �� �ְ� ��.
    void HitboxOn()
    {
        Weapon.weapoonHitbox.enabled = true;
    }
    void HitboxOff()
    {
        Weapon.weapoonHitbox.enabled = false;
    }
    void EnableAtk()
    {
        enableAtk = true;
    }
    void DisableAtk()
    {
        enableAtk = false;
    }




    public void InitializeStat()                  // ���� �ʱ�ȭ
    {
        playerStat.statPoint = (playerStat.level - 1) * 3;
        playerStat.InitialStat(playerStat.characterClass);
    }
    public void StatUp(Adjustable _stat)
    {
        if (playerStat.statPoint > 0)
        {
            switch (_stat)
            {
                case Adjustable.health:
                    --playerStat.statPoint;
                    ++playerStat.health;
                    SetState();
                    break;
                case Adjustable.stamina:
                    --playerStat.statPoint;
                    ++playerStat.stamina;
                    SetState();
                    break;
                case Adjustable.strength:
                    --playerStat.statPoint;
                    ++playerStat.strength;
                    SetState();
                    break;
                case Adjustable.dexterity:
                    --playerStat.statPoint;
                    ++playerStat.dexterity;
                    SetState();
                    break;
            }
        }
    }       // ���� ����
    public void SetState()
    {
        playerStat.HP = playerStat.health * 50 + playerStat.strength * 10;
        playerStat.curHP = playerStat.HP;
        playerStat.SP = playerStat.stamina * 10 + playerStat.strength * 2;
        playerStat.curSP = playerStat.SP;
        playerStat.criPro = (20f + Sigma(2f, 1.03f, playerStat.dexterity)) / 100f;
        playerStat.defMag = 1 - Mathf.Pow(1.02f, -playerStat.defPoint);
        if (Weapon.weapon != null)
        {
            playerStat.atkPoint = Weapon.weapon.atkPoint + Mathf.CeilToInt(Sigma(2f, 1.02f, playerStat.strength) + Sigma(1f, 1.1f, playerStat.dexterity));
        }
        else
        {
            playerStat.atkPoint = 0;
        }
        
    }
    public float Sigma(float a, float b, int c)
    {
        float tmp = 0;
        for (int i = 0; i <= c - 1; i++)
        {
            tmp += a * Mathf.Pow(b, (float)-i);
        }
        return tmp;
    }
    public int AttackDamage(float _atkMag, float _enemyDef)
    {
        float _criDamage;
        if (Random.Range(0f, 1f) <= playerStat.criPro)
        {
            _criDamage = PlayerStat.criMag;
        }
        else
        {
            _criDamage = 1f;
        }
        int _damage = Mathf.CeilToInt(playerStat.atkPoint * _atkMag * (1 - _enemyDef) * _criDamage
            * Random.Range(0.95f, 1.05f));
        return _damage;
    }
    public void Attack(Collider _enemy)
    {
        Debug.Log("����");
        Enemy enemy = _enemy.GetComponent<Enemy>();
        if (enemy != null)
        {
            int damage = AttackDamage(Weapon.weapon.atkMag, enemy.defMag);
            enemy.IsAttacked(damage);
            hpbar = Enemy_HP_UI.GetObject();
            hpbar.Recognize(enemy);
        }
    }
    public void PowerStrikeDamage()
    {
        int layerMask = 1 << 11;
        Vector3 halfHitbox = new Vector3(0.6f, 2f, 1f);
        Collider[] enemys = Physics.OverlapBox(transform.position + transform.forward, halfHitbox, transform.rotation, layerMask);
        if (enemys != null)
        {
            foreach(Collider col in enemys)
            {
                Attack(col);
            }
        }
    }
    public void TurnAttackDamage()
    {
        int layerMask = 1 << 11;
        Vector3 halfHitbox = new Vector3(1f, 2f, 1f);
        Collider[] enemys = Physics.OverlapBox(transform.position + transform.forward, halfHitbox, transform.rotation, layerMask);
        if (enemys != null)
        {
            foreach (Collider col in enemys)
            {
                Attack(col);
            }
        }
    }
    public void JumpAttackDamage()
    {
        int layerMask = 1 << 11;        
        Collider[] enemys = Physics.OverlapSphere(transform.position, 3f, layerMask);
        if (enemys != null)
        {
            foreach (Collider col in enemys)
            {
                Attack(col);
            }
        }
    }
    public void WarcryDamage()
    {
        int layerMask = 1 << 11;
        Collider[] enemys = Physics.OverlapSphere(transform.position, 2f, layerMask);
        if (enemys != null)
        {
            foreach (Collider col in enemys)
            {
                Attack(col);
            }
        }
    }
    /*
    public void ShieldOn()
    {
        if (!isShield)
        {            
            playerStat.defMag += 0.3f;
            isShield = true;
        }
        
    }


    public void ShieldOff()
    {
        if (isShield)
        {
            playerStat.defMag -= 0.3f;
            isShield = false;
        }
    }
    */

    public void IsAttacked(int _damage)
    {
        playerStat.curHP -= _damage;        
        if (playerStat.curHP <= 0)
        {
            Die();
        }
        playerAni.SetFloat("isAttacked", (float)_damage / playerStat.HP);
        
    }
    public void DamageReset()
    {
        playerAni.SetFloat("isAttacked", 0f);
    }
    public void UseStamina(float _stamina)
    {
        playerStat.curSP -= _stamina;
    }
    public void Exhaisted()     // ���׹̳ʸ� ���� �����ϸ� �ൿ�� �� �� ����.
    {
        playerAni.SetBool("isExhausted", true);        
        DisableAtk();
    }
    public void Recovered()     // ���¹̳ʰ� ��� ȸ���Ǹ� Exhausted ���¿��� ȸ����.
    {
        playerAni.SetBool("isExhausted", false);
        EnableAtk();
    }

    void LookTarget()
    {
        int layerMask = 1 << 11;
        Vector3 halfHitbox = new Vector3(2f, 2f, 1f);
        Collider[] enemys = Physics.OverlapBox(transform.position + transform.forward * 2, halfHitbox, transform.rotation, layerMask);
        if (enemys.Length > 0) 
        {
            Transform target = enemys[0].transform;
            transform.LookAt(target);
        }
    }
    void LevelUp()
    {
        ++playerStat.level;
        playerStat.statPoint += 3;
        playerStat.curExp -= playerStat.Exp;
        playerStat.Exp = 100;
    }
    // �̹� �ߵ��� isAttack Ʈ���Ÿ� �����. ���Է¿� ���� �ǵ�ġ ���� ������ ������ ���� ����.
    public void ResetAttackTrigger()   
    {
        playerAni.ResetTrigger("isAttack");
    }
}
