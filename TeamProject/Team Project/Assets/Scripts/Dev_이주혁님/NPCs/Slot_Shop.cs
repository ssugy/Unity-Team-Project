using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Shop : MonoBehaviour
{
    // 판매할 아이템의 번호.
    public int index;    
    
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
        priceText.text = item.price.ToString();
        
        mine = GetComponent<Button>();
        mine.onClick.AddListener(() => OnClick());
    }

    // 슬롯 클릭 시 실행될 메소드.
    public void OnClick()
    {
        Shop.shop.selected = ItemDatabase.s_instance.itemDB[index].Copy();        
        Shop.shop.UpdatePanel();
    }
}
