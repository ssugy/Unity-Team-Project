using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Door door;      // �ش� ���谡 �� ��.
    public GameObject message;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickUpKey();
        }        
    }
    void PickUpKey()
    {
        message.SetActive(true);
        door.isLocked = false;
        gameObject.SetActive(false);
    }
}
