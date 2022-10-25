using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

//사용하는 JsonData
//characterData에서 List<infoData>로 관리함
[System.Serializable]
public class JInfoData
{
    public List<infoData> infoDataList;
}
[System.Serializable]
public class infoData
{
    public int number;  // 슬롯 넘버.
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
    /// 0 : npc 번호
    /// 1 : 현재 진행도
    /// 2 : 수령 여부
    /// 3 : 완료 여부
    /// </summary>
    public int[] questProgress;
    public int[] questProgress2;
}
/// <summary>
/// 캐릭터 별 inventory 데이터 저장 json
/// </summary>
[System.Serializable]
public class JInvenData
{
    public List<InvenData> InvenDataList;
}
[System.Serializable]
public class InvenData
{
    public List<Item> itemList;
}

public class JY_CharacterListManager : MonoBehaviour
{
    #region 싱글톤 패턴
    private static JY_CharacterListManager instance;
    public static JY_CharacterListManager s_instance { get { return instance; } }
    #endregion

    //Data 관리 클래스(리스트)
    public JInfoData jInfoData;
    public JInvenData jInvenData;

    //파일 경로 및 json load에 쓰이는 string 변수
    private string infoPath;
    private string InvenPath;

    private string jsonData_Info;
    private string jsonData_Inven;

    //캐릭터선택번호
    public int selectNum;
    public Sprite selectPortrait;

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

        //Json파일 로드
        infoPath = Application.persistentDataPath + "/InfoData.json";
        InvenPath = Application.persistentDataPath + "/InvenData.json";

        // 세이브 파일이 존재하지 않으면 세이브 파일을 생성.
        FileInfo fileInfo = new FileInfo(infoPath);
        if (!fileInfo.Exists)
        {
            WriteInitialJson();
        }
        FileInfo fileInfo_I = new FileInfo(InvenPath);
        if (!fileInfo_I.Exists)
        {
            writeInitialInventoryJson();
        }

        jsonData_Info = File.ReadAllText(infoPath);
        jInfoData = JsonUtility.FromJson<JInfoData>(jsonData_Info);
        jsonData_Inven = File.ReadAllText(InvenPath);
        jInvenData = JsonUtility.FromJson<JInvenData>(jsonData_Inven);
    }

    void WriteInitialJson()
    {
        JInfoData tmp = new JInfoData();
        tmp.infoDataList = new List<infoData>();
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

            tmp.infoDataList.Add(init);
        }
        string json = JsonUtility.ToJson(tmp, true);
        File.WriteAllText(infoPath, json);        
    }

    void writeInitialInventoryJson()
    {
        JInvenData tmp = new JInvenData();
        tmp.InvenDataList = new List<InvenData>();

        for (int i = 0; i < 4; i++)
        {
            Item initItem = new Item();
            initItem.type = ItemType.EQUIPMENT;
            initItem.equipedState = EquipState.EQUIPED;
            initItem.name = "롱소드";

            InvenData init = new InvenData();
            init.itemList = new List<Item>();
            init.itemList.Add(initItem);
            tmp.InvenDataList.Add(init);
        }
        string json = JsonUtility.ToJson(tmp, true);
        File.WriteAllText(InvenPath, json);        
    }

    private void OnEnable()
    {
        // 이벤트 구독.
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
                jInfoData.infoDataList[i].name = jInfoData.infoDataList[i + 1].name;
                jInfoData.infoDataList[i].isNull = jInfoData.infoDataList[i + 1].isNull;
                jInfoData.infoDataList[i].level = jInfoData.infoDataList[i + 1].level;
                jInfoData.infoDataList[i].exp = jInfoData.infoDataList[i + 1].exp;
                jInfoData.infoDataList[i].job = jInfoData.infoDataList[i + 1].job;
                jInfoData.infoDataList[i].gender = jInfoData.infoDataList[i + 1].gender;
                jInfoData.infoDataList[i].species = jInfoData.infoDataList[i + 1].species;

                jInfoData.infoDataList[i].characterAvatar = jInfoData.infoDataList[i + 1].characterAvatar;
                jInfoData.infoDataList[i].status = jInfoData.infoDataList[i + 1].status;
                jInfoData.infoDataList[i].statusPoint = jInfoData.infoDataList[i + 1].statusPoint;
                jInfoData.infoDataList[i].questProgress = jInfoData.infoDataList[i + 1].questProgress;
                jInfoData.infoDataList[i].questProgress2 = jInfoData.infoDataList[i + 1].questProgress;

                jInvenData.InvenDataList[i].itemList = jInvenData.InvenDataList[i + 1].itemList;
            }
            else
            {
                jInfoData.infoDataList[i].name = null;
                jInfoData.infoDataList[i].isNull = true;
                jInfoData.infoDataList[i].level = 0;
                jInfoData.infoDataList[i].exp = 0;
                jInfoData.infoDataList[i].job = null;
                jInfoData.infoDataList[i].gender = null;
                jInfoData.infoDataList[i].species = null;
                int[] initArr = new int[4] { 0, 0, 0, 0 };
                jInfoData.infoDataList[i].characterAvatar = initArr;
                jInfoData.infoDataList[i].status = new int[4] { 7, 6, 10, 5 };
                jInfoData.infoDataList[i].statusPoint = 0;
                jInfoData.infoDataList[i].questProgress = initArr;
                jInfoData.infoDataList[i].questProgress2 = initArr;

                jInvenData.InvenDataList[i].itemList.Clear();
                Item initItem = new Item();
                initItem.type = ItemType.EQUIPMENT;
                initItem.equipedState = EquipState.EQUIPED;
                initItem.name = "롱소드";
                jInvenData.InvenDataList[i].itemList.Add(initItem);
            }
        }
        SaveListData();
        SaveInventoryListData();
    }
    //캐릭터생성, 삭제 시 데이터 갱신사항 Json파일에 Save
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
            Debug.Log("리스트 초기화");
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

