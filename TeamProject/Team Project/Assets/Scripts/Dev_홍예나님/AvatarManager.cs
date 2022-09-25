using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarManager : MonoBehaviour
{
    static public AvatarManager instance = null;
    string[] prefabNames = new string[2]{
        "Prefabs/BaseCharacterM.prefab", "Prefabs/BaseCharacterF.prefab"
    };

    public List<GameObject> prefabs;
    private List<GameObject> avatars = new List<GameObject>();

    int[] avatarData;
    static public AvatarManager GetInstance()
    {
        if (instance == null)
        {
            instance = new AvatarManager();
        }
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            LoadAvatars();
        }

        avatarData = new int[4];
    }

    private void LoadAvatars()
    {
        // ���߿� ���Ͽ��� �ƹ�Ÿ ������ ������ �ƹ�Ÿ�� ������ ����
        // ���� prefabs�� �ƹ��� ������ ���� ���, �⺻ �ƹ�Ÿ�� �ε��Ѵ�
        foreach (var name in prefabNames)
        {
            GameObject avatar = Resources.Load<GameObject>(name);
            if (avatar != null)
            {
                prefabs.Add(avatar);
            }
        }      

        prefabs.CopyTo(avatars.ToArray());
    }

    public void SaveAvatar(GameObject avt)
    {
        avatars.Add(avt);
        // ���߿� ���Ͽ� ������
    }

    public int NumAvatars()
    {
        return avatars.Count;
    }

    public GameObject MakeAvatar(int slot)
    { 
        if (slot >= 0 && slot < avatars.Count)
        {
            return avatars[slot];
        }
        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
