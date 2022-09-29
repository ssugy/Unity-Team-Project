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

    void QuestRenew(int QuestNum)
    {
        if(JY_QuestManager.s_instance.QuestDataList[QuestNum][5] == "TRUE" && JY_QuestManager.s_instance.QuestDataList[QuestNum][6] == "FALSE")
        {
            Quest_Title.text = JY_QuestManager.s_instance.QuestDataList[QuestNum][0];
            Quest_Main.text = JY_QuestManager.s_instance.QuestDataList[QuestNum][1];

            string checkText;
            int now = int.Parse(JY_QuestManager.s_instance.QuestDataList[QuestNum][3]);
            int goal = int.Parse(JY_QuestManager.s_instance.QuestDataList[QuestNum][4]);

            if (now < goal)
            {
                checkText = JY_QuestManager.s_instance.QuestDataList[QuestNum][2] + " "
                          + JY_QuestManager.s_instance.QuestDataList[QuestNum][3] + "/"
                          + JY_QuestManager.s_instance.QuestDataList[QuestNum][4];
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
    }
}
