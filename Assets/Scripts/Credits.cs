using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Credits : MonoBehaviour
{
    public GameObject endScene;
    public GameObject creditsRoll;
    public GameObject playCredits;

    public void quitGame()
    {
        creditsRoll.SetActive(false);
        endScene.SetActive(false);
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainMenu");
    }

    public void runCredits()
    {
        //change to correct canvas and start animation
        creditsRoll.SetActive(true);
        endScene.SetActive(false);
        StartCoroutine(WaitToReturn());
    }

    IEnumerator WaitToReturn()
    {
        yield return new WaitForSeconds(20);
        creditsRoll.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }

}
