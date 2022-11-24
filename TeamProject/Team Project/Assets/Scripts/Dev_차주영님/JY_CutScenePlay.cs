using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JY_CutScenePlay : MonoBehaviour
{
    public static JY_CutScenePlay instance;
    public GameObject CutScene1;
    public GameObject CutScene2;
    public GameObject BattleUI;
    public Enemy boss;
    public Camera mainCam;
    public Camera cutSceneCam;
    public Canvas fadeUI;
    public Image fade;
    public float fadeTime;
    public HP_Bar_Boss hpBarBoss;

    bool firstPlay;
    private void Awake()
    {
        instance = this;
        firstPlay = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (firstPlay == true)
            {
                JY_Boss_FireDungeon.s_instance.isAwake = true;
                hpBarBoss.Recognize(boss);
            }
        }       
    }
    IEnumerator CutScene_1()
    {
        //시작 페이드아웃
        StartCoroutine(Fade(0, 1));
        AudioManager.s_instance.SoundFadeInOut(AudioManager.SOUND_NAME.BGM, 0, 2);
        yield return new WaitForSeconds(2f);
        //카메라 전환
        mainCam.gameObject.SetActive(false);
        cutSceneCam.gameObject.SetActive(true);
        //UI 전환 및 페이드 인, 컷신 재생, BGM 재생
        BattleUI.SetActive(false);
        StartCoroutine(Fade(1, 0));
        CutScene1.SetActive(true);
        yield return new WaitForSeconds(5f);
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BossBGM_01, true);
        yield return new WaitForSeconds(5f);
        //컷신 종료 및 페이드 아웃, 카메라 전환
        StartCoroutine(Fade(0, 1));
        yield return new WaitForSeconds(2f);
        CutScene1.SetActive(false);
        cutSceneCam.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        JY_CharacterListManager.s_instance.playerList[0].enableMove = true;
        JY_CharacterListManager.s_instance.playerList[0].movement = Vector3.zero;
        yield return new WaitForSeconds(1f);
        StartCoroutine(Fade(1, 0));
        BattleUI.SetActive(true);

        hpBarBoss.gameObject.SetActive(true);
        hpBarBoss.Recognize(boss);
        boss.target = JY_CharacterListManager.s_instance.playerList[0].transform;
    }

    //중간 컷신은 조정해야함.
    IEnumerator CutScene_2()
    {
        mainCam.gameObject.SetActive(false);
        cutSceneCam.gameObject.SetActive(true);
        JY_CharacterListManager.s_instance.playerList[0].enableMove = false;
        BattleUI.SetActive(false);
        AudioManager.s_instance.SoundFadeInOut(AudioManager.SOUND_NAME.BossBGM_01, 0, 1);
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BossBGM_02, true, 0.5f);

        boss.gameObject.SetActive(false);
        boss.transform.position = boss.originPos;
        boss.transform.rotation = boss.originRotateion;
        CutScene2.SetActive(true);
        yield return new WaitForSeconds(18f);


        JY_CharacterListManager.s_instance.playerList[0].enableMove = true;
        JY_CharacterListManager.s_instance.playerList[0].movement = Vector3.zero;
        CutScene2.SetActive(false);
        BattleUI.SetActive(true);
        boss.gameObject.SetActive(true);
        cutSceneCam.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(true);
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
    
    public void PlayCutScene2()
    {
        StartCoroutine("CutScene_2");
    }
}
