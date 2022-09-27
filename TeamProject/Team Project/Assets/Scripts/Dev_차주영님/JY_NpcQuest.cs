using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_NpcQuest : MonoBehaviour
{
    public GameObject questMark;
    public GameObject questMark2;
    public Transform targetCam;
    public Transform Player;
    public GameObject dialogBtn;
    Transform playerTr;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Player.childCount ;i++)
        {
            if (Player.GetChild(i).gameObject.activeSelf == true)
            {
                playerTr = Player.GetChild(i).transform;
            }
            break;
        }
        QuestNpcChecker(gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        QuestNpcChecker(gameObject.name);
        markBillborad();
        DialogBtnActivate();
    }
    public void QuestNpcChecker(string npcName)
    {
        for (int i = 0; i < JY_QuestManger.s_instance.QuestDataList.Count; i++)
        {
            //�ش� npc���� player�� ���� ���� ����Ʈ�� ������� questmark Ȱ��ȭ
            if (JY_QuestManger.s_instance.QuestDataList[i][7] == npcName )
            {
                if(JY_QuestManger.s_instance.QuestDataList[i][5] == "FALSE")
                    questMark.SetActive(true);
                else if(JY_QuestManger.s_instance.QuestDataList[i][5] == "TRUE" &&
                        int.Parse(JY_QuestManger.s_instance.QuestDataList[i][3]) >= int.Parse(JY_QuestManger.s_instance.QuestDataList[i][4]) &&
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
        questMark.transform.LookAt(targetCam);
        questMark2.transform.LookAt(targetCam);
    }

    public void DialogBtnActivate()
    {
        //float distance = Vector3.Distance(JY_AvatarLoad.s_instance.origin.transform.position,transform.position);
        float distance = Vector3.Distance(playerTr.position,transform.position);

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
