using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EXPFillAmount : MonoBehaviour
{
    public Image barFront;
    PlayerStat stat;
    // Start is called before the first frame update
    void Start()
    {
        stat = Player.instance.playerStat;
    }

    // Update is called once per frame
    void Update()
    {
        barFront.fillAmount = (float)stat.curExp / stat.Exp;
    }
}
