using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using CartoonHeroes;
using static CartoonHeroes.SetCharacter;
using Photon.Pun;
using Photon.Realtime;

public enum Sex { NULL, MALE, FEMALE }
public enum Adjustable { HEALTH, STAMINA, STRENGTH, DEXTERITY }
public enum EquipPart { WEAPON, SHIELD, HELMET, CHEST, LEG }

[System.Serializable]
public class PlayerStat
{
    [Header("조정 가능")]
    public int health;
    public int stamina, strength, dexterity;   
    // 장비 교체시 임시 적용되는 수치
    public int tmpHealth, tmpStamina, tmpStrength, tmpDexterity;

    [Header("조정 불가")]
    public EJob job;
    public EGender gender;
    public int[] customized;
    public Dictionary<EquipPart, Item> equiped = new Dictionary<EquipPart, Item>();    

    public int statPoint, level, Exp, HP;
    private int curExp;
    public int CurExp           // 프로퍼티를 사용하여 현재 경험치가 증가했을 때만 LevelUp을 판정.
    {
        get { return curExp; }
        set
        {
            curExp = value;
            if (curExp >= Exp)
                JY_CharacterListManager.s_instance.playerList[0].LevelUp();
        }
    }                   
    private int curHP;
    public int CurHP
    {
        get { return curHP; }
        set 
        {
            curHP = value;
            if (curHP < 0)            
                curHP = 0;                         
            else if (curHP > HP)            
                curHP = HP;            
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
                curSP = 0;            
            else if (curSP > SP)            
                curSP = SP;            
        }
    }

    public float criPro;
    public const float criMag = 1.5f;

    public int defPoint;
    public float defMag;
    
    public int atkPoint;    

    public float HPRecoverCoolTime = 3f;
    public int HpRecover;
    public int SpRecover;
    private int gold;
    public int Gold
    {
        get { return gold; }
        set {            
            gold = value;
            if (gold < 0)            
                gold = 0;                        
            InventoryUI.instance?.UpdateGold();             
            }
    }
    public bool isDead;

    public void InitialStat(EJob _job)
    {
        switch (_job)
        {
            case EJob.WARRIOR:
                health = 7;
                stamina = 6;
                strength = 10;
                dexterity = 5;
                break;
            case EJob.MAGICIAN:
                health = 5;
                stamina = 9;
                strength = 5;
                dexterity = 9;
                break;
        }        
    }

    public void CopyToTemp()
    {
        tmpHealth = health;
        tmpStamina = stamina;
        tmpStrength = strength;
        tmpDexterity = dexterity;
    }

    // 캐릭터 생성 시, 혹은 스탯 초기화 시 할당할 클래스별 초기스탯.
}


public class Player : MonoBehaviourPun, IPunObservable
{    
    //public static Player instance;
    public PlayerStat playerStat;
    public Transform camAxis;                     // 메인 카메라 축.      
    public FloatingJoystick playerJoysitck;          // 조이스틱 입력을 받아옴.
    [Header("플레이어의 컴포넌트")]
    public Animator playerAni;                    // 플레이어의 애니메이션.    
    public CharacterController controller;        // 플레이어의 캐릭터 컨트롤러.
    public SetCharacter setChara;

    [Header("이동 관련 변수")]
    public float rotateSpeed;
    public float moveSpeed;

    [HideInInspector] public float gravity;
    [HideInInspector] public Vector3 movement;    // 조이스틱 입력 이동 방향.
    [HideInInspector] public bool enableMove;      // 이동 가능 여부를 표시.
    [HideInInspector] public bool enableAtk;       // 공격 가능 여부 표시.
    [HideInInspector] public bool enableRecoverSP; // 스태미너 회복 가능 여부 표시.
    [HideInInspector] public bool isGround;
    [HideInInspector] public bool isGaurd; // 방패막기 스킬 플래그
    [HideInInspector] public bool isJumpAttacked;

    public Transform rWeaponDummy;              // 오른손 무기 더미.
    private TrailRenderer rWeaponEffect;        // 오른손 무기 이펙트. (검기)
    public GameObject WeaponEffect;
    public Transform lWeaponDummy;              // 왼손 무기 더미.
    public Weapon rWeapon;
    public Shield lWeapon;
      
    private Dictionary<int, int> EXP_TABLE;      

    private void Awake()
    {                
        playerStat = new();
        EXP_TABLE = new Dictionary<int, int>();
        TextAsset expTable = Resources.Load<TextAsset>("EXP_TABLE");
        string[] tmpTxt = expTable.text.Split("\n");        
        tmpTxt = tmpTxt[1].Split(",");
        for (int i =1; i < tmpTxt.Length; i++)                    
            EXP_TABLE.Add(i, int.Parse(tmpTxt[i]));                                    
    }

    private void Start()
    {        
        camAxis = Camera.main.transform.parent;
        playerJoysitck = FloatingJoystick.instance;

        playerAni = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        setChara = GetComponent<SetCharacter>();

        isGround = true;
        enableMove = true;
        enableAtk = true;
        isGaurd = false;
        enableRecoverSP = true;

        movement = Vector3.zero;
        rotateSpeed = 5f;
        moveSpeed = 8f;
        gravity = 0f;

        if (rWeaponDummy.childCount > 0)
        {
            rWeaponEffect = rWeaponDummy.GetChild(0).GetChild(2).GetComponent<TrailRenderer>();
        }
        WeaponEffect.transform.SetParent(rWeaponDummy);
        WeaponEffect.SetActive(false);
        if (JY_CharacterListManager.s_instance != null)
        {
            playerStat.job = JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].job;
            playerStat.gender = JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].gender;
            playerStat.customized = JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].characterAvatar;
            playerStat.level = JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].level;
            if (EXP_TABLE.TryGetValue(playerStat.level, out int _exp))
                playerStat.Exp = _exp;        
            playerStat.CurExp = JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].exp;
            playerStat.Gold = JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].gold;
            playerStat.statPoint = JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].statusPoint;
            playerStat.health = JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].status[0];
            playerStat.stamina = JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].status[1];
            playerStat.strength = JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].status[2];
            playerStat.dexterity = JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].status[3];
        }
        JY_UIManager.instance?.StatusDataRenew();
        SetState();
        playerStat.CurHP = playerStat.HP;
        playerStat.CurSP = playerStat.SP;               
        controller.ObserveEveryValueChanged(_ => _.isGrounded).ThrottleFrame(100).Subscribe(_ => isGround = _);
        // UniRx를 이용하여 isGrounded 프로퍼티가 0.3초 이상 유지되어야 상태가 전이되게끔 함. isGrounded가 정교하지 않기 때문.
        
        AvatarSet();        
    }

    void Move()
    {
        if (playerJoysitck == null)
            return;

        /** 조이스틱 입력을 감지하여 플레이어의 이동 방향을 결정.
         * x, z값은 -1과 1 사이의 값으로 결정됨. */
        movement = new Vector3(playerJoysitck.Horizontal, 0,
            playerJoysitck.Vertical);

        if (!enableMove)  // 공격 중일 땐 이동을 못하게 함.        
            movement = Vector3.zero;
        
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
            playerAni.SetFloat("isMove", 0f);

        gravity = controller.isGrounded ? 0f : -30f;        
        
        controller.Move(transform.forward * moveSpeed * movement.magnitude *
            Time.deltaTime + new Vector3(0, gravity * Time.deltaTime, 0));
    }    
    void FixedUpdate()
    {
        if (photonView.IsMine || JY_CharacterListManager.s_instance.playerList[0].Equals(this))
        {
            Move();
            playerAni.SetBool("isGround", isGround);
            if (isJumpAttacked)
            {
                Vector3 dir = JY_CharacterListManager.s_instance.playerList[0].transform.position - JY_Boss_FireDungeon.s_instance.JumpAttackArea.transform.position;
                PlayerKnockBack(dir.normalized);
                Invoke("StopKnockBack",0.3f);
            }
        }        
    }

    public void Fall() => playerAni.SetBool("isGround", false);

    public void SetRotate()
    {
        Vector3 tmp = new Vector3(playerJoysitck.Horizontal, 0,
            playerJoysitck.Vertical);
        transform.rotation *= Quaternion.Euler(tmp);
    }
    public void NormalAttack()
    {
        if (rWeapon == null)
        {
            BattleUI.instance.equipEmpty.text = "무기를 착용하지 않았습니다.";
            BattleUI.instance.equipEmpty.gameObject.SetActive(true);
            return;
        }
        if (enableAtk)
        {
            //SetRotate();
            playerAni.SetTrigger("isAttack");
        }              
    }
    
    public void PowerStrike()       // 스킬 1.
    {
        if (PhotonNetwork.InRoom)
            photonView.RPC("PowerStrike_Do", RpcTarget.All);
        else
            PowerStrike_Do();
    }

    [PunRPC]
    public void PowerStrike_Do()
    {
        if (rWeapon == null)
        {
            BattleUI.instance.equipEmpty.text = "무기를 착용하지 않았습니다.";
            BattleUI.instance.equipEmpty.gameObject.SetActive(true);
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
        if (PhotonNetwork.InRoom)
            photonView.RPC("TurnAttack_Do", RpcTarget.All);
        else
            TurnAttack_Do();
    }
    [PunRPC]
    public void TurnAttack_Do()        // 스킬 2.
    {
        if (rWeapon == null)
        {
            BattleUI.instance.equipEmpty.text = "무기를 착용하지 않았습니다.";
            BattleUI.instance.equipEmpty.gameObject.SetActive(true);
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
        if (PhotonNetwork.InRoom)
            photonView.RPC("JumpAttack_Do", RpcTarget.All);
        else
            JumpAttack_Do();

    }
    [PunRPC]
    public void JumpAttack_Do()        // 스킬 3.
    {
        if (rWeapon == null)
        {
            BattleUI.instance.equipEmpty.text = "무기를 착용하지 않았습니다.";
            BattleUI.instance.equipEmpty.gameObject.SetActive(true);
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
        if (PhotonNetwork.InRoom)
            photonView.RPC("Warcry_Do", RpcTarget.All);
        else
            Warcry_Do();
    }

    [PunRPC]
    public void Warcry_Do()            // 스킬 4.
    {
        if (rWeapon == null)
        {
            BattleUI.instance.equipEmpty.text = "무기를 착용하지 않았습니다.";
            BattleUI.instance.equipEmpty.gameObject.SetActive(true);
            return;
        }
        if (enableAtk)
        {
            SetRotate();
            playerAni.Play("Player Skill 4");
            if (photonView.IsMine)
                StartCoroutine(BattleUI.instance.Cooldown(10f, BattleUI.instance.skill_4, BattleUI.instance.cool_4));
        }
    }

    public void Roll()
    {
        playerAni.SetBool("isRoll", true);
        CancelInvoke("_Roll");
        Invoke("_Roll", 0.4f);
    }
    void _Roll() => playerAni.SetBool("isRoll", false);

    public void RollMove() => controller.Move(transform.forward * 6f *
            Time.deltaTime + new Vector3(0, gravity * Time.deltaTime, 0));

    public void PlayerKnockBack(Vector3 dir) => controller.Move(dir * 6f * Time.deltaTime);
    void StopKnockBack()
    {
        isJumpAttacked = false;

    }
    public void LArmDown(PointerEventData data)
    {
        if (lWeapon == null)
        {
            BattleUI.instance.equipEmpty.text = "방패를 착용하지 않았습니다.";
            BattleUI.instance.equipEmpty.gameObject.SetActive(true);
            return;
        }
        playerAni.SetBool("isLArm", true);
    }
    public void LArmUp(PointerEventData data)
    {        
        playerAni.SetBool("isLArm", false);
    }
    public void WEOn()
    {
        if (WeaponEffect != null)
            WeaponEffect.SetActive(true);
    }
    public void WEOff()
    {
        if (WeaponEffect != null)
            WeaponEffect.SetActive(false);
    }

    //사용하지않는 함수로 리팩토링시 제거
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
        gameObject.layer = 16;      // 죽으면 레이어 Dead로 변경.
        if (photonView.IsMine)
        {
            Camera.main.GetComponent<MainCamController>().enabled = false;   // 플레이어가 사망하면 더 이상 카메라가 움직이지 않게 함.    
            Camera.main.GetComponent<WhenPlayerDie>().enabled = true;
            BattleUI.instance.deathUI.SetActive(true);
        }        
    }
    private void OnTriggerEnter(Collider other)
    {
        // 화염, 심연 등 닿으면 죽는 오브젝트와 닿으면 사망함.        
        if (other.CompareTag("Die"))        
            Die();
        if (!photonView.IsMine)
            return;
        if (other.tag == "Buff")
        {
            Buff buff = other.GetComponent<Buff>();
            switch (buff.type)
            {
                case Buff.Type.AtkPoint:
                    gameObject.AddComponent<Buff_AtkPoint>();
                    break;

                case Buff.Type.AtkSpeed:
                    gameObject.AddComponent<Buff_AtkSpeed>();
                    break;

                case Buff.Type.SpRecover:
                    gameObject.AddComponent<Buff_SpRecover>();
                    break;

                case Buff.Type.HP:
                    gameObject.AddComponent<Buff_HP>();
                    break;

                case Buff.Type.HPRecover:
                    gameObject.AddComponent<Buff_HpRecover>();
                    break;

                case Buff.Type.defPoint:
                    gameObject.AddComponent<Buff_defPoint>();
                    break;
            }

            Destroy(other.gameObject);
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
    void StopRecoverSP()
    {
        enableRecoverSP = false;
    }
    void StartRecoverSP()
    {
        enableRecoverSP = true;
    }

    void HitboxOn()
    {
        if (rWeapon.weaponHitbox != null)
        {
            rWeapon.weaponHitbox.enabled = true;
        }        
    }
    void HitboxOff()
    {
        if (rWeapon.weaponHitbox != null)
        {
            rWeapon.weaponHitbox.enabled = false;
        }
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
        playerStat.InitialStat(playerStat.job);
        SetState();               
        JY_UIManager.instance.StatusDataRenew();
    }
    public void StatUp(Adjustable _stat)
    {
        if (playerStat.statPoint > 0)
        {
            switch (_stat)
            {
                case Adjustable.HEALTH:
                    --playerStat.statPoint;
                    ++playerStat.health;
                    SetState();
                    break;
                case Adjustable.STAMINA:
                    --playerStat.statPoint;
                    ++playerStat.stamina;
                    SetState();
                    break;
                case Adjustable.STRENGTH:
                    --playerStat.statPoint;
                    ++playerStat.strength;
                    SetState();
                    break;
                case Adjustable.DEXTERITY:
                    --playerStat.statPoint;
                    ++playerStat.dexterity;
                    SetState();
                    break;
            }
        }        
        JY_UIManager.instance.StatusDataRenew();        
    }       
    // Adjustable 스탯으로부터 기타 스탯을 연산함.
    /// <summary>
    /// Health : 최대 체력 및 체력 회복속도에 영향을 미침 
    /// Stemina: 최대 기력 및 기력 회복속도에 영향을 미침
    /// Strength:공격력 상승 및 방어력 다소 상승
    /// Dexterity:공격속도 상승 및 회피 확률 증가
    /// </summary>
    public void SetState()
    {
        //Health
        playerStat.HP = 210 + playerStat.health * 20 + playerStat.strength * 5;   // 1레벨 스탯 기준 400
        playerStat.HpRecover = 10 + playerStat.health / 5;                          
        playerStat.CurHP = playerStat.CurHP;
        //Stemina
        playerStat.SP = 46 + playerStat.stamina * 4 + playerStat.strength * 1;    // 1레벨 스탯 기준 80
        playerStat.SpRecover = 10 + playerStat.stamina / 5;
        playerStat.CurSP = playerStat.CurSP;
        //Strength and Dexterity
        playerStat.criPro = (20f + Sigma(2f, 1.03f, playerStat.dexterity)) / 100f;
        playerStat.defMag = 1 - Mathf.Pow(1.02f, -playerStat.defPoint);
        if (rWeapon != null)
            playerStat.atkPoint = rWeapon.atkPoint + Mathf.CeilToInt(Sigma(2f, 1.02f, playerStat.strength) + Sigma(1f, 1.1f, playerStat.dexterity));       
        else        
            playerStat.atkPoint = 0;
        
    }
    //옵션이 적용된 플레이어 상태값 계산 : 아이템 교체시 적용
    public void SetStateOption()
    {
        //Health
        playerStat.HP = 210 + playerStat.tmpHealth * 20 + playerStat.tmpStrength * 5;   // 1레벨 스탯 기준 400
        playerStat.HpRecover = 10 + playerStat.tmpHealth / 5;
        playerStat.CurHP = playerStat.CurHP;
        //Stemina
        playerStat.SP = 46 + playerStat.tmpStamina * 4 + playerStat.tmpStrength * 1;    // 1레벨 스탯 기준 80
        playerStat.SpRecover = 10 + playerStat.tmpStamina / 5;
        playerStat.CurSP = playerStat.CurSP;
        //Strength and Dexterity
        playerStat.criPro = (20f + Sigma(2f, 1.03f, playerStat.tmpDexterity)) / 100f;
        playerStat.defMag = 1 - Mathf.Pow(1.02f, -playerStat.defPoint);
        if (rWeapon != null)
            playerStat.atkPoint = rWeapon.atkPoint + Mathf.CeilToInt(Sigma(2f, 1.02f, playerStat.tmpStrength) + Sigma(1f, 1.1f, playerStat.tmpDexterity));
        else
            playerStat.atkPoint = 0;

    }

    // 특별히 사용하기 위해 만든 시그마 연산용 함수. 일반적인 시그마 연산에는 사용하지 말 것.
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
        Enemy enemy = _enemy.GetComponent<Enemy>();
        if (enemy != null)
        {
            SoundAttack();
            int damage = AttackDamage(rWeapon.atkMag, enemy.defMag);
            enemy.IsAttacked(damage, transform.position);            
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

    public void IsAttacked(int _damage)
    {
        InstanceManager.s_instance.StopAllSkillEffect();
        WEOff();
        playerStat.CurHP -= _damage;

        if (isGaurd)
            playerStat.CurSP -= 10f;
        else
        {
            SoundHit();
            playerAni.SetFloat("isAttacked", (float)_damage / playerStat.HP);
        }

        if (playerStat.CurHP == 0)
            Die();

        if (JY_Boss_FireDungeon.s_instance != null)
        {
            if (JY_Boss_FireDungeon.s_instance.isJump)
                isJumpAttacked = true;
        }
    }
    public void DamageReset()
    {
        playerAni.SetFloat("isAttacked", 0f);
    }
    public void UseStamina(float _stamina)
    {
        playerStat.CurSP -= _stamina;
    }
    public void Exhausted()     // 스테미너를 전부 소진하면 행동을 할 수 없음.
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
        Vector3 halfHitbox = new Vector3(2f, 2f, 0.75f);
        Collider[] enemys = Physics.OverlapBox(transform.position + transform.forward * 0.75f, halfHitbox, transform.rotation, layerMask);
        if (enemys.Length > 0) 
        {
            // 근처에 있는 적 중 랜덤한 한 적을 바라보도록 함.
            Transform target = enemys[Random.Range(0, enemys.Length)].transform;
            // target을 LookAt할 때 플레이어의 y축 외 다른 축의 회전이 발생하지 않게 하기 위해 Vector3.up을 매개변수로 추가함.      
            transform.LookAt(target, Vector3.up);
        }
    }
    public void LevelUp()
    {
        ++playerStat.level;
        playerStat.statPoint += 3;
        playerStat.CurExp -= playerStat.Exp;              
        if (EXP_TABLE.TryGetValue(playerStat.level,out int _exp))
        {
            playerStat.Exp = _exp;
        }        
        playerStat.CurHP = playerStat.HP;        
        JY_UIManager.instance.levelupUI();
    }
    
    // 이미 발동된 isAttack 트리거를 취소함. 선입력에 의한 의도치 않은 공격이 나가는 것을 방지.
    public void ResetAttackTrigger()   
    {
        playerAni.ResetTrigger("isAttack");
    }
    public void questExp(int exp)
    {
        playerStat.CurExp += exp;
        SaveData();
        JY_CharacterListManager.s_instance.Save();
    }
    void SoundRun()
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.PLAYER_RUN);
    }
    void SoundSwing()
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.PLAYER_SWING);
    }
    void SoundHit()
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.PLAYER_HIT);
    }
    void SoundAttack()
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.PLAYER_ATTACK);
    }

    // 플레이어 스크립트에서 인포, 인벤 데이터를 JInfoData로 옮기는 메소드가 필요.
    public void SaveData()
    {
        if (JY_CharacterListManager.s_instance.playerList.Count > 0)
        {
            if (JY_CharacterListManager.s_instance.playerList[0].Equals(this))
            {
                Debug.Log("데이터 세이브");
                InfoData tmp = JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum];
                tmp.level = playerStat.level;
                tmp.exp = playerStat.CurExp;
                tmp.gold = playerStat.Gold;
                tmp.statusPoint = playerStat.statPoint;
                tmp.characterAvatar = playerStat.customized;
                tmp.status[(int)Adjustable.HEALTH] = playerStat.health;
                tmp.status[(int)Adjustable.STAMINA] = playerStat.stamina;
                tmp.status[(int)Adjustable.STRENGTH] = playerStat.strength;
                tmp.status[(int)Adjustable.DEXTERITY] = playerStat.dexterity;
                JY_CharacterListManager.CopyInventoryData(JY_CharacterListManager.s_instance.invenList[0].items, tmp.itemList);
                JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum] = tmp;
            }
        }
        
        
    }
    public void OnDisable()
    {        
        SaveData();             
        JY_CharacterListManager.s_instance.playerList.Remove(this);
        JY_CharacterListManager.s_instance.invenList.Remove(GetComponent<Inventory>());
    }
        

    public void AvatarSet()
    {
        if (setChara == null) return;

        for (int i = 0; i < 4; i++)        
            subOptionLoad(i, playerStat.customized[i]);        
    }
    public void subOptionLoad(int currentOption, int sub)
    {
        DeleteSubOption(currentOption);
        {
            GameObject addedObj = setChara.AddItem(setChara.itemGroups[currentOption], sub);
        }
    }
    void DeleteSubOption(int currentOption)
    {
        
        for (int j = 0; j < setChara.itemGroups[currentOption].slots; j++)
        {
            if (setChara.HasItem(setChara.itemGroups[currentOption], j))
            {
                List<GameObject> removedObjs = setChara.GetRemoveObjList(setChara.itemGroups[currentOption], j);
                for (int m = 0; m < removedObjs.Count; m++)
                {
                    if (removedObjs[m] != null)
                    {
                        DestroyImmediate(removedObjs[m]);
                    }
                }
            }
        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerStat.HP);
            stream.SendNext(playerStat.CurHP);
            stream.SendNext(playerStat.atkPoint);
            stream.SendNext(playerStat.defMag);
            stream.SendNext(gameObject.name);
            stream.SendNext(playerStat.customized);
            if (rWeapon == null)            
                stream.SendNext(string.Empty);            
            else            
                stream.SendNext(rWeapon.name);
            if (lWeapon == null)
                stream.SendNext(string.Empty);
            else
                stream.SendNext(lWeapon.name);


        }
        else
        {
            playerStat.HP = (int)stream.ReceiveNext();
            playerStat.CurHP = (int)stream.ReceiveNext();
            playerStat.atkPoint = (int)stream.ReceiveNext();
            playerStat.defMag = (float)stream.ReceiveNext();
            gameObject.name = (string)stream.ReceiveNext();

            playerStat.customized = (int[])stream.ReceiveNext();
            AvatarSet();

            string weaponName= (string)stream.ReceiveNext();
            if (weaponName.Equals(string.Empty))
            { 
                while (this.rWeaponDummy.GetComponentInChildren<Weapon>() != null)                
                    DestroyImmediate(this.rWeaponDummy.GetComponentInChildren<Weapon>().gameObject);
            }
            else if (rWeapon==null || !weaponName.Equals(rWeapon.name))
            {
                while (this.rWeaponDummy.GetComponentInChildren<Weapon>() != null)
                    DestroyImmediate(this.rWeaponDummy.GetComponentInChildren<Weapon>().gameObject);
                GameObject weaponSrc = Resources.Load<GameObject>("Item/Weapon/" + weaponName);
                GameObject weapon = Instantiate(weaponSrc, rWeaponDummy);
                weapon.name = string.Copy(weaponSrc.name);
            }

            string shieldName = (string)stream.ReceiveNext();
            if (shieldName.Equals(string.Empty))
            {
                while (this.lWeaponDummy.GetComponentInChildren<Shield>() != null)
                    DestroyImmediate(this.lWeaponDummy.GetComponentInChildren<Shield>().gameObject);
                while (this.lWeaponDummy.GetComponentInChildren<Staff>() != null)
                    DestroyImmediate(this.lWeaponDummy.GetComponentInChildren<Staff>().gameObject);
            }
            else if (lWeapon == null || !shieldName.Equals(lWeapon.name))
            {
                while (this.lWeaponDummy.GetComponentInChildren<Shield>() != null)
                    DestroyImmediate(this.lWeaponDummy.GetComponentInChildren<Shield>().gameObject);
                while (this.lWeaponDummy.GetComponentInChildren<Staff>() != null)
                    DestroyImmediate(this.lWeaponDummy.GetComponentInChildren<Staff>().gameObject);
                GameObject shieldSrc = Resources.Load<GameObject>("Item/Shield/" + shieldName);
                GameObject shield = Instantiate(shieldSrc, lWeaponDummy);
                shield.name = string.Copy(shieldSrc.name);
            }

        }
    }
}
