using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EquipOption;

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
    // �κ��丮�� ���̺� ���Ͽ� ������ �� �����ϴ� �׸��. (������ �����ʹ� DB���� �ε���)
    public Item(ItemType _type = ItemType.NONE, EquipState _state = EquipState.NONE, 
        string _name = null, int _count = 0, EquipOption _option = null)
    {
        type = _type;
        equipedState = _state;
        name = _name;        
        itemCount = _count;        
        option = _option;
    }
    
    // ���̺� ���Ͽ� ����Ǵ� �׸��.
    public ItemType type = ItemType.NONE;
    public EquipState equipedState = EquipState.NONE;
    public string name = "null";    
    public int itemCount;
    public EquipOption option = null;

    // ���̺� ���Ͽ� ������� �ʴ� �׸��.
    public int level;
    public int price;
    [TextArea(3, 5)]
    public string explanation = "null";
    public Sprite image = null;         // ������ ������.
    public List<ItemEffect> effects = null;
       

    public void Use() => effects.ForEach(e => e.ExecuteRole(this));    
    public void Equip() => effects[0].ExecuteRole(this);    
    public void Unequip() => effects[1].ExecuteRole(this);
    
    public Item Copy()          // ������ DB���� ������ Ŭ������ �ҷ��� �� Shallow Copy�� ���� ���� ���� �޼ҵ�.
    {
        Item copied = new Item();
        copied.type = this.type;
        copied.equipedState = this.equipedState;
        copied.name = this.name;
        copied.itemCount = this.itemCount;
        copied.level = this.level;
        copied.price = this.price;
        copied.explanation = this.explanation;
        copied.image = this.image;
        copied.effects = this.effects;
        // �ɼ� ����
        copied.option = this.option;
        return copied;
    }

    // �������� �̸� ������ ���� ���� ��,
    // �̸����κ��� �������� ���� ����, ����, ����, �̹���, ����Ʈ�� �ҷ���.
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
        this.level = tmp.level;
        this.price = tmp.price;
        this.explanation = tmp.explanation;
        this.image = tmp.image;
        this.effects = tmp.effects;
    }
    public void SetOption()
    {
        if (type == ItemType.EQUIPMENT)
        {
            EquipType itemType = (EquipType)effects[0].GetType();
            option = new EquipOption(itemType);
        }
    }
}

