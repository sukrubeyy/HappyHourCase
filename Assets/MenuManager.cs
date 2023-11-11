using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class MenuManager : Singleton<MenuManager>
{
    [Header("Input Fields")] public TMP_InputField nickNameInput;
    public TMP_InputField roomNameInput;

    [Header("Buttons")] public Button loginLobbyButton;
    public Button createRoomMenuButton;
    public Button createRoomButton;
    public Button findRoomMenuButton;
    public Button startButton;
    public Button backButtonForFindRoom;
    public Button backButtonForCreateRoom;
    public Button backButtonForJoinedRoom;

    [Header("Menus")] public GameObject loginMenu;
    public GameObject mainMenu;
    public GameObject createRoomMenu;
    public GameObject findRoomMenu;
    public GameObject joinedRoomMenu;

    public Transform menuPanel;

    [Header("Prefabs")] public GameObject roomListItemPrefab;
    public GameObject playerListItemPrefab;

    [Header("Content")] public Transform roomListContent;
    public Transform playerListContent;

    private void Start()
    {
        loginLobbyButton.onClick.AddListener(() =>
        {
            if (!string.IsNullOrEmpty(nickNameInput.text))
            {
                NetworkManager.Instance.JoinLobby(nickNameInput.text);
                OpenMenu(1);
            }
        });

        createRoomButton.onClick.AddListener(() =>
        {
            if (!string.IsNullOrEmpty(roomNameInput.text))
            {
                NetworkManager.Instance.CreateRoom(roomNameInput.text);
                OpenMenu(4);
            }
        });

        startButton.onClick.AddListener(() =>
        {
            //TODO: START GAME
        });

        createRoomMenuButton.onClick.AddListener(() => { OpenMenu(2); });

        findRoomMenuButton.onClick.AddListener(() => { OpenMenu(3); });

        backButtonForCreateRoom.onClick.AddListener(() => { OpenMenu(1); });
        backButtonForFindRoom.onClick.AddListener(() => { OpenMenu(1); });
        backButtonForJoinedRoom.onClick.AddListener(() =>
        {
            NetworkManager.Instance.LeaveRoom();
            OpenMenu(1);
        });
    }

    public void OpenMenu(int index)
    {
        for (int i = 0; i < menuPanel.childCount; i++)
        {
            menuPanel.GetChild(i).gameObject.SetActive(false);
        }

        menuPanel.GetChild(index).gameObject.SetActive(true);
    }

    public void RefresRoomList()
    {
        DeleteAllRoomButton();

        foreach (var room in NetworkManager.Instance.roomList)
        {
            if (room.RemovedFromList)
                continue;
            
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().Initialize(room);
        }
    }

    private void DeleteAllRoomButton()
    {
        for (int i = 0; i < roomListContent.childCount; i++)
        {
            Destroy(roomListContent.GetChild(i).gameObject);
        }
    }

    public void ChangeVisibilityStartButton(bool isMaster)
    {
        startButton.gameObject.SetActive(isMaster);
    }

    public void RefreshPlayerList()
    {
        for (int i = 0; i < playerListContent.childCount; i++)
        {
            Destroy(playerListContent.GetChild(i).gameObject);
        }

        foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Initialize(player.NickName);
        }
    }
}