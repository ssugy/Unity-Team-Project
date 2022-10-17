using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;
    public GameObject dungeonPanel;
    public Text dungeonExplanation;
    [TextArea]
    public List<string> explanationList;    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        dungeonExplanation.text = explanationList[0];
        dungeonPanel.SetActive(true);
    }

    private void OnDisable()
    {
        instance = null;
    }
}
