using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buff_SpRecover : MonoBehaviour
{
    Player mine;
    float time;
    Image blinktime;



    void Start()
    {
        mine = GetComponent<Player>();
        mine.playerStat.SpRecover += 1;
        GameObject Sources = Resources.Load<GameObject>("Sprites/BuffTime");
        blinktime = Instantiate(Sources, BattleUI.instance.BuffLayout).GetComponent<Image>();
        Sprite GetImage = Resources.Load<Sprite>("Sprites/skill3");
        blinktime.sprite = GetImage;

        StartCoroutine(Xtime());
    }

    IEnumerator Xtime()
    {
        yield return new WaitForSeconds(20f);
        while (time < 29.7f)
        {
            blinktime.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(0.1f);
            blinktime.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.1f);
        }

    }
    void Update()
    {
        time += Time.deltaTime;
        if (time > 30f)
        {
            mine.playerStat.SpRecover -= 1;
            Destroy(blinktime);
            Destroy(this);
        }
    }
}
