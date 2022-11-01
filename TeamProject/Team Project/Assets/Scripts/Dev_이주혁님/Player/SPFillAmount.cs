using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SPFillAmount : MonoBehaviour
{
    public Image barFront;
    public Text barText;
    PlayerStat stat;
    float recoverSpeed;
    bool isExhausted;

    // Start is called before the first frame update
    void Start()
    {
        stat = Player.instance.playerStat;
        recoverSpeed = 15f;
        isExhausted = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Player.instance.enableRecoverSP&& stat.CurSP < stat.SP)
        {
            Debug.Log("회복플래그");
            stat.CurSP += recoverSpeed * Time.deltaTime;
        }
        if (stat.CurSP <= 0.1f)
        {
            Player.instance.Exhausted();
            isExhausted = true;
        }
        else if (isExhausted && stat.CurSP >= stat.SP * 0.2)
        {
            Player.instance.Recovered();
            isExhausted = false;
        }
        barFront.fillAmount = (float)stat.CurSP / stat.SP;
        barText.text = ((int)stat.CurSP).ToString() + " / " + ((int)stat.SP).ToString();

    }
}
