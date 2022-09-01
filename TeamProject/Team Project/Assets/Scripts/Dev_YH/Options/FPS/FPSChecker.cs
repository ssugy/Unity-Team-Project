using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// FPS 체크는 OnGui를 통해서 진행
/// </summary>
public class FPSChecker : MonoBehaviour
{
    [Range(1, 100)] public int fpsFontSize;
    public Color fpsFontColor;
    public Text fpsText;
    float deltaTime;
    private void Start()
    {
        fpsFontSize = fpsFontSize == 0 ? 50 : fpsFontSize;  //지정안하면 50기본으로 
        StartCoroutine("StartFpsChecker");
        lowFPS = 500; // 0으로 초기화 되어있으면 바뀌지 않아서 임의값 지정.
        currentTime = Time.time;
    }

    float fps;
    float maxFPS;
    float lowFPS;
    float currentTime;
    IEnumerator StartFpsChecker()
    {
        while (true)
        {
            fpsText.text = $"FPS : {fps.ToString("F1")}\nMax : {maxFPS.ToString("F1")}\nLow : {lowFPS.ToString("F1")}";
            Canvas.ForceUpdateCanvases();
            yield return new WaitForSeconds(1);
            if (Time.time - currentTime > 15)
            {
                currentTime = Time.time;
                lowFPS = 500;
                maxFPS = 0;
            }
        }
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        fps = 1 / deltaTime;
        if (maxFPS < fps)
            maxFPS = fps;
        if (lowFPS > fps)
            lowFPS = fps;
    }

    //--- 프레임 체크용의 씬이동
    public void ChangeScene(GameObject go)
    {
        if (GameManager.s_instance.currentScene != GameManager.SceneName.Loading)
        {
            GameManager.s_instance.LoadScene(int.Parse(go.name));
        }
    }
}
