using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

//����ϴ� JsonData
//characterData���� List<infoData>�� ������
[System.Serializable]
public class JInfoData
{
    public List<InfoData> infoDataList;
}

[System.Serializable]
public class JInvenData
{
    public List<InvenData> InvenDataList;
}

// �޼ҵ带 ����ϰų� ����� ���� �ƴ϶�� ����ü�� �ٲٴ� ���� ���� ��.
[System.Serializable]
public struct InfoData
{
    public InfoData(int _num = 0, bool _isNull = true, string _name = null, int _level = 0, int _exp = 0, int _gold = 0, string _job = null, string _gender = null, int _statusPoint = 0)
    {
        num = _num;
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
    }
    public int num;  // ���� �ѹ�.
    public bool isNull;
    public string name;
    public int level;
    public int exp;
    public int gold;
    public string job;
    public string gender;
    public int statusPoint;

    /// <summary>
    /// 0 : �Ӹ�ī��
    /// 1 : ��
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
}
// �޼ҵ带 ����ϰų� ����� ���� �ƴ϶�� ����ü�� �ٲٴ� ���� ���� ��.
[System.Serializable]
public struct InvenData
{
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
    private string InvenPath;

    // Json���� �ε��� ���� ������.
    public JInfoData jInfoData;
    public JInvenData jInvenData;

    private string jsonData_Info;
    private string jsonData_Inven;

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

        //Json���� �ε�
        infoPath = Application.persistentDataPath + "/InfoData.json";
        InvenPath = Application.persistentDataPath + "/InvenData.json";

        // Info, Inven ���̺� ���� �� �ϳ��� �������� ������ ���̺� ������ ����.
        FileInfo file_Info = new FileInfo(infoPath);
        FileInfo file_Inven = new FileInfo(InvenPath);
        if (!file_Info.Exists || !file_Inven.Exists)
        {
            InitializeSave();            
        }                

        jsonData_Info = File.ReadAllText(infoPath);
        jInfoData = JsonUtility.FromJson<JInfoData>(jsonData_Info);
        jsonData_Inven = File.ReadAllText(InvenPath);
        jInvenData = JsonUtility.FromJson<JInvenData>(jsonData_Inven);
    }

    void InitializeSave()
    {        
        JInfoData tmp_1 = new ();
        tmp_1.infoDataList = new List<InfoData>();        
        
        for (int i = 0; i < 4; i++)
        {
            InfoData init = new (i);            
            tmp_1.infoDataList.Add(init);            
        }
        
        JInvenData tmp_2 = new ();
        tmp_2.InvenDataList = new List<InvenData>();

        InvenData init_IData = new InvenData();
        init_IData.itemList = new List<Item>();
        Item tmp_Item = new Item();
        tmp_Item.type = ItemType.EQUIPMENT;
        tmp_Item.equipedState = EquipState.EQUIPED;
        tmp_Item.name = "�ռҵ�";
        init_IData.itemList.Add(tmp_Item);

        for (int i = 0; i < 4; i++)
        {            
            tmp_2.InvenDataList.Add(init_IData);
        }

        // Info ���̺� �ۼ�.
        string json_1 = JsonUtility.ToJson(tmp_1, true);
        File.WriteAllText(infoPath, json_1);
        // Inven ���̺� �ۼ�.
        string json_2 = JsonUtility.ToJson(tmp_2, true);
        File.WriteAllText(InvenPath, json_2);
    }

    private void OnEnable()
    {
        // �̺�Ʈ ����.
        SceneManager.sceneLoaded += LoadAvatar;
    }
    private void OnDisable()
    {
        // �̺�Ʈ ���� ���.
        SceneManager.sceneLoaded -= LoadAvatar;
    }
    void LoadAvatar(Scene scene, LoadSceneMode mode)
    {
        // ��Ʈ�� ������ �ǵ��ư� �� �ش� ������Ʈ�� �ı���.
        if(scene.name.Equals("01. Intro"))
        {            
            instance = null;
            Destroy(gameObject);
        }
        JY_AvatarLoad.s_instance.origin = JY_PlayerReturn.instance.getPlayerOrigin();
        if (JY_AvatarLoad.s_instance.origin != null)
        {
            JY_AvatarLoad.s_instance.charMale = JY_AvatarLoad.s_instance.findGameObjectInChild("BaseCharacterM", JY_AvatarLoad.s_instance.origin.transform).gameObject;
            JY_AvatarLoad.s_instance.charFemale = JY_AvatarLoad.s_instance.findGameObjectInChild("BaseCharacterF", JY_AvatarLoad.s_instance.origin.transform).gameObject;
            JY_AvatarLoad.s_instance.LoadModelData(selectNum);
        }
    }    

    //���� �� list ����
    //�������� ������ �ʱ�ȭ
    public void DeleteCharacter(int listNum)
    {        
        for (int i = listNum; i < 4; i++)
        {
            if (i != 3)
            {
                InfoData tmp_1 = new (i);
                tmp_1.name = jInfoData.infoDataList[i + 1].name;
                tmp_1.isNull = jInfoData.infoDataList[i + 1].isNull;
                tmp_1.level = jInfoData.infoDataList[i + 1].level;
                tmp_1.exp = jInfoData.infoDataList[i + 1].exp;
                tmp_1.gold = jInfoData.infoDataList[i + 1].gold;
                tmp_1.job = jInfoData.infoDataList[i + 1].job;
                tmp_1.gender = jInfoData.infoDataList[i + 1].gender;

                tmp_1.characterAvatar = jInfoData.infoDataList[i + 1].characterAvatar;
                tmp_1.status = jInfoData.infoDataList[i + 1].status;
                tmp_1.statusPoint = jInfoData.infoDataList[i + 1].statusPoint;
                tmp_1.questProgress = jInfoData.infoDataList[i + 1].questProgress;
                tmp_1.questProgress2 = jInfoData.infoDataList[i + 1].questProgress;
                jInfoData.infoDataList[i] = tmp_1;

                InvenData tmp_2;
                tmp_2.itemList = new List<Item>();
                tmp_2.itemList = jInvenData.InvenDataList[i + 1].itemList;
                jInvenData.InvenDataList[i] = tmp_2;
            }
            else
            {
                InfoData tmp_1 = new(i);
                jInfoData.infoDataList[i] = tmp_1;

                jInvenData.InvenDataList[i].itemList.Clear();
                Item initItem = new Item();
                initItem.type = ItemType.EQUIPMENT;
                initItem.equipedState = EquipState.EQUIPED;
                initItem.name = "�ռҵ�";
                jInvenData.InvenDataList[i].itemList.Add(initItem);
            }
        }
        SaveListData();
        SaveInventoryListData();
    }
    //ĳ���ͻ���, ���� �� ������ ���Ż��� Json���Ͽ� Save
    public void SaveListData()
    {
        string json = JsonUtility.ToJson(jInfoData, true);
        File.WriteAllText(infoPath, json);
    }
    public void SaveInventoryListData()
    {
        string json = JsonUtility.ToJson(jInvenData, true);
        File.WriteAllText(InvenPath, json);
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
        

        for (int i = 0; i < jInvenData.InvenDataList[selectNum].itemList.Count; i++)
        {
            Item copied = new Item();            
            copied.type = jInvenData.InvenDataList[selectNum].itemList[i].type;
            copied.equipedState = jInvenData.InvenDataList[selectNum].itemList[i].equipedState;
            copied.name = jInvenData.InvenDataList[selectNum].itemList[i].name;
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

