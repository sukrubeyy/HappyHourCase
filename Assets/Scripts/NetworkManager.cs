using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : SingletonForPun<NetworkManager>
{
    [SerializeField] GameObject RoomManager;
    public List<RoomInfo> roomList;
    private void Start()
    {
        Debug.Log("Connecting to master");
        PhotonNetwork.ConnectUsingSettings();
        Instantiate(RoomManager);
    }

    #region PhotonMethods

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }
    
    public override void OnRoomListUpdate(List<RoomInfo> newRoomList)
    {
        roomList = newRoomList;
        MenuManager.Instance.RefresRoomList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (IsMainMenu())
             MenuManager.Instance.RefreshPlayerList();
        Debug.Log($"OnPlayerEnteredRoom {newPlayer.NickName}");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (IsMainMenu())
            MenuManager.Instance.RefreshPlayerList();
        Debug.Log($"OnPlayerLeftRoom {otherPlayer.NickName}");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Create Room Failed. ErrorCode : { returnCode } -- Mesage : {message}");
    }

    public override void OnJoinedRoom()
    {
        if (IsMainMenu())
        {
            MenuManager.Instance.RefreshPlayerList();
            MenuManager.Instance.ChangeVisibilityStartButton(PhotonNetwork.IsMasterClient);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log($"OnMasterClientSwitched {newMasterClient.NickName}");
        if(IsMainMenu())
            MenuManager.Instance.ChangeVisibilityStartButton(PhotonNetwork.IsMasterClient);
        else
            SpawnManager.Instance.SpawnWoods();
    }
    
    #endregion

    #region Custom Methods

    private bool IsMainMenu() => SceneManager.GetActiveScene().buildIndex == 0;
    public void JoinLobby(string nickName)
    {
        PhotonNetwork.NickName = nickName;
        print($"Welcome {nickName}");
        PhotonNetwork.JoinLobby();
    }
    public void JoinRoom(string roomName)=>        PhotonNetwork.JoinRoom(roomName);
  
    public void CreateRoom(string roomName,RoomOptions options)
    {
        PhotonNetwork.CreateRoom(roomName,options);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        JoinLobby(PhotonNetwork.NickName);
    }

    public void ChangeVisibilityRoom(bool _isVisible) => PhotonNetwork.CurrentRoom.IsVisible = _isVisible;
    
    public void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public bool IsInRoom => PhotonNetwork.InRoom;
    public bool IsInLobby => PhotonNetwork.InLobby;
    public bool IsConnected => PhotonNetwork.IsConnectedAndReady;

    #endregion
}
