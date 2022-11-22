using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Door : MonoBehaviourPun
{    
    public Animator doorPivotAni;         //doorPivot �ִϸ�����.
    public DoorButton doorButton;

    public bool isClose;    
    public bool isLocked;
    
    
    void Start()
    {        
        doorPivotAni = GetComponent<Animator>();             //doorPivot = �θ������Ʈ
        if (doorPivotAni == null)
        {
            doorPivotAni = GetComponentInParent<Animator>();
        }
        doorButton = DoorButton.instance;                    //�����ư�� �ν��Ͻ��� ���� � ��ũ��Ʈ������ �����Ҽ��ֵ��� ����
        isClose = true;                    
    }   
   
    private void OnTriggerEnter(Collider other)              // Ʈ���� �浹�� �̿��ؼ� � ���� ���� Ȯ��
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.s_instance.currentScene.Equals(GameManager.SceneName.World) || other.GetComponent<Player>().photonView.IsMine)
            {
                doorButton.gameObject.SetActive(true);       // doorButton gameObject Ȱ��ȭ 
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
                doorButton.gameObject.SetActive(false);               //�����ư ���ӿ�����Ʈ ��Ȱ��ȭ
                DoorButton.door = null;
            }            
        }
        
    }    
        
    // ���� ���� ������ RPC�� �����.
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
