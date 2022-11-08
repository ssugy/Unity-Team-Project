using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region �̱��� ����
    private static NetworkManager instance;
    public static NetworkManager s_instance { get => instance; }
    #endregion

    private void Awake()
    {
        #region �̱��� ����
        instance ??= this;
        if (instance == this)
            DontDestroyOnLoad(gameObject);
        else
            Destroy(gameObject);
        #endregion
    }
    public Room currentRoom = null;

    public void Connect() => PhotonNetwork.ConnectUsingSettings();
    public void Disconnect() => PhotonNetwork.Disconnect();

    // �ִ� �ο����� �ӽ÷� 1������ �س���.
    public void MatchMaking(int _dungeonNum)
    {
        PhotonNetwork.JoinOrCreateRoom(_dungeonNum.ToString(), new RoomOptions { MaxPlayers = 1 }, null);
    }

    // ���� ������ OnConnectedToMaster�� �����. ������ ������ �ǵ��ư��� ����.
    public void LeaveRoom()
    {
        currentRoom = null;
        // LeaveRoom�� ȣ���Ͽ��� ������ Room�� ������ ���� �̺��� �� ��. ���� PhotonNetwork.CurrentRoom�� ȣ������ �� ����ġ ���� ����� ���� �� ����.
        PhotonNetwork.LeaveRoom();
    }
    
    // �������� �濡 ������ �� ȣ��Ǵ� �Լ�.
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("������ �����: " + newPlayer.NickName);
        // ���� �� ���� ���� �� ����.
        if (PhotonNetwork.CurrentRoom.MaxPlayers.Equals(PhotonNetwork.CurrentRoom.PlayerCount))
        {
            // �� �̻� ����� ������ �� ���� ��.
            PhotonNetwork.CurrentRoom.IsOpen = false;
            GameManager.s_instance.LoadScene(int.Parse(PhotonNetwork.CurrentRoom.Name));
        }
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("���� �����: " + otherPlayer.NickName);
    }
    // �ڽ��� �濡 �������� �� ȣ��Ǵ� �Լ�.
    public override void OnJoinedRoom()
    {
        currentRoom = PhotonNetwork.CurrentRoom;
        Debug.Log("��Ī ����");
        // ���� �� ���� ���� �� ����.
        if (PhotonNetwork.CurrentRoom.MaxPlayers.Equals(PhotonNetwork.CurrentRoom.PlayerCount))
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            GameManager.s_instance.LoadScene(int.Parse(PhotonNetwork.CurrentRoom.Name));
        }
    }
    public override void OnLeftRoom()
    {        
        // ���� ������ OnConnecterToMaster�� ȣ��ǹǷ� SetMine�� �ι� ȣ��Ǵ� ���� ���� ����.
        //SceneManager.sceneLoaded -= SetMine;
    }

    public override void OnConnectedToMaster() 
    {                       
        // ���� ���� �κ��� ���� ��������� ����.
        if (GameManager.s_instance.currentScene.Equals(GameManager.SceneName.Lobby))
        {
            SceneManager.sceneLoaded += SetMine;
            GameManager.s_instance.LoadScene(4);
            PhotonNetwork.LocalPlayer.NickName =
                JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].name;
            Debug.Log("������ ������ �����: " + PhotonNetwork.LocalPlayer.NickName);            
        }            
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.sceneLoaded -= SetMine;
        Debug.Log(cause);
    }

    // �ڽ��� ĳ���͸� �ν��Ͻ�ȭ. PhotonNetwork.Instantiate�� �ݵ�� �� �ȿ� ���� ���� ����� �� �ִ�.
    public void SetMine(Scene scene, LoadSceneMode mode)
    {                    
        if (currentRoom != null)
        {
            Debug.Log("����");
            #region ���� �������� �濡 �����Ƿ� PhotonNetwork.Instantiate
            switch (JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].gender)
            {
                case EGender.MALE:
                    PhotonNetwork.Instantiate("Prefabs/BaseCharacterM", Vector3.zero, Quaternion.identity);
                    break;
                case EGender.FEMALE:
                    PhotonNetwork.Instantiate("Prefabs/BaseCharacterF", Vector3.zero, Quaternion.identity);
                    break;
                default:
                    Debug.Log("���� ������ ����");
                    break;
            }
            #endregion
            
        }        
        else
        {
            Debug.Log("������Ʈ");
            #region ���� �������� �濡 ���� �����Ƿ� Object.Instantiate
            switch (JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].gender)
            {
                case EGender.MALE:
                    Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/BaseCharacterM"));
                    break;
                case EGender.FEMALE:
                    Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/BaseCharacterF"));
                    break;
                default:
                    Debug.Log("���� ������ ����");
                    break;
            }
            #endregion
        }
    }
}
