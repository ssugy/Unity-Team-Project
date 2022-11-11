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
        questData_1.Add(2, "������ ���");
        questData_1.Add(3, "���̷���");
        questData_1.Add(4, "5");
        questData_1.Add(5, "�������� ���̷������ ���� ġ�� �ֽ��ϴ�. ���� ������ ���� ���ؼ��� ����� �ñ��մϴ�. ");
        questData_1.Add(6, "���̷����� 5���� óġ���ֽ� �� �����ʴϱ�?");
        questData_1.Add(7, "���� : Exp 15, 500 ���");
        questData_1.Add(8, "�����ּż� �����մϴ�!");
        questData_1.Add(9, "����� ��ġ�� �� ���� ��Ź�帳�ϴ�.");
        questData_1.Add(10, "���� ������ ������ ���縮�� �ֽ��ϴ�.");
        managerData.Add(0, questData_1);

        Dictionary<int, string> questData_2 = new Dictionary<int, string>();
        questData_2.Add(0, "�� �Ӹ��� ���� óġ");
        questData_2.Add(1, "ȭ�� ���� ���� ���ʿ� �ִ� ������ óġ�Ͻÿ�.");
        questData_2.Add(2, "������ ������");
        questData_2.Add(3, "�� �Ӹ� ����");
        questData_2.Add(4, "1");
        questData_2.Add(5, "ȭ�� ������ ���� ���� ������ �� �Ӹ��� ���� ������ ������ �ִٳ�. �� ������ ������ �����ϴٳ�.");
        questData_2.Add(6, "���̷����� óġ�� ���� ���� �ڳ״� ���� ������ �� �� ��������, �� ������ óġ�� �ְڴ°�?");
        questData_2.Add(7, "���� : Exp 30, 1000 ���");
        questData_2.Add(8, "������ ��ġ��ٴ�! �ڳ� ���� ����ϱ�.");
        questData_2.Add(9, "�� ������ óġ�ϱ�� ���� ����� ���� �ƴ���.");
        questData_2.Add(10,"��� �Ƿ��� ������ ���谡�� ������...");
        managerData.Add(1, questData_2);
    }
}
