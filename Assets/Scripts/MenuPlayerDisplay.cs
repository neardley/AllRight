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
            carDisplay.color = ToColor(colors[0]);
            if ((string)player.CustomProperties["Ready"] == "true")
            {
                isReadyText.text = "Ready";
            }
            else isReadyText.text = ""; //set to empty for Not Ready
        }
        else transform.localScale = Vector3.zero;
    }

    Color ToColor(string name)
    {
        Color color;
        switch (name.ToLowerInvariant())
        {
            case "red":
                color = new Color(1f, 0f, 0f, 1f);
                break;
            case "blue":
                color = new Color(0f, 0f, 1f, 1f);
                break;
            case "green":
                color = new Color(0f, 1f, 0f, 1f);
                break;
            case "white":
                color = new Color(1f, 1f, 1f, 1f);
                break;
            case "black":
                color = new Color(0f, 0f, 0f, 1f);
                break;
            case "orange":
                color = new Color(1f, 0.65f, 0f, 1f);
                break;
            case "pink":
                color = new Color(1f, 0.4f, 0.7f, 1f);
                break;
            case "purple":
                color = new Color(0.5f, 0f, 0.5f, 1f);
                break;
            case "yellow":
                color = new Color(1f, 1f, 0f, 1f);
                break;
            default:
                Debug.Log("Could not parse color. Defaulting to white");
                color = new Color(1f, 1f, 1f, 1f);
                break;
        }
        return color;
    }
}
