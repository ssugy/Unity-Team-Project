using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

//����ϴ� JsonData
[System.Serializable]
public class JInfoData
{
    public JInfoData() => infoDataList = new();    

    public List<InfoData> infoDataList;
}
[System.Serializable]
public struct InfoData
{
    // �������� _num�� �ƹ� �ǹ̾��� �Ű������Դϴ�.
    // ���� ����ü�� �����ڴ� Ŭ������ �޸� �ݵ�� �Ű������� �ʿ��մϴ�. �׷��⿡ �� ����ü ���� �Ű������� �����Ͽ����� �⺻���� �����Ͽ� �����δ� �Ű������� �Է����� �ʾƵ� �����ڸ� ����� �� �ֽ��ϴ�.
    // ������ �׷����� ���� struct ������ �ʱ�ȭ�� �� � �Ű������� �Է����� ������ �� '����' �߿� ������ �߻��մϴ�. ������ �������� ������ �߻����� �ʽ��ϴ�.
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
    /// 0 : ����
    /// 1 : ����
    /// 2 : ��
    /// 3 : ���
    /// </summary>
    public int[] characterAvatar;
    /// <summary>
    /// 0 : Health
    /// 1 : Stamina
    /// 2 : Strength
    /// 3 : Dextrerity
    /// </summary>
    public int[] status;    
    // ����Ʈ ������ �ƿ� �ٲ� �ʿ䰡 ����.
    /// <summary>
    /// 0 : npc ��ȣ
    /// 1 : ���� ���൵
    /// 2 : ���� ����
    /// 3 : �Ϸ� ����
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
    #region �̱��� ����. �ܺο��� s_instance ���.
    private static JY_CharacterListManager instance;
    public static JY_CharacterListManager s_instance { get { return instance; } }
    #endregion    

    // Json ���� ���.
    private string infoPath;   

    // Json���� �ε��� ���� ������.
    public JInfoData jInfoData;    
    private string stringJson;    

    // ���õ� ĳ������ ��ȣ
    public int selectNum;    

    // Awake���� Json ������ �ε���.
    private void Awake()
    {
        #region �̱��� ����
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

        // Json ���� ��� ����.
        infoPath = Application.persistentDataPath + "/InfoData.json";

        // ���̺� ������ �������� ������ ���̺� ������ ����.
        FileInfo file_Info = new(infoPath);

        
        if (!file_Info.Exists ) 
            InitializeSaveFile();

        // ���̺� ������ �ε�.
        stringJson = File.ReadAllText(infoPath);
        jInfoData = JsonUtility.FromJson<JInfoData>(stringJson);        
    }    
    private void OnEnable()
    {
        // ���� �ε�� ���� �̺�Ʈ ����.        
        SceneManager.sceneLoaded += Save_OnSceneLoad;
    }
    private void OnDisable()
    {
        // �̺�Ʈ ���� ���.        
        SceneManager.sceneLoaded -= Save_OnSceneLoad;
    }

    // ���� ���̺� ���� ����.
    void InitializeSaveFile()
    {
        JInfoData tmp = new();        
        InfoData init = new(0);       
        for (int i = 0; i < 4; i++)    
            tmp.infoDataList.Add(init);       
        File.WriteAllText(infoPath, JsonUtility.ToJson(tmp, true));
    }        

    // �ܼ��� JinfoData�� ���Ͽ� �Ű� ��.
    public void Save() => File.WriteAllText(infoPath, JsonUtility.ToJson(jInfoData, true));

    // ���� �ٲ� �� ����Ǵ� Save.
    public void Save_OnSceneLoad(Scene scene, LoadSceneMode mode) => File.WriteAllText(infoPath, JsonUtility.ToJson(jInfoData, true));

    // ĳ���� ���� �ڵ� �ܼ�ȭ. ĳ���� ���� �Ŀ��� ���̺� ���� ����.
    public void DeleteCharacter(int listNum)
    {
        jInfoData.infoDataList.RemoveAt(listNum);               
        jInfoData.infoDataList.Add(new(0));
        Save();
    }

    // �κ��丮 �����͸� jinfo�� ī��.
    public static void CopyInventoryData(List<Item> _source, List<Item> _destination)
    {
        _destination.Clear();
        _source.ForEach(e => { _destination.Add(new(e.type, e.equipedState, e.name, e.itemCount)); });
    }

    // ���̺� ���Ϸκ��� �κ��丮�� ī����.
    public void CopyInventoryDataToScript(List<Item> target)
    {
        // target�� null�̸� new List�� ���� target�� ����. null�� �ƴϸ� Clear.
        (target ??= new()).Clear();  
        jInfoData.infoDataList[selectNum].itemList.ForEach(e =>
        {
            Item copied = new(e.type, e.equipedState, e.name, e.itemCount);
            copied.ShallowCopy();
            target.Add(copied);
        });       
    }
}