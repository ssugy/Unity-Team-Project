using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image fade;

    public void FadeInOut() => StartCoroutine(FadeInAndOut());
    IEnumerator FadeInAndOut()
    {        
        float currentTime = 0f;
        float percent = 0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / 1f;

            Color color = fade.color;
            color.a = Mathf.Lerp(0, 1, percent);
            fade.color = color;
            yield return null;
        }

        currentTime = 0f;
        percent = 0f;
        yield return new WaitForSeconds(0.7f);

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / 1f;

            Color color = fade.color;
            color.a = Mathf.Lerp(1, 0, percent);
            fade.color = color;
            yield return null;
        }        
    }    
}
