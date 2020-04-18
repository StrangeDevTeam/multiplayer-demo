using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;

    public GameObject battleButton; // the "play" button that searches for lobbies
    public GameObject cancelButton;

    public Text infoText;

    private void Awake ()
    {
        lobby = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // connects to master photon server

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to photon master server");
        battleButton.SetActive(true);
    }

    public void onBattleButtonClick()
    {
        Debug.Log("Battle button was clicked");
        battleButton.SetActive(false);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("tried to join a random game but failed. there must be no open games available");
        CreateRoom();
    }
    void CreateRoom()
    {
        Debug.Log("Trying to create a new Room");
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 10 };
        PhotonNetwork.CreateRoom("Room " + randomRoomName, roomOps);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must already be a room with the same name");
        CreateRoom();
    }

    public void onCancelButtonCLick()
    {
        Debug.Log("Cancel button clicked");
        cancelButton.SetActive(false);
        battleButton.SetActive(true);

        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("we are now in room #"+ PhotonNetwork.CurrentRoom.Name);
        infoText.text = "joined room #" + PhotonNetwork.CurrentRoom.Name;
    }
}
