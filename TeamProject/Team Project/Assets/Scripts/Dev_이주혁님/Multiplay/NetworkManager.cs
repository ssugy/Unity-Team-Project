using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region 싱글톤 패턴
    private static NetworkManager instance;
    public static NetworkManager s_instance { get => instance; }
    #endregion

    private void Awake()
    {
        #region 싱글톤 생성
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

    // 최대 인원수를 임시로 1명으로 해놓음.
    public void MatchMaking(int _dungeonNum)
    {
        PhotonNetwork.JoinOrCreateRoom(_dungeonNum.ToString(), new RoomOptions { MaxPlayers = 1 }, null);
    }

    // 방을 떠나면 OnConnectedToMaster가 실행됨. 마스터 서버로 되돌아가기 때문.
    public void LeaveRoom()
    {
        currentRoom = null;
        // LeaveRoom을 호출하여도 실제로 Room을 떠나는 것은 이보다 더 뒤. 따라서 PhotonNetwork.CurrentRoom을 호출했을 때 예상치 않은 결과가 나올 수 있음.
        PhotonNetwork.LeaveRoom();
    }
    
    // 누군가가 방에 들어왔을 때 호출되는 함수.
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("입장한 사용자: " + newPlayer.NickName);
        // 방이 꽉 차면 던전 씬 진입.
        if (PhotonNetwork.CurrentRoom.MaxPlayers.Equals(PhotonNetwork.CurrentRoom.PlayerCount))
        {
            // 더 이상 사람이 참가할 수 없게 함.
            PhotonNetwork.CurrentRoom.IsOpen = false;
            GameManager.s_instance.LoadScene(int.Parse(PhotonNetwork.CurrentRoom.Name));
        }
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("나간 사용자: " + otherPlayer.NickName);
    }
    // 자신이 방에 참가했을 때 호출되는 함수.
    public override void OnJoinedRoom()
    {
        currentRoom = PhotonNetwork.CurrentRoom;
        Debug.Log("매칭 시작");
        // 방이 꽉 차면 던전 씬 진입.
        if (PhotonNetwork.CurrentRoom.MaxPlayers.Equals(PhotonNetwork.CurrentRoom.PlayerCount))
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            GameManager.s_instance.LoadScene(int.Parse(PhotonNetwork.CurrentRoom.Name));
        }
    }
    public override void OnLeftRoom()
    {        
        // 방을 떠나면 OnConnecterToMaster가 호출되므로 SetMine이 두번 호출되는 것을 막기 위함.
        //SceneManager.sceneLoaded -= SetMine;
    }

    public override void OnConnectedToMaster() 
    {                       
        // 현재 씬이 로비일 때만 월드씬으로 진입.
        if (GameManager.s_instance.currentScene.Equals(GameManager.SceneName.Lobby))
        {
            SceneManager.sceneLoaded += SetMine;
            GameManager.s_instance.LoadScene(4);
            PhotonNetwork.LocalPlayer.NickName =
                JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].name;
            Debug.Log("서버에 접속한 사용자: " + PhotonNetwork.LocalPlayer.NickName);            
        }            
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.sceneLoaded -= SetMine;
        Debug.Log(cause);
    }

    // 자신의 캐릭터를 인스턴스화. PhotonNetwork.Instantiate는 반드시 룸 안에 있을 때만 사용할 수 있다.
    public void SetMine(Scene scene, LoadSceneMode mode)
    {                    
        if (currentRoom != null)
        {
            Debug.Log("포톤");
            #region 던전 씬에서는 방에 있으므로 PhotonNetwork.Instantiate
            switch (JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].gender)
            {
                case EGender.MALE:
                    PhotonNetwork.Instantiate("Prefabs/BaseCharacterM", Vector3.zero, Quaternion.identity);
                    break;
                case EGender.FEMALE:
                    PhotonNetwork.Instantiate("Prefabs/BaseCharacterF", Vector3.zero, Quaternion.identity);
                    break;
                default:
                    Debug.Log("성별 데이터 오류");
                    break;
            }
            #endregion
            
        }        
        else
        {
            Debug.Log("오브젝트");
            #region 월드 씬에서는 방에 있지 않으므로 Object.Instantiate
            switch (JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].gender)
            {
                case EGender.MALE:
                    Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/BaseCharacterM"));
                    break;
                case EGender.FEMALE:
                    Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/BaseCharacterF"));
                    break;
                default:
                    Debug.Log("성별 데이터 오류");
                    break;
            }
            #endregion
        }
    }
}
