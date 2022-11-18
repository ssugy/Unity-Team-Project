using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_HpRecover : MonoBehaviour
{
    Player mine;
    float time;

    void Start()
    {
        mine = GetComponent<Player>();
        mine.playerStat.HpRecover += 10;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 30f)
        {
            mine.playerStat.HpRecover -= 10;
            Destroy(this);
        }
    }
}
