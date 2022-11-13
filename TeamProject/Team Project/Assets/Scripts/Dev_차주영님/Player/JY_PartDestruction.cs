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
