using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamController : MonoBehaviour
{       
    // ī�޶��� ȸ������ ���. ī�޶��� ��ġ�� �÷��̾� ������Ʈ�� ����ٴ�.
    [HideInInspector] public Transform camAxis;       // ���� ī�޶��� �θ� ������Ʈ.
    [HideInInspector] public Transform mainCam;               // ���� ī�޶� ������Ʈ. transform�� ����.
    [HideInInspector] public float camSpeed;                  // ī�޶� ȸ�� �ӵ�.
    [HideInInspector] public float rotateX;                   // ī�޶��� ���� ȸ�� ��.
    [HideInInspector] public float rotateY;                   // ī�޶��� �¿� ȸ�� ��.
    public DragOn dragOn;                                     // ī�޶� ȸ���� ���� ȭ�� �巡�� ���� �޾ƿ�.

    void Start()
    {        
        camAxis = transform.parent;
        mainCam = transform;
        camSpeed = 20f;        
        rotateY = 1f; // ������ ����Ǿ��� ��, ī�޶��� x�� ȸ���� �⺻������ ������.       
    }

    void Move()
    {
        Vector3 tmp = mainCam.position;          // ���� ī�޶��� ��ġ. 
        rotateX += dragOn.xAngle;                
        rotateY += dragOn.yAngle * -1;           // �巡�� ����� ī�޶� ȸ�� ������ ���߱� ���� -1�� ����.
        dragOn.xAngle = 0;
        dragOn.yAngle = 0;        
        if (rotateY > 1f) rotateY = 1f;          // ī�޶� �ʹ� ���� ȸ���ϴ� ���� �����ϴ� �ڵ�.
        if (rotateY < -0.5f) rotateY = -0.5f;
        /** ī�޶��� �߽����� ȭ���� �巡���� ����ŭ ȸ����Ŵ.
         * ī�޶�� �߽����� �ڽ� ������Ʈ�̹Ƿ� �Բ� ȸ����. */
        camAxis.rotation = Quaternion.Euler(new Vector3(
            camAxis.rotation.x + rotateY,
            camAxis.rotation.y + rotateX, 0f) * camSpeed);        
    }           
        
    void Update()
    {
        Move();        
    }
}
