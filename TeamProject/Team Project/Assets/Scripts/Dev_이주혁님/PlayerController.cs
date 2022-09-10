using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{    
    // 플레이어 오브젝트를 움직이는 컴포넌트 제작.
    // 플레이어가 움직일 때 오브젝트 회전을 카메라 회전과 맞추기 위한 변수 선언.
    public MainCamController mainCam;
    public Transform playerAxis;
    public Transform player;
    public Animator playerAni;
    public float playerSpeed;
    [HideInInspector] public Vector3 movement;    // 플레이어의 이동 방향.
    [HideInInspector] public bool enableAct;      // 플레이어의 이동 가능 여부를 표시. (공격 기능을 구현했을 때, 이동하면서 공격할 수 없도록)
    public FixedJoystick playerJoysitck;          // 조이스틱 입력을 받아옴.

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
        /** 조이스틱 입력을 감지하여 플레이어의 이동 방향을 결정.
         * x, z값은 -1과 1 사이의 값으로 결정됨. */
        movement = new Vector3(playerJoysitck.Horizontal, 0,
            playerJoysitck.Vertical);        
        if (movement != Vector3.zero)   // 무브먼트가 영벡터가 아닐 때 캐릭터 이동.
        {
            // 플레이어 축의 회전을 카메라 회전과 맞춰줌. x축, z축 회전은 없음.
            playerAxis.rotation = Quaternion.Euler(new Vector3(
                0, mainCam.camAxis.rotation.y +
                mainCam.rotateX, 0) * mainCam.camSpeed);
            // Translate 함수를 이용해 movement 방향으로 플레이어 축을 움직임.
            playerAxis.Translate(movement * Time.deltaTime * playerSpeed);
            /* Slerp 메소드는 플레이어의 현재 회전으로부터,
             * 목표 방향(movement가 가리키는 방향) 사이의 회전을 반환한다.
             * RotateTowards와 달리 보간을 사용하여 자연스러운 회전을 구현한다.
             * movement는 벡터3이므로 쿼터니언으로 변환하였다.*/
            player.localRotation = Quaternion.Slerp(player.localRotation, 
                Quaternion.LookRotation(movement), 5 * Time.deltaTime);
            /** movement가 0이 아니라는 것은 플레이어가 움직인다는 뜻.
             * 따라서 플레이어의 애니메이션 상태를 전환함. */
            playerAni.SetFloat("isMove", movement.magnitude);
        }
        else if (movement == Vector3.zero)
        {
            playerAni.SetFloat("isMove", 0f);
        }        
        mainCam.camAxis.position = player.position; // 카메라 중심 축이 플레이어 포지션을 따라다니도록 함.
    }
    
    void Update()
    {        
        if (enableAct)  // 공격 중일 땐 이동을 못하게 함.
        {
            PlayerMove();
        }         
    }
}
