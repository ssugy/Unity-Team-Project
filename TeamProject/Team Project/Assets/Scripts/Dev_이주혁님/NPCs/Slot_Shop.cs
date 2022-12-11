using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Shop : MonoBehaviour
{
    // �Ǹ��� �������� ��ȣ.
    public int index;
    public int price;
    
    public Image icon;
    public Text nameText;
    public Text priceText;

    private Item item;
    private Button mine;    

    void Start()
    {
        item = ItemDatabase.s_instance.itemDB[index].Copy();
        icon.sprite = item.image;
        nameText.text = item.name;
        priceText.text = price.ToString();
        
        mine = GetComponent<Button>();
        mine.onClick.AddListener(() => OnClick());
    }

    // ���� Ŭ�� �� ����� �޼ҵ�.
    public void OnClick()
    {
        Shop.shop.selected = ItemDatabase.s_instance.itemDB[index].Copy();
        Shop.shop.price = price;
        Shop.shop.UpdatePanel();
    }
}
