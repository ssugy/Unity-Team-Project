using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGold : MonoBehaviour
{
    public int ammount;   
    public int deleteTime;    
    public FieldGold(int _ammount,int _deleteTime = 30)
    {
        ammount = _ammount;        
        deleteTime = _deleteTime;
    }    
    private void Start()
    {        
        // 30초가 지나면 필드 아이템이 사라짐.
        Destroy(gameObject, deleteTime);
    }        
}
