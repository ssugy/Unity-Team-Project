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
        Quest_Title.text = JY_QuestManger.s_instance.QuestDataList[QuestNum][0];
        Quest_Main.text = JY_QuestManger.s_instance.QuestDataList[QuestNum][1];

        string checkText;
        int now = int.Parse(JY_QuestManger.s_instance.QuestDataList[QuestNum][3]);
        int goal = int.Parse(JY_QuestManger.s_instance.QuestDataList[QuestNum][4]);

        if (now < goal)
        {
            checkText = JY_QuestManger.s_instance.QuestDataList[QuestNum][2] + " "
                      + JY_QuestManger.s_instance.QuestDataList[QuestNum][3] + "/"
                      + JY_QuestManger.s_instance.QuestDataList[QuestNum][4];
        }
        else
        {
            checkText = "¿Ï·á";
        }
        Quest_check.text = checkText;
    }
    // Update is called once per frame
    void Update()
    {
        QuestRenew(QuestNum);
    }
}
