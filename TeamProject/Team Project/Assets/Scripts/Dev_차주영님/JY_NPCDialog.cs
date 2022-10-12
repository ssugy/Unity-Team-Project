using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JY_NPCDialog : MonoBehaviour
{
    public Camera mainCam;
    public Camera minimapCam;
    public Camera dialogCam;
    public GameObject BattleUI;

    public GameObject DialogUI;
    public Text scriptText;
    public GameObject acceptButton;
    public GameObject rejectionButton;
    public GameObject finishButton;
    public GameObject exitButton;
    public GameObject npc;


    private void Start()
    {
        EnterNpcDialog();
    }
    public void EnterNpcDialog()
    {
        minimapCam.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(false);
        dialogCam.gameObject.SetActive(true);
        DialogUI.SetActive(true);
        BattleUI.SetActive(false);

        dialogScript();
    }

    public void dialogScript()
    {
        if (JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[2] == 0)
        {
            scriptText.text = "���ϴ����� ���̷������ ���� ġ�� �ֽ��ϴ�. ���� ������ ���� ���ؼ��� ����� �ñ��մϴ�. �����ֽ� �� �����Ű���?";
            acceptButton.SetActive(true);
            rejectionButton.SetActive(true);
            finishButton.SetActive(false);
            exitButton.SetActive(false);
        }

        else if (JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[2] == 1
            && JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[1] 
            >= int.Parse( JY_QuestManager.s_instance.QuestData[0][4]))
        {
            scriptText.text = "�����ּż� �����մϴ�!";
            acceptButton.SetActive(false);
            rejectionButton.SetActive(false);
            finishButton.SetActive(true);
            exitButton.SetActive(false);
        }
        else
        {
            scriptText.text = "����� ��ġ�ø� �ٽ� ���� �ɾ��ּ���.";
            acceptButton.SetActive(false);
            rejectionButton.SetActive(false);
            finishButton.SetActive(false);
            exitButton.SetActive(true);
        }
    }

    public void quitNpcDialog()
    {
        minimapCam.gameObject.SetActive(true);
        mainCam.gameObject.SetActive(true);
        dialogCam.gameObject.SetActive(false);
        DialogUI.SetActive(false);
        BattleUI.SetActive(true);

    }
}
