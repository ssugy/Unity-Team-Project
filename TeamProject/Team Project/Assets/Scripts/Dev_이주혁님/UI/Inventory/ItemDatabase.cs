using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{    
    public static ItemDatabase instance;
    public List<Item> itemDB;
    private void Awake()
    {
        instance = this;
    }
}
