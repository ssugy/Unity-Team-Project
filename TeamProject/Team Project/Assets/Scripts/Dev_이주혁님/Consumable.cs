using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumabler : Item
{
    // Start is called before the first frame update
    void Start()
    {
        ableToConsume = true;
        ableToEquip = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
