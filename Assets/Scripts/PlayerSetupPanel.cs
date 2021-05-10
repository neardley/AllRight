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
    Transform colorButtonPanel;
    TextMeshProUGUI playerNameText, readyButtonText;
    Image carDisplay;


    // Start is called before the first frame update
    void Awake()
    {
        readyButtonText = transform.Find("ReadyButton").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        colorButtonPanel = transform.Find("ColorButtonPanel");
        playerNameText = transform.Find("PlayerNameText").gameObject.GetComponent<TextMeshProUGUI>();
        carDisplay = transform.Find("CarDisplay").gameObject.GetComponent<Image>();
    }

    override public void OnEnable()
    {
        base.OnEnable();

        playerNameText.text = PhotonNetwork.NickName;
        OnChangeColor("Red Blue");
        setReady(false);
        colorButtonPanel.gameObject.SetActive(true);
        readyButtonText.text = "Ready";
    }

    public void OnToggleReady()
    {
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
        carDisplay.color = Colors.ToColor(colors[0]);
    }

    public void OnChangeColor(string newColor)
    {
        Hashtable hash = new Hashtable();
        hash.Add("Color", newColor);
        PhotonNetwork.SetPlayerCustomProperties(hash);
    }
}
