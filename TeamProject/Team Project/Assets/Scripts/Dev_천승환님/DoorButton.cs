using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    public static DoorButton instance;
    public static Transform door;              // ¿­°í ´ÝÀ» ¹®

    private void Awake()
    {
        instance = this;
        door = null;
    }
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void DoorOpen()
    {
        door.rotation = door.rotation * Quaternion.Euler(0, 90f, 0);
        door.GetComponentInChildren<Door>().isClose = false;
    }
    public void DoorClose()
    {
        door.rotation = door.rotation * Quaternion.Euler(0, -90f, 0);
        door.GetComponentInChildren<Door>().isClose = true;
    }
    public void Door()
    {
        if (door.GetComponentInChildren<Door>().isClose)
        {
            DoorOpen();
        }
        else
        {
            DoorClose();
        }
    }
}
