using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JY_NpcQuest : MonoBehaviour
{
    public int npcNum;
    public GameObject questMark;
    public GameObject questMark_Gray;
    public GameObject questMark_Complete;    
    public Sprite NpcPortrait;


    int selectNum;
    // Start is called before the first frame update
    void Start()
    {
        if(JY_CharacterListManager.s_instance != null)
            selectNum = JY_CharacterListManager.s_instance.selectNum;
    }

    // Update is called once per frame
    void Update()
    {
        if(JY_CharacterListManager.s_instance != null)
        {
            QuestNpcChecker(npcNum);
        }
    }
    public void QuestNpcChecker(int npcNum)
    {
        //�ش� npc���� player�� ���� ���� ����Ʈ�� ������� questmark Ȱ��ȭ
        switch (npcNum)
        {
            case 0:
                //����Ʈ �̼��� ������ ���
                if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress[2] == 0)
                    questMark.SetActive(true);
                //����Ʈ ���� �����̸� ����Ʈ �Ϸ� ���� ������ ���
                else if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress[2] == 1 &&
                        JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress[1] >= int.Parse(JY_QuestManager.s_instance.QuestData[0][4]) &&
                        JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress[3] == 0)
                {
                    questMark.SetActive(false);
                    questMark_Gray.SetActive(false);
                    questMark_Complete.SetActive(true);
                }
                //����Ʈ ���ɻ����̸� ����Ʈ �Ϸ� �Ұ� ������ ���
                else if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress[2] == 1 &&
                        JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress[3] == 0)
                {
                    questMark.SetActive(false);
                    questMark_Gray.SetActive(true);
                    questMark_Complete.SetActive(false);
                }
                //����Ʈ�� �Ϸ��� ������ ���
                else
                {
                    questMark.SetActive(false);
                    questMark_Gray.SetActive(false);
                    questMark_Complete.SetActive(false);
                }
                break;
            case 1:
                //����Ʈ �̼��� ������ ���
                if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress[3] == 1 && JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress2[2] == 0)
                    questMark.SetActive(true);
                //����Ʈ ���� �����̸� ����Ʈ �Ϸ� ���� ������ ���
                else if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress2[2] == 1 &&
                        JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress2[1] >= int.Parse(JY_QuestManager.s_instance.QuestData[1][4]) &&
                        JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress2[3] == 0)
                {
                    questMark.SetActive(false);
                    questMark_Gray.SetActive(false);
                    questMark_Complete.SetActive(true);
                }
                //����Ʈ ���ɻ����̸� ����Ʈ �Ϸ� �Ұ� ������ ���
                else if (JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress2[2] == 1 &&
                        JY_CharacterListManager.s_instance.jInfoData.infoDataList[selectNum].questProgress2[3] == 0)
                {
                    questMark.SetActive(false);
                    questMark_Gray.SetActive(true);
                    questMark_Complete.SetActive(false);
                }
                //����Ʈ�� �Ϸ��� ������ ���
                else
                {
                    questMark.SetActive(false);
                    questMark_Gray.SetActive(false);
                    questMark_Complete.SetActive(false);
                }
                break;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        JY_QuestManager.s_instance.selectNpcNum = npcNum;
        JY_QuestManager.s_instance.NPCPortrait = NpcPortrait;
        JY_QuestManager.s_instance.dialogButton.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        JY_QuestManager.s_instance.selectNpcNum = -1;
        JY_QuestManager.s_instance.dialogButton.SetActive(false);
    }
}
