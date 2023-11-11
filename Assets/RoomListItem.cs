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
            NetworkManager.Instance.JoinRoom(roomText.text);
            MenuManager.Instance.OpenMenu(4);
        });
        
        if(roomInfo.RemovedFromList)
            Destroy(gameObject);
    }

    public void Initialize(RoomInfo _roomInfo)
    {
        roomInfo = _roomInfo;
        roomText.text = roomInfo.Name;
    }
}
