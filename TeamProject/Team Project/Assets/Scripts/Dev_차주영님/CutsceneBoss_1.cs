using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class CutsceneBoss_1 : MonoBehaviour
{
    public Camera cineCam;    
    public Enemy boss;        
    public HP_Bar_Boss hpBarBoss;
    public PlayableDirector director;
    
    public void OnAwakenBoss()
    {
        Debug.Log("보스전 시작");
        JY_Boss_FireDungeon.s_instance.isAwake = true;
        JY_UIManager.instance.partdestructionUIButton.SetActive(true);
        hpBarBoss.Recognize(boss);
        // 보스전 BGM 재생.
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BossBGM_01, true, 1f);
        cineCam.enabled = false;        
    }

    private void OnTriggerEnter(Collider other)
    {        
        // 멀티 플레이 시 플레이어 중 누구라도 먼저 보스룸에 진입 시 컷씬이 재생되고 보스룸으로 이동함.
        if (other.CompareTag("Player"))
        {
            // Collider를 Off해줌.
            GetComponent<Collider>().enabled = false;

            // 재생중인 던전 BGM을 off함.
            if (AudioManager.s_instance.bgmAudioSource != null)
                AudioManager.s_instance.SoundFadeInOut(AudioManager.s_instance.nowplayName, 0, 1f);

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
        }       
    }   
}
