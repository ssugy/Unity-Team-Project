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

    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void selectDungeon()
    {
        enterText.gameObject.SetActive(true);
        selectArrow.gameObject.SetActive(true);
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
