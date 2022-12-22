using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JY_Map : MonoBehaviour
{
    public Text MapName;
    [Header("맵 확대 UI")]
    public GameObject Map_Exapnsion_UI;
    public Image minimapPlayerImage;
    public Image minimapImage;
    [Header("플레이어 연동 오브젝트")]
    public Transform Top;
    public Transform Bottom;
    public Transform Right;
    public Transform Left;
    Player player;

    void Start()
    {
        MapInitiate();
        player = JY_CharacterListManager.s_instance.playerList[0];
    }

    void MapInitiate()
    {
        if(SceneManager.GetActiveScene().name.Equals("05. World"))
            MapName.text = "마을";
        else if (SceneManager.GetActiveScene().name.Equals("NewDungeon"))
            MapName.text = "지하 던전";
        else if (SceneManager.GetActiveScene().name.Equals("06. Dungeon_Fire"))
            MapName.text = "화염 던전";
    }

    /// <summary>
    /// 미니맵을 확대하는 함수
    /// </summary>
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

    /// <summary>
    /// 미니맵을 확대 하였을 때 플레이어의 위치를 최신화하는 함수
    /// </summary>
    void Player_Map_Connect()
    {
        if (player != null)
        {
            Vector2 mapArea = new Vector2(Vector2.Distance(Left.position, Right.position), Vector2.Distance(Bottom.position, Top.position));
            Vector2 charPos = new Vector2(Mathf.Abs(Left.position.x - player.transform.position.x), Mathf.Abs(Bottom.position.z - player.transform.position.z));
            Vector2 normalPos = new Vector2(charPos.x / mapArea.x, charPos.y / mapArea.y);
            minimapPlayerImage.rectTransform.anchoredPosition = new Vector2(minimapImage.rectTransform.sizeDelta.x * normalPos.x, minimapImage.rectTransform.sizeDelta.y * normalPos.y);
        }
    }
}
