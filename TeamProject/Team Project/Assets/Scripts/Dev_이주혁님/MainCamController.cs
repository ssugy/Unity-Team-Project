using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamController : MonoBehaviour
{       
    // ī�޶��� ȸ������ ���. ī�޶��� ��ġ�� �÷��̾� ������Ʈ�� ����ٴ�.
    [HideInInspector] public Transform camAxis;       // ���� ī�޶��� �θ� ������Ʈ.
    [HideInInspector] public Transform mainCam;       // ���� ī�޶� ������Ʈ. = transform
    [HideInInspector] public float camSpeed;          // ī�޶� ȸ�� �ӵ�.
    [HideInInspector] public float rotateX;           // ī�޶��� ���� ȸ�� ��.
    [HideInInspector] public float rotateY;           // ī�޶��� �¿� ȸ�� ��.
    public DragOn dragOn;                             // ī�޶� ȸ���� ���� ȭ�� �巡�� ���� �޾ƿ�.
    public Transform player;    
    int layerMask;

    void Start()
    {        
        camAxis = transform.parent;
        mainCam = transform;
        camSpeed = 20f;        
        rotateY = 1f; // ������ ����Ǿ��� ��, ī�޶��� x�� ȸ���� �⺻������ ������.
        layerMask = 1 << LayerMask.NameToLayer("Building");
    }

    void Rotate()
    {
        Vector3 tmp = mainCam.position;          // ���� ī�޶��� ��ġ. 
        rotateX += dragOn.xAngle;                
        rotateY += dragOn.yAngle * -1;           // �巡�� ����� ī�޶� ȸ�� ������ ���߱� ���� -1�� ����.
        dragOn.xAngle = 0;
        dragOn.yAngle = 0;        
        if (rotateY > 1.3f) rotateY = 1.3f;          // ī�޶� �ʹ� ���� ȸ���ϴ� ���� �����ϴ� �ڵ�.
        if (rotateY < -0.3f) rotateY = -0.3f;
        /** ī�޶��� �߽����� ȭ���� �巡���� ����ŭ ȸ����Ŵ.
         * ī�޶�� �߽����� �ڽ� ������Ʈ�̹Ƿ� �Բ� ȸ����. */
        camAxis.rotation = Quaternion.Euler(new Vector3(
            camAxis.rotation.x + rotateY,
            camAxis.rotation.y + rotateX, 0f) * camSpeed);        
    }          
    void Move()
    {
        
        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 dir = Vector3.Normalize(transform.position - (player.position + new Vector3(0, 0.9f, 0)));
        RaycastHit hitInfo;
        if (Physics.Raycast(player.position + new Vector3(0, 0.9f, 0), dir, out hitInfo, distance, layerMask)) 
        {
            transform.position = hitInfo.point;
        }
        else
        {
            transform.localPosition = new Vector3(0, 1.8f, -5f);
        }
    }
        
    void Update()
    {
        Rotate();
        Move();        
    }
}
