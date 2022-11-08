using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;           // 버튼을 사용하기 위해 using한 네임스페이스.
using UnityEngine.EventSystems; // 이벤트 트리거를 사용하기 위해 using한 네임스페이스.

public class BattleUI : MonoBehaviour
{
    public static BattleUI instance;
    public GameObject deathUI;
    public GameObject matchingUI;    

    // 전투 UI에 버튼들을 동적으로 할당하는 스크립트.
    Player player;  // 버튼에 할당할 플레이어 기능 함수 집합.
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
    public EventTrigger lArm;           // lArm 기능은 버튼이 아닌 이벤트트리거를 사용함.
    private EventTrigger.Entry lArm_PointerDown;
    private EventTrigger.Entry lArm_PointerUp;
    public Sprite upFrame;
    public Sprite downFrame;
    [Header("팝업 메시지")]
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
        // 이하는 이벤트트리거에 동적으로 pointer up/down 이벤트에 함수를 할당하는 방법. (매개변수 필요)
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
        // 게임 매니저를 이용하여 부활하면 마을로 돌아가도록 함.
        revive.onClick.AddListener(() => NetworkManager.s_instance.LeaveRoom());
        revive.onClick.AddListener(() => GameManager.s_instance.LoadScene(4));
        // 매칭 취소 버튼을 누르면 방에서 나옴.
        cancelMatching.onClick.AddListener(() => NetworkManager.s_instance.LeaveRoom());
        cancelMatching.onClick.AddListener(() => matchingUI.SetActive(false));
        // 마을로 가는 포탈을 타면 방을 나옴.
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
