using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    public Item item { get; set; }          
    public int deleteTime;

    private void Start()
    {               
        // 30초가 지나면 필드 아이템이 사라짐.
        Destroy(gameObject, deleteTime);
    }
    
    public void DestroyItem() => Destroy(gameObject);    
}
