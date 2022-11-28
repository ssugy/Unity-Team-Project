using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    static int nextSceneIndex;
    [SerializeField] Image progressBar;

    public static void LoadScene(int index)
    {
        nextSceneIndex = index;
        SceneManager.LoadScene(1);  // 항상 로딩씬으로 이동한 뒤 진행
    }

    public GameObject[] loadingBGImgs;
    public GameObject[] loadingMents;
    private void Start()
    {
        SetBGImgAndMent();
        StartCoroutine(LoadSceneProcess());
    }

    private void SetBGImgAndMent()
    {
        int selectedImgNum = Random.Range(0, loadingBGImgs.Length);
        int selectedMentNum = Random.Range(0, loadingMents.Length);
        for (int i = 0; i < loadingBGImgs.Length; i++)
        {
            if (i == selectedImgNum)
            {
                loadingBGImgs[i].SetActive(true);
            }
            else
            {
                loadingBGImgs[i].SetActive(false);
            }
        }

        for (int i = 0; i < loadingMents.Length; i++)
        {
            if (i == selectedMentNum)
            {
                loadingMents[i].SetActive(true);
            }
            else
            {
                loadingMents[i].SetActive(false);
            }
        }
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneIndex);

        // 씬 로딩이 끝난뒤에 자동으로 불러온 씬으로 이동할 것이냐라는 의미.
        // 플레이어에게 팁이나 설명등을 보여주고, 리소스 로드할 시간을 벌기위해 90프로에서 멈추고,
        // 어느정도 시간뒤에 100프로로 진행하게 합니다.
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                // 페이크 로딩 - 90프로 부터 100프로까지는 1초간 채웁니다.
                timer += Time.unscaledDeltaTime;    // https://bloodstrawberry.tistory.com/779 실제 지나간 시간
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);   // lerp에서 t는 선형보간의 퍼센트 값으로 1이면 b값이라는 의미이다.
                if (progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    GameManager.s_instance.currentScene = (GameManager.SceneName)nextSceneIndex;
                    AudioManager.s_instance.SceneBGMContorl(GameManager.s_instance.ActiveScene, (GameManager.SceneName)nextSceneIndex);
                    yield break;
                }
            }
        }
    }
}
