using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Door : MonoBehaviourPun
{    
    public Animator doorPivotAni;         //doorPivot 애니메이터.
    public DoorButton doorButton;

    public bool isClose;    
    public bool isLocked;
    
    
    void Start()
    {        
        doorPivotAni = GetComponent<Animator>();             //doorPivot = 부모오브젝트
        if (doorPivotAni == null)
        {
            doorPivotAni = GetComponentInParent<Animator>();
        }
        doorButton = DoorButton.instance;                    //도어버튼을 인스턴스로 접근 어떤 스크립트에서라도 접근할수있도록 만듬
        isClose = true;                    
    }   
   
    private void OnTriggerEnter(Collider other)              // 트리거 충돌을 이용해서 어떤 문을 열지 확인
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.s_instance.currentScene.Equals(GameManager.SceneName.World) || other.GetComponent<Player>().photonView.IsMine)
            {
                doorButton.gameObject.SetActive(true);       // doorButton gameObject 활성화 
                DoorButton.door = this;
            }            
        }        
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.s_instance.currentScene.Equals(GameManager.SceneName.World) || other.GetComponent<Player>().photonView.IsMine)
            {
                doorButton.gameObject.SetActive(false);               //도어버튼 게임오브젝트 비활성화
                DoorButton.door = null;
            }            
        }
        
    }    
        
    // 문의 열고 닫힘은 RPC를 사용함.
    public void Open()
    {
        if(PhotonNetwork.InRoom)
            photonView.RPC("OpenP", RpcTarget.All);
        else        
            OpenP();
        
    }
    [PunRPC]
    public void OpenP()
    {
        isClose = false;
        doorPivotAni.SetTrigger("Interact");
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.DOOR_01, false, 1f);        
    }
    
    public void Close()
    {
        if (PhotonNetwork.InRoom)
            photonView.RPC("CloseP", RpcTarget.All);
        else
            CloseP();
    }
    [PunRPC]
    public void CloseP()
    {
        isClose = true;
        doorPivotAni.SetTrigger("Interact");
        AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.DOOR_01, false, 1f);       
    }
    

}
