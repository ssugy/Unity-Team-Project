using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_UIManager : MonoBehaviour
{

    public Transform profileGroup;
    public Transform profileMenuGroup;
    public Transform questMenuGroup;

    bool profileSwitch;
    bool profileMenuSwitch;
    bool questMenuSwitch;

    private void Awake()
    {
        profileSwitch = false;
        profileMenuSwitch = false;
        questMenuSwitch = false;
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
            profileSwitch = false;
            profileMenuSwitch = false;
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

}
