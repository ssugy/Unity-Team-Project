using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JY_PartDestruction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IDragHandler
{
    public Camera mainCam;
    public GameObject Bullet;
    public DragOn dragOn;
    [HideInInspector] public float camSpeed;          // ī�޶� ȸ�� �ӵ�.
    [HideInInspector] public float rotateX;           // ī�޶��� ���� ȸ�� ��.
    [HideInInspector] public float rotateY;           // ī�޶��� �¿� ȸ�� ��.
    bool isShoot;
    MainCamController mainCamControl;
    Vector3 originPos;
    Vector3 targetPos;
    Vector3 shootVec;
    // Start is called before the first frame update
    void Awake()
    {
        isShoot = false;
        originPos = Vector3.zero;
        mainCamControl = mainCam.GetComponent<MainCamController>();
    }

    void Update()
    {
        if (isShoot && mainCam.gameObject.activeSelf)
        {
            mainCam.transform.localPosition = targetPos;
            mainCam.transform.localRotation = Quaternion.Euler(-15f, 0f, 0f);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mainCamControl.onPartDestruction = true;
        isShoot = true;
        originPos = mainCam.transform.localPosition;
        targetPos = originPos + new Vector3(0f,-1f,-1f);
        JY_UIManager.instance.ActiveAimUI(true);
    }

    /// <summary>
    /// �����ı��� ����ü�� ��� ��ġ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo, Mathf.Infinity))
            shootVec = hitinfo.point - JY_CharacterListManager.s_instance.playerList[0].rWeaponDummy.position;
        JY_UIManager.instance.TranslateAimUI(eventData.position);
    }
    
    /// <summary>
    /// �����ı��� ����ü�� �߻��ϴ� �Լ�
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        GameObject bullet = GameObject.Instantiate<GameObject>(Bullet, JY_CharacterListManager.s_instance.playerList[0].rWeaponDummy.transform.position
                                                            , Quaternion.identity, JY_CharacterListManager.s_instance.playerList[0].rWeaponDummy);
        Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = shootVec.normalized * 50f;

        isShoot = false;
        mainCam.transform.localPosition = originPos;
        mainCam.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        JY_UIManager.instance.ActiveAimUI(false);
        mainCamControl.onPartDestruction = false;
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.PLYAER_SHOOT);
    }
}
