using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JY_Guide : MonoBehaviour
{
    public Transform target_1;
    public Transform target_2;
    public Transform target_3;
    public Transform target_4;
    [HideInInspector]public Transform target;
    bool state;
    public Image Checkmark;
    public Transform TARGET { set { target = value; } }
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = JY_CharacterListManager.s_instance.playerList[0];
        target = target_1;
        state = true;
    }

    // Update is called once per frame
    void Update()
    {
        calcVec(target);
    }
    void calcVec(Transform target)
    {
        Vector3 dir = (target.position-player.transform.position).normalized;
        dir.y = 0f;
        Vector3 tmp = dir + player.transform.position;
        tmp.y += 0.5f;
        transform.position = tmp;
        transform.LookAt(target);
    }

    public void GuideOnOff()
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.CLICK_01);
        if (!state)
        {
            state = true;
            Checkmark.color = Color.red;
            gameObject.SetActive(true);
        }
        else
        {
            state = false;
            Checkmark.color = Color.white;
            gameObject.SetActive(false);
        }
    }
}
