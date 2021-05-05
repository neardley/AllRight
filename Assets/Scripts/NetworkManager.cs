using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/*
 * The Network manager is a singleton that acts as a wrapper 
 * around the basic photon networking functions. 
 */

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public PhotonView photonView;

    #region Singleton
    public static NetworkManager instance;

    void Awake ()
    {
        // if an instance already exists and it's not this one - destroy us
        if (instance != null && instance != this)
            Destroy(this);
        // otherwise, set the instance to this scipt
        else
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }
    #endregion Singleton

    void Start ()
    {
        // connect to the master server
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom (string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }

    public void JoinRoom (string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    [PunRPC]
    public void ChangeScene (string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
}