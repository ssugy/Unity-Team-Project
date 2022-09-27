using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Door door;      // 해당 열쇠가 열 문.

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
