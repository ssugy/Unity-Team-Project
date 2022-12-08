using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;
    public GameObject dungeonPanel;
    public GameObject dungeonGuide;
    public Text dungeonExplanation;
    public Image dungeonProgress;
    public float progressAmount;
    [TextArea]
    public List<string> explanationList;    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        dungeonExplanation.text = explanationList[0];
        dungeonProgress.fillAmount = 0f;
        progressAmount = progressCalc();
        dungeonPanel.SetActive(true);
    }

    private void OnDisable()
    {
        instance = null;
    }
    float progressCalc()
    {
        if (SceneManager.GetActiveScene().name == "06. Dungeon_Fire")
            return 1f / 4f;
        else
            return 1f / 2f;
    }
    public void DungeonProgress(int num)
    {
        dungeonExplanation.text = explanationList[num];
        dungeonProgress.fillAmount += progressAmount;
    }

    public void SetDungeonGuide(int num)
    {
        JY_Guide guide = dungeonGuide.GetComponent<JY_Guide>();
        switch (num)
        {
            case 1:
                guide.TARGET = guide.target_2;
                break;
            case 2:
                guide.TARGET = guide.target_3;
                break;
            case 3:
                guide.TARGET = guide.target_4;
                break;
            case 4:
                guide.TARGET = null;
                dungeonGuide.SetActive(false);
                break;
        }
    }
}
