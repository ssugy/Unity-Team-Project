using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Door : MonoBehaviour, IPunObservable
{    
    Animator doorPivotAni;         //doorPivot �ִϸ�����.
    DoorButton doorButton;    
    [HideInInspector] public bool isClose;

    public bool isLocked;
    
    void Start()
    {        
        doorPivotAni = GetComponent<Animator>();             //doorPivot = �θ������Ʈ
        doorButton = DoorButton.instance;                    //�����ư�� �ν��Ͻ��� ���� � ��ũ��Ʈ������ �����Ҽ��ֵ��� ����
        isClose = true;        
    }   
   
    private void OnTriggerEnter(Collider other)              // Ʈ���� �浹�� �̿��ؼ� � ���� ���� Ȯ��
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Player>().photonView.IsMine)
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
            if (other.GetComponent<Player>().photonView.IsMine)
            {
                doorButton.gameObject.SetActive(false);               //�����ư ���ӿ�����Ʈ ��Ȱ��ȭ
                DoorButton.door = null;
            }            
        }
    }
    public void Open()
    {
        if (isClose)
        {
            isClose = false;
            doorPivotAni.SetTrigger("Interact");
            AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.DOOR_01, false, 1f);
        }        
    }
    public void Close()
    {
        if (!isClose)
        {
            isClose = true;
            doorPivotAni.SetTrigger("Interact");
            AudioManager.s_instance.SoundPlay(AudioManager.SOUND_NAME.DOOR_01, false, 1f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)        
            stream.SendNext(isClose);        
        else        
            isClose = (bool)stream.ReceiveNext();                
    }
}
