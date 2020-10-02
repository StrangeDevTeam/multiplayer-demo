using BayatGames.SaveGameFree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCreation : MonoBehaviour
{
    private string MainMenuScreenName = "MainMenu";

    public InputField playerName;
    public Text corruption_text;
    public static bool ShowCorruptionMessage = false;
    public CharacterCreation cc;

    private void Start()
    {
        cc = GetComponentInChildren<CharacterCreation>();
        if(ShowCorruptionMessage)
        {
            corruption_text.gameObject.SetActive(true);
            ShowCorruptionMessage = false;
        }
    }

    public void onPlayClicked()
    {

        corruption_text.gameObject.SetActive(false);
        // player name validation
        // player name validation
        string name = playerName.text;
        if (!(name == ""))
        {
            PlayerData.data = new PlayerData(name, CharacterCreation.hairIndex);
            SaveGame.Save<PlayerData>(SavesManager.playerDataPath, PlayerData.data);

            SavesManager.backFromCreation = true;
            SceneManager.LoadScene(MainMenuScreenName, LoadSceneMode.Single);
        }
    }
}
