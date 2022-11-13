using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JY_PartDestruction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Camera mainCam;
    public Camera PartDestrcutionCam;
    public DragOn dragOn;
    [HideInInspector] public float camSpeed;          // 카메라 회전 속도.
    [HideInInspector] public float rotateX;           // 카메라의 상하 회전 값.
    [HideInInspector] public float rotateY;           // 카메라의 좌우 회전 값.
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
        rotateY += dragOn.yAngle * -1.1f;           // 드래그 방향과 카메라 회전 방향을 맞추기 위해 -1을 곱함.
        dragOn.xAngle = 0;
        dragOn.yAngle = 0;
        if (rotateY > 1.3f) rotateY = 1.3f;          // 카메라가 너무 많이 회전하는 것을 방지하는 코드.
        if (rotateY < -0.3f) rotateY = -0.3f;
        /** 카메라의 중심축을 화면을 드래그한 값만큼 회전시킴.
         * 카메라는 중심축의 자식 오브젝트이므로 함께 회전함. */
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
