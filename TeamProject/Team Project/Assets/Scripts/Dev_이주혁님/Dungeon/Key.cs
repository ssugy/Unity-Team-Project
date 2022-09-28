using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Door door;      // 해당 열쇠가 열 문.
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
