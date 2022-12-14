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
        // ������ ����.
        boss.isAwake = true;
    }

    public void OnSecondPhase()
    {
        StartCoroutine(BossWakeUp());
              

        // ������ BGM ���.
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.BossBGM_02, true, 1f);

        // �ƽſ� ī�޶� off
        cineCam.enabled = false;
    }

    void Update()
    {
        // �ƽ��� �÷��� �ƴٸ� ����.
        if (isPlay)
            return;

        // ���� ü���� 50% ���϶��.
        if ((float)boss.CurHealth / boss.maxHealth <= 0.5f)
        {
            // �ƽ��� ����� ���̹Ƿ� isPlay = true;
            isPlay = true;
            // ���� ü���� �ִ�� ȸ����. ������ �ൿ �Ұ� ���·� ����� ��ġ�� �ʱ�ȭ.
            boss.isAwake = false;
            boss.GetComponent<Animator>().Play("Hit_1");
            // ������ ���� ���ǵ带 �����ϴ� �Ķ���͸� ����.
            boss.GetComponent<Animator>().SetFloat("AttackSpeed", 1.05f);
            boss.CurHealth = boss.maxHealth;
            boss.transform.position = new Vector3(48f, 0f, 130f);
            boss.transform.rotation = Quaternion.identity;
            phase2_Light.SetActive(true);

            // ������� ���� BGM�� off��.
            if (AudioManager.s_instance.bgmAudioSource != null)
                AudioManager.s_instance.SoundFadeInOut(AudioManager.s_instance.nowplayName, 0, 2f);

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
        }

    }
}
