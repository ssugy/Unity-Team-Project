using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_LoadScene : MonoBehaviour
{
    //이동할 씬 번호
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
        //버튼에 따른 씬이동
        GameManager.s_instance.LoadScene(targetSceneNum);
    }

    //Character 미선택시 알람 UI On/Off
    void onAlarm()
    {
        alarm.SetActive(true);
    }
    void offAlarm()
    {
        alarm.SetActive(false);
    }


}
