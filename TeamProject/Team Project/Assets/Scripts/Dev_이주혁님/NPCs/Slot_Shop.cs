using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Shop : MonoBehaviour
{
    // �Ǹ��� �������� ��ȣ.
    public int index;
    public int price;

    Item item;
    public Image icon;
    public Text nameText;
    public Text priceText;
       
    Button mine;

    // Start is called before the first frame update
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
        Shop.shop.selected = item.Copy();
        Shop.shop.price = price;
        Shop.shop.UpdatePanel();
    }
}
