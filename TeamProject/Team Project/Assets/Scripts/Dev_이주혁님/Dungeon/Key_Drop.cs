using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Drop : MonoBehaviour
{
    public Door door;      // 해당 열쇠가 열 문.
    public GameObject message;

    
    public void PickUpKey()
    {
        message.SetActive(true);
        door.isLocked = false;        
    }
}
