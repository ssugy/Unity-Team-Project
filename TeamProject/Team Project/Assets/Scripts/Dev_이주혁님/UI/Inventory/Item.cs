using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    NONE,
    EQUIPMENT,
    CONSUMABLE,
    INGREDIENTS
}
public enum EquipState
{
    NONE,
    UNEQUIPED,
    EQUIPED    
}

[System.Serializable]
public class Item
{
    public Item(ItemType _type = ItemType.NONE, EquipState _state = EquipState.NONE, string _name = null, int _count = 0)
    {
        type = _type;
        equipedState = _state;
        name = _name;
        itemCount = _count;
    }

    public ItemType type = ItemType.NONE;
    public EquipState equipedState = EquipState.NONE;
    public string name = "null";
    public int itemCount;
    [TextArea(3, 5)]
    public string explanation = "null";
    public Sprite image = null;
    public List<ItemEffect> effects = null;
    private int itemID;    
    
    public void SetID(int id)
    {
        itemID = id;
    }

    public int GetID()
    {
        return itemID;
    }

    public void Use() => effects.ForEach(e => e.ExecuteRole(this));    
    public void Equip() => effects[0].ExecuteRole(this);    
    public void Unequip()
    {
        this.equipedState = EquipState.UNEQUIPED;
        effects[1].ExecuteRole(this);
    }
    public Item Copy()          // ������ DB���� ������ Ŭ������ �ҷ��� �� Shallow Copy�� ���� ���� ���� �޼ҵ�.
    {
        Item copied = new Item();
        copied.type = this.type;
        copied.equipedState = this.equipedState;
        copied.name = this.name;
        copied.explanation = this.explanation;
        copied.image = this.image;
        copied.effects = this.effects;
        return copied;
    }

    // �������� �̸� ������ ���� ���� ��, �̸����κ��� �������� ����, �̹���, ����Ʈ�� �ҷ���.
    public void ShallowCopy()
    {
        Item tmp = new Item();        
        for (int i=0;i< ItemDatabase.s_instance.itemDB.Count; i++)
        {
            if(this.name.Equals(ItemDatabase.s_instance.itemDB[i].name))
            {
                tmp = ItemDatabase.s_instance.itemDB[i];
                break;
            }
        }
        this.explanation = tmp.explanation;
        this.image = tmp.image;
        this.effects = tmp.effects;
    }
}

