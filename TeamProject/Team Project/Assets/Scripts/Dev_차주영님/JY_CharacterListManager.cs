using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//����ϴ� JsonData
//characterData���� List<infoData>�� ������
[System.Serializable]
public class CharacterData
{
    public List<infoData> infoDataList;
}
[System.Serializable]
public class infoData
{
    public int number;
    public bool isNull;
    public string name;
    public int level;
    public string job;
    public string species;
}
//������ ���� ������ ���� class

public class JY_CharacterListManager : MonoBehaviour
{
    public static JY_CharacterListManager instance;
    //Data ���� Ŭ����(����Ʈ)
    public CharacterData characterData;
    //���� ��� �� json road�� ���̴� string ����
    string path;
    string jsonData;

    private void Awake()
    {
        //SingleTone ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else
        {
            Destroy(gameObject);
        }
        //Json���� �ε�
        path = Application.dataPath + "/XML_JSON/" + "JY_Lobby_test.json";
        jsonData = File.ReadAllText(path);
        characterData = JsonUtility.FromJson<CharacterData>(jsonData);
    }

    void Update()
    {
    }

    //���� �� list ����
    //�������� ������ �ʱ�ȭ
    public void deleteCharacter(int listNum)
    {
        for (int i = listNum; i<4;i++)
        {
            if (i != 3)
            {
                characterData.infoDataList[i].name = characterData.infoDataList[i + 1].name;
                characterData.infoDataList[i].isNull = characterData.infoDataList[i + 1].isNull;
                characterData.infoDataList[i].level = characterData.infoDataList[i + 1].level;
                characterData.infoDataList[i].job = characterData.infoDataList[i + 1].job;
                characterData.infoDataList[i].species = characterData.infoDataList[i + 1].species;
            }
            else
            {
                characterData.infoDataList[i].name =null;
                characterData.infoDataList[i].isNull =true;
                characterData.infoDataList[i].level =0;
                characterData.infoDataList[i].job =null;
                characterData.infoDataList[i].species =null;
            }
        }
        saveListData();
    }
    //ĳ���ͻ���, ���� �� ������ ���Ż��� Json���Ͽ� Save
    void saveListData()
    {
        for(int i=0; i<4; i++)
        {
            string json = JsonUtility.ToJson(characterData,true);
            File.WriteAllText(path, json);
        }
    }


}

