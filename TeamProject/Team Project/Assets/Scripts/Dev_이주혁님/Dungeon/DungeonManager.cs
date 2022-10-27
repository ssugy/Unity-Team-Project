using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;
    public GameObject dungeonPanel;
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
            return 1f / 3f;
        else
            return 1f / 2f;
    }
}
