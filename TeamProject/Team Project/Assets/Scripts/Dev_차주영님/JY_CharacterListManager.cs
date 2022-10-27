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

    private string jsonData_Info;    

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
            InitializeSave();

        // ���̺� ������ �ε�.
        jsonData_Info = File.ReadAllText(infoPath);
        jInfoData = JsonUtility.FromJson<JInfoData>(jsonData_Info);        
    }

    // ���� ���̺� ���� ����.
    void InitializeSave()
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

    private void OnEnable()
    {
        // ���� �ε�� ���� �̺�Ʈ ����.
        SceneManager.sceneLoaded += LoadAvatar;
    }
    private void OnDisable()
    {
        // �̺�Ʈ ���� ���.
        SceneManager.sceneLoaded -= LoadAvatar;
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

    // ĳ���� ���� �ڵ� �ܼ�ȭ.
    public void DeleteCharacter(int listNum)
    {
        jInfoData.infoDataList.RemoveAt(listNum);        
        InfoData tmp = new(0);
        jInfoData.infoDataList.Add(tmp);
        SaveListData();        
    }
    // �ش� �Ŵ����� JInfoData�� ���Ͽ� �Ű� ��.
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

