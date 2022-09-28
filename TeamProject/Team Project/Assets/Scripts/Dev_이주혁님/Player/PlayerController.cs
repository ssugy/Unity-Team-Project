using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PlayerController : MonoBehaviour
{    

    public Transform camAxis;                     // ���� ī�޶� ��.      
    [Header("�÷��̾��� ������Ʈ")]
    public Animator playerAni;                    // �÷��̾��� �ִϸ��̼�.
    public FixedJoystick playerJoysitck;          // ���̽�ƽ �Է��� �޾ƿ�.
    public CharacterController controller;        // �÷��̾��� ĳ���� ��Ʈ�ѷ�.
    [Header("�̵� ���� ����")]
    [HideInInspector] public float rotateSpeed;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float gravity;
    [HideInInspector] public Vector3 movement;    // ���̽�ƽ �Է� �̵� ����.
    [HideInInspector] public bool enableMove;      // �̵� ���� ���θ� ǥ��.
    [HideInInspector] public bool enableAtk;       // ���� ���� ���� ǥ��.

    public Transform rWeaponDummy;              // ������ ���� ����.
    private TrailRenderer rWeaponEffect;        // ������ ���� ����Ʈ. (�˱�)

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
    void AtkBlock()
    {
        enableAtk = false;
    }         // �÷��̾ ������ �� ���� ��.
    void AtkPossible()
    {
        enableAtk = true;
    }      // �÷��̾ ������ �� �ְ� ��.
}
