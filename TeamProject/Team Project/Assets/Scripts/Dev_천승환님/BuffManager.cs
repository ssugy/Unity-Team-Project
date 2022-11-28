using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public GameObject[] buffPrefabs;
    public Transform[] buffSpawnTransforms;
    
    void Start()
    {
        for (int i = 0; i < buffPrefabs.Length; i++)
        {
            int tmp = Random.Range(0, buffPrefabs.Length);
            SpawnBuff(buffPrefabs[tmp], buffSpawnTransforms[i]);
        }
    }

    public void SpawnBuff(GameObject buff_Prefabs, Transform _transform)
    {
        GameObject buff = Instantiate(buff_Prefabs);
        
        buff.transform.position = _transform.position;
        buff.transform.rotation = _transform.rotation;
    }
}
