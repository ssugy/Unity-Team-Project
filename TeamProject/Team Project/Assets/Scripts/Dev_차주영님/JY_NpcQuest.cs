using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_NpcQuest : MonoBehaviour
{
    public GameObject questMark;
    public GameObject questMark_Gray;
    public GameObject questMark_Complete;
    public Transform targetCam;
    public Transform Player;
    Transform playerTr;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Player.childCount ;i++)
        {
            if (Player.GetChild(i).gameObject.activeSelf == true)
            {
                playerTr = Player.GetChild(i).transform;
                break;
            }
        }
        QuestNpcChecker(gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        QuestNpcChecker(gameObject.name);
        //markBillborad();
        DialogBtnActivate();
    }
    public void QuestNpcChecker(string npcName)
    {
        for (int i = 0; i < JY_QuestManager.s_instance.QuestDataList.Count; i++)
        {
            //해당 npc에게 player가 받지 않은 퀘스트가 있을경우 questmark 활성화
            if (JY_QuestManager.s_instance.QuestDataList[i][7] == npcName )
            {
                if(JY_QuestManager.s_instance.QuestDataList[i][5] == "FALSE")
                    questMark.SetActive(true);
                else if(JY_QuestManager.s_instance.QuestDataList[i][5] == "TRUE" &&
                        int.Parse(JY_QuestManager.s_instance.QuestDataList[i][3]) >= int.Parse(JY_QuestManager.s_instance.QuestDataList[i][4]) &&
                        JY_QuestManager.s_instance.QuestDataList[i][6] == "FALSE")
                {
                    questMark.SetActive(false);
                    questMark_Gray.SetActive(false);
                    questMark_Complete.SetActive(true);
                }
                else if (JY_QuestManager.s_instance.QuestDataList[i][5] == "TRUE" &&
                        JY_QuestManager.s_instance.QuestDataList[i][6] == "FALSE")
                {
                    questMark.SetActive(false);
                    questMark_Gray.SetActive(true);
                    questMark_Complete.SetActive(false);
                }
                else
                {
                    questMark.SetActive(false);
                    questMark_Gray.SetActive(false);
                    questMark_Complete.SetActive(false);
                }
            }
        }
    }

    public void markBillborad()
    {
        questMark.transform.LookAt(targetCam);
        questMark_Gray.transform.LookAt(targetCam);
        questMark_Complete.transform.LookAt(targetCam);
    }

    public void DialogBtnActivate()
    {
        float distance = Vector3.Distance(playerTr.position,transform.position);

        if (distance < 2f)
        {
            JY_QuestManager.s_instance.dialogButton.SetActive(true);
        }
        else
        {
            JY_QuestManager.s_instance.dialogButton.SetActive(false);
        }
    }
}
