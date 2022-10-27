using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public struct CharSlot
{

}

// 로비에서만 사용될 스크립트.
// CharacterListManager에 SelectNum을 넘겨주는 역할.
// 캐릭터를 표시하는 역할.

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

    // 선택한 캐릭터가 없으면 팝업창을 출력. 선택한 캐릭터가 있으면 해당 캐릭터로 월드에 접속.
    public void EnterWorld()
    {        
        if (JY_CharacterListManager.s_instance.selectNum < 0) noSelect.SetActive(true);
        else GameManager.s_instance.LoadScene(4);
    }               
}
