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
    // 생성자의 _num은 아무 의미없는 매개변수입니다.
    // 본래 구조체의 생성자는 클래스와 달리 반드시 매개변수가 필요합니다. 그렇기에 본 구조체 역시 매개변수를 포함하였지만 기본값을 설정하여 실제로는 매개변수를 입력하지 않아도 생성자를 사용할 수 있습니다.
    // 하지만 그럼에도 실제 struct 변수를 초기화할 때 어떤 매개변수도 입력하지 않으면 씬 '실행' 중에 에러가 발생합니다. 컴파일 과정에선 에러가 발생하지 않습니다.
    public InfoData(int _num, bool _isNull = true, string _name = null, int _level = 0, int _exp = 0, int _gold = 0, string _job = null, string _gender = null, int _statusPoint = 0)
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
    /// 0 : 얼굴
    /// 1 : 헤어
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
    private string stringJson;    

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
            InitializeSaveFile();

        // 세이브 파일을 로드.
        stringJson = File.ReadAllText(infoPath);
        jInfoData = JsonUtility.FromJson<JInfoData>(stringJson);        
    }    private void OnEnable()
    {
        // 씬이 로드될 때의 이벤트 구독.
        SceneManager.sceneLoaded += LoadAvatar;
        SceneManager.sceneLoaded += WriteSaveFile;
    }
    private void OnDisable()
    {
        // 이벤트 구독 취소.
        SceneManager.sceneLoaded -= LoadAvatar;
        SceneManager.sceneLoaded -= WriteSaveFile;
    }

    // 최초 세이브 파일 생성.
    void InitializeSaveFile()
    {
        JInfoData tmp = new();
        tmp.infoDataList = new();
        InfoData init = new(0);        

        for (int i = 0; i < 4; i++)    
            tmp.infoDataList.Add(init);           
       
        // 세이브 작성.
        string json = JsonUtility.ToJson(tmp, true);
        File.WriteAllText(infoPath, json);        
    }

    
    void LoadAvatar(Scene scene, LoadSceneMode mode)
    {        
        JY_AvatarLoad.s_instance.origin = JY_PlayerReturn.instance.getPlayerOrigin();
        if (JY_AvatarLoad.s_instance.origin != null)
        {
            JY_AvatarLoad.s_instance.charMale = JY_AvatarLoad.s_instance.findGameObjectInChild("BaseCharacterM", JY_AvatarLoad.s_instance.origin.transform).gameObject;
            JY_AvatarLoad.s_instance.charFemale = JY_AvatarLoad.s_instance.findGameObjectInChild("BaseCharacterF", JY_AvatarLoad.s_instance.origin.transform).gameObject;
            JY_AvatarLoad.s_instance.LoadModelData(selectNum);
        }
    }
    // 해당 매니저의 JInfoData를 파일에 옮겨 씀. 씬이 바뀔 때만 실행되어야 함.
    public void WriteSaveFile(Scene scene, LoadSceneMode mode)
    {        
        string json = JsonUtility.ToJson(jInfoData, true);
        File.WriteAllText(infoPath, json);
    }

    // 캐릭터 삭제 코드 단순화.
    public void DeleteCharacter(int listNum)
    {
        jInfoData.infoDataList.RemoveAt(listNum);        
        InfoData tmp = new(0);
        jInfoData.infoDataList.Add(tmp);             
    }


    
    
    

    public static void CopyInventoryData(List<Item> _source, List<Item> _destination)
    {
        _destination.Clear();
        for (int i = 0; i < _source.Count; i++)
        {
            Item copied = new Item();
            copied.type = _source[i].type;
            copied.equipedState = _source[i].equipedState;
            copied.name = _source[i].name;                      
            _destination.Add(copied);
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
            target.Add(copied);
            for (int j = 0; j < target.Count; j++)
            {
                target[j].ShallowCopy();
            }
        }
    }

}

