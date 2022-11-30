using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;

public class GarbageCollecting : MonoBehaviour
{
    #region �̱��� ����
    private static GarbageCollecting instance;
    public static GarbageCollecting s_instance
    {
        get 
        {
            if (instance == null)
            {
                GameObject obj = new("GarbageCollecting");
                instance = obj.AddComponent<GarbageCollecting>();
            }
           return instance;
        } 
    }
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    // ���Ұ� �߰��� ���� �̺�Ʈ�� �����ϰ��� ����Ʈ ��� ObservableCollection�� �����.
    public ObservableCollection<GameObject> garbage;

    private void Start()
    {
        garbage = new();
        // ObservableCollection�� �ڽ��� �߰��� �� �߻��ϴ� �̺�Ʈ�� ������.
        // �ڽ��� �߰��� �� �ڽ��� ������ �ľ��ϰ� 10�� �̻��̸� garbage�� Clear��.
        garbage.CollectionChanged += EmptyGarbage;
    }

    // �ڽ��� 10�� �̻��� �� �ڽĵ��� Destroy�ϰ� garbage�� ���.
    public void EmptyGarbage(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {        
        
        if (garbage.Count < 10) return;

        for(int i=0; i < garbage.Count; i++)        
            Destroy(garbage[i]);               
        garbage.Clear();
    }    
}
