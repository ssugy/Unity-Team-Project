using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JY_PartDestruction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Camera mainCam;
    public Camera PartDestrcutionCam;
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
        //PartDestrcutionCam.transform.SetParent(Player.instance.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (isShoot)
        {
            if (mainCam.gameObject.activeSelf)
            {
                mainCam.transform.position = targetPos;
                //mainCam.gameObject.SetActive(false);
                //PartDestrcutionCam.gameObject.SetActive(true);
            }
            //PartDestrcutionCam.gameObject.transform.localPosition=new Vector3(0,2f,4f);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        mainCamControl.onPartDestruction = true;
        isShoot = true;
        originPos = mainCam.transform.position;
        targetPos = Player.instance.transform.position + Vector3.up;
        JY_UIManager.instance.ActiveAimUI(true);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        //화면 가운데에서 광선을 쏜다.
        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit hitinfo;
        if(Physics.Raycast(ray, out hitinfo, Mathf.Infinity))
        {
            Vector3 shootVec = hitinfo.point - Player.instance.rWeaponDummy.position;
            GameObject bullet = GameObject.Instantiate<GameObject>(Bullet, Player.instance.rWeaponDummy.transform.position, Quaternion.identity,Player.instance.rWeaponDummy);
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
}
