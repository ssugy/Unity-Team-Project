using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_LoadScene : MonoBehaviour
{
    //�̵��� �� ��ȣ
    public int targetSceneNum;

    public void enterScene()
    {
        //��ư�� ���� ���̵�
        GameManager.s_instance.LoadScene(targetSceneNum);
    }
    
}
