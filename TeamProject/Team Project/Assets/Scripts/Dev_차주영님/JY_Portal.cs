using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JY_Portal : MonoBehaviour
{
    public int targetSceneNum;

    public Transform player;
    public Transform portalUI;
    public Transform selectArrow;
    public Text enterText;
    public Transform enterButton;

    string dungeonName;
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
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
        enterButton.gameObject.SetActive(true);
    }

    public void quitMenu()
    {
        selectArrow.gameObject.SetActive(false);
        enterButton.gameObject.SetActive(false);
        enterText.gameObject.SetActive(false);
        portalUI.gameObject.SetActive(false);
    }

    public void loadScene()
    {
        GameManager.s_instance.LoadScene(targetSceneNum);
    }

    private void OnTriggerEnter(Collider other)
    {
        portalUI.gameObject.SetActive(true);
    }
}
