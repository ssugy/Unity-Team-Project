using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_LoadScene : MonoBehaviour
{
    //½Ì±ÛÅæÀ» ¹ŞÀ» º¯¼ö
    public GameObject GameManager;
    //ÀÌµ¿ÇÒ ¾À ¹øÈ£
    public int targetSceneNum;

    void Start()
    {
        //½Ì±ÛÅæ GameObject¸¦ ¹Ş¾Æ¿È
        GameManager = GameObject.Find("GameManager");
    }

    public void enterScene()
    {
        //¹öÆ°¿¡ µû¸¥ ¾ÀÀÌµ¿
        GameManager.GetComponent<GameManager>().LoadScene(targetSceneNum);
    }
    
}
