using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public struct CharSlot
{

}

// �κ񿡼��� ���� ��ũ��Ʈ.
// CharacterListManager�� SelectNum�� �Ѱ��ִ� ����.
// ĳ���͸� ǥ���ϴ� ����.

public class LobbyUI : MonoBehaviour
{
    public GameObject noSelect;
    public Button enterWorld;
    public Button quitGame;
    public Button createChar_0;
    public Button createChar_1;
    public Button createChar_2;
    public Button createChar_3;

    void Start()
    {
        enterWorld.onClick.AddListener(() => EnterWorld());
        quitGame.onClick.AddListener(() => Application.Quit());
        createChar_0.onClick.AddListener(() => GameManager.s_instance.LoadScene(3));
        createChar_1.onClick.AddListener(() => GameManager.s_instance.LoadScene(3));
        createChar_2.onClick.AddListener(() => GameManager.s_instance.LoadScene(3));
        createChar_3.onClick.AddListener(() => GameManager.s_instance.LoadScene(3));
    }

    // ������ ĳ���Ͱ� ������ �˾�â�� ���. ������ ĳ���Ͱ� ������ �ش� ĳ���ͷ� ���忡 ����.
    public void EnterWorld()
    {        
        if (JY_CharacterListManager.s_instance.selectNum < 0) noSelect.SetActive(true);
        else GameManager.s_instance.LoadScene(4);
    }               
}
