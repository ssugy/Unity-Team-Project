using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HP_UI : MonoBehaviour
{
    public static Enemy_HP_UI instance;
    private Queue<HP_Bar> poolHP_Bar;
    public GameObject hp_Bar;
    public int poolSize;        // �ν����� â���� ���� �Է�

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
    /// ������Ʈ Ǯ���� �������� Ȯ�� �� �ش��ϴ� ������Ʈ�� ��ȯ.
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
    /// ����� ���� HP_Bar�� ȸ���ϴ� �Լ�.
    /// </summary>
    /// <param name="_bar">����� ���� HP_Bar</param>
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
