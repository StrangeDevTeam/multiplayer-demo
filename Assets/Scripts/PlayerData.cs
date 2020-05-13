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
    public static int equippedWeaponIndex = -1;


    public List<Quest> completedQuests;
    public List<Quest> activeQuests;
    

    public PlayerData()
    {
        // only used when loading data from file
    }
    public PlayerData(string playerName)
    {
        // used when loading data for first time
        Name = playerName;
        completedQuests = new List<Quest>();
        activeQuests = new List<Quest>();
    }
}
