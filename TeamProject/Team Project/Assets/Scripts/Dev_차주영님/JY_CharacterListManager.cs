using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

//����ϴ� JsonData
[System.Serializable]
public class JInfoData
{
    public List<InfoData> infoDataList;
}
[System.Serializable]
public struct InfoData
{
    // �������� _num�� �ƹ� �ǹ̾��� �Ű������Դϴ�.
    // ���� ����ü�� �����ڴ� Ŭ������ �޸� �ݵ�� �Ű������� �ʿ��մϴ�. �׷��⿡ �� ����ü ���� �Ű������� �����Ͽ����� �⺻���� �����Ͽ� �����δ� �Ű������� �Է����� �ʾƵ� �����ڸ� ����� �� �ֽ��ϴ�.
    // ������ �׷����� ���� struct ������ �ʱ�ȭ�� �� � �Ű������� �Է����� ������ �� '����' �߿� ������ �߻��մϴ�. ������ �������� ������ �߻����� �ʽ��ϴ�.
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
    /// 0 : ��
    /// 1 : ���
    /// 2 : ����
    /// 3 : ����
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
    public Sprite selectPortrait;

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
        FileInfo file_Info = new (infoPath);
        
        if (!file_Info.Exists ) 
            InitializeSaveFile();

        // ���̺� ������ �ε�.
        stringJson = File.ReadAllText(infoPath);
        jInfoData = JsonUtility.FromJson<JInfoData>(stringJson);        
    }    private void OnEnable()
    {
        // ���� �ε�� ���� �̺�Ʈ ����.
        SceneManager.sceneLoaded += LoadAvatar;
        SceneManager.sceneLoaded += WriteSaveFile;
    }
    private void OnDisable()
    {
        // �̺�Ʈ ���� ���.
        SceneManager.sceneLoaded -= LoadAvatar;
        SceneManager.sceneLoaded -= WriteSaveFile;
    }

    // ���� ���̺� ���� ����.
    void InitializeSaveFile()
    {
        JInfoData tmp = new();
        tmp.infoDataList = new();
        InfoData init = new(0);        

        for (int i = 0; i < 4; i++)    
            tmp.infoDataList.Add(init);           
       
        // ���̺� �ۼ�.
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
    // �ش� �Ŵ����� JInfoData�� ���Ͽ� �Ű� ��. ���� �ٲ� ���� ����Ǿ�� ��.
    public void WriteSaveFile(Scene scene, LoadSceneMode mode)
    {        
        string json = JsonUtility.ToJson(jInfoData, true);
        File.WriteAllText(infoPath, json);
    }

    // ĳ���� ���� �ڵ� �ܼ�ȭ.
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
            Debug.Log("����Ʈ �ʱ�ȭ");
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

