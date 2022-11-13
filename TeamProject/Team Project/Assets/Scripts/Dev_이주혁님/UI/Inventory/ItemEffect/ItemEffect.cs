using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    public abstract void ExecuteRole(Item _item);
    public new virtual int GetType()
    {
        return 0;
    }
}
