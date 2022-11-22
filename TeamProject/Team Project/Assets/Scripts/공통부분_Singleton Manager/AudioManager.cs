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
        if (name.Equals(SOUND_NAME.BGM)|| name.Equals(SOUND_NAME.BossBGM_01)|| name.Equals(SOUND_NAME.BossBGM_02 )|| name.Equals(SOUND_NAME.BGM_WORLD))
        {
            // ���� �ɼǶ����� ������� �������.
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
    /// ���� ���̵� ��/�ƿ� ���
    /// </summary>
    /// <param name="name">���� �̸�</param>
    /// <param name="destVolume">������ ����(0~1����) (0�̸� �ڵ� ����)</param>
    /// <param name="endTime">������ ��� ����Ǵµ� �ɸ��� �ð�(��)<param>
    public void SoundFadeInOut(SOUND_NAME name, float destVolume, float endTime)
    {
        // ���� �����ִ� ����� �ҽ� �߿�, ���� ���ϴ� �̸��� ����� �ҽ��� ã�Ҵ��� Ȯ��
        List<AudioSource> audioList = new List<AudioSource>();
        Transform[] chilTrans = GetComponentsInChildren<Transform>();
        foreach (Transform item in chilTrans)
        {
            if (item.name.Equals(name.ToString()))
            {
                audioList.Add(item.GetComponent<AudioSource>());
            }
        }

        // ����� �߰��Ȱ� ������ �״�� ����
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
                // ������ ���� ������ 0.05���ϸ�, �״�� �����ϱ�
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
    public AudioSource NOWPLAY { get { return bgmAudioSource; } }
    SOUND_NAME nowplayName;
    private bool isBGMMute;
    private bool isEffectMute;

    // �����̴� ���� 
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
                //World������ �̵��� BGM ����
                if(TargetScene == 4)
                {
                    AudioManager.s_instance.SoundFadeInOut(nowplayName, 0, 0.5f);
                    AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BGM_WORLD, true, 0.5f);
                }

                break;
            case 3:             //MakeCharacter
                break;
            case 4:             //World
                //Lobby������ �̵��� BGM ����
                if(TargetScene == 2)
                {
                    AudioManager.s_instance.SoundFadeInOut(nowplayName, 0, 0.5f);
                    AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BGM, true, 0.5f);
                }
                //Dungeon����� �̵��� BGM ��� ����
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
                //������ BGM�� ��� ���� ��� ���� ��� ���� ��Ų �� �̵�
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
