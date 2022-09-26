using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

//사용하는 JsonData
//characterData에서 List<infoData>로 관리함
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
    public string gender;
    public string species;
    public int[] characterAvatar;
}
//데이터 변경 저장을 위한 class

public class JY_CharacterListManager : MonoBehaviour
{
    public static JY_CharacterListManager instance;
    public static JY_CharacterListManager s_instance { get { return instance; } }

    //Data 관리 클래스(리스트)
    public CharacterData characterData;
    //파일 경로 및 json road에 쓰이는 string 변수
    string path;
    string jsonData;
    //캐릭터선택번호
    public int selectNum;
    public Sprite selectPortrait;

    private void Awake()
    {
        //SingleTone 생성
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            selectNum = -1;
        } 
        else
        {
            Destroy(gameObject);
        }
        //Json파일 로드
        path = Application.dataPath + "/XML_JSON/" + "JY_Lobby_test.json";
        jsonData = File.ReadAllText(path);
        characterData = JsonUtility.FromJson<CharacterData>(jsonData);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        JY_AvatarLoad.s_instance.origin = GameObject.FindWithTag("Player");
        if(JY_AvatarLoad.s_instance.origin != null)
        {
            JY_AvatarLoad.s_instance.charMale = JY_AvatarLoad.s_instance.findGameObjectInChild("BaseCharacterM", JY_AvatarLoad.s_instance.origin.transform).gameObject;
            JY_AvatarLoad.s_instance.charFemale = JY_AvatarLoad.s_instance.findGameObjectInChild("BaseCharacterF", JY_AvatarLoad.s_instance.origin.transform).gameObject;
            //JY_AvatarLoad.s_instance.LoadModelData(selectNum);
        }
    }
    void Update()
    {
    }

    //삭제 시 list 정렬
    //마지막은 무조건 초기화
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
                characterData.infoDataList[i].gender = characterData.infoDataList[i + 1].gender;
                characterData.infoDataList[i].species = characterData.infoDataList[i + 1].species;
            }
            else
            {
                characterData.infoDataList[i].name =null;
                characterData.infoDataList[i].isNull =true;
                characterData.infoDataList[i].level =0;
                characterData.infoDataList[i].job =null;
                characterData.infoDataList[i].gender =null;
                characterData.infoDataList[i].species =null;
            }
        }
        saveListData();
    }
    //캐릭터생성, 삭제 시 데이터 갱신사항 Json파일에 Save
    public void saveListData()
    {
        for(int i=0; i<4; i++)
        {
            string json = JsonUtility.ToJson(characterData,true);
            File.WriteAllText(path, json);
        }
    }
}

