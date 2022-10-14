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

    //������ On/Off �Լ�
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

    //���� �޴� On/Off�Լ�
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


    //������ �޴� On/Off �Լ�
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
    //����Ʈ �޴� On/Off�Լ�
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
        alarmText.text = "������! Lv."+JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].level.ToString();
        StatusDataRenew();
        Invoke("closeAlarm", 2f);
    }
    public void questAcceptUI()
    {
        alarmUI.SetActive(true);
        alarmText.text = "����Ʈ�� �����߽��ϴ�.";
        Invoke("closeAlarm", 2f);
    }
    public void questFinishUI()
    {
        alarmUI.SetActive(true);
        alarmText.text = "����Ʈ�� �Ϸ��߽��ϴ�.";
        Invoke("closeAlarm", 2f);
    }

    void closeAlarm()
    {
        alarmUI.SetActive(false);
    }
    // ���� ������
    public void ExitApplication()
    {
        Application.Quit();
    }
    // Lobby�� (ĳ���� ����â)���� �̵�
    public void loadLobbyScene()
    {
        JY_CharacterListManager.s_instance.selectNum = -1;
        GameManager.s_instance.LoadScene(2);
    }

    public void StatusDataRenew()
    {
        int level = JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].level;
        levelText.text = "Lv." + level.ToString();
        healthText.text = "ü��:" + JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].status[0].ToString();
        steminaText.text = "������:" + JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].status[1].ToString();
        strengthText.text = "��:" + JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].status[2].ToString();
        dexterityText.text = "��ø:" + JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].status[3].ToString();
        SPText.text = "���� ����Ʈ:"+JY_CharacterListManager.s_instance.characterData.infoDataList[JY_CharacterListManager.s_instance.selectNum].statusPoint.ToString();
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
