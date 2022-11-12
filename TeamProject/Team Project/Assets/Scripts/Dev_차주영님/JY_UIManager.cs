using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class JY_UIManager : MonoBehaviour
{
    public static JY_UIManager instance;
    public Transform questMenuGroup;
    public GameObject alarmUI;
    public Image WhiteFade;

    public Text alarmText;
    public Text nameText;
    public Text levelText;
    public Text healthText;
    public Text steminaText;
    public Text strengthText;
    public Text dexterityText;
    public Text SPText;

    bool profileSwitch;
    //bool statusSwitch;
    //bool profileMenuSwitch;
    bool questMenuSwitch;
    private void Awake()
    { 
        instance ??= this;
        profileSwitch = false;
        //profileMenuSwitch = false;
        questMenuSwitch = false;
        nameText.text = JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].name;        
    }

    private void OnEnable()
    {
        instance ??= this;
    }
    private void OnDisable()
    {
        instance = null;
    }

    //프로필 On/Off 함수
    public void switchProfile()
    {
        if(profileSwitch == false)
        {
            //profileGroup.gameObject.SetActive(true);
            profileSwitch = true;
        }
        else
        {
            //profileGroup.gameObject.SetActive(false);
            //profileMenuGroup.gameObject.SetActive(false);
            //StatusMenuGroup.gameObject.SetActive(false);
            profileSwitch = false;
            //statusSwitch = false;
            //profileMenuSwitch = false;
        }
    }

    //퀘스트 메뉴 On/Off함수
    public void switchQuestMenu()
    {
        if (questMenuSwitch == false)
        {
            questMenuGroup.gameObject.SetActive(true);
            questMenuSwitch = true;
        }
        else
        {
            questMenuGroup.gameObject.SetActive(false);
            questMenuSwitch = false;
        }
    }

    public void levelupUI()
    {
        alarmUI.SetActive(true);
        WhiteFadeIn();
        alarmText.text = "레벨업! Lv." + Player.instance.playerStat.level;        
        InstanceManager.s_instance.PlayPlayerEffect("LevelUpEffect");
        Invoke("closeAlarm", 2f);
        StatusDataRenew();
    }
    public void questAcceptUI()
    {
        alarmUI.SetActive(true);
        alarmText.text = "퀘스트를 수령했습니다.";
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Quest, false, 1f);
        Invoke("closeAlarm", 2f);
    }
    public void questFinishUI()
    {
        alarmUI.SetActive(true);
        alarmText.text = "퀘스트를 완료했습니다.";
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Quest, false, 1f);
        Invoke("closeAlarm", 2f);
    }

    void closeAlarm()
    {
        alarmUI.SetActive(false);
    }
    // 게임 나가기
    public void ExitApplication()
    {
        Application.Quit();
    }
    // Lobby씬 (캐릭터 선택창)으로 이동. 던전 씬에서 로비로 되돌아갈 때는 먼저 LeaveRoom을 호출한 다음 Disconnect를 해야 함.
    // 코루틴을 사용하여 로비 씬으로 로딩이 끝난 다음 LeaveRoom, Disconnect를 하도록 하여 해결함.
    public void loadLobbyScene()
    {
        StartCoroutine(Disconnect());            
    }
    IEnumerator LoadLobby()
    {
        yield return null;
        GameManager.s_instance.LoadScene((int)GameManager.SceneName.Lobby);        
    }
    IEnumerator Disconnect()
    {
        yield return LoadLobby();
        if (NetworkManager.s_instance.currentRoom != null) NetworkManager.s_instance.LeaveRoom();
        NetworkManager.s_instance.Disconnect();
    }

    public void StatusDataRenew()
    {    
        levelText.text = "Lv." + Player.instance.playerStat.level.ToString();
        healthText.text = "체력: " + Player.instance.playerStat.health.ToString();
        steminaText.text = "지구력: " + Player.instance.playerStat.stamina.ToString();
        strengthText.text = "근력: " + Player.instance.playerStat.strength.ToString();
        dexterityText.text = "민첩: " + Player.instance.playerStat.dexterity.ToString();
        SPText.text = "스탯 포인트:"+ Player.instance.playerStat.statPoint.ToString(); 
    }

    void WhiteFadeIn()
    {
        WhiteFade.gameObject.SetActive(true);

        float currentTime = 0f;
        float percent = 0f;
        while (percent < 1f)
        {
            Color color = WhiteFade.color;
            color.a = 1f;
            currentTime += Time.deltaTime;
            percent = currentTime / 3f;
            color.a = Mathf.Lerp(1f, 0f, percent);
            WhiteFade.color = color;
        }
        WhiteFade.gameObject.SetActive(false);
    }
    public void statusControl(int StatType)
    {
        switch (StatType)
        {
            case 0:
                Player.instance.StatUp(Adjustable.health);
                break;
            case 1:
                Player.instance.StatUp(Adjustable.stamina);
                break;
            case 2:
                Player.instance.StatUp(Adjustable.strength);
                break;
            case 3:
                Player.instance.StatUp(Adjustable.dexterity);
                break;
        }

    }

    public void statusControl_minus(int StatType)
    {
        switch (StatType)
        {
            case 0:
                Player.instance.StatUp(Adjustable.health);
                break;
            case 1:
                Player.instance.StatUp(Adjustable.stamina);
                break;
            case 2:
                Player.instance.StatUp(Adjustable.strength);
                break;
            case 3:
                Player.instance.StatUp(Adjustable.dexterity);
                break;
        }

    }


    public void InitializeStatus()
    {
        Player.instance.InitializeStat();
    }
}
