using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public GameObject noSelect;
    public Button enterWorld;
    public Button quitGame;

    void Start()
    {
        enterWorld.onClick.AddListener(() => EnterWorld());
        quitGame.onClick.AddListener(() => Application.Quit());
    }

    public void EnterWorld()
    {        
        if (JY_CharacterListManager.s_instance.selectNum < 0) noSelect.SetActive(true);
        else GameManager.s_instance.LoadScene(4);
    }               
}
