using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_QuestData : MonoBehaviour
{
    public List<string[]> QuestDataList;

    /// <summary>
    /// key int : 퀘스트 번호
    /// value int 0 : 퀘스트 이름
    /// value int 1 : 퀘스트 내용
    /// value int 2 : 퀘스트 지급 NPC명
    /// value int 3 : 퀘스트 목표(타겟)명
    /// value int 4 : 목표 처치(혹은 행동) 수
    /// </summary>
    public void questDataLoad(Dictionary<int, Dictionary<int, string>> managerData)
    {

        Dictionary<int, string> questData_1 = new Dictionary<int, string>();
        questData_1.Add(0, "스켈레톤 처치");
        questData_1.Add(1, "던전에서 출몰하는 스켈레톤을 처치하시오.");
        questData_1.Add(2, "test");
        questData_1.Add(3, "스켈레톤");
        questData_1.Add(4, "10");

        managerData.Add(0, questData_1);
    }
}
