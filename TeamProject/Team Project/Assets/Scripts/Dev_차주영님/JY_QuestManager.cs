using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class JY_QuestManager : MonoBehaviour
{
    //����Ʈ �Ŵ����̱���
    public static JY_QuestManager instance;
    public static JY_QuestManager s_instance { get { return instance; } }
    
    public List<string[]> QuestDataList;
    public GameObject dialogButton;
    public GameObject Quest_1_Bar;
    public GameObject Quest_1_Panel;

    public Text journalButton1;
    public Text journalButton2;
    public Text journalButton3;
    public Text journalButton4;
    string path;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        path = Application.dataPath + "/XML_JSON/" + "quest_data.csv";
        QuestDataList = new List<string[]>();
        QuestDataLoad();

        for(int i=0; i < QuestDataList.Count; i++)
        {
            if (QuestDataList[i][5] == "TRUE" && QuestDataList[i][6] == "FALSE")
                Quest_1_Bar.SetActive(true);
        }
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
    void QuestDataSave()
    {
        using (StreamWriter sr = new StreamWriter(path))
        {
            string line = string.Empty;
            for (int i = 0; i < QuestDataList.Count; i++)
            {
                line = string.Empty;
                line += QuestDataList[i][0];
                line += ",";
                line += QuestDataList[i][1];
                line += ",";
                line += QuestDataList[i][2];
                line += ",";
                line += QuestDataList[i][3];
                line += ",";
                line += QuestDataList[i][4];
                line += ",";
                line += QuestDataList[i][5];
                line += ",";
                line += QuestDataList[i][6];
                line += ",";
                line += QuestDataList[i][7];
                sr.WriteLine(line);
            }
            sr.Close();
        }
    }


    public void questJournalTitleRenew()
    {
        if(JY_QuestManager.s_instance.QuestDataList[1][5] == "TRUE" && JY_QuestManager.s_instance.QuestDataList[1][6] == "FALSE")
            journalButton1.text = QuestDataList[1][0];
        else
            journalButton1.text = "-";
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
            Quest_1_Bar.SetActive(true);
            QuestDataSave();
        }

        //����Ʈ �Ϸ�
        else if (QuestDataList[QuestNum][5]=="TRUE" && int.Parse(QuestDataList[QuestNum][3]) >= int.Parse(QuestDataList[QuestNum][4]))
        {
            QuestDataList[QuestNum][6] = "TRUE";
            Debug.Log("����Ʈ �Ϸ�");
            Quest_1_Bar.SetActive(false);
            Quest_1_Panel.SetActive(false);
            QuestDataSave();
        }
    }

}
