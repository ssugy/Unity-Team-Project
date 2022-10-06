using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamController : MonoBehaviour
{       
    // 카메라의 회전과 카메라 축의 이동을 담당. 카메라의 축은 플레이어의 위치를 따라다님.
    [HideInInspector] public Transform camAxis;       // 메인 카메라의 부모 오브젝트.    
    [HideInInspector] public float camSpeed;          // 카메라 회전 속도.
    [HideInInspector] public float rotateX;           // 카메라의 상하 회전 값.
    [HideInInspector] public float rotateY;           // 카메라의 좌우 회전 값.
    public DragOn dragOn;                             // 카메라 회전을 위한 화면 드래그 값을 받아옴.
    public Transform player;    
    int layerMask;

    void Start()
    {        
        camAxis = transform.parent;        
        camSpeed = 30f;        
        rotateY = 0f; // 게임이 실행되었을 때, 카메라의 x축 회전을 기본값으로 맞춰줌.
        layerMask = 1 << LayerMask.NameToLayer("Building");
        player = Player.instance.transform;
    }

    void Rotate()
    {
        rotateX += dragOn.xAngle * 6f;                
        rotateY += dragOn.yAngle * -1.1f;           // 드래그 방향과 카메라 회전 방향을 맞추기 위해 -1을 곱함.
        dragOn.xAngle = 0;
        dragOn.yAngle = 0;        
        if (rotateY > 1.3f) rotateY = 1.3f;          // 카메라가 너무 많이 회전하는 것을 방지하는 코드.
        if (rotateY < -0.3f) rotateY = -0.3f;
        /** 카메라의 중심축을 화면을 드래그한 값만큼 회전시킴.
         * 카메라는 중심축의 자식 오브젝트이므로 함께 회전함. */
        camAxis.rotation = Quaternion.Euler(new Vector3(
            camAxis.rotation.x + rotateY,
            camAxis.rotation.y + rotateX, 0f) * camSpeed);        
    }          
    void Move()
    {
        camAxis.position = player.position;        
        Vector3 dir = Vector3.Normalize(transform.position - (player.position + new Vector3(0, 0.9f, 0)));
        RaycastHit hitInfo;
        if (Physics.Raycast(player.position + new Vector3(0, 0.9f, 0), dir, out hitInfo, 6.2f, layerMask)) 
        {
            transform.position = hitInfo.point;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0f, 1.6f, -6f), Time.deltaTime*2);
        }
    }
        
    void FixedUpdate()
    {
        Rotate();
        Move();        
    }
}
