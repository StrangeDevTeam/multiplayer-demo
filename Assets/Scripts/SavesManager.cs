using BayatGames.SaveGameFree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SavesManager : MonoBehaviour
{
    private string characterCreationScreenName = "PlayerCreation";
    public static string playerDataPath = "playerdata.txt";

    public static bool SceneLoaded = false;
    public Text debugText;
    public bool devOverride = false;
    public static bool backFromCreation = false;

    // Start is called before the first frame update
    void Start()
    {
        if (devOverride && ! backFromCreation)
        {
            Debug.Log("Dev Override Triggered");
            PhotonNetwork.Disconnect();
            backFromCreation = true;
            SceneManager.LoadScene(characterCreationScreenName, LoadSceneMode.Single);
        }
        else
        {
            // if player data does not exist, generate it by making the user create a character
            if (!SaveGame.Exists(playerDataPath))
            {
                if (!backFromCreation)
                {
                    Debug.Log("no player data found");
                    PhotonNetwork.Disconnect();
                    SceneManager.LoadScene(characterCreationScreenName, LoadSceneMode.Single);
                }
                else
                {
                    Debug.Log("no data found, but player just created");
                    backFromCreation = false;
                    SceneLoaded = true;
                    PhotonNetwork.ConnectUsingSettings();
                }
            }
            // if player data is found, play the game
            else
            {
                Debug.Log("existing player data found");
                PlayerData.data = SaveGame.Load<PlayerData>(playerDataPath,new PlayerData("corrupt file"));
                SceneLoaded = true;
                // if player data is corrupted, force user to make new character
                if(PlayerData.data.Name == "corrupt file")
                {
                    PhotonNetwork.Disconnect();
                    PlayerCreation.ShowCorruptionMessage = true;
                    SceneManager.LoadScene(characterCreationScreenName, LoadSceneMode.Single);
                }
                else
                    debugText.text = "Welcome, " + PlayerData.data.Name;
            }
        }
    }

    public static void saveGame()
    {
        Debug.Log("game saved");
        SaveGame.Save<PlayerData>(SavesManager.playerDataPath, PlayerData.data);
        
    }

    private void Update()
    {
    }
}
