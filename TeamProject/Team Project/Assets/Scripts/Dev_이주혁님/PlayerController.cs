using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{    
    // �÷��̾� ������Ʈ�� �����̴� ������Ʈ ����.
    // �÷��̾ ������ �� ������Ʈ ȸ���� ī�޶� ȸ���� ���߱� ���� ���� ����.
    public Transform camAxis;                     // ķ ��.
    public Transform player;                      // �÷��̾�. = transform
    public Rigidbody playerrb;                    // �÷��̾��� ������ٵ�
    public Animator playerAni;                    // �÷��̾��� �ִϸ��̼�.
    public FixedJoystick playerJoysitck;          // ���̽�ƽ �Է��� �޾ƿ�.
    [HideInInspector] public float rotateSpeed;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public Vector3 movement;    // �÷��̾��� �̵� ����.
    [HideInInspector] public bool enableAct;      // �÷��̾��� �̵� ���� ���θ� ǥ��. (���� ����� �������� ��, �̵��ϸ鼭 ������ �� ������)
    [HideInInspector] public bool isGround;
    [HideInInspector] public float gravity;
    private void Awake()
    {
        enableAct = true;
    }

    private void Start()
    {        
        camAxis = Camera.main.transform.parent;        
        player = transform;
        playerrb = GetComponent<Rigidbody>();
        playerAni = GetComponent<Animator>();
        playerJoysitck = FixedJoystick.instance;
        rotateSpeed = 5f;
        moveSpeed = 8f;
        isGround = false;
        gravity = 0f;
    }

    void PlayerMove()
    {
        /** ���̽�ƽ �Է��� �����Ͽ� �÷��̾��� �̵� ������ ����.
         * x, z���� -1�� 1 ������ ������ ������. */
        movement = new Vector3(playerJoysitck.Horizontal, 0,
            playerJoysitck.Vertical);        
        if (movement != Vector3.zero)   // �����Ʈ�� �����Ͱ� �ƴ� �� ĳ���� �̵�.
        {
            if (isGround)
            {
                gravity = 0f;
            }
            else
            {
                gravity = -9.81f;
            }
            Quaternion target = Quaternion.Euler(new Vector3(0, camAxis.rotation.eulerAngles.y, 0))
                * Quaternion.LookRotation(movement);
            // �÷��̾��� ȸ���� ���� ī�޶� �ٶ󺸴� ���⿡�� ���̽�ƽ �Է°���ŭ ȸ���� ���� ���麸���� ��.
            player.rotation = Quaternion.Slerp(player.rotation,
                target, rotateSpeed * Time.deltaTime);
            // ������ٵ��� velocity�� �����Ͽ� �������� ������.            
            playerrb.velocity = player.forward * movement.magnitude * moveSpeed + new Vector3(0, gravity, 0);            
            /** movement�� 0�� �ƴ϶�� ���� �÷��̾ �����δٴ� ��.
             * ���� �÷��̾��� �ִϸ��̼� ���¸� ��ȯ��. */
            playerAni.SetFloat("isMove", movement.magnitude);
        }
        else if (movement == Vector3.zero)
        {
            Vector3 tmp = playerrb.velocity;
            tmp.x = 0f;
            tmp.z = 0f;
            playerrb.velocity = tmp;
            //playerrb.velocity = Vector3.zero;
            playerAni.SetFloat("isMove", 0f);
        }        
        
    }
    
    void FixedUpdate()
    {        
        if (enableAct)  // ���� ���� �� �̵��� ���ϰ� ��.
        {
            PlayerMove();
        }
        camAxis.position = player.position; // ī�޶� �߽� ���� �÷��̾� �������� ����ٴϵ��� ��.
                                            
        if (playerrb.velocity.y < -9.5f)
        {
            Vector3 tmp = playerrb.velocity;
            tmp.y = -9.5f;
            playerrb.velocity = tmp;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag.Equals("Terrain"))
        {
            isGround = false;
            Invoke("Fall", 0.2f);
        }
    }    
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag.Equals("Terrain"))
        {
            playerAni.SetBool("isGround", true);            
            isGround = true;
            CancelInvoke("Fall");            
        }
    }
    public void Fall()
    {
        playerAni.SetBool("isGround", false);
    }

}
