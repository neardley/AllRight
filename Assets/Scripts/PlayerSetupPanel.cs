using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerSetupPanel : MonoBehaviourPunCallbacks
{
    Transform readyButton, colorButtonPanel;
    TextMeshProUGUI playerNameText;
    Image carDisplay;

    // Start is called before the first frame update
    void Start()
    {
        readyButton = transform.Find("ReadyButton");
        colorButtonPanel = transform.Find("ColorButtonPanel");
        playerNameText = transform.Find("PlayerNameText").gameObject.GetComponent<TextMeshProUGUI>();
        carDisplay = transform.Find("CarDisplay").gameObject.GetComponent<Image>();

        playerNameText.text = PhotonNetwork.NickName;
        OnChangeColor("Green");
        setReady(false);
    }

    public void OnToggleReady()
    {
        TextMeshProUGUI readyButtonText = readyButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();

        if ((string)PhotonNetwork.LocalPlayer.CustomProperties["Ready"] == "true")
        {
            setReady(false);
            colorButtonPanel.gameObject.SetActive(true);
            readyButtonText.text = "Ready";
        }
        else
        {
            setReady(true);
            colorButtonPanel.gameObject.SetActive(false);
            readyButtonText.text = "Unready";
        }
    }

    void setReady(bool isReady)
    {
        Hashtable hash = new Hashtable();
        if (isReady) hash.Add("Ready", "true");
        else hash.Add("Ready", "false");
        PhotonNetwork.SetPlayerCustomProperties(hash);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        string[] colors = ((string)PhotonNetwork.LocalPlayer.CustomProperties["Color"]).Split(' ');
        carDisplay.color = ToColor(colors[0]);
    }

    public void OnChangeColor(string newColor)
    {
        Hashtable hash = new Hashtable();
        hash.Add("Color", newColor);
        PhotonNetwork.SetPlayerCustomProperties(hash);
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
