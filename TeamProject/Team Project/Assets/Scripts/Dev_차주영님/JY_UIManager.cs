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
    public GameObject partdestructionUI;
    public GameObject partdestructionUIButton;
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

    //������ On/Off �Լ�
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
        WhiteFadeIn();
        alarmText.text = "������! Lv." + JY_CharacterListManager.s_instance.playerList[0].playerStat.level;        
        InstanceManager.s_instance.ExtraEffectCreate("LevelUpEffect");
        Invoke("closeAlarm", 2f);
        StatusDataRenew();
    }
    public void questAcceptUI()
    {
        alarmUI.SetActive(true);
        alarmText.text = "����Ʈ�� �����߽��ϴ�.";
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Quest, false, 1f);
        Invoke("closeAlarm", 2f);
    }
    public void questFinishUI()
    {
        alarmUI.SetActive(true);
        alarmText.text = "����Ʈ�� �Ϸ��߽��ϴ�.";
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Quest, false, 1f);
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
    // Lobby�� (ĳ���� ����â)���� �̵�. ���� ������ �κ�� �ǵ��ư� ���� ���� LeaveRoom�� ȣ���� ���� Disconnect�� �ؾ� ��.
    // �ڷ�ƾ�� ����Ͽ� �κ� ������ �ε��� ���� ���� LeaveRoom, Disconnect�� �ϵ��� �Ͽ� �ذ���.
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
        levelText.text = "Lv." + JY_CharacterListManager.s_instance.playerList[0].playerStat.level.ToString();
        healthText.text = "ü��: " + JY_CharacterListManager.s_instance.playerList[0].playerStat.health.ToString();
        steminaText.text = "������: " + JY_CharacterListManager.s_instance.playerList[0].playerStat.stamina.ToString();
        strengthText.text = "�ٷ�: " + JY_CharacterListManager.s_instance.playerList[0].playerStat.strength.ToString();
        dexterityText.text = "��ø: " + JY_CharacterListManager.s_instance.playerList[0].playerStat.dexterity.ToString();
        SPText.text = "���� ����Ʈ:"+ JY_CharacterListManager.s_instance.playerList[0].playerStat.statPoint.ToString(); 
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
                JY_CharacterListManager.s_instance.playerList[0].StatUp(Adjustable.HEALTH);
                break;
            case 1:
                JY_CharacterListManager.s_instance.playerList[0].StatUp(Adjustable.STAMINA);
                break;
            case 2:
                JY_CharacterListManager.s_instance.playerList[0].StatUp(Adjustable.STRENGTH);
                break;
            case 3:
                JY_CharacterListManager.s_instance.playerList[0].StatUp(Adjustable.DEXTERITY);
                break;
        }

    }

    public void statusControl_minus(int StatType)
    {
        switch (StatType)
        {
            case 0:
                JY_CharacterListManager.s_instance.playerList[0].StatUp(Adjustable.HEALTH);
                break;
            case 1:
                JY_CharacterListManager.s_instance.playerList[0].StatUp(Adjustable.STAMINA);
                break;
            case 2:
                JY_CharacterListManager.s_instance.playerList[0].StatUp(Adjustable.STRENGTH);
                break;
            case 3:
                JY_CharacterListManager.s_instance.playerList[0].StatUp(Adjustable.DEXTERITY);
                break;
        }

    }


    public void InitializeStatus()
    {
        JY_CharacterListManager.s_instance.playerList[0].InitializeStat();
    }
    public void ActiveAimUI(bool state)
    {
        partdestructionUI.SetActive(state);
    }
    public void TranslateAimUI(Vector3 position)
    {
        partdestructionUI.transform.position = position;
    }
}
