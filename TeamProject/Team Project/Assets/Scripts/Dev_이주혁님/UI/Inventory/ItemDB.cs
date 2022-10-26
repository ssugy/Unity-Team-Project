using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="DB/Item")]
public class ItemDB : ScriptableObject
{
    public static ItemDB instance;
    public List<Item> itemList;
    private void Awake()
    {
        instance = this;
    }    
}
