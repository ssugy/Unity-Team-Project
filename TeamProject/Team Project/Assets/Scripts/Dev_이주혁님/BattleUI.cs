using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;           // 버튼을 사용하기 위해 using한 네임스페이스.
using UnityEngine.EventSystems; // 이벤트 트리거를 사용하기 위해 using한 네임스페이스.

public class BattleUI : MonoBehaviour
{
    // 전투 UI에 버튼들을 동적으로 할당하는 스크립트.
    PlayerController playerController;  // 버튼에 할당할 플레이어 기능 함수 집합.
    public Button normalAtk;
    public Button skill_1;
    public Button skill_2;
    public Button evasion;
    public EventTrigger lArm;           // lArm 기능은 버튼이 아닌 이벤트트리거를 사용함.
    private EventTrigger.Entry lArm_PointerDown;
    private EventTrigger.Entry lArm_PointerUp;

    // Start is called before the first frame update
    void Start()
    {
        playerController = PlayerController.player.GetComponent<PlayerController>();
        normalAtk.onClick.AddListener(playerController.NormalAttack);
        skill_1.onClick.AddListener(playerController.Skill_1);
        skill_2.onClick.AddListener(playerController.Skill_2);
        evasion.onClick.AddListener(playerController.Roll);
        // 이하는 이벤트트리거에 동적으로 pointer up/down 이벤트에 함수를 할당하는 방법. (매개변수 필요)
        lArm_PointerDown = new EventTrigger.Entry();
        lArm_PointerDown.eventID = EventTriggerType.PointerDown;
        lArm_PointerDown.callback.AddListener((data) => { playerController.LArmDown((PointerEventData)data); });
        lArm_PointerUp = new EventTrigger.Entry();
        lArm_PointerUp.eventID = EventTriggerType.PointerUp;
        lArm_PointerUp.callback.AddListener((data) => { playerController.LArmUp((PointerEventData)data); });
        lArm.triggers.Add(lArm_PointerDown);
        lArm.triggers.Add(lArm_PointerUp);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
