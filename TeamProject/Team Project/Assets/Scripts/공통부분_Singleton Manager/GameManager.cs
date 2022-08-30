using System.Collections;
using System.Collections.Generic;
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
    }

    public void LoadScene(int index)
    {
        LoadingSceneController.LoadScene(index);    // 로딩씬을 이용한 로딩
    }
}
