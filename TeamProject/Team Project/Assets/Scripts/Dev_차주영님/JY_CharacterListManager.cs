using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

//사용하는 JsonData
[System.Serializable]
public class JInfoData
{
    public List<InfoData> infoDataList;
}

[System.Serializable]
public struct InfoData
{
    public InfoData(int _num=0, bool _isNull = true, string _name = null, int _level = 0, int _exp = 0, int _gold = 0, string _job = null, string _gender = null, int _statusPoint = 0)
    {        
        isNull = _isNull;
        name = _name;
        level = _level;
        exp = _exp;
        gold = _gold;
        job = _job;
        gender = _gender;
        statusPoint = _statusPoint;
        characterAvatar = new int[4] { 0, 0, 0, 0 };
        status = new int[4] { 0, 0, 0, 0 };
        questProgress = new int[4] { 0, 0, 0, 0 };
        questProgress2 = new int[4] { 0, 0, 0, 0 };
        itemList = new();
    }    
    public bool isNull;
    public string name;
    public int level;
    public int exp;
    public int gold;
    public string job;
    public string gender;
    public int statusPoint;

    /// <summary>
    /// 0 : 머리카락
    /// 1 : 얼굴
    /// 2 : 상의
    /// 3 : 하의
    /// </summary>
    public int[] characterAvatar;
    /// <summary>
    /// 0 : Health
    /// 1 : Stamina
    /// 2 : Strength
    /// 3 : Dextrerity
    /// </summary>
    public int[] status;    
    // 퀘스트 로직을 아예 바꿀 필요가 있음.
    /// <summary>
    /// 0 : npc 번호
    /// 1 : 현재 진행도
    /// 2 : 수령 여부
    /// 3 : 완료 여부
    /// </summary>
    public int[] questProgress;
    public int[] questProgress2;
    public List<Item> itemList;
}


public class JY_CharacterListManager : MonoBehaviour
{
    #region 싱글톤 패턴. 외부에선 s_instance 사용.
    private static JY_CharacterListManager instance;
    public static JY_CharacterListManager s_instance { get { return instance; } }
    #endregion    

    // Json 파일 경로.
    private string infoPath;   

    // Json에서 로드한 파일 데이터.
    public JInfoData jInfoData;    

    private string jsonData_Info;    

    // 선택된 캐릭터의 번호
    public int selectNum;
    public Sprite selectPortrait;

    // Awake에서 Json 파일을 로드함.
    private void Awake()
    {
        #region 싱글톤 생성
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            this.selectNum = -1;
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        // Json 파일 경로 생성.
        infoPath = Application.persistentDataPath + "/InfoData.json";        

        // 세이브 파일이 존재하지 않으면 세이브 파일을 생성.
        FileInfo file_Info = new (infoPath);
        
        if (!file_Info.Exists ) 
            InitializeSave();

        // 세이브 파일을 로드.
        jsonData_Info = File.ReadAllText(infoPath);
        jInfoData = JsonUtility.FromJson<JInfoData>(jsonData_Info);        
    }

    // 최초 세이브 파일 생성.
    void InitializeSave()
    {        
        JInfoData tmp = new ();
        tmp.infoDataList = new ();
        InfoData init = new(1);        

        for (int i = 0; i < 4; i++)    
            tmp.infoDataList.Add(init);           
       
        // 세이브 작성.
        string json_1 = JsonUtility.ToJson(tmp, true);
        File.WriteAllText(infoPath, json_1);
        
    }

    private void OnEnable()
    {
        // 씬이 로드될 때의 이벤트 구독.
        SceneManager.sceneLoaded += LoadAvatar;
    }
    private void OnDisable()
    {
        // 이벤트 구독 취소.
        SceneManager.sceneLoaded -= LoadAvatar;
    }
    void LoadAvatar(Scene scene, LoadSceneMode mode)
    {
        // 인트로 씬으로 되돌아갈 때 해당 오브젝트를 파괴함.
        if(scene.name.Equals("01. Intro"))
        {            
            instance = null;
            Destroy(gameObject);
            return;
        }
        JY_AvatarLoad.s_instance.origin = JY_PlayerReturn.instance.getPlayerOrigin();
        if (JY_AvatarLoad.s_instance.origin != null)
        {
            JY_AvatarLoad.s_instance.charMale = JY_AvatarLoad.s_instance.findGameObjectInChild("BaseCharacterM", JY_AvatarLoad.s_instance.origin.transform).gameObject;
            JY_AvatarLoad.s_instance.charFemale = JY_AvatarLoad.s_instance.findGameObjectInChild("BaseCharacterF", JY_AvatarLoad.s_instance.origin.transform).gameObject;
            JY_AvatarLoad.s_instance.LoadModelData(selectNum);
        }
    }    

    //삭제 시 list 정렬
    //마지막은 무조건 초기화
    public void DeleteCharacter(int listNum)
    {        
        for (int i = listNum; i < 4; i++)
        {
            if (i != 3)
            {
                InfoData tmp = new (1);
                tmp.name = jInfoData.infoDataList[i + 1].name;
                tmp.isNull = jInfoData.infoDataList[i + 1].isNull;
                tmp.level = jInfoData.infoDataList[i + 1].level;
                tmp.exp = jInfoData.infoDataList[i + 1].exp;
                tmp.gold = jInfoData.infoDataList[i + 1].gold;
                tmp.job = jInfoData.infoDataList[i + 1].job;
                tmp.gender = jInfoData.infoDataList[i + 1].gender;

                tmp.characterAvatar = jInfoData.infoDataList[i + 1].characterAvatar;
                tmp.status = jInfoData.infoDataList[i + 1].status;
                tmp.statusPoint = jInfoData.infoDataList[i + 1].statusPoint;
                tmp.questProgress = jInfoData.infoDataList[i + 1].questProgress;
                tmp.questProgress2 = jInfoData.infoDataList[i + 1].questProgress;
                tmp.itemList = jInfoData.infoDataList[i + 1].itemList;
                jInfoData.infoDataList[i] = tmp;
            }
            else
            {
                InfoData tmp = new(1);                
                jInfoData.infoDataList[i] = tmp;
            }
        }
        SaveListData();        
    }
    //캐릭터생성, 삭제 시 데이터 갱신사항 Json파일에 Save
    public void SaveListData()
    {
        string json = JsonUtility.ToJson(jInfoData, true);
        File.WriteAllText(infoPath, json);
    }    

    public void CopyInventoryData(List<Item> origin, List<Item> target)
    {
        target.Clear();
        for (int i = 0; i < origin.Count; i++)
        {
            Item copied = new Item();
            copied.type = origin[i].type;
            copied.equipedState = origin[i].equipedState;
            copied.name = origin[i].name;
            copied.explanation = origin[i].explanation;
            copied.image = origin[i].image;
            copied.effects = origin[i].effects;            
            target.Add(copied);
        }
    }

    public void CopyInventoryDataToScript(List<Item> target)
    {
        
        if (target == null)
        {
            Debug.Log("리스트 초기화");
            target = new List<Item>();
        }
        else
        {
            target.Clear();
        }
        

        for (int i = 0; i < jInfoData.infoDataList[selectNum].itemList.Count; i++)
        {
            Item copied = new Item();            
            copied.type = jInfoData.infoDataList[selectNum].itemList[i].type;
            copied.equipedState = jInfoData.infoDataList[selectNum].itemList[i].equipedState;
            copied.name = jInfoData.infoDataList[selectNum].itemList[i].name;
            /*
            copied.explanation = characterInventoryData.InventoryJDataList[selectNum].itemList[i].explanation;
            copied.image = characterInventoryData.InventoryJDataList[selectNum].itemList[i].image;
            copied.effects = characterInventoryData.InventoryJDataList[selectNum].itemList[i].effects;
            */
            target.Add(copied);
            for (int j = 0; j < target.Count; j++)
            {
                target[j].ShallowCopy();
            }
        }
    }

}

