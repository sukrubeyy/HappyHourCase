using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text roomText;
    [SerializeField] private Button joinRoomButton;
    private RoomInfo roomInfo;

    private void Start()
    {
        joinRoomButton.onClick.AddListener(() =>
        {
             if (roomInfo.PlayerCount != roomInfo.MaxPlayers)
             {
                 NetworkManager.Instance.JoinRoom(roomText.text);
                 MenuManager.Instance.OpenMenu(4);
             }
             else
             {
                 Debug.LogError("MAX PLAYER SİZE - YOU CAN NOT JOIN THIS ROOM");
             }
        });
    }

    public void Initialize(RoomInfo _roomInfo)
    {
        roomInfo = _roomInfo;
        roomText.text = $"{roomInfo.Name} - {roomInfo.PlayerCount} / {roomInfo.MaxPlayers}";
    }
}