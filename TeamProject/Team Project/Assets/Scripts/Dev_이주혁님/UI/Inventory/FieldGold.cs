using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGold : MonoBehaviour
{
    public int ammount;
    public int rotateSpeed;     // 아이템 아이콘 회전 속도.
    public int deleteTime;    
    public FieldGold(int _ammount,int _rotateSpeed = 60,int _deleteTime = 30)
    {
        ammount = _ammount;
        rotateSpeed = _rotateSpeed;
        deleteTime = _deleteTime;
    }    
    private void Start()
    {        
        // 30초가 지나면 필드 아이템이 사라짐.
        Destroy(gameObject, deleteTime);
    }        
    private void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
