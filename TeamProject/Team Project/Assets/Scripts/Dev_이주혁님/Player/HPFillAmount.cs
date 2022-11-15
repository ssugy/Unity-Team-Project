using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPFillAmount : MonoBehaviour
{
    public Image barFront;
    public Text barText;
    PlayerStat stat;
    float HpRecoverTime;
    // Start is called before the first frame update
    void Start()
    {
        HpRecoverTime = 0f;
        stat = JY_CharacterListManager.s_instance.playerList[0].playerStat;
    }

    // Update is called once per frame
    void Update()
    {
        if (stat.CurHP < stat.HP)
        {
            HpRecoverTime += Time.deltaTime;
            if(HpRecoverTime >= stat.HPRecoverCoolTime)
            {
                HpRecoverTime -= stat.HPRecoverCoolTime;
                stat.CurHP += stat.HpRecover;
            }
        }

        barFront.fillAmount = (float)stat.CurHP / stat.HP;
        barText.text = stat.CurHP.ToString() + " / " + stat.HP.ToString();
    }
}
