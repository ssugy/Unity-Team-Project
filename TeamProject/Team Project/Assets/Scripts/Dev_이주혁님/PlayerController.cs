using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{    
    // �÷��̾� ������Ʈ�� �����̴� ������Ʈ ����.
    // �÷��̾ ������ �� ������Ʈ ȸ���� ī�޶� ȸ���� ���߱� ���� ���� ����.
    public MainCamController mainCam;
    public Transform playerAxis;
    public Transform player;
    public Animator playerAni;
    public float playerSpeed;
    [HideInInspector] public Vector3 movement;    // �÷��̾��� �̵� ����.
    [HideInInspector] public bool enableAct;      // �÷��̾��� �̵� ���� ���θ� ǥ��. (���� ����� �������� ��, �̵��ϸ鼭 ������ �� ������)
    public FixedJoystick playerJoysitck;          // ���̽�ƽ �Է��� �޾ƿ�.

    private void Awake()
    {
        enableAct = true;
    }

    private void Start()
    {
        mainCam = Camera.main.transform.GetComponent<MainCamController>();
        playerAxis = transform.parent;
        player = transform;
        playerAni = GetComponent<Animator>();
        playerSpeed = 8f;
        playerJoysitck = FixedJoystick.instance;
    }

    void PlayerMove()
    {
        /** ���̽�ƽ �Է��� �����Ͽ� �÷��̾��� �̵� ������ ����.
         * x, z���� -1�� 1 ������ ������ ������. */
        movement = new Vector3(playerJoysitck.Horizontal, 0,
            playerJoysitck.Vertical);        
        if (movement != Vector3.zero)   // �����Ʈ�� �����Ͱ� �ƴ� �� ĳ���� �̵�.
        {
            // �÷��̾� ���� ȸ���� ī�޶� ȸ���� ������. x��, z�� ȸ���� ����.
            playerAxis.rotation = Quaternion.Euler(new Vector3(
                0, mainCam.camAxis.rotation.y +
                mainCam.rotateX, 0) * mainCam.camSpeed);
            // Translate �Լ��� �̿��� movement �������� �÷��̾� ���� ������.
            playerAxis.Translate(movement * Time.deltaTime * playerSpeed);
            /* Slerp �޼ҵ�� �÷��̾��� ���� ȸ�����κ���,
             * ��ǥ ����(movement�� ����Ű�� ����) ������ ȸ���� ��ȯ�Ѵ�.
             * RotateTowards�� �޸� ������ ����Ͽ� �ڿ������� ȸ���� �����Ѵ�.
             * movement�� ����3�̹Ƿ� ���ʹϾ����� ��ȯ�Ͽ���.*/
            player.localRotation = Quaternion.Slerp(player.localRotation, 
                Quaternion.LookRotation(movement), 5 * Time.deltaTime);
            /** movement�� 0�� �ƴ϶�� ���� �÷��̾ �����δٴ� ��.
             * ���� �÷��̾��� �ִϸ��̼� ���¸� ��ȯ��. */
            playerAni.SetFloat("isMove", movement.magnitude);
        }
        else if (movement == Vector3.zero)
        {
            playerAni.SetFloat("isMove", 0f);
        }        
        mainCam.camAxis.position = player.position; // ī�޶� �߽� ���� �÷��̾� �������� ����ٴϵ��� ��.
    }
    
    void Update()
    {        
        if (enableAct)  // ���� ���� �� �̵��� ���ϰ� ��.
        {
            PlayerMove();
        }         
    }
}
