using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    Transform camAxis;    
    // Start is called before the first frame update
    void Start()
    {
        camAxis = Camera.main.transform.parent;        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 tmp = transform.position;
        tmp.x = camAxis.position.x;
        tmp.y = camAxis.position.y + 50f;
        tmp.z = camAxis.position.z;
        transform.position = tmp;
        tmp = Vector3.zero;
        tmp.x = 90f;
        tmp.y = camAxis.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(tmp);
    }
}
