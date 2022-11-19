using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JY_Portal : MonoBehaviour
{        
    public Transform portalPanel;            

    private void OnTriggerEnter(Collider other)
    {
        // ��Ż�� ������ �� �ڽ��� ���� UI�� Ȱ��ȭ. Ȥ�� ���������� �÷��̾ �ڽ� �ϳ����̹Ƿ� �г� Ȱ��ȭ.
        if(GameManager.s_instance.currentScene.Equals(GameManager.SceneName.World) || other.GetComponent<Player>().photonView.IsMine )
            portalPanel.gameObject.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (GameManager.s_instance.currentScene.Equals(GameManager.SceneName.World) || other.GetComponent<Player>().photonView.IsMine)
            portalPanel.gameObject.SetActive(false);
    }
}
