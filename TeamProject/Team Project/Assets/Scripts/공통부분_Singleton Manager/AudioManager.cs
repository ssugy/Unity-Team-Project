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
        SoundPlay(SOUND_NAME.BGM, true, 1f);  // ���� BGM ����
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
    
    public AudioClip[] clips;   // enum�� ������ ���󰡾ߵ�

    public void InitAudioSources()
    {
        // ��ųʸ��� Sound_Name, AudioClip ���� ����.
        sources = new();
        for (int i = 0; i < clips.Length; i++)        
            sources.Add((SOUND_NAME)i, clips[i]);

        // PlayerPrefs�� �̿��Ͽ� ���� �ɼ� ������ �����ϰ� �ҷ���.
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

        // �̸��� BGM�� ���ԵǸ�. (������ǿ� �ش��ϸ�)
        if (name.ToString().Contains("BGM"))
        {
            // ���Ұ� ���¸� ������ 0���� ����.
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
    public AudioSource bgmAudioSource;
    public AudioSource NOWPLAY { get { return bgmAudioSource; } }
    public SOUND_NAME nowplayName;
    private bool isBGMMute;
    private bool isEffectMute;

    // �����̴� ���� 
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
            effectVolumePivot = slider.value;   // ������ �Ҹ��� ���Ŀ� ��Ÿ�� �����͵��� �Ҹ��� ������ ��ġ�� ����
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

    // �� ���� �� ����Ǵ� BGM ����. 
    // ����: BGM ��ȯ ������ �ε��� ���� �ڷ�, �����ϸ鼭 PreviousScene ������ �ʿ���� �Ǿ����ϴ�.
    // �����丵 �� �����Ͻñ� �ٶ��ϴ�.
    // LoadingSceneController�� LoadSceneProcess �ڷ�ƾ���� �ε��� ��� ���� �� BGM�� ��ȯ�˴ϴ�.
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
