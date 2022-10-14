using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class JY_QuestManager : MonoBehaviour
{
    //Äù½ºÆ® ¸Å´ÏÀú½Ì±ÛÅæ
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
    public JY_UIManager uiManager;
    JY_NPCDialog dialogScript;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;

        QuestData = new Dictionary<int, Dictionary<int, string>>();
        JY_QuestData dataCompo = GetComponent<JY_QuestData>();
        dataCompo.questDataLoad(QuestData);
        if (JY_CharacterListManager.s_instance != null)
        {
            selectNum = JY_CharacterListManager.s_instance.selectNum;
            if (JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[2] == 1 &&
                JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[3] == 0)
            {
                Quest_1_Bar.SetActive(true);
            }
        }
        uiManager= GetComponentInParent<JY_UIManager>();
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
        switch (QuestNum)
        {
            case 0:
                if (JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[2] == 1 &&
                    JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[3] == 0)
                    JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[1]++;
                break;
        }
    }

    public void QuestChecker(int QuestNum) {
        dialogScript = dialogCam.GetComponent<JY_NPCDialog>();
        switch (QuestNum)
        {
            //¼ö·É
            case 0:
                JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[2] = 1;
                Debug.Log("Äù½ºÆ® ¼ö·É");
                Quest_1_Bar.SetActive(true);
                JY_CharacterListManager.s_instance.saveListData();
                uiManager.questAcceptUI();
                break;
            //¿Ï·á
            case 1:
                JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[3] = 1;
                Debug.Log("Äù½ºÆ® ¿Ï·á");
                Quest_1_Bar.SetActive(false);
                Quest_1_Panel.SetActive(false);
                JY_CharacterListManager.s_instance.saveListData();
                uiManager.questFinishUI();
                break;
            default:
                break;
        }
        dialogUI.SetActive(false);
        dialogScript.quitNpcDialog();
        //Äù½ºÆ® ¼ö·É
        /*
        if (JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[2]== 0)
        {
            JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[2] = 1;
            Debug.Log("Äù½ºÆ® ¼ö·É");
            Quest_1_Bar.SetActive(true);
            JY_CharacterListManager.s_instance.saveListData();
        }

        //Äù½ºÆ® ¿Ï·á
        else if (JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[2] == 1 
            && JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[1] >= int.Parse(QuestData[0][4]))
        {
            JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[3] = 1;
            Debug.Log("Äù½ºÆ® ¿Ï·á");
            Quest_1_Bar.SetActive(false);
            Quest_1_Panel.SetActive(false);
            JY_CharacterListManager.s_instance.saveListData();
        }*/
    }
}
