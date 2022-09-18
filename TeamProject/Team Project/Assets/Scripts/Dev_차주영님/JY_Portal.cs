using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JY_Portal : MonoBehaviour
{
    public GameObject GameManager;
    public int targetSceneNum;

    public Transform portalUI;
    public Transform selectArrow;
    public Text enterText;
    public Transform enterButton;

    MeshRenderer Renderer;
    float yOffset;
    // Start is called before the first frame update
    void Awake()
    {
        Renderer = GetComponent<MeshRenderer>();
        GameManager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        //textureMove();
        enterDonjeon();
    }

    void textureMove()
    {
        yOffset += Time.deltaTime * 0.5f;
        Renderer.material.SetTextureOffset("_MainTex", new Vector2(0,yOffset));
    }

    void enterDonjeon()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;
        if(Physics.Raycast(ray, out hitinfo, Mathf.Infinity))
        {
            if (hitinfo.collider.CompareTag("Portal")&&Input.GetMouseButtonDown(0))
            {
                portalUI.gameObject.SetActive(true);
            }
        }
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
        GameManager.GetComponent<GameManager>().LoadScene(targetSceneNum);
    }
}
