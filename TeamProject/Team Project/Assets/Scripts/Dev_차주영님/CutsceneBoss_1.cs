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
        Debug.Log("������ ����");
        JY_Boss_FireDungeon.s_instance.isAwake = true;
        JY_UIManager.instance.partdestructionUIButton.SetActive(true);
        hpBarBoss.Recognize(boss);
        // ������ BGM ���.
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BossBGM_01, true, 1f);
        cineCam.enabled = false;        
    }

    private void OnTriggerEnter(Collider other)
    {        
        // ��Ƽ �÷��� �� �÷��̾� �� ������ ���� �����뿡 ���� �� �ƾ��� ����ǰ� ���������� �̵���.
        if (other.CompareTag("Player"))
        {
            // Collider�� Off����.
            GetComponent<Collider>().enabled = false;

            // ������� ���� BGM�� off��.
            if (AudioManager.s_instance.bgmAudioSource != null)
                AudioManager.s_instance.SoundFadeInOut(AudioManager.s_instance.nowplayName, 0, 1f);

            // ���̽�ƽ�� OnPointerUp�� ������. (�����;� �� �巡�� ���� ������Ʈ�� null�� ����)
            FloatingJoystick.instance.OnPointerUp(FloatingJoystick.instance.eventData);           

            // �÷��̾��� ��ġ�� ������ ���� �������� �̵���. Character Controller ������Ʈ�� ���� ������
            // �����Ŵ��� �Ұ����ϹǷ� Controller�� off �ߴٰ� �����Ŵ� �� �ٽ� ����.
            // ���� Character Controller�� off�Ǹ� ������ �߻������� ���� ������ ������ �ٽ� on�ǹǷ� ���� X.
            JY_CharacterListManager.s_instance.playerList[0].controller.enabled = false;
            JY_CharacterListManager.s_instance.playerList[0].transform.position = new Vector3(48f, 0f, 100f);
            JY_CharacterListManager.s_instance.playerList[0].controller.enabled = true;

            // �ƽſ� ī�޶� ���ְ�, �ƽ��� �����.
            cineCam.enabled = true;
            director.Play();

            // Playable Director���� Track�� ������Ʈ�� �������� ���ε��ϴ� ��ũ��Ʈ.
            // �÷��̾�� ĳ���ʹ� ���� ���۵� �� �������� �����Ǳ� ������ �̸� ���ε��� �� ����.
            PlayableAsset pa = director.playableAsset;
            var bindings = pa.outputs;
            foreach (var track in bindings)
            {
                if (track.sourceObject is AnimationTrack)
                {
                    // Timeline�� ù��° �ִϸ��̼� Ʈ���� �ݵ�� �÷��̾�� ĳ���Ͱ� ��ġ�ؾ� ��.
                    // SetGenerigBinding�� �̿��ϸ� Timeline�� ������Ʈ�� �������� ���ε� ����.
                    director.SetGenericBinding(track.sourceObject, 
                        JY_CharacterListManager.s_instance.playerList[0].GetComponent<Animator>());
                    break;
                }
            }            
        }       
    }   
}
