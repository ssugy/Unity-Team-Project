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
    void Update()
    {
        Vector3 tmp = transform.position;
        tmp.x = camAxis.position.x;
        tmp.z = camAxis.position.z;
        transform.position = tmp;
    }
}
