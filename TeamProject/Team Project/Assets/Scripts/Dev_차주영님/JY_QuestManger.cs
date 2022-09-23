using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JY_QuestManger : MonoBehaviour
{
    //퀘스트 매니저싱글톤
    public static JY_QuestManger instance;
    public static JY_QuestManger s_instance { get { return instance; } }
    
    public List<string[]> QuestDataList;
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

    //퀘스트 데이터파일 로드
    //안드로이드 빌드시 경로 변경해야함, 추후수정
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
    public void QuestChecker(int QuestNum)
    {
        int now = int.Parse(JY_QuestManger.s_instance.QuestDataList[QuestNum][3]);
        now++;
        JY_QuestManger.s_instance.QuestDataList[QuestNum][3] = now.ToString();
    }

}
