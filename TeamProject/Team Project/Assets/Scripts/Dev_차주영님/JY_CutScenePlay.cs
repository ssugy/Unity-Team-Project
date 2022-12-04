using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;
using UnityEngine.Playables;

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
    public PlayableDirector director;

    bool firstPlay;
    private void Awake()
    {
        instance = this;
        firstPlay = true;
    }        

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Collider>().enabled = false;

        // 멀티 플레이 시 플레이어 중 누구라도 먼저 보스룸에 진입 시 컷씬이 재생되고 보스룸으로 이동함.
        if (other.CompareTag("Player"))
        {
            // 던전 BGM을 off함.
            if (AudioManager.s_instance.bgmAudioSource != null)
                AudioManager.s_instance.SoundFadeInOut(AudioManager.s_instance.nowplayName, 0, 1f);


            // 조이스틱을 평형 상태로 만들어 줌.
            FloatingJoystick.instance.OnPointerUp(FloatingJoystick.instance.eventData);

            // 플레이어의 위치를 보스룸 시작 지점으로 이동함. Character Controller 컴포넌트가 켜져 있으면
            // 포지셔닝이 불가능하므로 Controller를 off 했다가 포지셔닝 후 다시 켜줌.
            // 본래 Character Controller가 off되면 에러가 발생하지만 같은 프레임 내에서 다시 on되므로 에러 X.
            JY_CharacterListManager.s_instance.playerList[0].controller.enabled = false;
            JY_CharacterListManager.s_instance.playerList[0].transform.position = new Vector3(48f, 0f, 100f);
            JY_CharacterListManager.s_instance.playerList[0].controller.enabled = true;

            // 시네머신이 부착된 오브젝트를 on함. 활성 상태일 때 재생 프로퍼티로 인해 바로 컷신이 재생됨.
            CutScene1.SetActive(true);

            // Playable Director에서 Track에 오브젝트를 동적으로 바인딩하는 스크립트.
            // 플레이어블 캐릭터는 씬이 시작될 때 동적으로 생성되기 때문에 미리 바인딩할 수 없다.
            PlayableAsset pa = director.playableAsset;
            var bindings = pa.outputs;
            foreach (var track in bindings)
            {
                if (track.sourceObject is AnimationTrack)
                {
                    // Timeline에 첫번째 애니메이션 트랙은 반드시 플레이어블 캐릭터가 위치해야 함.
                    // SetGenerigBinding을 이용하면 Timeline에 오브젝트를 동적으로 바인딩 가능.
                    director.SetGenericBinding(track.sourceObject, 
                        JY_CharacterListManager.s_instance.playerList[0].GetComponent<Animator>());
                    break;
                }
            }

            JY_Boss_FireDungeon.s_instance.isAwake = true;
            JY_UIManager.instance.partdestructionUIButton.SetActive(true);
            hpBarBoss.Recognize(boss);
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
