using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAudioManager : MonoBehaviour
{
    private int currentSound = -1;

    public AudioSource sourceBGM;
    public AudioSource sourceSound;

    public List<AudioClip> bgms;
    public List<AudioClip> effects;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBGM(int idx)
    {
        if (currentSound != idx && idx >=0 && bgms.Count > idx)
        {
            sourceBGM.Stop();
            currentSound = idx;
            sourceBGM.clip = bgms[idx];
            sourceBGM.Play();
        }
    }

    public void PlaySound(int idx)
    {
        if (idx >= 0 && effects.Count > idx)
        {
            //sourceSound.clip = effects[idx];
            sourceSound.PlayOneShot(effects[idx]);
        }
    }

    public int NumSounds()
    {
        return effects.Count;
    }

    public int NumBGMs()
    {
        return bgms.Count;
    }


    public void PlayNextBGM()
    {
        currentSound++;
        if (bgms.Count <= currentSound)
        {
            currentSound = 0;
        }
        if (bgms.Count > currentSound)
        {
            Debug.Log("Playing BGM #" + currentSound);
            sourceBGM.clip = bgms[currentSound];
            sourceBGM.loop = true;
            sourceBGM.Play();
        }

    }
}
