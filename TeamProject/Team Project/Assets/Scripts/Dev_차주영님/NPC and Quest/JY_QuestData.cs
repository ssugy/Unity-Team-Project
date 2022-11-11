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
        questData_1.Add(2, "마을의 기사");
        questData_1.Add(3, "스켈레톤");
        questData_1.Add(4, "5");
        questData_1.Add(5, "던전에서 스켈레톤들이 진을 치고 있습니다. 깊은 곳으로 가기 위해서라도 토벌이 시급합니다. ");
        questData_1.Add(6, "스켈레톤을 5마리 처치해주실 수 있으십니까?");
        questData_1.Add(7, "보상 : Exp 15, 500 골드");
        questData_1.Add(8, "도와주셔서 감사합니다!");
        questData_1.Add(9, "토벌을 마치신 후 보고 부탁드립니다.");
        questData_1.Add(10, "던전 곳곳엔 위험이 도사리고 있습니다.");
        managerData.Add(0, questData_1);

        Dictionary<int, string> questData_2 = new Dictionary<int, string>();
        questData_2.Add(0, "두 머리의 괴물 처치");
        questData_2.Add(1, "화염 던전 가장 안쪽에 있는 보스를 처치하시오.");
        questData_2.Add(2, "마을의 기사단장");
        questData_2.Add(3, "두 머리 괴물");
        questData_2.Add(4, "1");
        questData_2.Add(5, "화염 던전의 가장 깊은 곳에는 두 머리를 가진 끔찍한 괴물이 있다네. 그 괴물은 굉장히 위험하다네.");
        questData_2.Add(6, "스켈레톤을 처치한 것을 보아 자네는 깊은 곳까지 갈 수 있을테지, 그 괴물을 처치해 주겠는가?");
        questData_2.Add(7, "보상 : Exp 30, 1000 골드");
        questData_2.Add(8, "정말로 해치우다니! 자네 정말 대단하군.");
        questData_2.Add(9, "그 괴물을 처치하기는 여간 어려운 일이 아니지.");
        questData_2.Add(10,"어디 실력이 출중한 모험가는 없을까...");
        managerData.Add(1, questData_2);
    }
}
