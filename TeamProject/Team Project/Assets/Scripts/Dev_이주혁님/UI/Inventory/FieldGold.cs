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
        // 30�ʰ� ������ �ʵ� �������� �����.
        Destroy(gameObject, deleteTime);
    }        
}
