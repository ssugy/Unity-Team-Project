using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_SpRecover : MonoBehaviour
{
    Player mine;
    float time;

    void Start()
    {
        mine = GetComponent<Player>();
        mine.playerStat.SpRecover += 1;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 30f)
        {
            mine.playerStat.SpRecover -= 1;
            Destroy(this);
        }
    }
}
