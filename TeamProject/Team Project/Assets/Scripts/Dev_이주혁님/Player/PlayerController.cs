using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PlayerController : MonoBehaviour
{    

    public Transform camAxis;                     // 메인 카메라 축.      
    [Header("플레이어의 컴포넌트")]
    public Animator playerAni;                    // 플레이어의 애니메이션.
    public FixedJoystick playerJoysitck;          // 조이스틱 입력을 받아옴.
    public CharacterController controller;        // 플레이어의 캐릭터 컨트롤러.
    [Header("이동 관련 변수")]
    [HideInInspector] public float rotateSpeed;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float gravity;
    [HideInInspector] public Vector3 movement;    // 조이스틱 입력 이동 방향.
    [HideInInspector] public bool enableMove;      // 이동 가능 여부를 표시.
    [HideInInspector] public bool enableAtk;       // 공격 가능 여부 표시.

    public Transform rWeaponDummy;              // 오른손 무기 더미.
    private TrailRenderer rWeaponEffect;        // 오른손 무기 이펙트. (검기)

    public static Transform player;

    private void Awake()
    {
        player = null;
        enableMove = true;
        enableAtk = true;
        movement = Vector3.zero;
    }
    private void OnEnable()
    {
        player = transform;
    }
    private void OnDisable()
    {
        player = null;
    }

    private void Start()
    {        
        camAxis = Camera.main.transform.parent;                
        playerAni = GetComponent<Animator>();
        playerJoysitck = FixedJoystick.instance;
        controller = GetComponent<CharacterController>();
        rotateSpeed = 5f;
        moveSpeed = 8f;        
        gravity = 0f;
        if (rWeaponDummy.childCount != 0)
        {
            rWeaponEffect = rWeaponDummy.GetChild(0).GetChild(2).GetComponent<TrailRenderer>();
        }
        
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
        
        if (!controller.isGrounded)
        {            
            Invoke("Fall", 0.1f);
        }
        else
        {
            CancelInvoke("Fall");
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
            playerAni.Play("Player NormalAttack");
        }
    }
    public void Skill_1()
    {
        if (enableAtk)
        {
            SetRotate();
            playerAni.Play("Player Skill 1");
        }
    }
    public void Skill_2()
    {
        if (enableAtk)
        {
            SetRotate();
            playerAni.Play("Player Skill 2");
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
        playerAni.SetBool("isDead", true);
        transform.tag = "Dead";
        Camera.main.GetComponent<MainCamController>().enabled = false;   // 플레이어가 사망하면 더 이상 카메라가 움직이지 않게 함.    
        Camera.main.GetComponent<WhenPlayerDie>().enabled = true;
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
    void AtkBlock()
    {
        enableAtk = false;
    }         // 플레이어가 공격할 수 없게 함.
    void AtkPossible()
    {
        enableAtk = true;
    }      // 플레이어가 공격할 수 있게 함.
}
