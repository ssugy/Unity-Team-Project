using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JY_QuestManger : MonoBehaviour
{
    //����Ʈ �Ŵ����̱���
    public static JY_QuestManger instance;
    public static JY_QuestManger s_instance { get { return instance; } }
    
    public List<string[]> QuestDataList;
    public GameObject dialogButton;
    string path;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        path = Application.dataPath + "/XML_JSON/" + "quest_data.csv";
        QuestDataList = new List<string[]>();
        QuestDataLoad();
    }

    //����Ʈ ���������� �ε�
    //�ȵ���̵� ����� ��� �����ؾ���, ���ļ���
    void QuestDataLoad()
    {
        using (StreamReader sr = new StreamReader(path))
        {
            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                string[] datas = line.Split(',');
                QuestDataList.Add(datas);
            }
            sr.Close();
        }
    }

    public void QuestProgress(int QuestNum)
    {
        int now = int.Parse(QuestDataList[QuestNum][3]);
        now++;
        QuestDataList[QuestNum][3] = now.ToString();
    }

    public void QuestChecker(int QuestNum) {
        if (QuestDataList[QuestNum][6] == "TRUE")
            return;
        //����Ʈ ����
        if (QuestDataList[QuestNum][5] == "FALSE")
        {
            QuestDataList[QuestNum][5] = "TRUE";
            Debug.Log("����Ʈ ����");
        }
        //����Ʈ �Ϸ�
        else if (QuestDataList[QuestNum][5]=="TRUE" && int.Parse(QuestDataList[QuestNum][3]) >= int.Parse(QuestDataList[QuestNum][4]))
        {
            QuestDataList[QuestNum][6] = "TRUE";
            Debug.Log("����Ʈ �Ϸ�");
        }
    }

}
