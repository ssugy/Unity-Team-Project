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
        // 나중에 파일에서 아바타 정보를 가져와 아바타를 구성해 놓음
        // 만일 prefabs에 아무런 정보가 없을 경우, 기본 아바타를 로딩한다
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
        // 나중에 파일에 저장함
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
