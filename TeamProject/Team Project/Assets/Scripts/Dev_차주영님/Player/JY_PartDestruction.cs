using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JY_PartDestruction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IDragHandler
{
    public Camera mainCam;
    public GameObject Bullet;
    public DragOn dragOn;
    [HideInInspector] public float camSpeed;          // 카메라 회전 속도.
    [HideInInspector] public float rotateX;           // 카메라의 상하 회전 값.
    [HideInInspector] public float rotateY;           // 카메라의 좌우 회전 값.
    bool isShoot;
    MainCamController mainCamControl;
    Vector3 originPos;
    Vector3 targetPos;
    // Start is called before the first frame update
    void Awake()
    {
        isShoot = false;
        originPos = Vector3.zero;
        mainCamControl = mainCam.GetComponent<MainCamController>();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isShoot)
        {
            if (mainCam.gameObject.activeSelf)
            {
                mainCam.transform.position = targetPos;
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        mainCamControl.onPartDestruction = true;
        isShoot = true;
        originPos = mainCam.transform.position;
        targetPos = JY_CharacterListManager.s_instance.playerList[0].transform.position + Vector3.up;
        JY_UIManager.instance.ActiveAimUI(true);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        //광선을 쏜다.
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hitinfo;
        if(Physics.Raycast(ray, out hitinfo, Mathf.Infinity))
        {
            Vector3 shootVec = hitinfo.point - JY_CharacterListManager.s_instance.playerList[0].rWeaponDummy.position;
            GameObject bullet = GameObject.Instantiate<GameObject>(Bullet, JY_CharacterListManager.s_instance.playerList[0].rWeaponDummy.transform.position, Quaternion.identity, JY_CharacterListManager.s_instance.playerList[0].rWeaponDummy);
            Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = shootVec.normalized * 50f;
        }

        isShoot = false;
        mainCam.transform.position = originPos;
        JY_UIManager.instance.ActiveAimUI(false);
        mainCamControl.onPartDestruction = false;
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.PLYAER_SHOOT);
        //PartDestrcutionCam.gameObject.SetActive(false);
        //mainCam.gameObject.SetActive(true);
    }
    public void OnDrag(PointerEventData eventData)
    {
        JY_UIManager.instance.TranslateAimUI(eventData.position);
    }
}
