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
        if (_unique == null)
        {
            _unique = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        //AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BGM);
    }

    private void Update()
    {
        #region �׽�Ʈ �ڵ� - ����üũ
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.CLICK_01);
        }
        #endregion
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
}
