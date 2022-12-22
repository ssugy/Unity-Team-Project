using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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
    // 방을 떠나면 OnConnectedToMaster가 실행됨. 마스터 서버로 되돌아가기 때문.
    public void LeaveRoom()
    {
        currentRoom = null;
        if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// 최대 인원수를 임시로 1명으로 해놓음.
    /// 방 이름은 가려는 던전의 씬 번호 + 방 번호. (5_0, 5_1....)
    /// </summary>
    /// <param name="_dungeonNum">Fire던전(5), Dark던전(6)</param>
    /// <param name="_people">최대 접속 인원</param>
    /// <param name="roomNum">방번호</param>
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
            // CreateRoom이나 joinRoom의 반환 bool 값은 방 생성 성공/실패 여부가 아님. 단순히 작업이 전송되었는가를 가리킴.
            PhotonNetwork.CreateRoom(_dungeonNum.ToString() + "_" + roomNum.ToString(), roomOptions, null);
        else
            PhotonNetwork.JoinOrCreateRoom(_dungeonNum.ToString() + "_" + roomNum.ToString(), roomOptions, null);

        // 방 입장에 실패했으면 다음 방 번호로 재입장 시도. 방에 접속하는 시간이 있으므로 유예해야 함.
        yield return new WaitForSeconds(1f);
        if (currentRoom == null)
            StartCoroutine(Matching(_dungeonNum, _people, ++roomNum));
    }

    // 자신이 방에 참가했을 때 호출되는 함수.
    public override void OnJoinedRoom()
    {
        currentRoom = PhotonNetwork.CurrentRoom;
        Debug.Log("매칭 시작");
        Debug.Log("방 이름: " + PhotonNetwork.CurrentRoom.Name);
        int index = int.Parse(PhotonNetwork.CurrentRoom.Name.Substring(0, 1));
        PhotonNetwork.LoadLevel(index);
        GameManager.s_instance.ActiveScene = (GameManager.SceneName)index;
        GameManager.s_instance.currentScene = (GameManager.SceneName)index;
        AudioManager.s_instance.SceneBGMContorl((GameManager.SceneName)index);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("방을 나감");
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("나간 사용자: " + otherPlayer.NickName);
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

    /// <summary>
    /// 캐릭터를 생성하기 위한 Instatiate함수의 경우 로비인 경우/로비가 아닌 경우를 구분해야되어서 작성한 함수.
    /// </summary>
    /// <param name="scene">캐릭터 생성하고자 하는 씬</param>
    /// <param name="mode">동기, 비동기 모드 확인</param>
    public void SetMine(Scene scene, LoadSceneMode mode)
    {
        // 로딩 씬에서는 인스턴스를 생성하지 않음. 빌드에서의 씬 Index를 통해 로딩씬을 판별.
        if (SceneManager.GetActiveScene().buildIndex == 1)
            return;

        if (currentRoom != null)                    
            #region 던전 씬에서는 방에 있으므로 PhotonNetwork.Instantiate
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
                    Debug.Log("성별 데이터 오류");
                    break;
            }
            #endregion                           
        else                  
            #region 월드 씬에서는 방에 있지 않으므로 Object.Instantiate
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
                    Debug.Log("성별 데이터 오류");
                    break;
            }
            #endregion        
    }
}
