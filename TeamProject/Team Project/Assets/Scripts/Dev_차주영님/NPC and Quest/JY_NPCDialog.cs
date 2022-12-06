using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JY_NPCDialog : MonoBehaviour
{    
    public Camera dialogCam;
    public GameObject BattleUI;

    public GameObject DialogUI;
    public Image DialogPortrait;
    public Text NPCNameText;
    public Text scriptText;
    public GameObject nextButton;
    public GameObject acceptButton;
    public GameObject rejectionButton;
    public GameObject finishButton;
    public GameObject exitButton;    
    public GameObject player;

    public GameObject dummy_1;
    public GameObject dummy_2;
    public List<Cinemachine.CinemachinePath> paths;
    int dialogPartNum;

    private void OnEnable()
    {
        player = JY_CharacterListManager.s_instance.playerList[0].gameObject;
    }

    public void EnterNpcDialog()
    {
        dialogCam.enabled = true;
        dialogCam.GetComponent<Cinemachine.CinemachineDollyCart>().m_Position = 0;
        dialogCam.GetComponent<Cinemachine.CinemachineDollyCart>().m_Path = paths[JY_QuestManager.s_instance.selectNpcNum];        
        
        NPCCamPosition(JY_QuestManager.s_instance.selectNpcNum);
        DialogUI.SetActive(true);
        DialogPortrait.sprite = JY_QuestManager.s_instance.NPCPortrait;
        BattleUI.SetActive(false);

        dialogScript(JY_QuestManager.s_instance.selectNpcNum);
    }

    public void dialogScript(int selectNPCNum)
    {
        dialogPartNum = 0;
        NPCNameText.text = JY_QuestManager.s_instance.QuestData[selectNPCNum][2];
        nextButton.SetActive(true);
        switch (selectNPCNum)
        {
            case 0:
                //수령
                if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[2] == 0)
                {
                    dialogPartNum = 5;
                    scriptText.text = JY_QuestManager.s_instance.QuestData[selectNPCNum][dialogPartNum];
                }
                //완료
                else if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[2] == 1
                    && JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[1]
                    >= int.Parse(JY_QuestManager.s_instance.QuestData[0][4])
                    && JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[3] == 0)
                {
                    dialogPartNum = 8;
                    scriptText.text = JY_QuestManager.s_instance.QuestData[selectNPCNum][dialogPartNum];
                }
                //진행중 대화
                else if(JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[2] == 1
                    && JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[3] == 0)
                {
                    dialogPartNum = 9;
                    scriptText.text = JY_QuestManager.s_instance.QuestData[selectNPCNum][dialogPartNum];
                    nextButton.SetActive(false);
                    acceptButton.SetActive(false);
                    rejectionButton.SetActive(false);
                    finishButton.SetActive(false);
                    exitButton.SetActive(true);
                }
                //미수령 및 완료 이후 대화
                else
                {
                    dialogPartNum = 10;
                    scriptText.text = JY_QuestManager.s_instance.QuestData[selectNPCNum][dialogPartNum];
                    nextButton.SetActive(false);
                    acceptButton.SetActive(false);
                    rejectionButton.SetActive(false);
                    finishButton.SetActive(false);
                    exitButton.SetActive(true);
                }
                break;
            case 1:
                if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[3] == 1 &&
                    JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress2[2] == 0)
                {
                    dialogPartNum = 5;
                    scriptText.text = JY_QuestManager.s_instance.QuestData[selectNPCNum][dialogPartNum];
                }
                //완료
                else if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress2[2] == 1
                    && JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress2[1]
                    >= int.Parse(JY_QuestManager.s_instance.QuestData[1][4])
                    && JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress2[3] == 0)
                {
                    dialogPartNum = 8;
                    scriptText.text = JY_QuestManager.s_instance.QuestData[selectNPCNum][dialogPartNum];
                }
                //진행중 대화
                else if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress2[2] == 1
                    && JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress2[3] == 0)
                {
                    dialogPartNum = 9;
                    scriptText.text = JY_QuestManager.s_instance.QuestData[selectNPCNum][dialogPartNum];
                    nextButton.SetActive(false);
                    acceptButton.SetActive(false);
                    rejectionButton.SetActive(false);
                    finishButton.SetActive(false);
                    exitButton.SetActive(true);
                }
                //미수령 및 완료 이후 대화
                else
                {
                    dialogPartNum = 10;
                    scriptText.text = JY_QuestManager.s_instance.QuestData[selectNPCNum][dialogPartNum];
                    nextButton.SetActive(false);
                    acceptButton.SetActive(false);
                    rejectionButton.SetActive(false);
                    finishButton.SetActive(false);
                    exitButton.SetActive(true);
                }
                break;
        }
        
    }
    public void NextDialog()
    {
        int questNum = JY_QuestManager.s_instance.selectNpcNum;
        dialogPartNum++;

        if(dialogPartNum == 7)
        {
            nextButton.SetActive(false);
            acceptButton.SetActive(true);
            rejectionButton.SetActive(true);
            finishButton.SetActive(false);
            exitButton.SetActive(false);
        }
        else if(dialogPartNum == 9)
        {
            nextButton.SetActive(false);
            acceptButton.SetActive(false);
            rejectionButton.SetActive(false);
            finishButton.SetActive(true);
            exitButton.SetActive(false);
            dialogPartNum = 7;
        }

        scriptText.text = JY_QuestManager.s_instance.QuestData[questNum][dialogPartNum];
    }
    public void quitNpcDialog()
    {
        dialogCam.enabled = false;
        nextButton.SetActive(false);
        acceptButton.SetActive(false);
        rejectionButton.SetActive(false);
        finishButton.SetActive(false);
        exitButton.SetActive(false);
        DialogUI.SetActive(false);
        BattleUI.SetActive(true);
        JY_UIManager.instance.StatusDataRenew();
    }

    public void NPCCamPosition(int NPCNum)
    {
        switch (NPCNum)
        {
            case 0:
                dialogCam.transform.position = dummy_1.transform.position;
                dialogCam.transform.rotation = dummy_1.transform.rotation;
                break;
            case 1:
                dialogCam.transform.position = dummy_2.transform.position;
                dialogCam.transform.rotation = dummy_2.transform.rotation;
                break;
        }
    }
}
