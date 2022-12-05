using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EGA_EffectSound : MonoBehaviour
{
    public bool Repeating = true;
    public float RepeatTime = 2.0f;
    public float StartTime = 0.0f;
    public bool RandomVolume;
    public float minVolume = .4f;
    public float maxVolume = 1f;
    private AudioClip clip;

    private AudioSource soundComponent;

    void Awake ()
    {
        soundComponent = GetComponent<AudioSource>();
        clip = soundComponent.clip;
    }
    private void OnEnable()
    {
        soundComponent.PlayOneShot(clip);
        Invoke("effectOff", 1.5f);
    }
    void effectOff()
    {
        InstanceManager.s_instance.ExtraEffectOff();
    }
}
