using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;           // 버튼을 사용하기 위해 using한 네임스페이스.
using UnityEngine.EventSystems; // 이벤트 트리거를 사용하기 위해 using한 네임스페이스.

// 전투 UI에 버튼들을 동적으로 할당하는 스크립트.
public class BattleUI : MonoBehaviour
{
    public static BattleUI instance;

    [Header("UI 패널")]
    public GameObject deathUI;
    public GameObject matchingUI;    
    
    Player player;  // 버튼에 할당할 플레이어 기능 함수 집합.
    [Header("플레이어 기능 버튼")]
    public Button normalAtk;
    public Button skill_1;
    public Button skill_2;
    public Button skill_3;
    public Button skill_4;
    public Button evasion;
    public EventTrigger lArm;           // lArm 기능은 버튼이 아닌 이벤트트리거를 사용함.
    private EventTrigger.Entry lArm_PointerDown;
    private EventTrigger.Entry lArm_PointerUp;

    [Header("UI 버튼")]
    public Button revive;
    public Button cancelMatching;
    public Button backToWorld;
    public Button selectFire;
    public Button selectUnderground;
    public Button solo;
    public Button doppio;

    [Header("스킬 쿨다운 이미지")]
    public Image cool_1;
    public Image cool_2;
    public Image cool_3;
    public Image cool_4;

    [Header("방패 업/다운 시 표시할 스프라이트")]
    public Sprite upFrame;
    public Sprite downFrame;

    [Header("팝업 메시지")]
    public Text equipEmpty;

    [Header("던전 입장 관련 변수")]
    public int targetSceneNum;
    public Transform selectArrow;
    public Text enterText;

    private void OnEnable()
    {
        instance ??= this;
    }
    private void OnDisable()
    {
        instance = null;
    }

    public Transform BuffLayout;

    void Start()
    {
        // 조작에 사용할 플레이어는 자기 자신의 플레이어뿐. (playerList의 0번)
        player = JY_CharacterListManager.s_instance.playerList[0];
        normalAtk.onClick.AddListener(player.NormalAttack);
        skill_1.onClick.AddListener(player.PowerStrike);        
        skill_2.onClick.AddListener(player.TurnAttack);        
        skill_3.onClick.AddListener(player.JumpAttack);        
        skill_4.onClick.AddListener(player.Warcry);        
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
        cancelMatching.onClick.AddListener(() => StartCoroutine(Matchable()));

        // 마을로 가는 포탈을 타면 방을 나옴.
        backToWorld.onClick.AddListener(() => NetworkManager.s_instance.LeaveRoom());
        backToWorld.onClick.AddListener(() => GameManager.s_instance.LoadScene(4));

        selectFire.onClick.AddListener(() => SelectDungeon(5));
        selectUnderground.onClick.AddListener(() => SelectDungeon(6));
        solo.onClick.AddListener(() => StartCoroutine(EnterDungeon(1)));
        doppio.onClick.AddListener(() => StartCoroutine(EnterDungeon(2)));
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

    // 던전 입장.
    public IEnumerator EnterDungeon(byte _people)
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Portal);
        // 매칭 UI를 활성화하고 매칭 시작.
        BattleUI.instance.matchingUI.SetActive(true);            
        NetworkManager.s_instance.MatchMaking(targetSceneNum, _people, 0);

        // 매칭이 시작되면 던전 입장 버튼은 비활성화됨. 매칭이 취소되면 1초 후 Interactable하게 됨.
        solo.interactable = false;
        doppio.interactable = false;

        // 매칭을 시작하자마자 바로 취소하게 되면, room 입장에 실패한 것으로 간주하여 NetworkManager에서 방을 새로이 접속하려는 코루틴이 실행됨.
        // 이를 막기 위해 매칭 취소 버튼은 2초 간 비활성화되었다가 2초 후에 매칭 취소가 가능해짐.
        cancelMatching.interactable = false;
        yield return new WaitForSeconds(2f);
        cancelMatching.interactable = true;
    }    

    public void SelectDungeon(int dungeonNum)
    {
        targetSceneNum = dungeonNum;
        Vector3 tmp = selectArrow.transform.localPosition;
        tmp.x = (dungeonNum == 5) ? 268.7277f : -54f;
        selectArrow.localPosition = tmp;              
        enterText.text = (dungeonNum == 5) ? "화염 던전에 입장하시겠습니까?" : "지하 던전에 입장하시겠습니까?";
        selectArrow.gameObject.SetActive(true);
        enterText.gameObject.SetActive(true);
        solo.gameObject.SetActive(true);
        doppio.gameObject.SetActive(true);
    }
    IEnumerator Matchable()
    {
        StopCoroutine("Matchable");
        yield return new WaitForSeconds(2f);
        solo.interactable = true;
        doppio.interactable = true;
    }
}
