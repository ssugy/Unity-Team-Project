using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class JY_QuestManager : MonoBehaviour
{
    //퀘스트 매니저싱글톤
    public static JY_QuestManager instance;
    public static JY_QuestManager s_instance { get { return instance; } }
    
    Dictionary<int, Dictionary<int, string>> questData;
    public Dictionary<int, Dictionary<int, string>> QuestData { get { return questData; } }
    public JY_QuestData QuestData_1;
    public JY_QuestData QuestData_2;

    public GameObject dialogButton;
    public GameObject dialogUI;
    public Camera dialogCam;
    public GameObject Quest_1_Bar;
    public GameObject Quest_1_Panel;

    public Text journalButton1;
    public Text journalButton2;
    public Text journalButton3;
    public Text journalButton4;
    
    public int selectNum;
    public int selectNpcNum;
    public JY_UIManager uiManager;
    public Sprite NPCPortrait;
    JY_NPCDialog dialogScript;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;

        questData = new Dictionary<int, Dictionary<int, string>>();
        questDataLoad(0,QuestData_1);
        questDataLoad(1,QuestData_2);
        if (JY_CharacterListManager.s_instance != null)
        {
            selectNum = JY_CharacterListManager.s_instance.selectNum;
            //진행 중인 퀘스트가 있을 경우 툴바 활성화 
            if ( (JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress[2] == 1 &&JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress[3] == 0) || 
                 (JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress2[2] == 1 &&JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress2[3] == 0) )
            {
                Quest_1_Bar.SetActive(true);
            }
        }        
        selectNpcNum = -1;
    }
    private void OnEnable()
    {
        instance ??= this;
    }
    private void OnDisable()
    {
        instance = null;
    }

    public void questJournalTitleRenew()
    {
        if(JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress[2] == 1 &&
           JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress[3] == 0)
            journalButton1.text = QuestData[0][0];
        else
            journalButton1.text = "-";
    }
    public void QuestProgress(int QuestNum)
    {
        switch (QuestNum)
        {
            case 0:
                if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress[2] == 1 &&
                    JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress[3] == 0)
                    JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress[1]++;
                break;
            case 1:
                if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress2[2] == 1 &&
                    JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress2[3] == 0)
                {
                    Debug.Log("enter");
                    JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress2[1]++;
                }

                break;
        }
    }

    public void QuestChecker(int state) {
        dialogScript = dialogCam.GetComponent<JY_NPCDialog>();
        switch (state)
        {
            //수령
            case 0:
                if (selectNpcNum == 0)
                {

                    JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[2] = 1;
                }
                else
                {
                    JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress2[2] = 1;
                }
                Debug.Log("퀘스트 수령");
                Quest_1_Bar.SetActive(true);                
                uiManager.questAcceptUI();
                break;
            //완료
            case 1:
                if (selectNpcNum == 0)
                {

                    JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress[3] = 1;
                    JY_CharacterListManager.s_instance.playerList[0].questExp(15);
                }
                else
                {
                    JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].questProgress2[3] = 1;
                    JY_CharacterListManager.s_instance.playerList[0].questExp(30);
                }
                Debug.Log("퀘스트 완료");
                Quest_1_Bar.SetActive(false);
                Quest_1_Panel.SetActive(false);                
                uiManager.questFinishUI();
                break;
            default:
                break;
        }
        dialogUI.SetActive(false);
        dialogScript.quitNpcDialog();
    }
    public void questDataLoad(int _QuestNum,JY_QuestData _QuestData)
    {
        Dictionary<int, string> _questData = new Dictionary<int, string>();
        _questData.Add(0, _QuestData.QUESTNAME);
        _questData.Add(1, _QuestData.QUESTCONTENT);
        _questData.Add(2, _QuestData.QUESTNPC);
        _questData.Add(3, _QuestData.QUESTTARGET);
        _questData.Add(4, _QuestData.QUESTGOAL.ToString());
        _questData.Add(5, _QuestData.QUESTDIALOG_1);
        _questData.Add(6, _QuestData.QUESTDIALOG_2);
        _questData.Add(7, _QuestData.QUESTDIALOG_REWARD);
        _questData.Add(8, _QuestData.QUESTDIALOG_3);
        _questData.Add(9, _QuestData.QUESTDIALOG_4);
        _questData.Add(10, _QuestData.QUESTDIALOG_5);

        questData.Add(_QuestNum, _questData);
    }
}
