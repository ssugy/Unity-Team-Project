using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buff_AtkPoint : MonoBehaviour
{
    Player player;
    Image buffImg;

    void Start()
    {
        player = GetComponent<Player>();
        player.playerStat.atkPoint += 30;

        TrailRenderer Effect = player.WeaponEffect.GetComponent<TrailRenderer>();
        Effect.material.color = new Color(1f, 0f, 0f);
        Effect.endColor = new Color(1f, 0f, 0f);

        GameObject Sources = Resources.Load<GameObject>("Sprites/BuffTime");
        buffImg = Instantiate(Sources , BattleUI.instance.BuffLayout).GetComponent<Image>();
        Sprite GetImage = Resources.Load<Sprite>("Sprites/skill2");
        buffImg.sprite = GetImage;

        StartCoroutine(Xtime());
    }

    float startTime;
    float currentTime;
    Color blankColor;
    bool isColorWhite;
    IEnumerator Xtime()
    {
        startTime = Time.time;
        currentTime = Time.time;
        blankColor = new Color(1, 1, 1, 0);
        isColorWhite = true;
        float colorChangeSpeed = 1;
        float lerpProgress = 0;
        yield return new WaitForSeconds(20f);   // 20초 지난뒤에 깜빡임 시작 (깜빡이는 속도는 최초 1초부터 0.3초까지 줄어든다)
        while (startTime + 10 > currentTime)
        {
            if (colorChangeSpeed > 0.3f)
            {
                colorChangeSpeed -= Time.deltaTime / 7; // 7초에 걸쳐서 서서히 변환
            }

            currentTime = Time.time;
            if (isColorWhite)
            {
                Debug.Log($"{lerpProgress} {colorChangeSpeed}");
                lerpProgress += Time.deltaTime / colorChangeSpeed; 
                buffImg.color = Color.Lerp(Color.white, blankColor, lerpProgress);
                if (buffImg.color == blankColor)
                {
                    isColorWhite = false;
                    lerpProgress = 0;
                }
                yield return null;
            }
            else
            {
                lerpProgress += Time.deltaTime / colorChangeSpeed;
                buffImg.color = Color.Lerp(blankColor, Color.white, Time.deltaTime / colorChangeSpeed);
                if (buffImg.color == Color.white)
                {
                    isColorWhite = true;
                    lerpProgress = 0;
                }
                yield return null;
            }
        }

        player.playerStat.atkPoint -= 30;
        TrailRenderer Effect = player.WeaponEffect.GetComponent<TrailRenderer>();
        Effect.material.color = new Color(1f, 1f, 1f);
        Effect.endColor = new Color(1f, 1f, 1f, 0f);
        Destroy(buffImg);
        Destroy(this);
    }
}
