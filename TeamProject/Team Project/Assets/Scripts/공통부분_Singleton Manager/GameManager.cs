using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/**
 * ����������, �÷�(���)���� Ŭ������ �����ϱ� ���� ����.
 * �÷������� �ڸ� ����. �� Ŭ���� 1���� �÷����� �߸�.
 */
[System.Serializable]
public class LanguageTranslate
{
    public string langKey;
    public List<string> value = new List<string>(); // ���⼭���� ���ϴ� ���� ������.
}

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
        GetTranlate();  // �̱��� ��ü�̱⿡, ���ӽ��� ���� �ѹ��� ���� �����͸� �����ɴϴ�.
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

    #region �ٱ�������
    // ������ ���� �� �� �ְ� export format�� tsv�� ����
    const string translateURL = "https://docs.google.com/spreadsheets/d/1Ry_qSRYWiida2KRXf_h1HIhuqmBYh4C6xKT2FPH_i3o/export?format=tsv";
    public List <LanguageTranslate> languages;  // ������ �����ʹ� ���⿡ Ŭ������(��)�� ����Ǿ��ִ�.
    public int currentLanguage;    // ����:0, �ѱ���:1, �Ϻ���, ���Ͼ�, ��������, �߱���(�������� ��Ʈ�� ������ ����)

    //���⼭ �̺�Ʈ ��� ó���ϱ�.
    public Action LocalizeChanged = () => { };

    [ContextMenu("���� ��������")]
    void GetTranlate()
    {
        currentLanguage = 1;
        StartCoroutine(GetTranlateCoroutine());
    }

    IEnumerator GetTranlateCoroutine()
    {
        UnityWebRequest www = UnityWebRequest.Get(translateURL);
        yield return www.SendWebRequest();
        print(www.downloadHandler.text);
        SetTranslateList(www.downloadHandler.text);
    }

    // tab���� ���е� ������ �ޱ� ������, �ش� ������ 2����string�迭�� ����
    private void SetTranslateList(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;   // ������ ����
        int columnSize = row[0].Split('\t').Length;
        string[,] sentence = new string[rowSize, columnSize];
        
        // sentence�� ��� ��
        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');
            for (int j = 0; j < columnSize; j++)
            {
                sentence[i, j] = column[j];
            }
        }

        // sentence�� �� ������ languages��� ����Ʈ�� ��� �ֱ�
        languages = new List<LanguageTranslate>();
        for (int i = 0; i < columnSize; i++)
        {
            LanguageTranslate lang = new LanguageTranslate();
            lang.langKey = sentence[0, i];
            for (int j = 0; j < rowSize; j++)
            {
                lang.value.Add(sentence[j, i]);
            }
            languages.Add(lang);
        }
       LocalizeChanged();
    }
    
    #endregion
}
