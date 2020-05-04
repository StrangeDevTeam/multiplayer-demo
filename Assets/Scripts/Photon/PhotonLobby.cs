using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby; // the instance of this class, (singleton)

    [Header("'quickplay' inputs")]
    public GameObject offlineButton; // the "Offline Mode" button
    public GameObject QuickPlayButton; // the "quick play" button
    public GameObject cancelButton; // the "cancel" button
    public byte defaultMaxPlayers = 10; // by default, this is the max amount of players in one session

    [Header("'Create game' inputs")]
    public GameObject createButton; // the "create" button
    public InputField RoomName; // the name of the room the user wants to create
    public UnityEngine.UI.Toggle isVisible; // if the user wants the room to be visible
    public UnityEngine.UI.Toggle isOpen; // if the user wants the room to be open
    public InputField maxPlayers; // how many players the user wants to be able to join
    public Text errorText; // the error message that displays to the user if something goes wrong

    [Header("'join game' inputs")]
    public GameObject joinButton; // the "join" button
    public InputField RoomJoinName; // the name of the room the user wants to join
    public Text errorJoinText; // the error text for joining rooms

    [Header("'show games' inputs")]
    public GameObject roomListingsPanel; // the panel that shows all the sessions/rooms to the player
    public GameObject showGamesButton; // the button that shows the panel on screen
    public GameObject hideGamesButton; // the button that hides the panel off screen
    public bool FirstRun = true; // true only when the game is starting up
    public float gamesPanelOffset = 900; // the deafult position of the roomListings panel when on screen

    // singleton declaration
    private void Awake () 
    {
        lobby = this;
    }

    // connect to master photon server
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); 
    }


    //once connected, enable buttons
    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to photon master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        QuickPlayButton.SetActive(true);
        createButton.SetActive(true);
        joinButton.SetActive(true);
        PhotonNetwork.JoinLobby();
        FirstRun = true;
    }

    //when quickplay button clicked, join a random room
    public void OnQuickPlayClicked()
    {
        Debug.Log("quickplay button was clicked");
        QuickPlayButton.SetActive(false);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    // if no rooms can be found then create one
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("tried to join a random game but failed. there must be no open games available");
        CreateRoom();
    }

    // creates a room visible to other players, open for anyone to join, and with a max player count of 10
    void CreateRoom()
    {
        Debug.Log("Trying to create a new Room");
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = defaultMaxPlayers };
        PhotonNetwork.CreateRoom("Room " + randomRoomName, roomOps);
    }

    // if the room failed to create, then there must already be a room of that name, create a new one with a different number
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must already be a room with the same name");
        CreateRoom();
    }

    // when cancel button is clicked, leave the room/session
    public void onCancelButtonCLick()
    {
        Debug.Log("Cancel button clicked");
        cancelButton.SetActive(false);
        QuickPlayButton.SetActive(true);

        PhotonNetwork.LeaveRoom();
    }

    // log info when a room is sucessfully joined
    public override void OnJoinedRoom()
    {
        Debug.Log("successfully joined room '"+ PhotonNetwork.CurrentRoom.Name+"'");
    }

    ///##############################################
    ///                 custom functions
    ///##############################################

    // run when user presses "Create!" button after creating their own game
    public void OnGoClicked()
    {
        string text;
        if (maxPlayers.text == "")
        {
            text = "20"; // default 20 players
        }
        else
        {
            text = maxPlayers.text;
        }
        int max_players;
        if (int.TryParse(text,out max_players))
            CreateMyOwnRoom(RoomName.text, isVisible.isOn, isOpen.isOn, max_players);
        else
        {
            errorText.text = "Please make sure you use a number.";
        }
    }

    // creates a room to users specifications
    void CreateMyOwnRoom(string roomName, bool IsVisible , bool IsOpen, int MaxPlayers)
    {
        Debug.Log("Trying to create a new Room");
        RoomOptions roomOps = new RoomOptions() { IsVisible = IsVisible, IsOpen = IsOpen, MaxPlayers = (byte)MaxPlayers };
        PhotonNetwork.CreateRoom(roomName, roomOps);
    }

    // run when the user presses the "Join!" button after typing a room name to join
    public void OnJoinClicked()
    {
        if (RoomJoinName.text == "")
        {
            errorJoinText.text = "enter Room name";
        }
        else
        {
            joinRoom(RoomJoinName.text);
        }
    }

    // used to join a room/session with a specific name
    public void joinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    // run when there is an error in normal matchmaking (not when there is an error with quickplay)
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
            Debug.Log("user tried to join room that does not exist");
            errorJoinText.text = "Room does not exist!";
    }

    // run when the "show joinable games" button is clicked
    public void onShowGamesClicked()
    {
        FirstRun = false;
        showGamesButton.SetActive(false);
        roomListingsPanel.transform.position = new Vector2(roomListingsPanel.transform.position.x, (roomListingsPanel.transform.position.y + gamesPanelOffset));
    }

    // run when the "hide joinable games" is clicked
    public void onHideGamesClicked()
    {
        showGamesButton.SetActive(true);
        roomListingsPanel.transform.position = new Vector2(roomListingsPanel.transform.position.x, (roomListingsPanel.transform.position.y - gamesPanelOffset)); // see message below

        /* 
         * due to how Photon  and unity work, i can not disable the RoomListings
         * if i do then sessions will not be correctly added to the menu
         * so as an alternative to disabling it, i move it really far off the screen so no player should ever see it
         */
    }


}
