using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;

public class GarbageCollecting : MonoBehaviour
{
    #region 싱글톤 패턴
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

    // 원소가 추가될 때의 이벤트를 구독하고자 리스트 대신 ObservableCollection을 사용함.
    public ObservableCollection<GameObject> garbage;

    private void Start()
    {
        garbage = new();
        // ObservableCollection에 자식이 추가될 때 발생하는 이벤트를 구독함.
        // 자식이 추가될 때 자식의 개수를 파악하고 10개 이상이면 garbage를 Clear함.
        garbage.CollectionChanged += EmptyGarbage;
    }

    // 자식이 10개 이상일 때 자식들을 Destroy하고 garbage를 비움.
    public void EmptyGarbage(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {        
        
        if (garbage.Count < 10) return;

        for(int i=0; i < garbage.Count; i++)        
            Destroy(garbage[i]);               
        garbage.Clear();
    }    
}
