using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_NpcQuest : MonoBehaviour
{
    public GameObject questMark;
    public GameObject questMark2;
    public Transform target;
    public Transform target2;

    // Start is called before the first frame update
    void Start()
    {
        QuestNpcChecker(gameObject.name);
        target = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        markBillborad();
        DialogBtnActivate();
    }
    public void QuestNpcChecker(string npcName)
    {
        for (int i = 0; i < JY_QuestManger.s_instance.QuestDataList.Count; i++)
        {
            //해당 npc에게 player가 받지 않은 퀘스트가 있을경우 questmark 활성화
            if (JY_QuestManger.s_instance.QuestDataList[i][7] == npcName )
            {
                if (JY_QuestManger.s_instance.QuestDataList[i][6] == "TRUE")
                    continue;
                if(JY_QuestManger.s_instance.QuestDataList[i][5] == "FALSE")
                    questMark.SetActive(true);
                else if(JY_QuestManger.s_instance.QuestDataList[i][5] == "TRUE" &&
                        JY_QuestManger.s_instance.QuestDataList[i][3] == JY_QuestManger.s_instance.QuestDataList[i][4] &&
                        JY_QuestManger.s_instance.QuestDataList[i][6] == "FALSE")
                {
                    questMark.SetActive(false);
                    questMark2.SetActive(true);
                }
                else
                {
                    questMark.SetActive(false);
                    questMark2.SetActive(false);
                }
            }
        }
    }

    public void markBillborad()
    {
        questMark.transform.LookAt(target);
        questMark2.transform.LookAt(target);
    }

    public void DialogBtnActivate()
    {
        //float distance = Vector3.Distance(JY_AvatarLoad.s_instance.origin.transform.position,transform.position);
        float distance = Vector3.Distance(target2.position,transform.position);

        if (distance < 2f)
        {
            JY_QuestManger.s_instance.dialogButton.SetActive(true);
        }
        else
        {
            JY_QuestManger.s_instance.dialogButton.SetActive(false);
        }
    }
}
