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
        CLICK,
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
}
