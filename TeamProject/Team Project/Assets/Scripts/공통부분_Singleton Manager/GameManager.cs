using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _unique;
    public static GameManager s_instance { get { return _unique; } }

    private void Awake()
    {
        _unique = this;
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        //AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BGM);
    }

    private void Update()
    {
        
    }

    //-------- 게임매니저 함수 구현
    public enum SceneName
    {
        Intro,
        Loading,
        Robby,
        MakeCharacter,
        World,
        Dungeon,
        Dungeon_Fire,
    }
    public SceneName currentScene;
    public void LoadScene(int index)
    {
        currentScene = SceneName.Loading;   // 로딩이 시작되면 Loading씬으로 변경한 뒤, 끝나면 목적씬으로 변경
        LoadingSceneController.LoadScene(index);    // 로딩씬을 이용한 로딩
    }

    #region 인트로 옵션 창
    public GameObject introOptionPannel;
    public void ShowIntroOptionPannel()
    {
        introOptionPannel.SetActive(true);
    }

    public void CloseOptionPannel()
    {
        introOptionPannel.SetActive(false);
    }
    #endregion
}
