using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPFillAmount : MonoBehaviour
{
    public Image barFront;
    public Text barText;
    PlayerStat stat;
    // Start is called before the first frame update
    void Start()
    {
        stat = Player.instance.playerStat;
    }

    // Update is called once per frame
    void Update()
    {
        barFront.fillAmount = (float)stat.CurHP / stat.HP;
        barText.text = stat.CurHP.ToString() + " / " + stat.HP.ToString();
    }
}
