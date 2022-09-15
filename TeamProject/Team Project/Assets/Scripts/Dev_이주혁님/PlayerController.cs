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
    }

    void PlayerMove()
    {
        /** ���̽�ƽ �Է��� �����Ͽ� �÷��̾��� �̵� ������ ����.
         * x, z���� -1�� 1 ������ ������ ������. */
        movement = new Vector3(playerJoysitck.Horizontal, 0,
            playerJoysitck.Vertical);        
        if (movement != Vector3.zero)   // �����Ʈ�� �����Ͱ� �ƴ� �� ĳ���� �̵�.
        {
            Quaternion target = Quaternion.Euler(new Vector3(0, camAxis.rotation.eulerAngles.y, 0))
                * Quaternion.LookRotation(movement);
            // �÷��̾��� ȸ���� ���� ī�޶� �ٶ󺸴� ���⿡�� ���̽�ƽ �Է°���ŭ ȸ���� ���� ���麸���� ��.
            player.rotation = Quaternion.Slerp(player.rotation,
                target, rotateSpeed * Time.deltaTime);
            // ������ٵ��� velocity�� �����Ͽ� �������� ������.            
            playerrb.velocity = player.forward * movement.magnitude * moveSpeed;                      
            /** movement�� 0�� �ƴ϶�� ���� �÷��̾ �����δٴ� ��.
             * ���� �÷��̾��� �ִϸ��̼� ���¸� ��ȯ��. */
            playerAni.SetFloat("isMove", movement.magnitude);
        }
        else if (movement == Vector3.zero)
        {
            playerrb.velocity = Vector3.zero;
            playerAni.SetFloat("isMove", 0f);
        }        
        
    }
    
    void Update()
    {        
        if (enableAct)  // ���� ���� �� �̵��� ���ϰ� ��.
        {
            PlayerMove();
        }
        camAxis.position = player.position; // ī�޶� �߽� ���� �÷��̾� �������� ����ٴϵ��� ��.        
    }
}
