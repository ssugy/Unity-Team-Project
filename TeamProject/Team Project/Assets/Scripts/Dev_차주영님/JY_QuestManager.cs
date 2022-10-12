using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class JY_QuestManager : MonoBehaviour
{
    //����Ʈ �Ŵ����̱���
    public static JY_QuestManager instance;
    public static JY_QuestManager s_instance { get { return instance; } }
    
    public Dictionary<int, Dictionary<int, string>> QuestData;
    public GameObject dialogButton;
    public GameObject dialogUI;
    public Camera dialogCam;
    public GameObject Quest_1_Bar;
    public GameObject Quest_1_Panel;

    public Text journalButton1;
    public Text journalButton2;
    public Text journalButton3;
    public Text journalButton4;
    
    int selectNum;
    JY_NPCDialog dialogScript;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;

        QuestData = new Dictionary<int, Dictionary<int, string>>();
        JY_QuestData dataCompo = GetComponent<JY_QuestData>();
        dataCompo.questDataLoad(QuestData);
        dialogScript = dialogCam.GetComponent<JY_NPCDialog>();
        selectNum = JY_CharacterListManager.s_instance.selectNum;
        Debug.Log(JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[2]);
        Debug.Log(JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[3]);
        if (JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[2] == 1 &&
            JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[3] == 0)
        {
            Quest_1_Bar.SetActive(true);
        }
    }

    public void questJournalTitleRenew()
    {
        if(JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[2] == 1 &&
           JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[3] == 0)
            journalButton1.text = QuestData[0][0];
        else
            journalButton1.text = "-";
    }
    public void QuestProgress(int QuestNum)
    {
        JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[1]++;
    }

    public void QuestChecker(int QuestNum) {

        switch (QuestNum)
        {
            //����
            case 0:
                JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[2] = 1;
                Debug.Log("����Ʈ ����");
                Quest_1_Bar.SetActive(true);
                JY_CharacterListManager.s_instance.saveListData();
                //uiManager.questAcceptUI();
                break;
            //�Ϸ�
            case 1:
                JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[3] = 1;
                Debug.Log("����Ʈ �Ϸ�");
                Quest_1_Bar.SetActive(false);
                Quest_1_Panel.SetActive(false);
                JY_CharacterListManager.s_instance.saveListData();
                //uiManager.questFinishUI();
                break;
            default:
                break;
        }
        dialogUI.SetActive(false);
        dialogScript.quitNpcDialog();
        //����Ʈ ����
        /*
        if (JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[2]== 0)
        {
            JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[2] = 1;
            Debug.Log("����Ʈ ����");
            Quest_1_Bar.SetActive(true);
            JY_CharacterListManager.s_instance.saveListData();
        }

        //����Ʈ �Ϸ�
        else if (JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[2] == 1 
            && JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[1] >= int.Parse(QuestData[0][4]))
        {
            JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[3] = 1;
            Debug.Log("����Ʈ �Ϸ�");
            Quest_1_Bar.SetActive(false);
            Quest_1_Panel.SetActive(false);
            JY_CharacterListManager.s_instance.saveListData();
        }*/
    }

}
