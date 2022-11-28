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
        SceneManager.LoadScene(1);  // �׻� �ε������� �̵��� �� ����
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

        // �� �ε��� �����ڿ� �ڵ����� �ҷ��� ������ �̵��� ���̳Ķ�� �ǹ�.
        // �÷��̾�� ���̳� ������� �����ְ�, ���ҽ� �ε��� �ð��� �������� 90���ο��� ���߰�,
        // ������� �ð��ڿ� 100���η� �����ϰ� �մϴ�.
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
                // ����ũ �ε� - 90���� ���� 100���α����� 1�ʰ� ä��ϴ�.
                timer += Time.unscaledDeltaTime;    // https://bloodstrawberry.tistory.com/779 ���� ������ �ð�
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);   // lerp���� t�� ���������� �ۼ�Ʈ ������ 1�̸� b���̶�� �ǹ��̴�.
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
