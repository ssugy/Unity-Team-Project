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
        SoundPlay(SOUND_NAME.BGM, true);  // ����� ����
    }


    //----------------- ����� �ҽ� �ʱ�ȭ : ������ҽ��� ����Ʈ �������� �����.
    /**
     * ����� �÷��̴� �ϴ��� ������ ���󰣴�.
     * 1. ��ü ����
     * 2. �ش� ��ü�� ����� �ҽ� ������Ʈ �߰�
     * 3. ����� �ҽ� �ֱ�
     * 4. ���尡 ������ destroy�ϱ�.
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
    public AudioClip[] clips;   // enum�� ������ ���󰡾ߵ�

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
            // ���� �ɼǶ����� ������� �������.
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

    //�����÷��� �ݹ� - Unity Docs�������δ� isPlaying������ Ȯ���ϴ� ������ �ݹ�ó���� �����ֽ��ϴ�.
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
     * 1. ���� �ɼ��� �����ϱ� ���ؼ���, ����� ������Ʈ�� ���� ������ �� �־� �ϰ�,
     * 2. ȿ���� �����, ���� �ϳ� �߰��ؼ� �ش纯���� ���� ������ ���带 ����.
     * 3. mute����� ������ ������� ������� �Ķ���� ������ ���ϴ� �������� �ؾ��� �����ʰ� ������ �����ϴ�.
     */
    private float bgmVolumePivot;
    private float effectVolumePivot;
    private float beforeEffectVolumePivot;
    private AudioSource bgmAudioSource;
    private bool isBGMMute;
    private bool isEffectMute;

    // �����̴� ���� 
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
            effectVolumePivot = slider.value;   // ������ �Ҹ��� ���Ŀ� ��Ÿ�� �����͵��� �Ҹ��� ������ ��ġ�� ����
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
