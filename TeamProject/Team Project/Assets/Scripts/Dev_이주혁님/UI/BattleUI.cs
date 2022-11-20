using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;           // ��ư�� ����ϱ� ���� using�� ���ӽ����̽�.
using UnityEngine.EventSystems; // �̺�Ʈ Ʈ���Ÿ� ����ϱ� ���� using�� ���ӽ����̽�.

// ���� UI�� ��ư���� �������� �Ҵ��ϴ� ��ũ��Ʈ.
public class BattleUI : MonoBehaviour
{
    public static BattleUI instance;

    [Header("UI �г�")]
    public GameObject deathUI;
    public GameObject matchingUI;    
    
    Player player;  // ��ư�� �Ҵ��� �÷��̾� ��� �Լ� ����.
    [Header("�÷��̾� ��� ��ư")]
    public Button normalAtk;
    public Button skill_1;
    public Button skill_2;
    public Button skill_3;
    public Button skill_4;
    public Button evasion;
    public EventTrigger lArm;           // lArm ����� ��ư�� �ƴ� �̺�ƮƮ���Ÿ� �����.
    private EventTrigger.Entry lArm_PointerDown;
    private EventTrigger.Entry lArm_PointerUp;

    [Header("UI ��ư")]
    public Button revive;
    public Button cancelMatching;
    public Button backToWorld;
    public Button selectFire;
    public Button selectUnderground;
    public Button solo;
    public Button doppio;

    [Header("��ų ��ٿ� �̹���")]
    public Image cool_1;
    public Image cool_2;
    public Image cool_3;
    public Image cool_4;

    [Header("���� ��/�ٿ� �� ǥ���� ��������Ʈ")]
    public Sprite upFrame;
    public Sprite downFrame;

    [Header("�˾� �޽���")]
    public Text equipEmpty;

    [Header("���� ���� ���� ����")]
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
        // ���ۿ� ����� �÷��̾�� �ڱ� �ڽ��� �÷��̾��. (playerList�� 0��)
        player = JY_CharacterListManager.s_instance.playerList[0];
        normalAtk.onClick.AddListener(player.NormalAttack);
        skill_1.onClick.AddListener(player.PowerStrike);        
        skill_2.onClick.AddListener(player.TurnAttack);        
        skill_3.onClick.AddListener(player.JumpAttack);        
        skill_4.onClick.AddListener(player.Warcry);        
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
        cancelMatching.onClick.AddListener(() => StartCoroutine(Matchable()));

        // ������ ���� ��Ż�� Ÿ�� ���� ����.
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

    // ���� ����.
    public IEnumerator EnterDungeon(byte _people)
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Portal);
        // ��Ī UI�� Ȱ��ȭ�ϰ� ��Ī ����.
        BattleUI.instance.matchingUI.SetActive(true);            
        NetworkManager.s_instance.MatchMaking(targetSceneNum, _people, 0);

        // ��Ī�� ���۵Ǹ� ���� ���� ��ư�� ��Ȱ��ȭ��. ��Ī�� ��ҵǸ� 1�� �� Interactable�ϰ� ��.
        solo.interactable = false;
        doppio.interactable = false;

        // ��Ī�� �������ڸ��� �ٷ� ����ϰ� �Ǹ�, room ���忡 ������ ������ �����Ͽ� NetworkManager���� ���� ������ �����Ϸ��� �ڷ�ƾ�� �����.
        // �̸� ���� ���� ��Ī ��� ��ư�� 2�� �� ��Ȱ��ȭ�Ǿ��ٰ� 2�� �Ŀ� ��Ī ��Ұ� ��������.
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
        enterText.text = (dungeonNum == 5) ? "ȭ�� ������ �����Ͻðڽ��ϱ�?" : "���� ������ �����Ͻðڽ��ϱ�?";
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
