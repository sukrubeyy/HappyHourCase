using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Wood : MonoBehaviour, IPickable
{
    [SerializeField] private PhotonView PV;
    public void Pick(CharacterManager characterManager)
    {
        characterManager.IncreaseWoodCount();
        PV.RPC("DestroyWood", RpcTarget.All);
    }

    [PunRPC]
    private void DestroyWood()
    {
        Destroy(gameObject);
    }
}