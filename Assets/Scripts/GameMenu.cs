using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameMenu : MonoBehaviour
{

    public void OnExitButton()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainMenu");
    }
}
