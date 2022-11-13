using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JY_PartDestruction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Camera mainCam;
    public Camera PartDestrcutionCam;
    public DragOn dragOn;
    [HideInInspector] public float camSpeed;          // ī�޶� ȸ�� �ӵ�.
    [HideInInspector] public float rotateX;           // ī�޶��� ���� ȸ�� ��.
    [HideInInspector] public float rotateY;           // ī�޶��� �¿� ȸ�� ��.
    bool isShoot;
    // Start is called before the first frame update
    void Awake()
    {
        isShoot = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShoot)
        {
            if (mainCam.gameObject.activeSelf)
            {
                mainCam.gameObject.SetActive(false);
                PartDestrcutionCam.gameObject.SetActive(true);
            }
            PartDestrcutionCam.gameObject.transform.position = Player.instance.transform.position + new Vector3(0,2f,4f);
        }
    }
    void Rotate()
    {
        rotateX += dragOn.xAngle * 12f;
        rotateY += dragOn.yAngle * -1.1f;           // �巡�� ����� ī�޶� ȸ�� ������ ���߱� ���� -1�� ����.
        dragOn.xAngle = 0;
        dragOn.yAngle = 0;
        if (rotateY > 1.3f) rotateY = 1.3f;          // ī�޶� �ʹ� ���� ȸ���ϴ� ���� �����ϴ� �ڵ�.
        if (rotateY < -0.3f) rotateY = -0.3f;
        /** ī�޶��� �߽����� ȭ���� �巡���� ����ŭ ȸ����Ŵ.
         * ī�޶�� �߽����� �ڽ� ������Ʈ�̹Ƿ� �Բ� ȸ����. */
        PartDestrcutionCam.transform.rotation = Quaternion.Euler(new Vector3(
            PartDestrcutionCam.transform.rotation.x + rotateY,
            PartDestrcutionCam.transform.rotation.y + rotateX, 0f) * camSpeed);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isShoot = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isShoot = false;
        PartDestrcutionCam.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(true);
    }
}
