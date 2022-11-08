using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    public FieldItem(int _itemID)
    {
        itemID = _itemID;
    }
    private Item item;
    private SpriteRenderer image;
    public int itemID;          // 데이터배이스에서 불러올 아이템 ID 넘버.
    public int rotateSpeed;     // 아이템 아이콘 회전 속도.
    public int deleteTime;

    private void Start()
    {
        // 필드에 있는 아이템을 직접 가져오지 않고 현재 아이템과 같은 것을 복사하여 가져옴.
        item = ItemDatabase.s_instance.itemDB[itemID].Copy();
        image = GetComponentInChildren<SpriteRenderer>();
        SetItem(item);        
        // 30초가 지나면 필드 아이템이 사라짐.
        Destroy(gameObject, deleteTime);
    }

    public void SetItem(Item _item)
    {
        item.name = _item.name;
        item.image = _item.image;
        item.type = _item.type;
        image.sprite = item.image;
        //아이템 ID 추가
        item.SetID(itemID);
    }

    public Item GetItem()
    {
        return item;
    }
    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
