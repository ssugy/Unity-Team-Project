using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;           // ��ư�� ����ϱ� ���� using�� ���ӽ����̽�.
using UnityEngine.EventSystems; // �̺�Ʈ Ʈ���Ÿ� ����ϱ� ���� using�� ���ӽ����̽�.

public class BattleUI : MonoBehaviour
{
    // ���� UI�� ��ư���� �������� �Ҵ��ϴ� ��ũ��Ʈ.
    PlayerController playerController;  // ��ư�� �Ҵ��� �÷��̾� ��� �Լ� ����.
    public Button normalAtk;
    public Button skill_1;
    public Button skill_2;
    public Button evasion;
    public EventTrigger lArm;           // lArm ����� ��ư�� �ƴ� �̺�ƮƮ���Ÿ� �����.
    private EventTrigger.Entry lArm_PointerDown;
    private EventTrigger.Entry lArm_PointerUp;
    public Sprite upFrame;
    public Sprite downFrame;

    // Start is called before the first frame update
    void Start()
    {
        playerController = PlayerController.player.GetComponent<PlayerController>();
        normalAtk.onClick.AddListener(playerController.NormalAttack);
        skill_1.onClick.AddListener(playerController.Skill_1);
        skill_2.onClick.AddListener(playerController.Skill_2);
        evasion.onClick.AddListener(playerController.Roll);
        // ���ϴ� �̺�ƮƮ���ſ� �������� pointer up/down �̺�Ʈ�� �Լ��� �Ҵ��ϴ� ���. (�Ű����� �ʿ�)
        lArm_PointerDown = new EventTrigger.Entry();
        lArm_PointerDown.eventID = EventTriggerType.PointerDown;
        lArm_PointerDown.callback.AddListener((data) => { playerController.LArmDown((PointerEventData)data); });
        lArm_PointerDown.callback.AddListener((data) => { LArmDown_Frame((PointerEventData)data); });
        lArm_PointerUp = new EventTrigger.Entry();
        lArm_PointerUp.eventID = EventTriggerType.PointerUp;
        lArm_PointerUp.callback.AddListener((data) => { playerController.LArmUp((PointerEventData)data); });
        lArm_PointerUp.callback.AddListener((data) => { LArmUp_Frame((PointerEventData)data); });
        lArm.triggers.Add(lArm_PointerDown);
        lArm.triggers.Add(lArm_PointerUp);

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
