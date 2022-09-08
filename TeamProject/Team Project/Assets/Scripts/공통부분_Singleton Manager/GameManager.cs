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

    //-------- ���ӸŴ��� �Լ� ����
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
        currentScene = SceneName.Loading;   // �ε��� ���۵Ǹ� Loading������ ������ ��, ������ ���������� ����
        LoadingSceneController.LoadScene(index);    // �ε����� �̿��� �ε�
    }

    #region ��Ʈ�� �ɼ� â
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
