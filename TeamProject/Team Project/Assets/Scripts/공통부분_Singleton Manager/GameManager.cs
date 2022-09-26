using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/**
 * 번역내용을, 컬럼(언어)별로 클래스로 구분하기 위한 내용.
 * 컬럼단위로 자를 예정. 이 클래스 1개가 컬럼으로 잘림.
 */
[System.Serializable]
public class LanguageTranslate
{
    public string langKey;
    public List<string> value = new List<string>(); // 여기서부터 원하는 값들 들어가있음.
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
        GetTranlate();  // 싱글톤 객체이기에, 게임시작 최초 한번에 번역 데이터를 가져옵니다.
    }

    private void Update()
    {
        #region 테스트 코드 - 사운드체크
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.CLICK_01);
        }
        #endregion
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

    #region 다국어지원
    // 탭으로 구분 할 수 있게 export format을 tsv로 설정
    const string translateURL = "https://docs.google.com/spreadsheets/d/1Ry_qSRYWiida2KRXf_h1HIhuqmBYh4C6xKT2FPH_i3o/export?format=tsv";
    public List <LanguageTranslate> languages;  // 번역된 데이터는 여기에 클래스별(언어별)로 저장되어있다.
    public int currentLanguage;    // 영어:0, 한국어:1, 일본어, 독일어, 프랑스어, 중국어(스프레드 시트와 동일한 순서)

    //여기서 이벤트 등록 처리하기.
    public Action LocalizeChanged = () => { };

    [ContextMenu("번역 가져오기")]
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

    // tab으로 구분된 문장을 받기 때문에, 해당 문장을 2차원string배열로 구분
    private void SetTranslateList(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;   // 세로의 갯수
        int columnSize = row[0].Split('\t').Length;
        string[,] sentence = new string[rowSize, columnSize];
        
        // sentence에 모두 들어감
        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');
            for (int j = 0; j < columnSize; j++)
            {
                sentence[i, j] = column[j];
            }
        }

        // sentence에 들어간 내용을 languages라는 리스트에 모두 넣기
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
