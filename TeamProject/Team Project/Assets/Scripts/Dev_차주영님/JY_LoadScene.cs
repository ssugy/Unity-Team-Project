using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_LoadScene : MonoBehaviour
{
    //이동할 씬 번호
    public int targetSceneNum;

    public void enterScene()
    {
        //버튼에 따른 씬이동
        GameManager.s_instance.LoadScene(targetSceneNum);
    }
    
}
