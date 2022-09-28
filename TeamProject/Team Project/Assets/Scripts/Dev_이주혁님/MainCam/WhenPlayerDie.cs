using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhenPlayerDie : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.parent.rotation = Quaternion.Lerp(Camera.main.transform.parent.rotation, Quaternion.Euler(90f, Camera.main.transform.parent.rotation.eulerAngles.y, 0), 2 * Time.deltaTime);
        Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(0, 0, -10f), Time.deltaTime);
    }
}
