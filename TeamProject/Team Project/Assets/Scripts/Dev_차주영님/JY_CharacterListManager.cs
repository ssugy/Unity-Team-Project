using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public int number;  // ���� �ѹ�.
    public bool isNull;
    public string name;
    public int level;
    public int exp;
    public string job;
    public string gender;
    public string species;
    public int[] characterAvatar;
    /// <summary>
    /// 0 : Health
    /// 1 : Stamina
    /// 2 : Strength
    /// 3 : Dextrerity
    /// </summary>
    public int[] status;
    public int statusPoint;
    /// <summary>
    /// 0 : npc ��ȣ
    /// 1 : ���� ���൵
    /// 2 : ���� ����
    /// 3 : �Ϸ� ����
    /// </summary>
    public int[] questProgress;
    public int[] questProgress2;
}
/// <summary>
/// ĳ���� �� inventory ������ ���� json
/// </summary>
[System.Serializable]
public class InventoryCharSlot
{
    public List<InventoryJSonData> InventoryJDataList;
}
[System.Serializable]
public class InventoryJSonData
{
    public List<Item> itemList;
}
public class JY_CharacterListManager : MonoBehaviour
{
    private static JY_CharacterListManager instance;
    public static JY_CharacterListManager s_instance { get { return instance; } }

    //Data ���� Ŭ����(����Ʈ)
    public CharacterData characterData;
    public InventoryCharSlot characterInventoryData;
    //���� ��� �� json load�� ���̴� string ����
    private string path;
    private string InventoryPath;
    private string jsonData;
    private string jsonData_Inventory;

    //ĳ���ͼ��ù�ȣ
    public int selectNum;
    public Sprite selectPortrait;

    private void Awake()
    {
        //SingleTone ����
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
        //Json���� �ε�
        path = Application.persistentDataPath + "/InfoData.json";
        InventoryPath = Application.persistentDataPath + "/InventoryData.json";
        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists)
        {
            WriteInitialJson();
        }
        FileInfo fileInfo_I = new FileInfo(InventoryPath);
        if (!fileInfo_I.Exists)
        {
            writeInitialInventoryJson();
        }
        jsonData = File.ReadAllText(path);
        characterData = JsonUtility.FromJson<CharacterData>(jsonData);
        jsonData_Inventory = File.ReadAllText(InventoryPath);
        characterInventoryData = JsonUtility.FromJson<InventoryCharSlot>(jsonData_Inventory);
    }

    void WriteInitialJson()
    {
        CharacterData initCharData = new CharacterData();
        initCharData.infoDataList = new List<infoData>();
        for (int i = 0; i < 4; i++)
        {
            infoData init = new infoData();
            init.number = i;
            init.isNull = true;
            init.name = null;
            init.level = 0;
            init.exp = 0;
            init.job = null;
            init.gender = null;
            init.species = null;

            int[] initArr = new int[4] { 0, 0, 0, 0 };
            init.characterAvatar = initArr;
            init.status = new int[4] { 7, 6, 10, 5 };
            init.statusPoint = 0;
            init.questProgress = initArr;
            init.questProgress2 = initArr;

            initCharData.infoDataList.Add(init);
        }
        string json = JsonUtility.ToJson(initCharData, true);
        File.WriteAllText(path, json);        
    }

    void writeInitialInventoryJson()
    {
        InventoryCharSlot initInventorySlot = new InventoryCharSlot();
        initInventorySlot.InventoryJDataList = new List<InventoryJSonData>();

        for (int i = 0; i < 4; i++)
        {
            Item initItem = new Item();
            initItem.type = ItemType.EQUIPMENT;
            initItem.equipedState = EquipState.EQUIPED;
            initItem.name = "�ռҵ�";

            InventoryJSonData init = new InventoryJSonData();
            init.itemList = new List<Item>();
            init.itemList.Add(initItem);
            initInventorySlot.InventoryJDataList.Add(init);
        }
        string json = JsonUtility.ToJson(initInventorySlot, true);
        File.WriteAllText(InventoryPath, json);        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        JY_AvatarLoad.s_instance.origin = JY_PlayerReturn.instance.getPlayerOrigin();
        if (JY_AvatarLoad.s_instance.origin != null)
        {
            JY_AvatarLoad.s_instance.charMale = JY_AvatarLoad.s_instance.findGameObjectInChild("BaseCharacterM", JY_AvatarLoad.s_instance.origin.transform).gameObject;
            JY_AvatarLoad.s_instance.charFemale = JY_AvatarLoad.s_instance.findGameObjectInChild("BaseCharacterF", JY_AvatarLoad.s_instance.origin.transform).gameObject;
            JY_AvatarLoad.s_instance.LoadModelData(selectNum);
        }
    }
    void Update()
    {
    }

    //���� �� list ����
    //�������� ������ �ʱ�ȭ
    public void deleteCharacter(int listNum)
    {
        Debug.Log("enter");
        for (int i = listNum; i < 4; i++)
        {
            if (i != 3)
            {
                characterData.infoDataList[i].name = characterData.infoDataList[i + 1].name;
                characterData.infoDataList[i].isNull = characterData.infoDataList[i + 1].isNull;
                characterData.infoDataList[i].level = characterData.infoDataList[i + 1].level;
                characterData.infoDataList[i].exp = characterData.infoDataList[i + 1].exp;
                characterData.infoDataList[i].job = characterData.infoDataList[i + 1].job;
                characterData.infoDataList[i].gender = characterData.infoDataList[i + 1].gender;
                characterData.infoDataList[i].species = characterData.infoDataList[i + 1].species;

                characterData.infoDataList[i].characterAvatar = characterData.infoDataList[i + 1].characterAvatar;
                characterData.infoDataList[i].status = characterData.infoDataList[i + 1].status;
                characterData.infoDataList[i].statusPoint = characterData.infoDataList[i + 1].statusPoint;
                characterData.infoDataList[i].questProgress = characterData.infoDataList[i + 1].questProgress;
                characterData.infoDataList[i].questProgress2 = characterData.infoDataList[i + 1].questProgress;

                characterInventoryData.InventoryJDataList[i].itemList = characterInventoryData.InventoryJDataList[i + 1].itemList;
            }
            else
            {
                characterData.infoDataList[i].name = null;
                characterData.infoDataList[i].isNull = true;
                characterData.infoDataList[i].level = 0;
                characterData.infoDataList[i].exp = 0;
                characterData.infoDataList[i].job = null;
                characterData.infoDataList[i].gender = null;
                characterData.infoDataList[i].species = null;
                int[] initArr = new int[4] { 0, 0, 0, 0 };
                characterData.infoDataList[i].characterAvatar = initArr;
                characterData.infoDataList[i].status = initArr;
                characterData.infoDataList[i].statusPoint = 0;
                characterData.infoDataList[i].questProgress = initArr;
                characterData.infoDataList[i].questProgress2 = initArr;

                characterInventoryData.InventoryJDataList[i].itemList.Clear();
                Item initItem = new Item();
                initItem.type = ItemType.EQUIPMENT;
                initItem.equipedState = EquipState.EQUIPED;
                initItem.name = "�ռҵ�";
                characterInventoryData.InventoryJDataList[i].itemList.Add(initItem);
            }
        }
        saveListData();
        saveInventoryListData();
    }
    //ĳ���ͻ���, ���� �� ������ ���Ż��� Json���Ͽ� Save
    public void saveListData()
    {
        string json = JsonUtility.ToJson(characterData, true);
        File.WriteAllText(path, json);
    }
    public void saveInventoryListData()
    {
        string json = JsonUtility.ToJson(characterInventoryData, true);
        File.WriteAllText(InventoryPath, json);
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
            Debug.Log("copy");
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
        

        for (int i = 0; i < characterInventoryData.InventoryJDataList[selectNum].itemList.Count; i++)
        {
            Item copied = new Item();            
            copied.type = characterInventoryData.InventoryJDataList[selectNum].itemList[i].type;
            copied.equipedState = characterInventoryData.InventoryJDataList[selectNum].itemList[i].equipedState;
            copied.name = characterInventoryData.InventoryJDataList[selectNum].itemList[i].name;
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

