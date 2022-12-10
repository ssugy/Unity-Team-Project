using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JY_Map : MonoBehaviour
{
    public Text MapName;
    /*[Header("미니맵 UI")]
    public GameObject Map_WORLD;
    public GameObject Map_FIRE_DUNGEON;
    public GameObject Map_UNDERGROUND_DUNGEON;*/
    [Header("맵 확대 UI")]
    public GameObject Map_Exapnsion_UI;
    public Image minimapPlayerImage;
    public Image minimapImage;
    [Header("플레이어 연동 오브젝트")]
    public Transform Top;
    public Transform Bottom;
    public Transform Right;
    public Transform Left;
    public bool TestOnOff;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        MapInitiate();
        player = JY_CharacterListManager.s_instance.playerList[0];
    }

    void MapInitiate()
    {
        if(SceneManager.GetActiveScene().name.Equals("05. World"))
        {
            //Map_WORLD.SetActive(true);
            MapName.text = "마을";
        }
        else if (SceneManager.GetActiveScene().name.Equals("NewDungeon"))
        {
            //Map_UNDERGROUND_DUNGEON.SetActive(true);
            MapName.text = "지하 던전";
        }
        else if (SceneManager.GetActiveScene().name.Equals("06. Dungeon_Fire"))
        {
            //Map_FIRE_DUNGEON.SetActive(true);
            MapName.text = "화염 던전";
        }
    }

    public void MapExpansion()
    {
        Map_Exapnsion_UI.SetActive(true);
        minimapImage.gameObject.SetActive(true);
    }
    private void Update()
    {
        if (Map_Exapnsion_UI.activeSelf)
            Player_Map_Connect();

    }
    void Player_Map_Connect()
    {
        if (player != null && TestOnOff)
        {
            Vector2 mapArea = new Vector2(Vector3.Distance(Left.position, Right.position), Vector3.Distance(Bottom.position, Top.position));
            Vector2 charPos = new Vector2(Vector3.Distance(Left.position, new Vector3(player.transform.position.x,0f,0f)),
                                          Vector3.Distance(Bottom.position, new Vector3(0f,0f,player.transform.position.z)) );

            Vector2 normalPos = new Vector2(charPos.x / mapArea.x, charPos.y / mapArea.y);

            minimapPlayerImage.rectTransform.anchoredPosition = new Vector2(minimapImage.rectTransform.sizeDelta.x * normalPos.x, minimapImage.rectTransform.sizeDelta.y * normalPos.y);
        }
    }
}
