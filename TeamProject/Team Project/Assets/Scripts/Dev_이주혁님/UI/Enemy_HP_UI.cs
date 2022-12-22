using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HP_UI : MonoBehaviour
{
    public static Enemy_HP_UI instance;
    private Queue<HP_Bar> poolHP_Bar;
    public GameObject hp_Bar;
    public int poolSize;        // 인스펙터 창에서 직접 입력

    void Start()
    {
        instance = this;
        poolHP_Bar = new();
        Initialize(poolSize);
    }
    private void Initialize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            poolHP_Bar.Enqueue(CreateNewObject());
        }
    }
    private HP_Bar CreateNewObject()
    {
        var newObj = Instantiate(hp_Bar, transform).GetComponent<HP_Bar>();
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    /// <summary>
    /// 오브젝트 풀링이 가능한지 확인 후 해당하는 오브젝트를 반환.
    /// </summary>
    /// <returns></returns>
    public HP_Bar GetObject()
    {
        if (instance.poolHP_Bar.Count > 0)
        {
            var obj = instance.poolHP_Bar.Dequeue();            
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = instance.CreateNewObject();            
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    /// <summary>
    /// 사용이 끝난 HP_Bar를 회수하는 함수.
    /// </summary>
    /// <param name="_bar">사용이 끝난 HP_Bar</param>
    public void ReturnObject(HP_Bar _bar)
    {                
        _bar.gameObject.SetActive(false);
        if (instance.poolHP_Bar.Count >= 3)
        {
            Destroy(_bar.gameObject);
            return;
        }
        instance.poolHP_Bar.Enqueue(_bar);
    }
}
