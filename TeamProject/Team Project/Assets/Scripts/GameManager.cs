using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// �⺻������ �̱��� ����. YH:220713
    /// </summary>    
    private static GameManager _unique;
    public static GameManager s_instance
    {
        get { return _unique; }
    }

    private void Awake()
    {
        _unique = this;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
