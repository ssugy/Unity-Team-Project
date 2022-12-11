using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SPFillAmount : MonoBehaviour
{
    public Image barFront;
    public Text barText;
    PlayerStat stat;
    bool isExhausted;

    // Start is called before the first frame update
    void Start()
    {
        stat = JY_CharacterListManager.s_instance.playerList[0].playerStat;
        isExhausted = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (JY_CharacterListManager.s_instance.playerList[0].enableRecoverSP && stat.CurSP < stat.SP)
        {
            stat.CurSP += stat.SpRecover * Time.deltaTime;
        }
        if (stat.CurSP <= 0.1f)
        {
            JY_CharacterListManager.s_instance.playerList[0].Exhausted();
            isExhausted = true;
        }
        else if (isExhausted && stat.CurSP >= stat.SP * 0.2)
        {
            JY_CharacterListManager.s_instance.playerList[0].Recovered();
            isExhausted = false;
        }
        barFront.fillAmount = (float)stat.CurSP / stat.SP;
        if (stat.CurSP < 0)            
            barText.text = "0 / " + ((int)stat.SP).ToString();        
        else
            barText.text = ((int)stat.CurSP).ToString() + " / " + ((int)stat.SP).ToString();

    }
}
