using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGold : MonoBehaviour
{
    public int ammount;
    public int rotateSpeed;     // ������ ������ ȸ�� �ӵ�.
    public int deleteTime;    
    public FieldGold(int _ammount,int _rotateSpeed = 60,int _deleteTime = 30)
    {
        ammount = _ammount;
        rotateSpeed = _rotateSpeed;
        deleteTime = _deleteTime;
    }    
    private void Start()
    {        
        // 30�ʰ� ������ �ʵ� �������� �����.
        Destroy(gameObject, deleteTime);
    }        
    private void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
