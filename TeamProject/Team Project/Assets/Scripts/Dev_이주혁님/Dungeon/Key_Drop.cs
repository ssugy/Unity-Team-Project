using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Drop : MonoBehaviour
{
    public Enemy enemy;
    public Door door;      // �ش� ���谡 �� ��.
    public GameObject message;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        
    }

    public void PickUpKey()
    {
        message.SetActive(true);
        door.isLocked = false;        
        
    }
    private void Update()
    {
        if (enemy.curHealth <= 0)
        {
            PickUpKey();
        }
    }
}
