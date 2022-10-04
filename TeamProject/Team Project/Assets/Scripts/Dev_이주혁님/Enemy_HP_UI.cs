using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HP_UI : MonoBehaviour
{
    public static Enemy_HP_UI instance;
    private Queue<HP_Bar> poolHP_Bar = new Queue<HP_Bar>();
    public GameObject hp_Bar;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Initialize(3);
    }

    private HP_Bar CreateNewObject()
    {
        var newObj = Instantiate(hp_Bar, transform).GetComponent<HP_Bar>();
        newObj.gameObject.SetActive(false);
        return newObj;
    }
    private void Initialize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            poolHP_Bar.Enqueue(CreateNewObject());
        }
    }
    public static HP_Bar GetObject()
    {
        // 빌려줄 수 있는 오브젝트가 1개라도 있으면
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
    public static void ReturnObject(HP_Bar _bar)
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
