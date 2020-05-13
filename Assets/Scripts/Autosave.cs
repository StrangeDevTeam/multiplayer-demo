using BayatGames.SaveGameFree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autosave : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

        void OnApplicationQuit()
        {
        SavesManager.saveGame();
            Debug.Log("Application ending after " + Time.time + " seconds");
        }
}
