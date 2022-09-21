using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{    
    // 플레이어 오브젝트를 움직이는 컴포넌트 제작.
    // 플레이어가 움직일 때 오브젝트 회전을 카메라 회전과 맞추기 위한 변수 선언.
    public Transform camAxis;                     // 캠 축.      
    public Animator playerAni;                    // 플레이어의 애니메이션.
    public FixedJoystick playerJoysitck;          // 조이스틱 입력을 받아옴.
    public CharacterController playerController;
    [HideInInspector] public float rotateSpeed;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float gravity;
    [HideInInspector] public Vector3 movement;    // 플레이어의 이동 방향.
    [HideInInspector] public bool enableAct;      // 플레이어의 이동 가능 여부를 표시. (공격 기능을 구현했을 때, 이동하면서 공격할 수 없도록)
    
    
    private void Awake()
    {
        enableAct = true;
    }

    private void Start()
    {        
        camAxis = Camera.main.transform.parent;                
        playerAni = GetComponent<Animator>();
        playerJoysitck = FixedJoystick.instance;
        playerController = GetComponent<CharacterController>();
        rotateSpeed = 5f;
        moveSpeed = 8f;        
        gravity = 0f;        
    }

    void Move()
    {
        /** 조이스틱 입력을 감지하여 플레이어의 이동 방향을 결정.
         * x, z값은 -1과 1 사이의 값으로 결정됨. */
        movement = new Vector3(playerJoysitck.Horizontal, 0,
            playerJoysitck.Vertical);
        if (!enableAct)  // 공격 중일 땐 이동을 못하게 함.
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
        if (!playerController.isGrounded)
        {
            gravity = -30f;                       
        }
        else
        {
            gravity = 0f;
        }
        playerController.Move(transform.forward * moveSpeed * movement.magnitude *
            Time.deltaTime + new Vector3(0, gravity * Time.deltaTime, 0));
    }
    
    void FixedUpdate()
    {                
        Move();        
        camAxis.position = transform.position; // 카메라 중심 축이 플레이어 포지션을 따라다니도록 함.
        if (!playerController.isGrounded)
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
    

}
