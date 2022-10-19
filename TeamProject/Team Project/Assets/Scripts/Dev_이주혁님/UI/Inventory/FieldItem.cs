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
    public int itemID;          // �����͹��̽����� �ҷ��� ������ ID �ѹ�.
    private int rotateSpeed;     // ������ ������ ȸ�� �ӵ�.

    private void Start()
    {
        // �ʵ忡 �ִ� �������� ���� �������� �ʰ� ���� �����۰� ���� ���� �����Ͽ� ������.
        item = ItemDatabase.instance.itemDB[itemID].Copy();
        image = GetComponentInChildren<SpriteRenderer>();
        SetItem(item);
        rotateSpeed = 60;
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
    private void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
