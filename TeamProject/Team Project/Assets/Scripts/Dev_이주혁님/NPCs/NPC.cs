using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NpcType
{
    NONE,
    QUEST,
    MERCHANT
}
public class NPC : MonoBehaviour
{
    public int index;
    public NpcType TYPE;
    public string NAME;

    public JY_NPCDialog npcDialog;
    public Camera dialogCam;
    public Sprite portrait;
        
    public GameObject dialogUI;
    public Image DialogPortrait;
    public Text dialogText;
    public Text nameText;

    public GameObject buyButton;
    public GameObject sellButton;
    public GameObject produceButton;
    public GameObject enhanceButton;
    public GameObject exitButton;
    public GameObject nextButton;

    public Button talk;
    public Cinemachine.CinemachinePath path;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("대화 버튼 활성화");
            switch (TYPE)
            {
                case NpcType.QUEST:
                    {                        
                        talk.onClick.RemoveAllListeners();
                        talk.onClick.AddListener(() => npcDialog.EnterNpcDialog());
                        JY_QuestManager.s_instance.dialogButton.SetActive(true);
                        break;
                    }
                case NpcType.MERCHANT:
                    {                        
                        talk.onClick.RemoveAllListeners();
                        talk.onClick.AddListener(() => EnterNpcDialog());
                        JY_QuestManager.s_instance.dialogButton.SetActive(true);
                        break;
                    }
                default:
                    {
                        Debug.Log("NPC 타입 설정 되지 않음.");
                        break;
                    }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        JY_QuestManager.s_instance.dialogButton.SetActive(false);
    }
    public void EnterNpcDialog()
    {
        
        dialogCam.enabled = true;
        dialogCam.GetComponent<Cinemachine.CinemachineDollyCart>().m_Position = 0;
        dialogCam.GetComponent<Cinemachine.CinemachineDollyCart>().m_Path = path;
                
        dialogUI.SetActive(true);
        DialogPortrait.sprite = portrait;
        BattleUI.instance.gameObject.SetActive(false);

        nextButton.SetActive(false);
        exitButton.SetActive(true);
        buyButton.SetActive(true);
        sellButton.SetActive(true);
        produceButton.SetActive(true);
        enhanceButton.SetActive(true);

        nameText.text = NAME;
        dialogText.text = "어서 오시게. 지금은 마을 일손이 부족해 내가 상인 역할도 맡고 있지.";

    }
}
