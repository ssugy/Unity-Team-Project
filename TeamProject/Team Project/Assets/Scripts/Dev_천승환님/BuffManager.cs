using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public GameObject[] buffPrefabs;
    public Transform[] buffSpawnTransforms;
    
    void Start()
    {
        // 1. ���� �迭�� �ѹ� ����
        for (int i = 0; i < buffPrefabs.Length; i++)
        {
            int tmp = Random.Range(0, buffPrefabs.Length);
            GameObject go = buffPrefabs[i];
            buffPrefabs[i] = buffPrefabs[tmp];
            buffPrefabs[tmp] = go;
        }

        // 2. ����������ġ�� �ѹ� �����ڿ�
        for (int i = 0; i < buffSpawnTransforms.Length; i++)
        {
            int tmp = Random.Range(0, buffSpawnTransforms.Length);
            Transform tr = buffSpawnTransforms[i];
            buffSpawnTransforms[i] = buffSpawnTransforms[tmp];
            buffSpawnTransforms[tmp] = tr;
        }

        // 3. ���� �Ǵ� ����������ġ �� ���� ���ڸ� ��������  ������ �����Ѵ�.
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
