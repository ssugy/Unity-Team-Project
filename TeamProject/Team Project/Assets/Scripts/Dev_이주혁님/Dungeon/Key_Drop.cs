using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Drop : MonoBehaviour
{
    public Door door;      // �ش� ���谡 �� ��.
    public GameObject message;

    
    public void PickUpKey()
    {
        message.SetActive(true);
        door.isLocked = false;        
    }
}
