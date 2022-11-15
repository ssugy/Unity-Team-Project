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
        stat = JY_CharacterListManager.s_instance.playerList[0].playerStat;
    }

    // Update is called once per frame
    void Update()
    {
        barFront.fillAmount = (float)stat.CurExp / stat.Exp;
    }
}
