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
    public int number;
    public bool isNull;
    public string name;
    public int level;
    public int exp;
    public string job;
    public string gender;
    public string species;
    public int[] characterAvatar;
    /// <summary>
    /// index 0 : ���ݷ�
    /// index 1 : ������
    /// index 2 : ��
    /// index 3 : ��ø
    /// </summary>
    public int[] status;
    public int statusPoint;
    /// <summary>
    /// value 0 : npc ��ȣ
    /// value 1 : ���� ���൵
    /// value 2 : ���� ����
    /// value 3 : �Ϸ� ����
    /// </summary>
    public int[] questProgress;
    public int[] questProgress2;
}
//������ ���� ������ ���� class

public class JY_CharacterListManager : MonoBehaviour
{
    public static JY_CharacterListManager instance;
    public static JY_CharacterListManager s_instance { get { return instance; } }

    //Data ���� Ŭ����(����Ʈ)
    public CharacterData characterData;
    //���� ��� �� json road�� ���̴� string ����
    string path;
    string jsonData;
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
            selectNum = -1;
        }
        else
        {
            Destroy(gameObject);
        }
        //Json���� �ε�
        //path = Application.dataPath + "/XML_JSON/" + "JY_Lobby_test.json";
        path = Application.persistentDataPath + "/JY_Lobby_test.json";
        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists)
        {
            writeInitialJson();
        }
        jsonData = File.ReadAllText(path);
        characterData = JsonUtility.FromJson<CharacterData>(jsonData);
    }

    void writeInitialJson()
    {
        CharacterData initCharData = new CharacterData();
        initCharData.infoDataList = new List<infoData>();

        for(int i = 0; i < 4; i++)
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
            init.status = initArr;
            init.statusPoint = 0;
            init.questProgress = initArr;
            init.questProgress2 = initArr;

            initCharData.infoDataList.Add(init);
        }

        for (int i = 0; i < 4; i++)
        {
            string json = JsonUtility.ToJson(initCharData, true);
            File.WriteAllText(Application.persistentDataPath + "/JY_Lobby_test.json", json);
        }
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
        for (int i = listNum; i<4;i++)
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
            }
            else
            {
                characterData.infoDataList[i].name =null;
                characterData.infoDataList[i].isNull =true;
                characterData.infoDataList[i].level =0;
                characterData.infoDataList[i].exp =0;
                characterData.infoDataList[i].job =null;
                characterData.infoDataList[i].gender =null;
                characterData.infoDataList[i].species =null;
                int[] initArr = new int[4] { 0, 0, 0, 0 };
                characterData.infoDataList[i].characterAvatar = initArr;
                characterData.infoDataList[i].status = initArr;
                characterData.infoDataList[i].statusPoint = 0;
                characterData.infoDataList[i].questProgress = initArr;
                characterData.infoDataList[i].questProgress2 = initArr;
            }
        }
        saveListData();
    }
    //ĳ���ͻ���, ���� �� ������ ���Ż��� Json���Ͽ� Save
    public void saveListData()
    {
        for(int i=0; i<4; i++)
        {
            string json = JsonUtility.ToJson(characterData,true);
            File.WriteAllText(path, json);
        }
    }
}

