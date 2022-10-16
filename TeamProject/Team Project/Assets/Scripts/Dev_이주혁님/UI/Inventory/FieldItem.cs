using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    public Item item;
    public SpriteRenderer image;
    public int num;
    private void Start()
    {
        item = ItemDatabase.instance.itemDB[num].Copy();
        SetItem(item);
    }

    public void SetItem(Item _item)
    {
        item.name = _item.name;
        item.image = _item.image;
        item.type = _item.type;
        image.sprite = item.image;
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
