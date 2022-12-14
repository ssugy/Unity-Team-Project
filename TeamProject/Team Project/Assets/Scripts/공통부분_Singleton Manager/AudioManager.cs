using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

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
        SoundPlay(SOUND_NAME.BGM, true, 1f);  // 최초 BGM 실행
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
        BOSS_DEAD,
        PLAYER_SHIELD,
        BGM_DUNGEON_01,
        BGM_DUNGEON_02,
        BGM_LOBBY,
        PLAYER_POTION,
        Dragon_Fire,
        Buff_Get,
        PlayerSkill_1_PowerStrike
    }
    
    public AudioClip[] clips;   // enum의 순서를 따라가야됨

    public void InitAudioSources()
    {
        // 딕셔너리에 Sound_Name, AudioClip 쌍을 만듦.
        sources = new();
        for (int i = 0; i < clips.Length; i++)        
            sources.Add((SOUND_NAME)i, clips[i]);

        // PlayerPrefs를 이용하여 사운드 옵션 설정을 저장하고 불러옴.
        if (PlayerPrefs.HasKey("Volume_BGM")) bgmVolumePivot = PlayerPrefs.GetFloat("Volume_BGM");
        else PlayerPrefs.SetFloat("Volume_BGM", bgmVolumePivot = 0.5f);
        if (PlayerPrefs.HasKey("Volume_SFX")) effectVolumePivot = PlayerPrefs.GetFloat("Volume_SFX");
        else PlayerPrefs.SetFloat("Volume_SFX", effectVolumePivot = 0.5f);
        if (PlayerPrefs.HasKey("Mute_BGM"))
        {
            if (PlayerPrefs.GetInt("Mute_BGM") == 0)
                isBGMMute = false;
            else
                isBGMMute = true;
        }
        else
        {
            PlayerPrefs.SetInt("Mute_BGM", 0);
            isBGMMute = false;
        }            
        if (PlayerPrefs.HasKey("Mute_SFX"))
        {
            if (PlayerPrefs.GetInt("Mute_SFX") == 0)
                isEffectMute = false;
            else
                isEffectMute = true;
        }
        else
        {
            PlayerPrefs.SetInt("Mute_SFX", 0);
            isEffectMute = false;
        }       
        beforeEffectVolumePivot = effectVolumePivot;        
    }

    public void SoundPlay(SOUND_NAME name, bool isLoop = false, float volume = 0.5f)
    {
        GameObject go = new GameObject(name.ToString());
        go.transform.SetParent(transform);

        AudioSource goAudio = go.AddComponent<AudioSource>();
        goAudio.clip = sources[name];
        goAudio.loop = isLoop;

        // 이름에 BGM이 포함되면. (배경음악에 해당하면)
        if (name.ToString().Contains("BGM"))
        {
            // 음소거 상태면 볼륨을 0으로 조정.
            goAudio.volume = isBGMMute ? 0 : volume * bgmVolumePivot;            
            bgmAudioSource = goAudio;
            nowplayName = name;            
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
    public AudioSource bgmAudioSource;
    public AudioSource NOWPLAY { get { return bgmAudioSource; } }
    public SOUND_NAME nowplayName;
    private bool isBGMMute;
    private bool isEffectMute;

    // 슬라이더 값은 
    public void BGMSliderValueChanged(Slider slider)
    {
        if(NOWPLAY != null)
        {
            bgmVolumePivot = slider.value;
            PlayerPrefs.SetFloat("Volume_BGM", slider.value);
            if (!isBGMMute)
            {
                bgmAudioSource.volume = slider.value;
            }
        }
    }

    public void BGMMute(Toggle toggle)
    {
        isBGMMute = toggle.isOn;
        if (toggle.isOn)
        {
            if (NOWPLAY != null)
                bgmAudioSource.volume = 0;
            PlayerPrefs.SetInt("Mute_BGM", 1);
        }
        else
        {
            if (NOWPLAY != null)
                bgmAudioSource.volume = bgmVolumePivot;
            PlayerPrefs.SetInt("Mute_BGM", 0);
        }
    }

    public void EffectSliderValueChanged(Slider slider)
    {
        beforeEffectVolumePivot = slider.value;
        PlayerPrefs.SetFloat("Volume_SFX", slider.value);
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
            PlayerPrefs.SetInt("Mute_SFX", 1);
            effectVolumePivot = 0;
        }
        else
        {
            isEffectMute= false;
            PlayerPrefs.SetInt("Mute_SFX", 0);
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

    // 씬 진입 시 재생되는 BGM 관리. 
    // 주혁: BGM 전환 시점을 로딩이 끝난 뒤로, 변경하면서 PreviousScene 변수는 필요없게 되었습니다.
    // 리팩토링 시 참고하시기 바랍니다.
    // LoadingSceneController의 LoadSceneProcess 코루틴에서 로딩이 모두 끝난 뒤 BGM이 전환됩니다.
    public void SceneBGMContorl(SceneName TargetScene)
    {        
        switch (TargetScene)
        {            
            case SceneName.Lobby:
                if (bgmAudioSource != null)
                    SoundFadeInOut(nowplayName, 0, 0.5f);
                SoundPlay(SOUND_NAME.BGM_LOBBY, true, 1f);
                break;
            case SceneName.World:             //World
                if (bgmAudioSource != null)
                    SoundFadeInOut(nowplayName, 0, 0.5f);
                SoundPlay(SOUND_NAME.BGM_WORLD, true, 1f);             
                break;
            case SceneName.Dungeon:             //Dungeon
                if (bgmAudioSource != null)
                    SoundFadeInOut(nowplayName, 0, 0.5f);
                SoundPlay(SOUND_NAME.BGM_DUNGEON_02, true, 1f);
                break;
            case SceneName.Dungeon_Fire:             //Dungeon_Fire
                if (bgmAudioSource != null)
                    SoundFadeInOut(nowplayName, 0, 0.5f);
                SoundPlay(SOUND_NAME.BGM_DUNGEON_01, true, 1f);
                break;
        }
    }
}
