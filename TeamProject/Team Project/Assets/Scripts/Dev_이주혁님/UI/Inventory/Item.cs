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
    public ItemType type;
    public EquipState equipedState;
    public string name;
    [TextArea(3,5)]
    public string explanation;
    public Sprite image;
    public List<ItemEffect> effects;
    public int count;
    public virtual bool Use()
    {
        bool isUsed = false;
        foreach(ItemEffect one in effects)
        {
            isUsed = one.ExecuteRole();
        }        
        return isUsed;
    }   
}

public class Equipment : Item
{
    public bool isEquiped = false;

    public bool EquipItem()
    {
        if (!isEquiped)
        {
            isEquiped = true;
            return true;
        }
        isEquiped = false;
        return false;
    }
}
