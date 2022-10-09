using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_QuestData : MonoBehaviour
{
    public List<string[]> QuestDataList;

    /// <summary>
    /// key int : ����Ʈ ��ȣ
    /// value int 0 : ����Ʈ �̸�
    /// value int 1 : ����Ʈ ����
    /// value int 2 : ����Ʈ ���� NPC��
    /// value int 3 : ����Ʈ ��ǥ(Ÿ��)��
    /// value int 4 : ��ǥ óġ(Ȥ�� �ൿ) ��
    /// </summary>
    public void questDataLoad(Dictionary<int, Dictionary<int, string>> managerData)
    {

        Dictionary<int, string> questData_1 = new Dictionary<int, string>();
        questData_1.Add(0, "���̷��� óġ");
        questData_1.Add(1, "�������� ����ϴ� ���̷����� óġ�Ͻÿ�.");
        questData_1.Add(2, "test");
        questData_1.Add(3, "���̷���");
        questData_1.Add(4, "10");

        managerData.Add(0, questData_1);
    }
}
