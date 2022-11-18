using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_AtkPoint : MonoBehaviour
{
    Player mine;
    float time;
    
    void Start()
    {
        mine = GetComponent<Player>();
        mine.playerStat.atkPoint += 30;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time>30f)
        {
            mine.playerStat.atkPoint -= 30;
            Destroy(this);
        }
    }
}
