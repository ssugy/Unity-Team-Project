using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SPFillAmount : MonoBehaviour
{
    public Image barFront;
    public Text barText;
    Player player;    

    // Start is called before the first frame update
    void Start()
    {
        player = JY_CharacterListManager.s_instance.playerList[0];        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.enableRecoverSP)
            player.playerStat.CurSP += player.playerStat.SpRecover * Time.deltaTime;

        barFront.fillAmount = (float)player.playerStat.CurSP / player.playerStat.SP;
        if (player.playerStat.CurSP < 0)            
            barText.text = "0 / " + ((int)player.playerStat.SP).ToString();        
        else
            barText.text = ((int)player.playerStat.CurSP).ToString() 
                + " / " + ((int)player.playerStat.SP).ToString();

    }
}
