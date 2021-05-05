using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

//This handels updating the networkPlayerPanels in the main menu
//todo set carDisplay to color and remove color display texxt
public class MenuPlayerDisplay : MonoBehaviour
{
    TextMeshProUGUI playerNameText, isReadyText;
    Image carDisplay;
    int targetPlayer = -1;

    // Start is called before the first frame update
    void Awake()
    {
        playerNameText = transform.Find("PlayerNameText").gameObject.GetComponent<TextMeshProUGUI>();
        isReadyText = transform.Find("IsReadyText").gameObject.GetComponent<TextMeshProUGUI>();
        carDisplay = transform.Find("CarDisplay").gameObject.GetComponent<Image>();

        //hide panel on start
        transform.localScale = Vector3.zero;
    }

    public void SetTarget(int newTargetIndex)
    {
        //Debug.Log("Setting target to: " + targetPlayer.ToString());
        targetPlayer = newTargetIndex;
    }

    public void UpdateUI()
    {
        
        if (targetPlayer != -1|| targetPlayer > PhotonNetwork.CurrentRoom.PlayerCount)
        {
            transform.localScale = Vector3.one;
            Player player = PhotonNetwork.CurrentRoom.Players[targetPlayer];
            playerNameText.text = player.NickName;
            string[] colors = ((string)player.CustomProperties["Color"]).Split(' ');
            carDisplay.color = Colors.ToColor(colors[0]);
            if ((string)player.CustomProperties["Ready"] == "true")
            {
                isReadyText.text = "Ready";
            }
            else isReadyText.text = ""; //set to empty for Not Ready
        }
        else transform.localScale = Vector3.zero;
    }


}
