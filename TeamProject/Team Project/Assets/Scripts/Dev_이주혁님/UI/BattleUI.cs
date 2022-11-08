using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;           // ��ư�� ����ϱ� ���� using�� ���ӽ����̽�.
using UnityEngine.EventSystems; // �̺�Ʈ Ʈ���Ÿ� ����ϱ� ���� using�� ���ӽ����̽�.

public class BattleUI : MonoBehaviour
{
    public static BattleUI instance;
    public GameObject deathUI;
    public GameObject matchingUI;    

    // ���� UI�� ��ư���� �������� �Ҵ��ϴ� ��ũ��Ʈ.
    Player player;  // ��ư�� �Ҵ��� �÷��̾� ��� �Լ� ����.
    public Button normalAtk;
    public Button skill_1;
    public Button skill_2;
    public Button skill_3;
    public Button skill_4;
    public Button revive;
    public Button cancelMatching;
    public Button backToWorld;
    public Button backToWorldClose;
    public Image cool_1;
    public Image cool_2;
    public Image cool_3;
    public Image cool_4;
    public Button evasion;
    public EventTrigger lArm;           // lArm ����� ��ư�� �ƴ� �̺�ƮƮ���Ÿ� �����.
    private EventTrigger.Entry lArm_PointerDown;
    private EventTrigger.Entry lArm_PointerUp;
    public Sprite upFrame;
    public Sprite downFrame;
    [Header("�˾� �޽���")]
    public Text equipEmpty;    
    void Start()
    {
        instance ??= this;
        player = Player.instance;
        normalAtk.onClick.AddListener(player.NormalAttack);
        skill_1.onClick.AddListener(player.PowerStrike);
        //skill_1.onClick.AddListener(() => StartCoroutine(Cooldown(4f, skill_1, cool_1)));
        skill_2.onClick.AddListener(player.TurnAttack);
        //skill_2.onClick.AddListener(() => StartCoroutine(Cooldown(4f, skill_2, cool_2)));
        skill_3.onClick.AddListener(player.JumpAttack);
        //skill_3.onClick.AddListener(() => StartCoroutine(Cooldown(8f, skill_3, cool_3)));
        skill_4.onClick.AddListener(player.Warcry);
        //skill_4.onClick.AddListener(() => StartCoroutine(Cooldown(10f, skill_4, cool_4)));
        evasion.onClick.AddListener(player.Roll);
        // ���ϴ� �̺�ƮƮ���ſ� �������� pointer up/down �̺�Ʈ�� �Լ��� �Ҵ��ϴ� ���. (�Ű����� �ʿ�)
        lArm_PointerDown = new EventTrigger.Entry();
        lArm_PointerDown.eventID = EventTriggerType.PointerDown;
        lArm_PointerDown.callback.AddListener((data) => { player.LArmDown((PointerEventData)data); });
        lArm_PointerDown.callback.AddListener((data) => { LArmDown_Frame((PointerEventData)data); });
        lArm_PointerUp = new EventTrigger.Entry();
        lArm_PointerUp.eventID = EventTriggerType.PointerUp;
        lArm_PointerUp.callback.AddListener((data) => { player.LArmUp((PointerEventData)data); });
        lArm_PointerUp.callback.AddListener((data) => { LArmUp_Frame((PointerEventData)data); });
        lArm.triggers.Add(lArm_PointerDown);
        lArm.triggers.Add(lArm_PointerUp);
        // ���� �Ŵ����� �̿��Ͽ� ��Ȱ�ϸ� ������ ���ư����� ��.
        revive.onClick.AddListener(() => NetworkManager.s_instance.LeaveRoom());
        revive.onClick.AddListener(() => GameManager.s_instance.LoadScene(4));
        // ��Ī ��� ��ư�� ������ �濡�� ����.
        cancelMatching.onClick.AddListener(() => NetworkManager.s_instance.LeaveRoom());
        cancelMatching.onClick.AddListener(() => matchingUI.SetActive(false));
        // ������ ���� ��Ż�� Ÿ�� ���� ����.
        backToWorld.onClick.AddListener(() => NetworkManager.s_instance.LeaveRoom());
        backToWorld.onClick.AddListener(() => GameManager.s_instance.LoadScene(4));
        backToWorldClose.onClick.AddListener(() => backToWorldClose.transform.parent.gameObject.SetActive(false));
    }
    private void OnEnable()
    {
        instance ??= this;
    }
    private void OnDisable()
    {
        instance = null;
    }

    void LArmDown_Frame(PointerEventData data)
    {
        Image frame = lArm.transform.GetComponent<Image>();
        frame.sprite = downFrame;
    }
    void LArmUp_Frame(PointerEventData data)
    {
        Image frame = lArm.transform.GetComponent<Image>();
        frame.sprite = upFrame;
    }   
    public IEnumerator Cooldown(float _cooltime, Button _button, Image _image)
    {
        _button.interactable = false;
        _image.fillAmount = 1f;
        StartCoroutine(Cooldown_Sprite(_cooltime, _image));
        yield return new WaitForSeconds(_cooltime + 0.1f);
        _button.interactable = true;
    }
    IEnumerator Cooldown_Sprite(float _cooltime, Image _image)
    {        
        _image.fillAmount -= 0.1f/_cooltime;
        yield return new WaitForSeconds(0.1f);
        if (_image.fillAmount > 0.025f)
        {
            StartCoroutine(Cooldown_Sprite(_cooltime, _image));
        }

    }
    
}
