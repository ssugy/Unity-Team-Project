using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_HP : MonoBehaviour
{
    Player mine;
    float time;

    void Start()
    {
        mine = GetComponent<Player>();
        mine.playerStat.HP += 300;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 30f)
        {
            mine.playerStat.HP -= 300;
            Destroy(this);
        }
    }
}
