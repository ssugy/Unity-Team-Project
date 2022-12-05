using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JY_Map : MonoBehaviour
{
    public Text MapName;
    [Header("�̴ϸ� UI")]
    public GameObject Map_WORLD;
    public GameObject Map_FIRE_DUNGEON;
    public GameObject Map_UNDERGROUND_DUNGEON;
    [Header("�� Ȯ�� UI")]
    public GameObject Map_Exapnsion_UI;
    public GameObject Map_WORLD_E;
    public GameObject Map_FIRE_DUNGEON_E;
    public GameObject Map_UNDERGROUND_DUNGEON_E;
    // Start is called before the first frame update
    void Start()
    {
        MapInitiate();
    }

    void MapInitiate()
    {
        if(SceneManager.GetActiveScene().name.Equals("05. World"))
        {
            Map_WORLD.SetActive(true);
            MapName.text = "����";
        }
        else if (SceneManager.GetActiveScene().name.Equals("NewDungeon"))
        {
            Map_UNDERGROUND_DUNGEON.SetActive(true);
            MapName.text = "���� ����";
        }
        else if (SceneManager.GetActiveScene().name.Equals("06. Dungeon_Fire"))
        {
            Map_FIRE_DUNGEON.SetActive(true);
            MapName.text = "ȭ�� ����";
        }
    }

    public void MapExpansion()
    {
        Map_Exapnsion_UI.SetActive(true);

        if (SceneManager.GetActiveScene().name.Equals("05. World"))
            Map_WORLD_E.SetActive(true);
        else if (SceneManager.GetActiveScene().name.Equals("NewDungeon"))
            Map_UNDERGROUND_DUNGEON_E.SetActive(true);
        else if (SceneManager.GetActiveScene().name.Equals("06. Dungeon_Fire"))
            Map_FIRE_DUNGEON_E.SetActive(true);
    }
}
