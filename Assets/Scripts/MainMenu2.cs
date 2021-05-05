using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

/* Menu.cs
// Contains callbacks for menu buttons and text boxes
// Also enables/disables menu elements as needed
// Requiered UI elements to be added in unity editor:
//      Buttons:
            Join Room
            Create Room
            Leave Room
            Start Game
            Quick Start

        TextMeshPro Input fields:
            Player Name
            Room Name

        TextMeshPro Text:
            PlayerList display

        Images:
            All Right Logo
            Player BG Box
*/


public class MainMenu2 : MonoBehaviourPunCallbacks
{
    public PhotonView photonView;

    [Header("Start")]
    public Transform startPanel;
    public TMP_InputField playerNameInput, roomNameInput;
    public Button createRoomButton, joinRoomButton, quickStartButton;
    public Image allRightLogo, playerBox;

    [Header("Room")]
    public Transform roomPanel;
    public Button leaveRoomButton;
    public Button startGameButton;
    public TextMeshProUGUI playerListText;
    int readyCount = 0;


    List<MenuPlayerDisplay> networkPlayerDisplays;


    public string GameSceneName = "Game";

    bool startOnJoin;

    void Start()
    {
        startOnJoin = false;

        createRoomButton.interactable = false;
        joinRoomButton.interactable = false;
        quickStartButton.interactable = false;
        playerNameInput.interactable = false;
        roomNameInput.interactable = false;
        
        playerListText.text = "";

        //Adding callbacks in script rather than unity editor
        createRoomButton.onClick.AddListener(OnCreateRoomButton);
        joinRoomButton.onClick.AddListener(OnJoinRoomButton);
        startGameButton.onClick.AddListener(OnStartGameButton);
        quickStartButton.onClick.AddListener(OnQuickStartButton);
        leaveRoomButton.onClick.AddListener(OnLeaveRoomButton);
        playerNameInput.onValueChanged.AddListener(delegate { OnPlayerNameChange(); });

        //get refs to player display panels and sort list
        networkPlayerDisplays = new List<MenuPlayerDisplay>(FindObjectsOfType<MenuPlayerDisplay>(true));
        networkPlayerDisplays.Sort((x,y) => string.Compare(x.gameObject.name, y.gameObject.name));

        //init player custom props
        Hashtable hash = new Hashtable();
        hash.Add("Ready", "false");
        hash.Add("Color", "green");
        PhotonNetwork.SetPlayerCustomProperties(hash);
    }

    public override void OnConnectedToMaster()
    {
        createRoomButton.interactable = true;
        joinRoomButton.interactable = true;
        quickStartButton.interactable = true;
        playerNameInput.interactable = true;
        roomNameInput.interactable = true;
    }

    public void OnPlayerNameChange()
    {
        PhotonNetwork.NickName = playerNameInput.text;
    }

    public override void OnCreatedRoom()
    {
        //start game now if quickstart
        if (startOnJoin) OnStartGameButton();
    }

    public override void OnJoinedRoom()
    {
        roomPanel.gameObject.SetActive(true);
        startPanel.gameObject.SetActive(false);

        photonView.RPC("UpdateRoomUI", RpcTarget.All);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        int roomNumber = Random.Range(0, 100);
        NetworkManager.instance.CreateRoom("Room " + roomNumber.ToString());
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        playerListText.text = message;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateReadyCount();
        UpdateRoomUI();
    }

    public override void OnPlayerEnteredRoom(Player otherPlayer)
    {
        UpdateReadyCount();
        UpdateRoomUI();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        UpdateReadyCount();
        UpdateRoomUI();
    }

    [PunRPC]
    public void UpdateRoomUI()
    {
        SetDisplayTargets();
        playerListText.text = "Room:\n" + PhotonNetwork.CurrentRoom.Name + "\n\n";
        playerListText.text += "Ready: (" + readyCount + "/" + PhotonNetwork.CurrentRoom.PlayerCount + ")";

        /*foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            //Debug.Log(player.Value.NickName + player.Value.CustomProperties["Ready"]);
            playerListText.text += player.Key + player.Value.NickName;
            if((string) player.Value.CustomProperties["Ready"] == "true")
            {
                playerListText.text += " Ready";
            }
            else playerListText.text += " Not Ready";

            playerListText.text += "\n";
        }*/

        if (PhotonNetwork.IsMasterClient && readyCount == PhotonNetwork.CurrentRoom.PlayerCount)
            startGameButton.interactable = true;
        else
            startGameButton.interactable = false;

        if(PhotonNetwork.CurrentRoom.Players.Count != 0)
        {
            foreach (MenuPlayerDisplay display in networkPlayerDisplays) display.UpdateUI();
        }
        
    }

    public void OnCreateRoomButton()
    {
        SetRandomPlayerName();
        NetworkManager.instance.CreateRoom(roomNameInput.text);
    }

    public void OnJoinRoomButton()
    {
        if (roomNameInput.text == "") playerListText.text = "Room Name cannot be blank";
        else
        {
            SetRandomPlayerName();
            NetworkManager.instance.JoinRoom(roomNameInput.text);
        }
    }

    public void OnLeaveRoomButton()
    {
        PhotonNetwork.LeaveRoom();
        playerListText.text = "";
        roomPanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
    }

    public void OnStartGameButton()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, GameSceneName);
    }

    public void OnQuickStartButton()
    {
        int roomNumber = Random.Range(0, 100);
        SetRandomPlayerName();
        startOnJoin = true;
        NetworkManager.instance.CreateRoom("Room " + roomNumber.ToString());
    }

    public void OnJoinRandomButton()
    {
        NetworkManager.instance.JoinRandomRoom();
    }

    void SetRandomPlayerName()
    {
        if (playerNameInput.text == "")
        {
            int playerNumber = Random.Range(0, 100);
            PhotonNetwork.NickName = "Player " + playerNumber.ToString();
        }
    }

    //Sets displays to target a network player; any extra displays are set to -1
    void SetDisplayTargets()
    {
        //find valid player IDs
        Dictionary<int, Player>.KeyCollection.Enumerator validIDs = PhotonNetwork.CurrentRoom.Players.Keys.GetEnumerator();

        foreach (MenuPlayerDisplay display in networkPlayerDisplays)
        {
            if(validIDs.MoveNext() == true)
            {
                Player player = PhotonNetwork.CurrentRoom.Players[validIDs.Current];

                //if local move next
                //else set id and then move next

                if (player.IsLocal)
                {
                    if (validIDs.MoveNext()) player = PhotonNetwork.CurrentRoom.Players[validIDs.Current];
                    else { display.SetTarget(-1); continue; }
                }
                display.SetTarget(validIDs.Current);
            }
            else display.SetTarget(-1);
        }
    }
    
    void UpdateReadyCount()
    {
        readyCount = 0;
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if ((string)player.Value.CustomProperties["Ready"] == "true")
            {
                readyCount++;
            }
        }
    }
}
