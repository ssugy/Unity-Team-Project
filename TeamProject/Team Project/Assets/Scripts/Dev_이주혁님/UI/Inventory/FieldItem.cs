using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    public Item item { get; set; }          
    public int deleteTime;

    private void Start()
    {               
        // 30�ʰ� ������ �ʵ� �������� �����.
        Destroy(gameObject, deleteTime);
    }
    
    public void DestroyItem() => Destroy(gameObject);    
}
