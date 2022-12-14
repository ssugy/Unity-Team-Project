using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class CutsceneBoss_2 : MonoBehaviour
{    
    private bool isPlay = false;
    public Camera cineCam;
    public JY_Boss_FireDungeon boss;
    public PlayableDirector director;
    public GameObject phase2_Light;

    IEnumerator BossWakeUp()
    {
        yield return new WaitForSeconds(3f);
        // 보스를 깨움.
        boss.isAwake = true;
    }

    public void OnSecondPhase()
    {
        StartCoroutine(BossWakeUp());
              

        // 보스전 BGM 재생.
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BossBGM_02, true, 1f);

        // 컷신용 카메라 off
        cineCam.enabled = false;
    }

    void Update()
    {
        // 컷신이 플레이 됐다면 종료.
        if (isPlay)
            return;

        // 보스 체력이 50% 이하라면.
        if ((float)boss.CurHealth / boss.maxHealth <= 0.5f)
        {
            // 컷신을 재생할 것이므로 isPlay = true;
            isPlay = true;
            // 보스 체력을 최대로 회복함. 보스를 행동 불가 상태로 만들고 위치를 초기화.
            boss.isAwake = false;
            boss.GetComponent<Animator>().Play("Hit_1");
            // 보스의 공격 스피드를 제어하는 파라미터를 변경.
            boss.GetComponent<Animator>().SetFloat("AttackSpeed", 1.05f);
            boss.CurHealth = boss.maxHealth;
            boss.transform.position = new Vector3(48f, 0f, 130f);
            boss.transform.rotation = Quaternion.identity;
            phase2_Light.SetActive(true);

            // 재생중인 던전 BGM을 off함.
            if (AudioManager.s_instance.bgmAudioSource != null)
                AudioManager.s_instance.SoundFadeInOut(AudioManager.s_instance.nowplayName, 0, 2f);

            // 조이스틱의 OnPointerUp을 실행함. (포인터업 시 드래그 중인 오브젝트를 null로 만듦)
            FloatingJoystick.instance.OnPointerUp(FloatingJoystick.instance.eventData);

            // 플레이어의 위치를 보스룸 시작 지점으로 이동함. Character Controller 컴포넌트가 켜져 있으면
            // 포지셔닝이 불가능하므로 Controller를 off 했다가 포지셔닝 후 다시 켜줌.
            // 본래 Character Controller가 off되면 에러가 발생하지만 같은 프레임 내에서 다시 on되므로 에러 X.
            JY_CharacterListManager.s_instance.playerList[0].controller.enabled = false;
            JY_CharacterListManager.s_instance.playerList[0].transform.position = new Vector3(48f, 0f, 100f);
            JY_CharacterListManager.s_instance.playerList[0].controller.enabled = true;

            // 컷신용 카메라를 켜주고, 컷신을 재생함.
            cineCam.enabled = true;
            director.Play();
        }

    }
}
