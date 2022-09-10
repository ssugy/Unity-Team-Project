using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamController : MonoBehaviour
{       
    // 카메라의 회전만을 담당. 카메라의 위치는 플레이어 오브젝트를 따라다님.
    [HideInInspector] public Transform camAxis;       // 메인 카메라의 부모 오브젝트.
    [HideInInspector] public Transform mainCam;               // 메인 카메라 오브젝트. transform과 같음.
    [HideInInspector] public float camSpeed;                  // 카메라 회전 속도.
    [HideInInspector] public float rotateX;                   // 카메라의 상하 회전 값.
    [HideInInspector] public float rotateY;                   // 카메라의 좌우 회전 값.
    public DragOn dragOn;                                     // 카메라 회전을 위한 화면 드래그 값을 받아옴.

    void Start()
    {        
        camAxis = transform.parent;
        mainCam = transform;
        camSpeed = 20f;        
        rotateY = 1f; // 게임이 실행되었을 때, 카메라의 x축 회전을 기본값으로 맞춰줌.       
    }

    void Move()
    {
        Vector3 tmp = mainCam.position;          // 현재 카메라의 위치. 
        rotateX += dragOn.xAngle;                
        rotateY += dragOn.yAngle * -1;           // 드래그 방향과 카메라 회전 방향을 맞추기 위해 -1을 곱함.
        dragOn.xAngle = 0;
        dragOn.yAngle = 0;        
        if (rotateY > 1f) rotateY = 1f;          // 카메라가 너무 많이 회전하는 것을 방지하는 코드.
        if (rotateY < -0.5f) rotateY = -0.5f;
        /** 카메라의 중심축을 화면을 드래그한 값만큼 회전시킴.
         * 카메라는 중심축의 자식 오브젝트이므로 함께 회전함. */
        camAxis.rotation = Quaternion.Euler(new Vector3(
            camAxis.rotation.x + rotateY,
            camAxis.rotation.y + rotateX, 0f) * camSpeed);        
    }           
        
    void Update()
    {
        Move();        
    }
}
