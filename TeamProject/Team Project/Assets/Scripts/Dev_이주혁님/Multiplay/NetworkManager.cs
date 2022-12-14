using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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
    // ���� ������ OnConnectedToMaster�� �����. ������ ������ �ǵ��ư��� ����.
    public void LeaveRoom()
    {
        currentRoom = null;
        if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
    }

    // �ִ� �ο����� �ӽ÷� 1������ �س���.
    // �� �̸��� ������ ������ �� ��ȣ + �� ��ȣ.
    // ���� ��� FIre_Dungeon ���� �����ϸ� �� �̸��� 5_0, ���� �̸��� �ִٸ� 5_1, 5_2... ������ �̾���.
    public void MatchMaking(int _dungeonNum, byte _people, byte roomNum)
    {
        StartCoroutine(Matching(_dungeonNum, _people, roomNum));
    }
    IEnumerator Matching(int _dungeonNum, byte _people, byte roomNum)
    {        
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = _people;        

        if (_people == 1)
            // CreateRoom�̳� joinRoom�� ��ȯ bool ���� �� ���� ����/���� ���ΰ� �ƴ�. �ܼ��� �۾��� ���۵Ǿ��°��� ����Ŵ.
            PhotonNetwork.CreateRoom(_dungeonNum.ToString() + "_" + roomNum.ToString(), roomOptions, null);
        else
            PhotonNetwork.JoinOrCreateRoom(_dungeonNum.ToString() + "_" + roomNum.ToString(), roomOptions, null);

        // �� ���忡 ���������� ���� �� ��ȣ�� ������ �õ�. �濡 �����ϴ� �ð��� �����Ƿ� �����ؾ� ��.
        yield return new WaitForSeconds(1f);
        if (currentRoom == null)
            StartCoroutine(Matching(_dungeonNum, _people, ++roomNum));
    }

    // �ڽ��� �濡 �������� �� ȣ��Ǵ� �Լ�.
    public override void OnJoinedRoom()
    {
        currentRoom = PhotonNetwork.CurrentRoom;
        Debug.Log("��Ī ����");
        Debug.Log("�� �̸�: " + PhotonNetwork.CurrentRoom.Name);
        int index = int.Parse(PhotonNetwork.CurrentRoom.Name.Substring(0, 1));
        PhotonNetwork.LoadLevel(index);
        GameManager.s_instance.ActiveScene = (GameManager.SceneName)index;
        GameManager.s_instance.currentScene = (GameManager.SceneName)index;
        AudioManager.s_instance.SceneBGMContorl((GameManager.SceneName)index);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("���� ����");
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("���� �����: " + otherPlayer.NickName);
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
        // �ε� �������� �ν��Ͻ��� �������� ����. ���忡���� �� Index�� ���� �ε����� �Ǻ�.
        if (SceneManager.GetActiveScene().buildIndex == 1)
            return;

        if (currentRoom != null)                    
            #region ���� �������� �濡 �����Ƿ� PhotonNetwork.Instantiate
            switch (JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].gender)
            {
                case EGender.MALE:
                    {
                        GameObject mine = PhotonNetwork.Instantiate("Prefabs/BaseCharacterM", Vector3.zero, Quaternion.identity);
                        mine.name = PhotonNetwork.LocalPlayer.NickName;
                        JY_CharacterListManager.s_instance.playerList.Insert(0, mine.GetComponent<Player>());
                        JY_CharacterListManager.s_instance.invenList.Insert(0, mine.GetComponent<Inventory>());

                    }                    
                    break;
                case EGender.FEMALE:
                    {
                        GameObject mine = PhotonNetwork.Instantiate("Prefabs/BaseCharacterF", Vector3.zero, Quaternion.identity);
                        mine.name = PhotonNetwork.LocalPlayer.NickName;
                        JY_CharacterListManager.s_instance.playerList.Insert(0, mine.GetComponent<Player>());
                        JY_CharacterListManager.s_instance.invenList.Insert(0, mine.GetComponent<Inventory>());
                    }
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
                    {
                        GameObject mine = Instantiate(Resources.Load<GameObject>("Prefabs/BaseCharacterM"));
                        mine.name = PhotonNetwork.LocalPlayer.NickName;
                        JY_CharacterListManager.s_instance.playerList.Insert(0, mine.GetComponent<Player>());
                        JY_CharacterListManager.s_instance.invenList.Insert(0, mine.GetComponent<Inventory>());
                    }
                    break;
                case EGender.FEMALE:
                    {
                        GameObject mine = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/BaseCharacterF"));
                        mine.name = PhotonNetwork.LocalPlayer.NickName;
                        JY_CharacterListManager.s_instance.playerList.Insert(0, mine.GetComponent<Player>());
                        JY_CharacterListManager.s_instance.invenList.Insert(0, mine.GetComponent<Inventory>());
                    }
                    break;
                default:
                    Debug.Log("���� ������ ����");
                    break;
            }
            #endregion        
    }
}
