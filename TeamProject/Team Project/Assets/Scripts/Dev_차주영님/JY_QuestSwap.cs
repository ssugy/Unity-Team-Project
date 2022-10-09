using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JY_QuestSwap : MonoBehaviour
{

    public Text Quest_Title;
    public Text Quest_Main;
    public Text Quest_check;

    public int QuestNum;
    int selectNum;

    public void Start()
    {
        selectNum = JY_CharacterListManager.s_instance.selectNum;
    }
    void QuestRenew(int QuestNum)
    {
        if(JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[2] == 1 &&
           JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[3] == 0)
        {
            Quest_Title.text = JY_QuestManager.s_instance.QuestData[0][0];
            Quest_Main.text = JY_QuestManager.s_instance.QuestData[0][1];

            string checkText;
            int now = JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[1];
            int goal = int.Parse(JY_QuestManager.s_instance.QuestData[0][4]);

            if (now < goal)
            {
                checkText = JY_QuestManager.s_instance.QuestData[0][3]+" "
                          + now.ToString()+ "/"
                          + JY_QuestManager.s_instance.QuestData[0][4];
            }
            else
            {
                checkText = "¿Ï·á";
            }
            Quest_check.text = checkText;
        }
        else
        {
            Quest_Title.text = null;
            Quest_Main.text = null;
            Quest_check.text = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        QuestRenew(QuestNum);
        JY_QuestManager.s_instance.questJournalTitleRenew();
    }
}
