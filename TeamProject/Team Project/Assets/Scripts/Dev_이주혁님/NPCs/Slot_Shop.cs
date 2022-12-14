using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Shop : MonoBehaviour
{          
    public Image icon;
    public Text nameText;
    public Text priceText;
    public Item item;

    private Button mine;
    private void OnEnable()
    {
        mine = GetComponent<Button>();
        mine.onClick.AddListener(() => OnClick());
    }

    // ���� Ŭ�� �� ����� �޼ҵ�.
    public void OnClick()
    {
        if (Shop.shop != null)
        {
            Shop.shop.selected = this.item;
            Shop.shop.UpdatePanel();
        }               
    }
}
