using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSceneStartup : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        InventoryMenu.singleton.UpdateInventoryUI();
        //PlayerData.data.playerInv.AddItem(ItemDatabase.SearchDatabaseByID(1));
        //PlayerData.data.playerInv.AddItem(ItemDatabase.SearchDatabaseByID(2));
        //PlayerData.data.playerInv.AddItem(ItemDatabase.SearchDatabaseByID(3));
        //PlayerData.data.playerInv.AddItem(ItemDatabase.SearchDatabaseByID(1));
        //PlayerData.data.playerInv.AddItem(ItemDatabase.SearchDatabaseByID(2));
        //PlayerData.data.playerInv.AddItem(ItemDatabase.SearchDatabaseByID(3));
        //PlayerData.data.playerInv.AddItem(ItemDatabase.SearchDatabaseByID(1));
        //PlayerData.data.playerInv.AddItem(ItemDatabase.SearchDatabaseByID(2));
        //PlayerData.data.playerInv.AddItem(ItemDatabase.SearchDatabaseByID(3));
        //PlayerData.data.playerInv.AddItem(ItemDatabase.SearchDatabaseByID(2));
        //PlayerData.data.playerInv.AddItem(ItemDatabase.SearchDatabaseByID(3));
        //PlayerData.data.playerInv.AddItem(ItemDatabase.SearchDatabaseByID(2));


    }

    private void Update()
    {
        //if user has quests already active, show them on the questhelper
        if (PlayerData.data.activeQuests.Count > 0)
        {
            UIController.ShowQuestHelper();
        }
        else // when the user has completed all theri active quests, hide the quest helper
        {
            UIController.HideQuestHelper();
        }
    }

}
