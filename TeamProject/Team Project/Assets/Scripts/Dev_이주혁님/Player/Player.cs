using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
public enum CharacterClass
{
    NULL,
    warrior,
    thief,
    explorer,
    sorcerer,
    enemy
}
public enum Sex
{
    NULL,
    male,
    female
}
public enum Adjustable
{
    health,
    stamina,
    strength,
    dexterity
}
public enum EquipPart
{
    WEAPON,
    SHIELD,
    HELMET,
    CHEST,
    LEG
}
[System.Serializable]
public class PlayerStat
{
    [Header("Adjustable")]
    public int health;
    public int stamina;
    public int strength;
    public int dexterity;

    [Header("Statistic")]
    public CharacterClass characterClass;
    public Sex sex;
    public int level;
    public int[] customized;
    public Dictionary<EquipPart, Item> equiped = new Dictionary<EquipPart, Item>();
    public int Exp;    
    private int curExp;
    public int CurExp           // 프로퍼티를 사용하여 현재 경험치가 증가했을 때만 LevelUp을 판정.
    {
        get { return curExp; }
        set
        {
            curExp = value;
            if (curExp >= Exp)
            {
                Player.instance.LevelUp();
            }
            
        }
    }
    public int HP;
    private int curHP;
    public int CurHP
    {
        get { return curHP; }
        set 
        {
            curHP = value;
            if (curHP < 0)
            {
                curHP = 0;
            }
            else if (curHP > HP)
            {
                curHP = HP;
            }
        }
    }
    public float SP;
    private float curSP;
    public float CurSP
    {
        get { return curSP; }
        set 
        {
            curSP = value;
            if (curSP < 0)
            {
                curSP = 0;
            }
        }
    }
    public float criPro;
    public const float criMag = 1.5f;
    public int defPoint;
    public float defMag;
    public int statPoint;
    public int atkPoint;
    private int gold;
    public int Gold
    {
        get { return gold; }
        set {            
            gold = value;
            if (gold < 0)
            {
                gold = 0;
            }
            }
    }
    public bool isDead;

    public void InitialStat(CharacterClass _class)
    {
        switch (_class)
        {
            case CharacterClass.warrior:
                health = 7;
                stamina = 6;
                strength = 10;
                dexterity = 5;
                break;
            case CharacterClass.thief:
                health = 5;
                stamina = 8;
                strength = 6;
                dexterity = 9;
                break;
            case CharacterClass.explorer:
                health = 7;
                stamina = 7;
                strength = 7;
                dexterity = 7;
                break;
            case CharacterClass.sorcerer:
                health = 5;
                stamina = 9;
                strength = 5;
                dexterity = 9;
                break;
        }        
    }
    // 캐릭터 생성 시, 혹은 스탯 초기화 시 할당할 클래스별 초기스탯.
}



public class Player : MonoBehaviour
{    
    public static Player instance;
    public PlayerStat playerStat;
    public Transform camAxis;                     // 메인 카메라 축.      
    [Header("플레이어의 컴포넌트")]
    public Animator playerAni;                    // 플레이어의 애니메이션.
    public FloatingJoystick playerJoysitck;          // 조이스틱 입력을 받아옴.
    public CharacterController controller;        // 플레이어의 캐릭터 컨트롤러.
    
    [Header("이동 관련 변수")]
    public float rotateSpeed;
    public float moveSpeed;
    [HideInInspector] public float gravity;
    [HideInInspector] public Vector3 movement;    // 조이스틱 입력 이동 방향.
    [HideInInspector] public bool enableMove;      // 이동 가능 여부를 표시.
    public bool enableAtk;       // 공격 가능 여부 표시.

    public Transform rWeaponDummy;              // 오른손 무기 더미.
    private TrailRenderer rWeaponEffect;        // 오른손 무기 이펙트. (검기)
    public Transform lWeaponDummy;              // 왼손 무기 더미.
    [HideInInspector] public bool isGround;
    private HP_Bar hpbar;
    private Dictionary<int, int> EXP_TABLE;


    private void Awake()
    {        
        instance = this;
        EXP_TABLE = new Dictionary<int, int>();
        TextAsset expTable = Resources.Load<TextAsset>("EXP_TABLE");
        string[] tmpTxt = expTable.text.Split("\n");        
        tmpTxt = tmpTxt[1].Split(",");
        for (int i =1; i < tmpTxt.Length; i++)
        {            
            EXP_TABLE.Add(i, int.Parse(tmpTxt[i]));                       
        }
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

        if (JY_CharacterListManager.s_instance != null && JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].level != 1)
        {
            playerStat.level = JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].level;
            if (EXP_TABLE.TryGetValue(playerStat.level, out int _exp))
            {
                playerStat.Exp = _exp;
            }
            playerStat.CurExp = JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].exp;
            playerStat.statPoint = JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].statusPoint;
            playerStat.health = JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].status[0];
            playerStat.stamina = JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].status[1];
            playerStat.strength = JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].status[2];
            playerStat.dexterity = JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].status[3];
        }
        SetState();
        controller.ObserveEveryValueChanged(_ => _.isGrounded).ThrottleFrame(30).Subscribe(_ => isGround = _);
        // UniRx를 이용하여 isGrounded 프로퍼티가 0.3초 이상 유지되어야 상태가 전이되게끔 함. isGrounded가 정교하지 않기 때문.
    }

    void Move()
    {
        /** 조이스틱 입력을 감지하여 플레이어의 이동 방향을 결정.
         * x, z값은 -1과 1 사이의 값으로 결정됨. */
        movement = new Vector3(playerJoysitck.Horizontal, 0,
            playerJoysitck.Vertical);
        if (!enableMove)  // 공격 중일 땐 이동을 못하게 함.
        {
            movement = Vector3.zero;
        }
        if (movement != Vector3.zero)   // 무브먼트가 영벡터가 아닐 때 캐릭터 이동.
        {
            Quaternion target = Quaternion.Euler(new Vector3(0, camAxis.rotation.eulerAngles.y, 0))
                * Quaternion.LookRotation(movement);
            // 플레이어의 회전은 현재 카메라가 바라보는 방향에서 조이스틱 입력값만큼 회전한 값을 구면보간한 것.
            transform.rotation = Quaternion.Slerp(transform.rotation,
                target, rotateSpeed * Time.deltaTime);
            /** movement가 0이 아니라는 것은 플레이어가 움직인다는 뜻.
             * 따라서 플레이어의 애니메이션 상태를 전환함. */
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
        if (Weapon.weapon == null)
        {
            BattleUI.instance.weaponEmpty.SetActive(true);
            return;
        }
        if (enableAtk)
        {
            SetRotate();
            playerAni.SetTrigger("isAttack");
        }              
    }
    public void PowerStrike()       // 스킬 1.
    {
        if (Weapon.weapon == null)
        {
            BattleUI.instance.weaponEmpty.SetActive(true);
            return;
        }
        if (enableAtk)
        {
            SetRotate();
            playerAni.Play("Player Skill 1");
            StartCoroutine(BattleUI.instance.Cooldown(4f, BattleUI.instance.skill_1, BattleUI.instance.cool_1));
        }
    }
    public void TurnAttack()        // 스킬 2.
    {
        if (Weapon.weapon == null)
        {
            BattleUI.instance.weaponEmpty.SetActive(true);
            return;
        }
        if (enableAtk)
        {
            SetRotate();
            playerAni.Play("Player Skill 2");
            StartCoroutine(BattleUI.instance.Cooldown(4f, BattleUI.instance.skill_2, BattleUI.instance.cool_2));
        }
              
    }
    public void JumpAttack()        // 스킬 3.
    {
        if (Weapon.weapon == null)
        {
            BattleUI.instance.weaponEmpty.SetActive(true);
            return;
        }
        if (enableAtk)
        {
            SetRotate();
            playerAni.Play("Player Skill 3");
            StartCoroutine(BattleUI.instance.Cooldown(8f, BattleUI.instance.skill_3, BattleUI.instance.cool_3));
        }
    }
    public void Warcry()            // 스킬 4.
    {
        if (Weapon.weapon == null)
        {
            BattleUI.instance.weaponEmpty.SetActive(true);
            return;
        }
        if (enableAtk)
        { 
            SetRotate();
            playerAni.Play("Player Skill 4");
            StartCoroutine(BattleUI.instance.Cooldown(10f, BattleUI.instance.skill_4, BattleUI.instance.cool_4));
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
        playerStat.CurHP = 0;
        DisableAtk();
        playerAni.SetTrigger("isDead");
        transform.tag = "Dead";
        Camera.main.GetComponent<MainCamController>().enabled = false;   // 플레이어가 사망하면 더 이상 카메라가 움직이지 않게 함.    
        Camera.main.GetComponent<WhenPlayerDie>().enabled = true;
        BattleUI.instance.deathUI.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Die"))
        {
            Die();
        }
    }

    // 애니메이션 이벤트 함수.
    void SetEvasion()
    {
        transform.tag = "Evasion";
    }       // 플레이어의 태그를 Evasion으로 바꿈.
    void ResetEvasion()
    {
        transform.tag = "Player";
    }     // 플레이어의 태그를 Player로 바꿈.
    void FreezePlayer()
    {
        enableMove = false;
    }     // 플레이어가 이동할 수 없게 함.
    void UnFreezePlayer()
    {
        enableMove = true;
    }   // 플레이어가 이동할 수 있게 함.
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




    public void InitializeStat()                  // 스탯 초기화
    {
        playerStat.statPoint = (playerStat.level - 1) * 3;
        playerStat.InitialStat(playerStat.characterClass);
        SetState();
        SaveStatData();
        JY_QuestManager.s_instance.uiManager.StatusDataRenew();
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
        SaveStatData();
        JY_QuestManager.s_instance.uiManager.StatusDataRenew();
    }       // 스탯 투자
    public void SetState()
    {
        playerStat.HP = playerStat.health * 50 + playerStat.strength * 10;
        playerStat.CurHP = playerStat.HP;
        playerStat.SP = playerStat.stamina * 10 + playerStat.strength * 2;
        playerStat.CurSP = playerStat.SP;
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
        Debug.Log("공격");
        Enemy enemy = _enemy.GetComponent<Enemy>();
        if (enemy != null)
        {
            int damage = AttackDamage(Weapon.weapon.atkMag, enemy.defMag);
            enemy.IsAttacked(damage);
            hpbar = Enemy_HP_UI.GetObject();
            hpbar.Recognize(enemy);
        }
        // 예나
        else
        {
            BossControl boss = _enemy.GetComponent<BossControl>();
            if (boss != null)
            {
                int damage = AttackDamage(Weapon.weapon.atkMag, BossManager.instance.defMag);
                BossManager.instance.IsAttacked(damage);
                hpbar = Enemy_HP_UI.GetObject();
                hpbar.RecognizeBoss(boss);
            }
        }
    }
    public void PowerStrikeDamage()
    {
        int layerMask = 1 << 11;
        Vector3 halfHitbox = new Vector3(1.2f, 2f, 1f);
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
        playerStat.CurHP -= _damage;        
        if (playerStat.CurHP <= 0)
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
        playerStat.CurSP -= _stamina;
    }
    public void Exhaisted()     // 스테미너를 전부 소진하면 행동을 할 수 없음.
    {
        playerAni.SetBool("isExhausted", true);        
        DisableAtk();
    }
    public void Recovered()     // 스태미너가 모두 회복되면 Exhausted 상태에서 회복함.
    {
        playerAni.SetBool("isExhausted", false);
        EnableAtk();
    }

    void LookTarget()
    {
        int layerMask = 1 << 11;
        Vector3 halfHitbox = new Vector3(2f, 2f, 1f);
        Collider[] enemys = Physics.OverlapBox(transform.position + transform.forward, halfHitbox, transform.rotation, layerMask);
        if (enemys.Length > 0) 
        {
            Transform target = enemys[0].transform;
            transform.LookAt(target);
        }
    }
    public void LevelUp()
    {
        ++playerStat.level;
        playerStat.statPoint += 3;
        playerStat.CurExp -= playerStat.Exp;
        JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].exp = playerStat.CurExp;
        if (EXP_TABLE.TryGetValue(playerStat.level,out int _exp))
        {
            playerStat.Exp = _exp;
        }        
        playerStat.CurHP = playerStat.HP;

        JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].level = playerStat.level;
        SaveStatData();
        JY_QuestManager.s_instance.uiManager.levelupUI();
    }
    void SaveStatData()
    {
        JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].statusPoint = playerStat.statPoint;
        JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].status[0] = playerStat.health;
        JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].status[1] = playerStat.stamina;
        JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].status[2] = playerStat.strength;
        JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].status[3] = playerStat.dexterity;
        JY_CharacterListManager.s_instance.saveListData();
    }
    // 이미 발동된 isAttack 트리거를 취소함. 선입력에 의한 의도치 않은 공격이 나가는 것을 방지.
    public void ResetAttackTrigger()   
    {
        playerAni.ResetTrigger("isAttack");
    }
    public void questExp(int exp)
    {
        playerStat.CurExp += exp;
        JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].exp = playerStat.CurExp;
    }
}
