using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JY_Portal : MonoBehaviour
{
    public int targetSceneNum;

    public Transform player;
    public Transform portalUI;
    public Transform selectArrow;
    public Text enterText;    

    public Button solo;
    public Button doppio;
    string dungeonName;

    private void OnEnable()
    {
        //solo.onClick.AddListener(() => loadScene(1));
        //doppio.onClick.AddListener(() => loadScene(2));
    }


    public void selectDungeon(int dungeonNum)
    {
        targetSceneNum = dungeonNum;
        Vector3 tmp = selectArrow.transform.localPosition;
        tmp.x = (dungeonNum == 5) ? 268.7277f : -54f;
        selectArrow.localPosition = tmp;
        selectArrow.gameObject.SetActive(true);
        dungeonName = (dungeonNum == 5) ? "화염 던전" : "지하 던전";
        enterText.text = dungeonName + "에 입장하시겠습니까?";
        enterText.gameObject.SetActive(true);
        solo.gameObject.SetActive(true);
        doppio.gameObject.SetActive(true);
    }

    public void quitMenu()
    {
        selectArrow.gameObject.SetActive(false);
        solo.gameObject.SetActive(false);
        doppio.gameObject.SetActive(false);
        enterText.gameObject.SetActive(false);
        portalUI.gameObject.SetActive(false);
    }
    public void quitMenu2()
    {
        portalUI.gameObject.SetActive(false);
    }

    // 던전 씬 로드에만 사용. (포탈2 없앰)
    public void LoadScene_Solo()
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Portal);     
        // 매칭 UI를 활성화하고 매칭 시작.
        BattleUI.instance.matchingUI.SetActive(true);
        NetworkManager.s_instance.MatchMaking(targetSceneNum, 1,0);        
    }
    public void LoadScene_Doppio()
    {
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.Portal);
        // 매칭 UI를 활성화하고 매칭 시작.
        BattleUI.instance.matchingUI.SetActive(true);
        NetworkManager.s_instance.MatchMaking(targetSceneNum, 2,0);
    }

    private void OnTriggerEnter(Collider other)
    {
        portalUI.gameObject.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        portalUI.gameObject.SetActive(false);
    }
}
