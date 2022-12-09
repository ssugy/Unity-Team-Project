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
                    infoType.text = "���";
                    break;
                }
            case ItemType.CONSUMABLE:
                {
                    infoType.text = "�Һ�";
                    break;
                }
            case ItemType.INGREDIENTS:
                {
                    infoType.text = "���";
                    break;
                }
        }
        infoLevel.text = "Lv. " + selected.level.ToString();
        infoExplain.text = selected.explanation;
    }

    // ���� ��ư�� �Ҵ�� �޼ҵ�.
    public void BuyItem()
    {
        // �÷��̾��� �������� ������ ���ݺ��� ���� �� üũ. �������� ���ٸ� ����.
        if (JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold < price)
        {
            Debug.Log("���� ����");
            return;
        }
            

        // ������ �߰��� ���������� (�÷��̾��� �κ��丮�� ���� ���� �ʾҴٸ�)
        if (JY_CharacterListManager.s_instance.invenList[0].AddItem(selected, false))
        {
            Debug.Log("���� ����");
            JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold -= price;
            goldText.text = JY_CharacterListManager.s_instance.playerList[0].playerStat.Gold.ToString();
            return;
        }
        Debug.Log("���� ����");
    }
}
