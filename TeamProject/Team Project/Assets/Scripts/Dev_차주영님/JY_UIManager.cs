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
    public Text staminaText;
    public Text strengthText;
    public Text dexterityText;
    public Text SPText;

    public Image portrait;

    private Player player;
    bool questMenuSwitch;

    private void Awake()
    { 
        instance ??= this;        
        questMenuSwitch = false;             
    }

    private IEnumerator Start()
    {
        player = JY_CharacterListManager.s_instance.playerList[0];
        // ���õ� ĳ������ �̸��� �ҷ���.
        nameText.text = player.name;
        yield return null;
        switch(player.playerStat.gender, player.playerStat.job)
        {
            case (EGender.FEMALE, EJob.WARRIOR):
                {
                    portrait.sprite = Resources.Load<Sprite>("UI_Change/Portrait/f_warrior");
                    break;
                }
            case (EGender.FEMALE, EJob.MAGICIAN):
                {
                    portrait.sprite = Resources.Load<Sprite>("UI_Change/Portrait/f_magician");
                    break;
                }             
            case (EGender.MALE, EJob.WARRIOR):
                {
                    portrait.sprite = Resources.Load<Sprite>("UI_Change/Portrait/m_warrior");
                    break;                    
                }
            case (EGender.MALE, EJob.MAGICIAN):
                {
                    portrait.sprite = Resources.Load<Sprite>("UI_Change/Portrait/m_magician");
                    break;
                }
            default:
                {
                    Debug.Log("����");
                    break;
                }
        }
    }

    private void OnEnable()
    {
        instance ??= this;
    }
    private void OnDisable()
    {
        instance = null;
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
        levelText.text = $"Lv.{player.playerStat.level}";
        healthText.text = $"ü��: {player.playerStat.health} " +
            $"+ ({player.playerStat.AddedHealth})";
        staminaText.text = $"������: {player.playerStat.stamina} " +
            $"+ ({player.playerStat.AddedStamina})";
        strengthText.text = $"�ٷ�: {player.playerStat.strength} " +
            $"+ ({player.playerStat.AddedStrength})";
        dexterityText.text = $"��ø: {player.playerStat.dexterity} " +
            $"+ ({player.playerStat.AddedDexterity})";
        SPText.text = $"���� ����Ʈ: {player.playerStat.statPoint}"; 
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
        player.StatUp((Adjustable)StatType);       
    }  
    
    public void InitializeStatus()
    {
        player.InitializeStat();
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
