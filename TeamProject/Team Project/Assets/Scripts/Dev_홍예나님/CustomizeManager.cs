using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeManager : MonoBehaviour
{
    enum AppearanceDetail
    {
        FACE_STYLE,
        HAIR_STYLE,
        HAIR_COLOR,
        SKIN_COLOR,
        TOP,
        TOP_COLOR,
        LEGS,
    }

    [SerializeField] private GameObject[] torsoModels;
    [SerializeField] private Transform[] plevisAnchor; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
