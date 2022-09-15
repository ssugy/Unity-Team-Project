using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Transform doorPivot;
    public DoorButton doorButton;
    public bool isClose;
    // Start is called before the first frame update
    void Start()
    {
        doorPivot = transform.parent;
        doorButton = DoorButton.instance;
        isClose = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        doorButton.gameObject.SetActive(true);
        DoorButton.door = doorPivot;
    }
    private void OnTriggerExit(Collider other)
    {
        doorButton.gameObject.SetActive(false);
        DoorButton.door = null ;
    }
}
