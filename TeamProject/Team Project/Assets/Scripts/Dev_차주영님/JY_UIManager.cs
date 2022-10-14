using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JY_UIManager : MonoBehaviour
{
    public Transform profileGroup;
    public Transform profileMenuGroup;
    public Transform StatusMenuGroup;
    public Transform questMenuGroup;
    public GameObject alarmUI;

    public Text alarmText;
    public Text nameText;
    public Text levelText;
    public Text healthText;
    public Text steminaText;
    public Text strengthText;
    public Text dexterityText;
    public Text SPText;

    bool profileSwitch;
    bool statusSwitch;
    bool profileMenuSwitch;
    bool questMenuSwitch;

    private void Awake()
    {
        profileSwitch = false;
        profileMenuSwitch = false;
        questMenuSwitch = false;

        if(JY_CharacterListManager.s_instance != null)
        {
            nameText.text = JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].name;
            StatusDataRenew();
        }
    }

    //프로필 On/Off 함수
    public void switchProfile()
    {
        if(profileSwitch == false)
        {
            profileGroup.gameObject.SetActive(true);
            profileSwitch = true;
        }
        else
        {
            profileGroup.gameObject.SetActive(false);
            profileMenuGroup.gameObject.SetActive(false);
            StatusMenuGroup.gameObject.SetActive(false);
            profileSwitch = false;
            statusSwitch = false;
            profileMenuSwitch = false;
        }
    }

    //스텟 메뉴 On/Off함수
    public void switchStatusMenu()
    {
        if (statusSwitch == false)
        {
            StatusMenuGroup.gameObject.SetActive(true);
            statusSwitch = true;
        }
        else
        {
            StatusMenuGroup.gameObject.SetActive(false);
            statusSwitch = false;
        }
    }


    //프로필 메뉴 On/Off 함수
    public void switchProfileMenu()
    {
        if (profileMenuSwitch == false)
        {
            profileMenuGroup.gameObject.SetActive(true);
            profileMenuSwitch = true;
        }
        else
        {
            profileMenuGroup.gameObject.SetActive(false);
            profileMenuSwitch = false;
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
        alarmText.text = "레벨업! Lv."+JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].level.ToString();
        StatusDataRenew();
        Invoke("closeAlarm", 2f);
    }
    public void questAcceptUI()
    {
        alarmUI.SetActive(true);
        alarmText.text = "퀘스트를 수령했습니다.";
        Invoke("closeAlarm", 2f);
    }
    public void questFinishUI()
    {
        alarmUI.SetActive(true);
        alarmText.text = "퀘스트를 완료했습니다.";
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
    // Lobby씬 (캐릭터 선택창)으로 이동
    public void loadLobbyScene()
    {
        JY_CharacterListManager.s_instance.selectNum = -1;
        GameManager.s_instance.LoadScene(2);
    }

    public void StatusDataRenew()
    {
        int level = JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].level;
        levelText.text = "Lv." + level.ToString();
        healthText.text = "체력:" + JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].status[0].ToString();
        steminaText.text = "지구력:" + JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].status[1].ToString();
        strengthText.text = "힘:" + JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].status[2].ToString();
        dexterityText.text = "민첩:" + JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].status[3].ToString();
        SPText.text = "스탯 포인트:"+JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].statusPoint.ToString();
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

    public void InitializeStatus()
    {
        Player.instance.InitializeStat();
    }
}
