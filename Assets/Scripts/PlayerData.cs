using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public static PlayerData data; // singleton
    public string Name;

    //inv
    public Inventory playerInv = new Inventory(); // create an inventory for the player
    public int equippedWeaponIndex = -1;
    public int hairID;


    public List<Quest> completedQuests;
    public List<Quest> activeQuests;


    public static List<string> hairStylePaths = new List<string>()
    {
        "Sprites/Hair/hair1",
        "Sprites/Hair/hair2",
        "Sprites/Hair/hair3",
        "Sprites/Hair/hair4"
    };


    public PlayerData()
    {
        // only used when loading data from file
    }
    /// <summary>
    /// only used when creating dummy data for corrupt playerdata
    /// </summary>
    /// <param name="playerName"></param>
    public PlayerData(string playerName)
    {
        // used when loading data for first time
        Name = playerName;

        completedQuests = new List<Quest>();
        activeQuests = new List<Quest>();
    }
    public PlayerData(string playerName, int hairIndex)
    {
        // used when loading data for first time
        Name = playerName;
        hairID = hairIndex;
        completedQuests = new List<Quest>();
        activeQuests = new List<Quest>();
    }
}
