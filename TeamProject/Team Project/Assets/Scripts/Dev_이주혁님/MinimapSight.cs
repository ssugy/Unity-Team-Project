using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapSight : MonoBehaviour
{
    public Transform player;
    

    // Update is called once per frame
    void Update()
    {
        Vector3 tmp = new Vector3(0, player.rotation.eulerAngles.y, 0);
        transform.localRotation = Quaternion.Euler(tmp);
    }
}
