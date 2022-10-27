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
    public GameObject LevelUPEffect;

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
    GameObject effect;
    List<GameObject> effectList;
    private void Awake()
    {
        profileSwitch = false;
        profileMenuSwitch = false;
        questMenuSwitch = false;
        nameText.text = JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].name;        
        effectList = new List<GameObject>();
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
        alarmText.text = "������! Lv."+JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].level.ToString();
        effect = Instantiate<GameObject>(LevelUPEffect, Player.instance.transform);
        effect.transform.localPosition = Vector3.forward;
        effectList.Add(effect);
        StatusDataRenew();
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Key);
        Invoke("stopLevelupEffect", 2.5f);
        Invoke("closeAlarm", 2f);
    }
    public void questAcceptUI()
    {
        alarmUI.SetActive(true);
        alarmText.text = "����Ʈ�� �����߽��ϴ�.";
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Key);
        Invoke("closeAlarm", 2f);
    }
    public void questFinishUI()
    {
        alarmUI.SetActive(true);
        alarmText.text = "����Ʈ�� �Ϸ��߽��ϴ�.";
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Key);
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
        GameManager.s_instance.LoadScene(2);
    }

    public void StatusDataRenew()
    {    
        levelText.text = "Lv." + Player.instance.playerStat.level.ToString();
        healthText.text = "ü��:" + Player.instance.playerStat.health.ToString();
        steminaText.text = "������:" + Player.instance.playerStat.stamina.ToString();
        strengthText.text = "��:" + Player.instance.playerStat.strength.ToString();
        dexterityText.text = "��ø:" + Player.instance.playerStat.dexterity.ToString();
        SPText.text = "���� ����Ʈ:"+ Player.instance.playerStat.statPoint.ToString(); 
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

    void stopLevelupEffect()
    {
        for(int i=0; i<effectList.Count; i++)
        {
            Destroy(effectList[i]);
        }
    }
}
