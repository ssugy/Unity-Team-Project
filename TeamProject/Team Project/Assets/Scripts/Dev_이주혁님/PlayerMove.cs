using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{    
    // �÷��̾� ������Ʈ�� �����̴� ������Ʈ ����.
    // �÷��̾ ������ �� ������Ʈ ȸ���� ī�޶� ȸ���� ���߱� ���� ���� ����.
    public Transform camAxis;                     // ķ ��.      
    public Animator playerAni;                    // �÷��̾��� �ִϸ��̼�.
    public FixedJoystick playerJoysitck;          // ���̽�ƽ �Է��� �޾ƿ�.
    public CharacterController playerController;
    [HideInInspector] public float rotateSpeed;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float gravity;
    [HideInInspector] public Vector3 movement;    // �÷��̾��� �̵� ����.
    [HideInInspector] public bool enableAct;      // �÷��̾��� �̵� ���� ���θ� ǥ��. (���� ����� �������� ��, �̵��ϸ鼭 ������ �� ������)
    
    
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
        /** ���̽�ƽ �Է��� �����Ͽ� �÷��̾��� �̵� ������ ����.
         * x, z���� -1�� 1 ������ ������ ������. */
        movement = new Vector3(playerJoysitck.Horizontal, 0,
            playerJoysitck.Vertical);
        if (!enableAct)  // ���� ���� �� �̵��� ���ϰ� ��.
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
        camAxis.position = transform.position; // ī�޶� �߽� ���� �÷��̾� �������� ����ٴϵ��� ��.
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
