using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_LoadScene : MonoBehaviour
{
    //�̱����� ���� ����
    public GameObject GameManager;
    //�̵��� �� ��ȣ
    public int targetSceneNum;

    void Start()
    {
        //�̱��� GameObject�� �޾ƿ�
        GameManager = GameObject.Find("GameManager");
    }

    public void enterScene()
    {
        //��ư�� ���� ���̵�
        GameManager.GetComponent<GameManager>().LoadScene(targetSceneNum);
    }
    
}
