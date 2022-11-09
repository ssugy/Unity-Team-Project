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
    private void Awake()
    {        
        instance ??= this;
        if (instance == this)
            DontDestroyOnLoad(gameObject);
        else
            Destroy(gameObject);        
    }
    #endregion

    public Room currentRoom = null;

    public void Connect() => PhotonNetwork.ConnectUsingSettings();
    public void Disconnect() => PhotonNetwork.Disconnect();

    // �ִ� �ο����� �ӽ÷� 1������ �س���.
    // �� �̸��� ������ ������ �� ��ȣ + �� ��ȣ.
    // ���� ��� FIre_Dungeon ���� �����ϸ� �� �̸��� 5_0, ���� �̸��� �ִٸ� 5_1, 5_2... ������ �̾���.
    public void MatchMaking(int _dungeonNum, byte _people, byte roomNum)
    {
        StartCoroutine(Matching(_dungeonNum, _people, roomNum));
    }
    IEnumerator Matching(int _dungeonNum, byte _people, byte roomNum)
    {
        if (_people == 1)
            // CreateRoom�̳� joinRoom�� ��ȯ bool ���� �� ���� ����/���� ���ΰ� �ƴ�. �ܼ��� �۾��� ���۵Ǿ��°��� ����Ŵ.
            PhotonNetwork.CreateRoom(_dungeonNum.ToString() + "_" + roomNum.ToString(), new RoomOptions { MaxPlayers = _people }, null);
        else
            PhotonNetwork.JoinOrCreateRoom(_dungeonNum.ToString() + "_" + roomNum.ToString(), new RoomOptions { MaxPlayers = _people }, null);

        // �� ���忡 ���������� ���� �� ��ȣ�� ������ �õ�. �濡 �����ϴ� �ð��� �����Ƿ� �����ؾ� ��.
        yield return new WaitForSeconds(1f);
        if (currentRoom == null)
            StartCoroutine(Matching(_dungeonNum, _people, ++roomNum));
    }
    

    // ���� ������ OnConnectedToMaster�� �����. ������ ������ �ǵ��ư��� ����.
    public void LeaveRoom()
    {
        currentRoom = null;
        if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
    }
    
    // �������� �濡 ������ �� ȣ��Ǵ� �Լ�.
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("������ �����: " + newPlayer.NickName);
        Debug.Log("�� �̸�: " + PhotonNetwork.CurrentRoom.Name);
        // ���� �� ���� ���� �� ����.
        if (PhotonNetwork.CurrentRoom.MaxPlayers.Equals(PhotonNetwork.CurrentRoom.PlayerCount))
        {
            // �� �̻� ����� ������ �� ���� ��.
            PhotonNetwork.CurrentRoom.IsOpen = false;
            GameManager.s_instance.LoadScene(int.Parse(PhotonNetwork.CurrentRoom.Name.Substring(0, 1)));
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
        Debug.Log("�� �̸�: " + PhotonNetwork.CurrentRoom.Name);
        // ���� �� ���� ���� �� ����.
        if (PhotonNetwork.CurrentRoom.MaxPlayers.Equals(PhotonNetwork.CurrentRoom.PlayerCount))
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            GameManager.s_instance.LoadScene(int.Parse(PhotonNetwork.CurrentRoom.Name.Substring(0, 1)));
        }
    }
    public override void OnLeftRoom()
    {        
        
    }


    // ���� ���� �κ��� ���� �ڵ带 ����. ���� -> ���� �̵����� ������� ����.
    public override void OnConnectedToMaster() 
    {                              
        if (GameManager.s_instance.currentScene.Equals(GameManager.SceneName.Lobby))
        {
            SceneManager.sceneLoaded += SetMine;
            PhotonNetwork.LocalPlayer.NickName =
                JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].name;
            Debug.Log("������ ������ �����: " + PhotonNetwork.LocalPlayer.NickName);
            GameManager.s_instance.LoadScene(4);                      
        }            
    }

    // �κ�� �ǵ��ư� �� �����.
    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.sceneLoaded -= SetMine;        
        Debug.Log(cause);
    }

    // �ڽ��� ĳ���͸� �ν��Ͻ�ȭ. PhotonNetwork.Instantiate�� �ݵ�� �� �ȿ� ���� ���� ����� �� �ִ�.
    public void SetMine(Scene scene, LoadSceneMode mode)
    {                    
        if (currentRoom != null)                    
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
        else                  
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
