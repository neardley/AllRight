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

    public List<PlayerController> players;      // array of player scripts
    private int playersInGame;
    private MinimapBehavior miniMap;

    public static PlayerManager instance;

    void Awake()
    {
        instance = this;
        miniMap = FindObjectOfType<MinimapBehavior>();
        players = new List<PlayerController>(PhotonNetwork.PlayerList.Length);
    }

    void Start()
    {
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
        int id = PhotonNetwork.LocalPlayer.ActorNumber;
        if (id >= 1 && id <= spawnPoints.Length)
        {
            GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation, spawnPoints[id - 1].position, spawnPoints[id - 1].rotation, 0);
            PlayerController playerScript = playerObj.GetComponent<PlayerController>();
            playerScript.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
        }
        else Debug.Log("Photon Player ID not in spawnpoint range");
    }

    public PlayerController GetPlayer(string name)
    {
        return players.Find(x => x.photonPlayer.NickName == name);
    }

    public PlayerController GetPlayer(int id)
    {
        return players.Find(x => x.id == id);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " Has left game");
        players.Remove(GetPlayer(otherPlayer.ActorNumber));
        miniMap.FindTargets();
    }
}
