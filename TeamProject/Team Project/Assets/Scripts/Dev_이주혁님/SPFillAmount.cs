using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SPFillAmount : MonoBehaviour
{
    public Image barFront;
    PlayerStat stat;
    float recoverSpeed;
    // Start is called before the first frame update
    void Start()
    {
        stat = Player.instance.playerStat;
        recoverSpeed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        barFront.fillAmount = (float)stat.curSP / stat.SP;
        if (stat.curSP < stat.SP)
        {
            stat.curSP += recoverSpeed * Time.deltaTime;
        }
        if (stat.curSP <= 0)
        {
            Player.instance.Exhaisted();
            recoverSpeed = 6f;
        }
        else if (stat.curSP >= stat.SP * 0.8)
        {
            Player.instance.Recovered();
            recoverSpeed = 10f;
        }
        
    }
}
