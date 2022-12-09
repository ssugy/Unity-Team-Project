using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop shop;
    public Item selected;
    public int price;

    public GameObject infoPanel;
    public Image infoIcon;
    public Text infoName;
    public Text infoType;
    public Text infoLevel;
    public Text infoExplain;

    public Button buy;
    public Text goldText;
    private void OnEnable()
    {
        goldText.text = JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        shop = this;
        buy.onClick.RemoveAllListeners();
        buy.onClick.AddListener(() => BuyItem());
    }    
    public void UpdatePanel()
    {
        infoPanel.SetActive(true);
        infoIcon.sprite = selected.image;
        infoName.text = selected.name;
        switch (selected.type)
        {
            case ItemType.EQUIPMENT:
                {
                    infoType.text = "장비";
                    break;
                }
            case ItemType.CONSUMABLE:
                {
                    infoType.text = "소비";
                    break;
                }
            case ItemType.INGREDIENTS:
                {
                    infoType.text = "재료";
                    break;
                }
        }
        infoLevel.text = "Lv. " + selected.level.ToString();
        infoExplain.text = selected.explanation;
    }

    // 구매 버튼에 할당될 메소드.
    public void BuyItem()
    {
        // 플레이어의 소지금이 아이템 가격보다 적은 지 체크. 소지금이 적다면 리턴.
        if (JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold < price)
        {
            Debug.Log("구매 실패");
            return;
        }
            

        // 아이템 추가에 성공했으면 (플레이어의 인벤토리가 가득 차지 않았다면)
        if (JY_CharacterListManager.s_instance.invenList[0].AddItem(selected, false))
        {
            Debug.Log("구매 성공");
            JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold -= price;
            goldText.text = JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold.ToString();
            return;
        }
        Debug.Log("구매 실패");
    }
}
