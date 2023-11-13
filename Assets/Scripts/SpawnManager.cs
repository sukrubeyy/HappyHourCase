using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private Transform[] playerPoints;
    [SerializeField] private Transform[] woodPoints;
    private void Start()
    {
        if(PhotonNetwork.IsMasterClient)
            CreateWoods();
    }

    private void CreateWoods()
    {
        for (int i = 0; i < woodPoints.Length; i++)
        {
            PhotonNetwork.Instantiate(System.IO.Path.Combine("PhotonPrefabs", "Wood"), woodPoints[i].position, Quaternion.identity);
        }
    }

    public Transform GetSpawnPoint()
    {
        return playerPoints[Random.Range(0, playerPoints.Length)];
    }
}