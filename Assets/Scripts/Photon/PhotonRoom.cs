using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoom room; // the instance of this class running
    private PhotonView PV; 


    public bool isGameLoaded;
    public int currentScene;
    public int multiplayerScene;

    //player info
    private Player[] photonPlayers;
    private object playersInRoom;
    private object myNumberInRoom;


    //when joining a room, if the room instance is not yours, then get the owner's instance
    private void Awake()
    {
        if ( PhotonRoom.room == null)
        {
            room = this;
        }
        else
        {
            if ( PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
        PV = GetComponent<PhotonView>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    // runs when a room is joined
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We are now in a room");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();

        StartGame();

    }
    // if you are not the owner of the room, then load the level
    void StartGame()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            // user joined a lobby that is not their own
            return;
        }
        PhotonNetwork.LoadLevel(multiplayerScene);
    }

    // when the level is loaded, create your user's player
    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (SavesManager.SceneLoaded)
        {
            Debug.Log("######## scene loaded");
            currentScene = scene.buildIndex;
            if (currentScene == multiplayerScene)
            {
                CreatePlayer();
            }
        }
    }

    // instantiate player from prefab
    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
    }
}
