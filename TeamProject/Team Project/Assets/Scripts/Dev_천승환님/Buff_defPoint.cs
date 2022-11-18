using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_defPoint : MonoBehaviour
{
    Player mine;
    float time;

    void Start()
    {
        mine = GetComponent<Player>();
        mine.playerStat.defPoint += 10;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 30f)
        {
            mine.playerStat.defPoint -= 10;
            Destroy(this);
        }
    }
}
