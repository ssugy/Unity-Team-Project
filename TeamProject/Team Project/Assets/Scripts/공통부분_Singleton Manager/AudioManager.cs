using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _unique;
    public static AudioManager s_instance { get { return _unique; } }

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
        CLICK_01,
        CLICK_02,
        DOOR_01,
        BossBGM_01,
        BossBGM_02,
        Key,
        Portal,
        PLAYER_RUN,
        PLAYER_SWING,
        PLAYER_HIT,
        PLAYER_ATTACK,
        MONSTER_GROWL,
        BOSS_WALK,
        BOSS_SWING,
        Quest,
        BGM_WORLD,
        Get_Gold,
        BOSS_HIT,
        BOSS_KICK,
        Boss_JUMP,
        PLYAER_SHOOT,
        Boss_FireBall,
        BOSS_DEAD
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

        bgmVolumePivot = 1;
        effectVolumePivot = 1;
        beforeEffectVolumePivot = effectVolumePivot;
        isBGMMute = false;
        isEffectMute = false;
    }

    public void SoundPlay(SOUND_NAME name, bool isLoop = false, float volume = 0.5f)
    {
        GameObject go = new GameObject(name.ToString());
        AudioSource goAudio = go.AddComponent<AudioSource>();
        goAudio.clip = sources[name];
        goAudio.loop = isLoop;
        go.transform.SetParent(transform);
        if (name.Equals(SOUND_NAME.BGM)|| name.Equals(SOUND_NAME.BossBGM_01)|| name.Equals(SOUND_NAME.BossBGM_02 )|| name.Equals(SOUND_NAME.BGM_WORLD))
        {
            // 사운드 옵션때문에 배경음은 따로취급.
            goAudio.volume = volume * bgmVolumePivot;
            bgmAudioSource = goAudio;
            nowplayName = name;
            if (isBGMMute)
            {
                goAudio.volume = 0f;
            }
        }
        else
        {
            goAudio.volume = volume * effectVolumePivot;
        }
        goAudio.Play();

        if (!isLoop)
        {
            StartCoroutine(EndAudio(goAudio));
        }
    }

    
    /// <summary>
    /// 사운드 페이드 인/아웃 기능
    /// </summary>
    /// <param name="name">사운드 이름</param>
    /// <param name="destVolume">목적지 볼륨(0~1사이) (0이면 자동 종료)</param>
    /// <param name="endTime">볼륨이 모두 변경되는데 걸리는 시간(초)<param>
    public void SoundFadeInOut(SOUND_NAME name, float destVolume, float endTime)
    {
        // 현재 켜져있는 오디오 소스 중에, 내가 원하는 이름의 오디오 소스를 찾았는지 확인
        List<AudioSource> audioList = new List<AudioSource>();
        Transform[] chilTrans = GetComponentsInChildren<Transform>();
        foreach (Transform item in chilTrans)
        {
            if (item.name.Equals(name.ToString()))
            {
                audioList.Add(item.GetComponent<AudioSource>());
            }
        }

        // 오디오 추가된게 없으면 그대로 리턴
        if (audioList.Count == 0)
            return;

        for (int i = 0; i < audioList.Count; i++)
        {
            StartCoroutine(ChangeVolume(audioList[i], destVolume, endTime));
        }
    }

    public void StopAllBGM()
    {
        AudioSource[] audioList = GetComponentsInChildren<AudioSource>();
        foreach(AudioSource one in audioList)
            Destroy(one.gameObject);
    }
    IEnumerator ChangeVolume(AudioSource audio, float destVol, float finishTime)
    {
        float diffVol = destVol - audio.volume;
        while (true)
        {
            audio.volume += diffVol * Time.deltaTime / finishTime;

            if (audio.volume >= destVol - 0.05f && audio.volume <= destVol + 0.05f)
            {
                // 목적지 사운드 볼륨이 0.05이하면, 그대로 종료하기
                if (audio.volume <= 0.05f)
                {
                    audio.Stop();
                    StartCoroutine(EndAudio(audio));
                    break;
                }
                break;
            }
            yield return null;
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

    /**
     * 1. 사운드 옵션을 제작하기 위해서는, 배경음 오브젝트는 따로 저장을 해 둬야 하고,
     * 2. 효과음 사운드는, 변수 하나 추가해서 해당변수를 곱한 값으로 사운드를 변경.
     * 3. mute기능은 현재의 배경음을 배경음용 파라미터 변수와 곱하는 방향으로 해야지 끄지않고 변경이 가능하다.
     */
    private float bgmVolumePivot;
    private float effectVolumePivot;
    private float beforeEffectVolumePivot;
    private AudioSource bgmAudioSource;
    public AudioSource NOWPLAY { get { return bgmAudioSource; } }
    SOUND_NAME nowplayName;
    private bool isBGMMute;
    private bool isEffectMute;

    // 슬라이더 값은 
    public void BGMSliderValueChanged(Slider slider)
    {
        if(NOWPLAY != null)
        {
            bgmVolumePivot = slider.value;
            if (!isBGMMute)
            {
                bgmAudioSource.volume = slider.value;
            }
        }
    }

    public void BGMMute(Toggle toggle)
    {
        if (toggle.isOn)
        {
            isBGMMute = true;
            bgmAudioSource.volume = 0;
        }
        else
        {
            isBGMMute = false;
            bgmAudioSource.volume = bgmVolumePivot;
        }
    }

    public void EffectSliderValueChanged(Slider slider)
    {
        beforeEffectVolumePivot = slider.value;
        if (!isEffectMute)
        {
            effectVolumePivot = slider.value;   // 이펙터 소리는 이후에 나타날 이펙터들의 소리에 영향을 미치게 적용
        }
    }

    public void EffectMute(Toggle toggle)
    {
        if (toggle.isOn)
        {
            isEffectMute = true;
            effectVolumePivot = 0;
        }
        else
        {
            isEffectMute= false;
            effectVolumePivot = beforeEffectVolumePivot;
        }
    }
    public void CallSFXPlay(SOUND_NAME name,float delay)
    {
        StartCoroutine(SFXPlay(name, delay));
    }
    IEnumerator SFXPlay(SOUND_NAME name, float delay)
    {
        yield return new WaitForSeconds(delay);
        SoundPlay(name);
    }
    public void SceneBGMContorl(int ActiveScene, int TargetScene)
    {
        switch (ActiveScene)
        {
            case 0:             //Intro
                break;
            case 1:             //Loading
                break;
            case 2:             //Lobby
                //World씬으로 이동시 BGM 변경
                if(TargetScene == 4)
                {
                    AudioManager.s_instance.SoundFadeInOut(nowplayName, 0, 0.5f);
                    AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BGM_WORLD, true, 0.5f);
                }

                break;
            case 3:             //MakeCharacter
                break;
            case 4:             //World
                //Lobby씬으로 이동시 BGM 변경
                if(TargetScene == 2)
                {
                    AudioManager.s_instance.SoundFadeInOut(nowplayName, 0, 0.5f);
                    AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BGM, true, 0.5f);
                }
                //Dungeon씬들로 이동시 BGM 재생 종료
                else if(TargetScene == 5 || TargetScene == 6)
                    AudioManager.s_instance.SoundFadeInOut(nowplayName, 0, 0.5f);
                break;
            case 5:             //Dungeon
                if (bgmAudioSource != null)
                    AudioManager.s_instance.SoundFadeInOut(nowplayName, 0, 0.5f);

                if (TargetScene == 2)
                    AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BGM, true, 0.5f);
                else if(TargetScene == 4)
                    AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BGM_WORLD, true, 0.5f);
                break;
            case 6:             //Dungeon_Fire
                //보스전 BGM이 재생 중일 경우 전부 재생 종료 시킨 후 이동
                if(bgmAudioSource!=null)
                    AudioManager.s_instance.SoundFadeInOut(nowplayName, 0,2f);
                if(TargetScene == 2)
                    AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BGM, true, 0.5f);
                else if(TargetScene == 4)
                    AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BGM_WORLD, true, 0.5f);
                break;

        }
    }
}
