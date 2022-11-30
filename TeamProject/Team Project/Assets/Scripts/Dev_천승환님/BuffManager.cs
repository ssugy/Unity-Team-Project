using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public GameObject[] buffPrefabs;
    public Transform[] buffSpawnTransforms;
    
    void Start()
    {
        // 1. 버프 배열을 한번 섞고
        for (int i = 0; i < buffPrefabs.Length; i++)
        {
            int tmp = Random.Range(0, buffPrefabs.Length);
            GameObject go = buffPrefabs[i];
            buffPrefabs[i] = buffPrefabs[tmp];
            buffPrefabs[tmp] = go;
        }

        // 2. 버프생성위치도 한번 섞은뒤에
        for (int i = 0; i < buffSpawnTransforms.Length; i++)
        {
            int tmp = Random.Range(0, buffSpawnTransforms.Length);
            Transform tr = buffSpawnTransforms[i];
            buffSpawnTransforms[i] = buffSpawnTransforms[tmp];
            buffSpawnTransforms[tmp] = tr;
        }

        // 3. 버프 또는 버프생성위치 중 낮은 숫자를 기준으로  버프를 생성한다.
        int buffSpawnCnt = buffSpawnTransforms.Length > buffPrefabs.Length? buffPrefabs.Length:buffSpawnTransforms.Length;
        for (int i = 0; i < buffSpawnCnt; i++)
        {
            SpawnBuff(buffPrefabs[i], buffSpawnTransforms[i]);
        }
    }

    public void SpawnBuff(GameObject buff_Prefabs, Transform _transform)
    {
        GameObject buff = Instantiate(buff_Prefabs);
        
        buff.transform.position = _transform.position;
        buff.transform.rotation = _transform.rotation;
    }
}
