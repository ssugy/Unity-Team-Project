using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamController : MonoBehaviour
{       
    // ī�޶��� ȸ���� ī�޶� ���� �̵��� ���. ī�޶��� ���� �÷��̾��� ��ġ�� ����ٴ�.
    [HideInInspector] public Transform camAxis;       // ���� ī�޶��� �θ� ������Ʈ.    
    [HideInInspector] public float camSpeed;          // ī�޶� ȸ�� �ӵ�.
    [HideInInspector] public float rotateX;           // ī�޶��� ���� ȸ�� ��.
    [HideInInspector] public float rotateY;           // ī�޶��� �¿� ȸ�� ��.
    public DragOn dragOn;                             // ī�޶� ȸ���� ���� ȭ�� �巡�� ���� �޾ƿ�.
    public Transform player;    
    int layerMask;

    void Start()
    {        
        camAxis = transform.parent;        
        camSpeed = 30f;        
        rotateY = 0f; // ������ ����Ǿ��� ��, ī�޶��� x�� ȸ���� �⺻������ ������.
        layerMask = 1 << LayerMask.NameToLayer("Building");
        player = Player.instance.transform;
    }

    void Rotate()
    {
        rotateX += dragOn.xAngle * 6f;                
        rotateY += dragOn.yAngle * -1.1f;           // �巡�� ����� ī�޶� ȸ�� ������ ���߱� ���� -1�� ����.
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
