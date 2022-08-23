using System.Collections;
using System.Collections.Generic;
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
        SoundPlay(SOUND_NAME.BGM);  // ����� ����
    }


    //----------------- ����� �ҽ� �ʱ�ȭ : ������ҽ��� ����Ʈ �������� �����.
    Dictionary<SOUND_NAME, AudioSource> sources;
    public enum SOUND_NAME
    {
        BGM,
    }
    public SOUND_NAME Name;

    public void InitAudioSources()
    {
        sources = new Dictionary<SOUND_NAME, AudioSource>();
        sources.Add(SOUND_NAME.BGM, GetComponent<AudioSource>());
    }

    public void SoundPlay(SOUND_NAME name)
    {
        sources[name].Play();
    }
}
