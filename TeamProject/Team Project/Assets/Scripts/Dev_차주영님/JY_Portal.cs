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
        // 포탈에 접촉한 게 자신일 때만 UI를 활성화. 혹은 마을에서는 플레이어가 자신 하나뿐이므로 패널 활성화.
        if(GameManager.s_instance.currentScene.Equals(GameManager.SceneName.World) || other.GetComponent<Player>().photonView.IsMine )
            portalPanel.gameObject.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (GameManager.s_instance.currentScene.Equals(GameManager.SceneName.World) || other.GetComponent<Player>().photonView.IsMine)
            portalPanel.gameObject.SetActive(false);
    }
}
