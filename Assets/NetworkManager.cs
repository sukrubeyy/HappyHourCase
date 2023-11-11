using System.Collections;
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
    
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void JoinLobby(string nickName)
    {
        PhotonNetwork.NickName = nickName;
        print($"Welcome {nickName}");
        PhotonNetwork.JoinLobby();
    }
    public void JoinRoom(string roomName)=>        PhotonNetwork.JoinRoom(roomName);
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }

    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }

    public override void OnRoomListUpdate(List<RoomInfo> newRoomList)
    {
        roomList = newRoomList;
        MenuManager.Instance.RefresRoomList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        MenuManager.Instance.RefreshPlayerList();
        Debug.Log($"OnPlayerEnteredRoom {newPlayer.NickName}");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        MenuManager.Instance.RefreshPlayerList();
        Debug.LogError($"OnPlayerLeftRoom {otherPlayer.NickName}");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Create Room Failed. ErrorCode : { returnCode } -- Mesage : {message}");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.RefreshPlayerList();
        //TODO: MenuManager'da PlayerListele
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log($"OnMasterClientSwitched {newMasterClient.NickName}");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    } 
}
