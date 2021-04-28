using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public PhotonView photonView;

    public string playerPrefabLocation;     // player prefab path in the Resources folder
    public Transform[] spawnPoints;         // array of player spawn points

    public PlayerController[] players;      // array of player scripts
    private int playersInGame;

    public static PlayerManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        players = new PlayerController[PhotonNetwork.PlayerList.Length];
        photonView.RPC("OnJoinGame", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void OnJoinGame()
    {
        playersInGame++;

        // spawn players when all the players are in the scene
        if (playersInGame == PhotonNetwork.PlayerList.Length)
            SpawnPlayer();
    }

    void SpawnPlayer()
    {
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation, spawnPoints[0].position, Quaternion.identity, 0);
        PlayerController playerScript = playerObj.GetComponent<PlayerController>();
        playerScript.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
        playerObj.transform.position = spawnPoints[playerScript.id - 1].position;
    }
}
