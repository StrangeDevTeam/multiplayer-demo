using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby; // the instance of this class

    public GameObject battleButton; // the "play" button
    public GameObject cancelButton; // the "cancel" button
    public Text infoText; // text

    private void Awake ()
    {
        lobby = this;
    }

    // connect to master photon server
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); 
    }
    //once connected, enable battle button
    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to photon master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        battleButton.SetActive(true);
    }
    //when battle button clicked, join a random room
    public void onBattleButtonClick()
    {
        Debug.Log("Battle button was clicked");
        battleButton.SetActive(false);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }
    // if no rooms can be found then create one
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("tried to join a random game but failed. there must be no open games available");
        CreateRoom();
    }
    // creates a room visible to other players, open for anyone to join, and witha max player count of 10
    void CreateRoom()
    {
        Debug.Log("Trying to create a new Room");
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 10 };
        PhotonNetwork.CreateRoom("Room " + randomRoomName, roomOps);
    }
    // if the room failed to create, then there must alreacy be a room of that name, create a new one with a different number
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must already be a room with the same name");
        CreateRoom();
    }

    // when cancel button is clicked, leave the lobby
    public void onCancelButtonCLick()
    {
        Debug.Log("Cancel button clicked");
        cancelButton.SetActive(false);
        battleButton.SetActive(true);

        PhotonNetwork.LeaveRoom();
    }

    // log info when a room is sucessfully joined
    public override void OnJoinedRoom()
    {
        Debug.Log("we are now in room #"+ PhotonNetwork.CurrentRoom.Name);
        infoText.text = "joined room #" + PhotonNetwork.CurrentRoom.Name;
    }

    ///##############################################
    ///                 custom functions
    ///##############################################


    public InputField RoomName;
    public Toggle isVisible;
    public Toggle isOpen;
    public InputField maxPlayers;
    public Text errorText;
    public void OnGoClicked()
    {
        int max_players;
        if (int.TryParse(maxPlayers.text,out max_players))
            CreateMyOwnRoom(RoomName.text, isVisible.isOn, isOpen.isOn, max_players);
        else
        {
            errorText.text = "Please make sure you use a number.";
        }
    }
    public void OnJoinClicked() // TODO 
    {

    }
    public void joinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }


    // creates a room visible to other players, open for anyone to join, and witha max player count of 10
    void CreateMyOwnRoom(string roomName, bool IsVisible , bool IsOpen, int MaxPlayers)
    {
        Debug.Log("Trying to create a new Room");
        RoomOptions roomOps = new RoomOptions() { IsVisible = IsVisible, IsOpen = IsOpen, MaxPlayers = (byte)MaxPlayers };
        PhotonNetwork.CreateRoom(roomName, roomOps);
    }

}
