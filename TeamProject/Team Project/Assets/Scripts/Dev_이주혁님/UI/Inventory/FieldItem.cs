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
    public int rotateSpeed;     // ������ ������ ȸ�� �ӵ�.
    public int deleteTime;

    private void Start()
    {
        // �ʵ忡 �ִ� �������� ���� �������� �ʰ� ���� �����۰� ���� ���� �����Ͽ� ������.
        item = ItemDatabase.s_instance.itemDB[itemID].Copy();
        image = GetComponentInChildren<SpriteRenderer>();
        SetItem(item);        
        // 30�ʰ� ������ �ʵ� �������� �����.
        Destroy(gameObject, deleteTime);
    }

    public void SetItem(Item _item)
    {
        item.name = _item.name;
        item.image = _item.image;
        item.type = _item.type;
        image.sprite = item.image;
        //������ ID �߰�
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
