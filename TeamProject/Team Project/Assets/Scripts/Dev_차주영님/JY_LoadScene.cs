using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_LoadScene : MonoBehaviour
{
    //�̵��� �� ��ȣ
    public int targetSceneNum;
    public GameObject alarm;

    public void enterScene()
    {
        if (JY_CharacterListManager.s_instance.selectNum == -1 && targetSceneNum == 4)
        {
            onAlarm();
            Invoke("offAlarm", 1f);
            return;
        }
        //��ư�� ���� ���̵�
        GameManager.s_instance.LoadScene(targetSceneNum);
    }

    //Character �̼��ý� �˶� UI On/Off
    void onAlarm()
    {
        alarm.SetActive(true);
    }
    void offAlarm()
    {
        alarm.SetActive(false);
    }


}
