using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

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

        TextMeshPro Input feilds:
            Player Name
            Room Name

        TextMeshPro Text:
            PlayerList display
*/


public class MainMenu : MonoBehaviourPunCallbacks
{
    public PhotonView photonView;

    public TextMeshProUGUI playerListText;

    public Button createRoomButton;
    public Button joinRoomButton;
    public Button leaveRoomButton;
    public Button startGameButton;
    public Button quickStartButton;

    public TMP_InputField playerNameInput;
    public TMP_InputField roomNameInput;

    public string GameSceneName = "Game";

    bool startOnJoin;

    void Start()
    {
        startOnJoin = false;
        createRoomButton.interactable = false;
        joinRoomButton.interactable = false;
        startGameButton.interactable = false;
        quickStartButton.interactable = false;
        leaveRoomButton.interactable = false;
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
        createRoomButton.interactable = false;
        joinRoomButton.interactable = false;
        quickStartButton.interactable = false;
        startGameButton.interactable = false;
        leaveRoomButton.interactable = true;
        playerNameInput.interactable = false;
        roomNameInput.interactable = false;

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
        UpdateRoomUI();
    }

    [PunRPC]
    public void UpdateRoomUI()
    {
        playerListText.text = "Room: " + PhotonNetwork.CurrentRoom.Name + "\nPlayer List:\n";

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            playerListText.text += player.Value.NickName + "\n";
        }

        if (PhotonNetwork.IsMasterClient)
            startGameButton.interactable = true;
        else
            startGameButton.interactable = false;
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
        startGameButton.interactable = false;
        leaveRoomButton.interactable = false;
        quickStartButton.interactable = true;
        createRoomButton.interactable = true;
        joinRoomButton.interactable = true;
        playerNameInput.interactable = true;
        roomNameInput.interactable = true;
    }

    public void OnStartGameButton()
    {
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
}
