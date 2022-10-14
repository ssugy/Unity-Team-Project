using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JY_CutScenePlay : MonoBehaviour
{
    public GameObject CutScene1;
    public GameObject CutScene2;
    public GameObject BattleUI;
    public Camera mainCam;
    public Canvas fadeUI;
    public Image fade;
    public float fadeTime;

    bool firstPlay;
    Vector3 CamAxisPos;
    Vector3 CamPos;
    private void Awake()
    {
        firstPlay = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(firstPlay == true)
        {
            Player.instance.enableMove = false;
            Player.instance.movement = Vector3.zero;
            StartCoroutine("CutScene_1");
            firstPlay = false;
            CamAxisPos = mainCam.transform.parent.transform.position;
            CamPos = mainCam.transform.position;
        }

    }
    IEnumerator CutScene_1()
    {

        StartCoroutine(Fade(0, 1));
        StartCoroutine("AudioDestroy");
        yield return new WaitForSeconds(2f);
        BattleUI.SetActive(false);
        StartCoroutine(Fade(1, 0));
        CutScene1.SetActive(true);
        yield return new WaitForSeconds(5f);
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BossBGM_01, true);
        yield return new WaitForSeconds(5f);

        StartCoroutine(Fade(0, 1));
        yield return new WaitForSeconds(2f);
        CutScene1.SetActive(false);
        yield return new WaitForSeconds(2f);
        Player.instance.enableMove = true;
        Player.instance.movement = Vector3.zero;
        mainCam.transform.parent.transform.position = CamAxisPos;
        mainCam.transform.position = CamPos;
        yield return new WaitForSeconds(1f);
        StartCoroutine(Fade(1, 0));
        BattleUI.SetActive(true);
    }

    //중간 컷신은 조정해야함.
    IEnumerator CutScene_2()
    {
        StartCoroutine(Fade(0, 1));
        //StartCoroutine("AudioDestroy");
        yield return new WaitForSeconds(2f);
        BattleUI.SetActive(false);
        StartCoroutine(Fade(1, 0));
        CutScene2.SetActive(true);
    }

    IEnumerator Fade(float start, float end)
    {
        if (start == 0)
            fadeUI.gameObject.SetActive(true);

        float currentTime = 0f;
        float percent = 0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            Color color = fade.color;
            color.a = Mathf.Lerp(start, end, percent);
            fade.color = color;
            yield return null;
        }
        if (start == 1)
            fadeUI.gameObject.SetActive(false);
    }
    
    IEnumerator AudioDestroy()
    {
        AudioSource target = AudioManager.s_instance.GetComponentInChildren<AudioSource>();
        Destroy(target.gameObject);
        yield return null;
    }
}
