using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    private static ItemDatabase instance;
    public static ItemDatabase s_instance { get => instance; }
    public List<Item> itemDB;
    private void Awake()
    {
        instance ??= this;
        if (instance == this)
            DontDestroyOnLoad(gameObject);
        else
            Destroy(gameObject);
    } 
}
