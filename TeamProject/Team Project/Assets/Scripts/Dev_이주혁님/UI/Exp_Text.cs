using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exp_Text : MonoBehaviour
{
    Text expText;
    PlayerStat stat;
    
    // Start is called before the first frame update
    void Start()
    {
        expText = GetComponent<Text>();
        stat = Player.instance.playerStat;
    }

    // Update is called once per frame
    void Update()
    {
        expText.text = (stat.CurExp * 100 / stat.Exp).ToString() + "% " 
            + "(" + stat.CurExp.ToString() + " / " + stat.Exp.ToString() + ")";
    }
}
