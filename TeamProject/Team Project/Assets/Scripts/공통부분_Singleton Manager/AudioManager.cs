using System;
using System.Collections;
using System.Collections.Generic;
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
        if (name.Equals(SOUND_NAME.BGM))
        {
            // 사운드 옵션때문에 배경음은 따로취급.
            goAudio.volume = volume * bgmVolumePivot;
            bgmAudioSource = goAudio;
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
    private bool isBGMMute;
    private bool isEffectMute;

    // 슬라이더 값은 
    public void BGMSliderValueChanged(Slider slider)
    {
        bgmVolumePivot = slider.value;
        if (!isBGMMute)
        {
            bgmAudioSource.volume = slider.value;
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
}
