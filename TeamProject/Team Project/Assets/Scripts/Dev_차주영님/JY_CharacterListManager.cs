using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

//사용하는 JsonData
[System.Serializable]
public class JInfoData
{
    public JInfoData() => infoDataList = new();    

    public List<InfoData> infoDataList;
}
[System.Serializable]
public struct InfoData
{
    // 생성자의 _num은 아무 의미없는 매개변수입니다.
    // 본래 구조체의 생성자는 클래스와 달리 반드시 매개변수가 필요합니다. 그렇기에 본 구조체 역시 매개변수를 포함하였지만 기본값을 설정하여 실제로는 매개변수를 입력하지 않아도 생성자를 사용할 수 있습니다.
    // 하지만 그럼에도 실제 struct 변수를 초기화할 때 어떤 매개변수도 입력하지 않으면 씬 '실행' 중에 에러가 발생합니다. 컴파일 과정에선 에러가 발생하지 않습니다.
    public InfoData(int _num, bool _isNull = true, string _name = null, int _level = 0, int _exp = 0, int _gold = 0, EJob _job = EJob.NONE, EGender _gender = EGender.NONE, int _statusPoint = 0)
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
    public EJob job;
    public EGender gender;
    public int statusPoint;

    /// <summary>
    /// 0 : 하의
    /// 1 : 상의
    /// 2 : 얼굴
    /// 3 : 헤어
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
public enum EJob
{
    NONE,
    WARRIOR,
    MAGICIAN
}
public enum EGender 
{ 
    NONE,
    MALE, 
    FEMALE    
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
        FileInfo file_Info = new(infoPath);

        
        if (!file_Info.Exists ) 
            InitializeSaveFile();

        // 세이브 파일을 로드.
        stringJson = File.ReadAllText(infoPath);
        jInfoData = JsonUtility.FromJson<JInfoData>(stringJson);        
    }    
    private void OnEnable()
    {
        // 씬이 로드될 때의 이벤트 구독.        
        SceneManager.sceneLoaded += Save_OnSceneLoad;
    }
    private void OnDisable()
    {
        // 이벤트 구독 취소.        
        SceneManager.sceneLoaded -= Save_OnSceneLoad;
    }

    // 최초 세이브 파일 생성.
    void InitializeSaveFile()
    {
        JInfoData tmp = new();        
        InfoData init = new(0);       
        for (int i = 0; i < 4; i++)    
            tmp.infoDataList.Add(init);       
        File.WriteAllText(infoPath, JsonUtility.ToJson(tmp, true));
    }        

    // 단순히 JinfoData를 파일에 옮겨 씀.
    public void Save() => File.WriteAllText(infoPath, JsonUtility.ToJson(jInfoData, true));

    // 씬이 바뀔 때 실행되는 Save.
    public void Save_OnSceneLoad(Scene scene, LoadSceneMode mode) => File.WriteAllText(infoPath, JsonUtility.ToJson(jInfoData, true));

    // 캐릭터 삭제 코드 단순화. 캐릭터 삭제 후에는 세이브 파일 저장.
    public void DeleteCharacter(int listNum)
    {
        jInfoData.infoDataList.RemoveAt(listNum);               
        jInfoData.infoDataList.Add(new(0));
        Save();
    }

    // 인벤토리 데이터를 jinfo로 카피.
    public static void CopyInventoryData(List<Item> _source, List<Item> _destination)
    {
        _destination.Clear();
        _source.ForEach(e => { _destination.Add(new(e.type, e.equipedState, e.name, e.itemCount)); });
    }

    // 세이브 파일로부터 인벤토리를 카피함.
    public void CopyInventoryDataToScript(List<Item> target)
    {
        // target이 null이면 new List를 만들어서 target에 대입. null이 아니면 Clear.
        (target ??= new()).Clear();  
        jInfoData.infoDataList[selectNum].itemList.ForEach(e =>
        {
            Item copied = new(e.type, e.equipedState, e.name, e.itemCount);
            copied.ShallowCopy();
            target.Add(copied);
        });       
    }
}