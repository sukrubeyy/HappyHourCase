using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : SingletonForPun<RoomManager>
{
    [SerializeField] private PhotonView PV;

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene sceen, LoadSceneMode loadSceneMode)
    {
        if (sceen.buildIndex == 1)
        {
            Debug.Log("CharacterManager Creat");
            PhotonNetwork.Instantiate(System.IO.Path.Combine("PhotonPrefabs", "CharacterManager"), Vector3.zero, Quaternion.identity);
        }
    }
}

