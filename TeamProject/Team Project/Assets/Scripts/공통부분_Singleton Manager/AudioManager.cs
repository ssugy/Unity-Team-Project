using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditorInternal;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _unique;
    public static AudioManager s_instance { get { return _unique; } }

    private void Awake()
    {
        _unique = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitAudioSources();
        SoundPlay(SOUND_NAME.BGM, true);  // 배경음 실행
    }


    //----------------- 오디오 소스 초기화 : 오디오소스를 리스트 형식으로 만든다.
    /**
     * 오디오 플레이는 하단의 순서를 따라간다.
     * 1. 빈객체 생성
     * 2. 해당 객체에 오디오 소스 컴포넌트 추가
     * 3. 오디오 소스 넣기
     * 4. 사운드가 끝나면 destroy하기.
     */
    Dictionary<SOUND_NAME, AudioClip> sources;
    public enum SOUND_NAME
    {
        BGM,
        CLICK,
    }
    public SOUND_NAME Name;
    public AudioClip[] clips;   // enum의 순서를 따라가야됨

    public void InitAudioSources()
    {
        sources = new Dictionary<SOUND_NAME, AudioClip>();
        for (int i = 0; i < clips.Length; i++)
        {
            sources.Add((SOUND_NAME)i, clips[i]);
        }
    }

    public void SoundPlay(SOUND_NAME name, bool isLoop = false, float volume = 0.5f)
    {
        GameObject go = new GameObject(name.ToString());
        go.transform.SetParent(transform);
        AudioSource goAudio = go.AddComponent<AudioSource>();
        goAudio.clip = sources[name];
        goAudio.loop = isLoop;
        goAudio.volume = volume;
        goAudio.Play();

        if (!isLoop)
        {
            StartCoroutine(EndAudio(goAudio));
        }
    }

    //사운드플레이 콜백 - Unity Docs기준으로는 isPlaying변수를 확인하는 것으로 콜백처리가 나와있습니다.
    IEnumerator EndAudio(AudioSource audioSource)
    {
        while (true)
        {
            if (!audioSource.isPlaying)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(audioSource.gameObject);
    }
}
