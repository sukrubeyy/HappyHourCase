using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class MenuManager : Singleton<MenuManager>
{
    [Header("Input Fields")] 
    [SerializeField] private TMP_InputField nickNameInput;
    [SerializeField] private TMP_InputField roomNameInput;
    [SerializeField] private TMP_InputField maxPlayerInput;

    [Header("Buttons")] 
    [SerializeField] private Button loginLobbyButton;
    [SerializeField] private Button createRoomMenuButton;
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button findRoomMenuButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button backButtonForFindRoom;
    [SerializeField] private Button backButtonForCreateRoom;
    [SerializeField] private Button backButtonForJoinedRoom;

    [Header("Menus")] 
    [SerializeField] private GameObject loginMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject createRoomMenu;
    [SerializeField] private GameObject findRoomMenu;
    [SerializeField] private GameObject joinedRoomMenu;

    [Header("Panels")] 
    [SerializeField] private Transform menuPanel;

    [Header("Prefabs")] 
    [SerializeField] private GameObject roomListItemPrefab;
    [SerializeField] private GameObject playerListItemPrefab;

    [Header("Content")] 
    [SerializeField] private Transform roomListContent;
    [SerializeField] private Transform playerListContent;

    private void Start()
    {
        loginLobbyButton.onClick.AddListener(() =>
        {
            if (!NetworkManager.Instance.IsConnected)
            {
                Debug.LogWarning("PUN2 is not initialize. Pls wait a few seconds....!!");
                return;
            }
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
                RoomOptions roomOptions = new RoomOptions();
                if (!string.IsNullOrEmpty(maxPlayerInput.text) && int.TryParse(maxPlayerInput.text, out int maxPlayer))
                    roomOptions.MaxPlayers = maxPlayer;

                NetworkManager.Instance.CreateRoom(roomNameInput.text,roomOptions);
                OpenMenu(4);
            }
        });

        startButton.onClick.AddListener(() =>
        {
            NetworkManager.Instance.LoadScene();
            NetworkManager.Instance.ChangeVisibilityRoom(false);
        });

        createRoomMenuButton.onClick.AddListener(() =>
        {
            if (!NetworkManager.Instance.IsConnected)
            {
                Debug.LogWarning("You haven't connected to the lobby yet. PLS WAİT...!!");
                return;
            }
            OpenMenu(2);
        });

        findRoomMenuButton.onClick.AddListener(() =>
        {
            if (!NetworkManager.Instance.IsConnected)
            {
                Debug.LogWarning("You haven't connected to the lobby yet. PLS WAİT...!!");
                return;
            }
            OpenMenu(3);
        });

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
        DeleteAllRoomButtonItem();

        foreach (var room in NetworkManager.Instance.roomList)
        {
            if (room.RemovedFromList)
                continue;
            
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().Initialize(room);
        }
    }
    private void DeleteAllRoomButtonItem()
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
       
DeleteAllPlayerListItem();
        foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Initialize(player.NickName);
        }
    }

    private void DeleteAllPlayerListItem()
    {
        for (int i = 0; i < playerListContent.childCount; i++)
        {
            Destroy(playerListContent.GetChild(i).gameObject);
        }
    }
}