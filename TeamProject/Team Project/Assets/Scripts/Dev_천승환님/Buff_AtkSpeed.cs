using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_AtkSpeed : MonoBehaviour
{
    Player mine;
    float time;

    void Start()
    {
        mine = GetComponent<Player>();
        mine.playerAni.SetFloat("AtkSpeed", mine.playerAni.GetFloat("AtkSpeed") + 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 30f)
        {
            mine.playerAni.SetFloat("AtkSpeed", mine.playerAni.GetFloat("AtkSpeed") - 0.2f);
            Destroy(this);
        }
    }
}
