using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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

    public void Connect() => PhotonNetwork.ConnectUsingSettings();
    public void Disconnect() => PhotonNetwork.Disconnect();
    // �ִ� �ο����� �ӽ÷� 1������ �س���.
    public void MatchMaking(int _dungeonNum) => PhotonNetwork.JoinOrCreateRoom(_dungeonNum.ToString(), new RoomOptions { MaxPlayers = 1 }, null);  
    
    // ���� ������ OnConnectedToMaster�� �����. ������ ������ �ǵ��ư��� ����.
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();
    
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
        Debug.Log("��Ī ����");
        // ���� �� ���� ���� �� ����.
        if (PhotonNetwork.CurrentRoom.MaxPlayers.Equals(PhotonNetwork.CurrentRoom.PlayerCount))
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            GameManager.s_instance.LoadScene(int.Parse(PhotonNetwork.CurrentRoom.Name));
        }
    }



    public override void OnConnectedToMaster() 
    {
        // ���� ���� �κ��� ���� ��������� ����.
        if (GameManager.s_instance.currentScene.Equals(GameManager.SceneName.Lobby))
        {
            GameManager.s_instance.LoadScene(4);
            PhotonNetwork.LocalPlayer.NickName =
                JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].name;
            Debug.Log("������ ������ �����: " + PhotonNetwork.LocalPlayer.NickName);
        }                  
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(cause);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
