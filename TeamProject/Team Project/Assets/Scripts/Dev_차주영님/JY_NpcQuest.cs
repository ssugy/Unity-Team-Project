using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_NpcQuest : MonoBehaviour
{
    public int npcNum;
    public GameObject questMark;
    public GameObject questMark_Gray;
    public GameObject questMark_Complete;
    public Transform targetCam;
    public Transform Player;
    Transform playerTr;

    int selectNum;
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
        if(JY_CharacterListManager.s_instance != null)
            selectNum = JY_CharacterListManager.s_instance.selectNum;
    }

    // Update is called once per frame
    void Update()
    {
        if(JY_CharacterListManager.s_instance != null)
        {
            QuestNpcChecker(npcNum);
            DialogBtnActivate();
        }
    }
    public void QuestNpcChecker(int npcNum)
    {
        //해당 npc에게 player가 받지 않은 퀘스트가 있을경우 questmark 활성화
        if (JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[0] == npcNum)
        {
            //퀘스트 미수령 상태일 경우
            if (JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[2] == 0)
                questMark.SetActive(true);
            //퀘스트 수령 상태이며 퀘스트 완료 가능 상태일 경우
            else if (JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[2] == 1 &&
                    JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[1] >= int.Parse(JY_QuestManager.s_instance.QuestData[0][4]) &&
                    JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[3] == 0)
            {
                questMark.SetActive(false);
                questMark_Gray.SetActive(false);
                questMark_Complete.SetActive(true);
            }
            //퀘스트 수령상태이며 퀘스트 완료 불가 상태일 경우
            else if (JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[2] == 1 &&
                    JY_CharacterListManager.s_instance.characterData.infoDataList[selectNum].questProgress[3] == 0)
            {
                questMark.SetActive(false);
                questMark_Gray.SetActive(true);
                questMark_Complete.SetActive(false);
            }
            //퀘스트를 완료한 상태일 경우
            else
            {
                questMark.SetActive(false);
                questMark_Gray.SetActive(false);
                questMark_Complete.SetActive(false);
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
