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

    // 최대 인원수를 임시로 1명으로 해놓음.
    // 방 이름은 가려는 던전의 씬 번호 + 방 번호.
    // 예를 들어 FIre_Dungeon 방을 생성하면 방 이름은 5_0, 같은 이름이 있다면 5_1, 5_2... 순으로 이어짐.
    public void MatchMaking(int _dungeonNum, byte _people, byte roomNum)
    {
        StartCoroutine(Matching(_dungeonNum, _people, roomNum));
    }
    IEnumerator Matching(int _dungeonNum, byte _people, byte roomNum)
    {
        if (_people == 1)
            // CreateRoom이나 joinRoom의 반환 bool 값은 방 생성 성공/실패 여부가 아님. 단순히 작업이 전송되었는가를 가리킴.
            PhotonNetwork.CreateRoom(_dungeonNum.ToString() + "_" + roomNum.ToString(), new RoomOptions { MaxPlayers = _people }, null);
        else
            PhotonNetwork.JoinOrCreateRoom(_dungeonNum.ToString() + "_" + roomNum.ToString(), new RoomOptions { MaxPlayers = _people }, null);

        // 방 입장에 실패했으면 다음 방 번호로 재입장 시도. 방에 접속하는 시간이 있으므로 유예해야 함.
        yield return new WaitForSeconds(1f);
        if (currentRoom == null)
            StartCoroutine(Matching(_dungeonNum, _people, ++roomNum));
    }
    

    // 방을 떠나면 OnConnectedToMaster가 실행됨. 마스터 서버로 되돌아가기 때문.
    public void LeaveRoom()
    {
        currentRoom = null;
        if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
    }
    
    // 누군가가 방에 들어왔을 때 호출되는 함수.
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("입장한 사용자: " + newPlayer.NickName);
        Debug.Log("방 이름: " + PhotonNetwork.CurrentRoom.Name);
        // 방이 꽉 차면 던전 씬 진입.
        if (PhotonNetwork.CurrentRoom.MaxPlayers.Equals(PhotonNetwork.CurrentRoom.PlayerCount))
        {
            // 더 이상 사람이 참가할 수 없게 함.
            PhotonNetwork.CurrentRoom.IsOpen = false;
            GameManager.s_instance.LoadScene(int.Parse(PhotonNetwork.CurrentRoom.Name.Substring(0, 1)));
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
        Debug.Log("방 이름: " + PhotonNetwork.CurrentRoom.Name);
        // 방이 꽉 차면 던전 씬 진입.
        if (PhotonNetwork.CurrentRoom.MaxPlayers.Equals(PhotonNetwork.CurrentRoom.PlayerCount))
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            GameManager.s_instance.LoadScene(int.Parse(PhotonNetwork.CurrentRoom.Name.Substring(0, 1)));
        }
    }
    public override void OnLeftRoom()
    {        
        
    }


    // 현재 씬이 로비일 때만 코드를 실행. 던전 -> 월드 이동에선 실행되지 않음.
    public override void OnConnectedToMaster() 
    {                              
        if (GameManager.s_instance.currentScene.Equals(GameManager.SceneName.Lobby))
        {
            SceneManager.sceneLoaded += SetMine;
            PhotonNetwork.LocalPlayer.NickName =
                JY_CharacterListManager.s_instance.jInfoData.infoDataList[JY_CharacterListManager.s_instance.selectNum].name;
            Debug.Log("서버에 접속한 사용자: " + PhotonNetwork.LocalPlayer.NickName);
            GameManager.s_instance.LoadScene(4);                      
        }            
    }

    // 로비로 되돌아갈 때 실행됨.
    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.sceneLoaded -= SetMine;        
        Debug.Log(cause);
    }

    // 자신의 캐릭터를 인스턴스화. PhotonNetwork.Instantiate는 반드시 룸 안에 있을 때만 사용할 수 있다.
    public void SetMine(Scene scene, LoadSceneMode mode)
    {                    
        if (currentRoom != null)                    
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
        else                  
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
