using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Door door;      // �ش� ���谡 �� ��.

    private void OnTriggerEnter(Collider other)
    {
        PickUpKey();
    }
    void PickUpKey()
    {
        door.isLocked = false;
        gameObject.SetActive(false);
    }
}
