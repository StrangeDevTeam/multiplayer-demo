// Copyright(c) 2020 arcturus125 & StrangeDevTeam
// Free to use and modify as you please, Not to be published, distributed, licenced or sold without permission from StrangeDevTeam
// Requests for the above to be made here: https://www.reddit.com/r/StrangeDev/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    //public static Quest ActiveQuest = null;
    public static int ID_Counter = 0; // the id of the next quest to be created

    public int ID; // each quest has a unique counter
    public bool complete = false; // becomes true when all steps of the quest are complete
    public bool startedLocal = false; // becomes true when the quest has been given to the player
    public bool TurnedIn = false; // becomes true when TurnInQuest() is run
    public string title = "quest title";
    public string info = "quest info";
    public List<QuestObjective> objectives = new List<QuestObjective>(); //the different objectives of the quest
    Item[] rewards; // the items given to the user on completion of the quest 



    public Quest()
    {
        // used in loading quests from file, NOT to be used
    }
   /// <summary>
    /// creates a Quest with a title info and a list of objectives
    /// </summary>
    /// <param name="pTitle">the title of the quest</param>
    /// <param name="pInfo">the information of the quest</param>
    /// <param name="pObjectives"> the list of QuestObjectives that are objectives for this quest</param>
    public Quest(string pTitle, string pInfo, List<QuestObjective> pObjectives)
    {
        complete = false;
        title = pTitle;
        info = pInfo;
        objectives = pObjectives;
        //set QuestObjective.ParentQuest for each objective
        foreach (QuestObjective objective in pObjectives)
        {
            objective.attachParent(this);
        }
        ID = ID_Counter;
        ID_Counter++;
    }
    /// <summary>
    /// creates a Quest with a title info and a single objective
    /// </summary>
    /// <param name="pTitle">the title of the quest</param>
    /// <param name="pInfo">the information of the quest</param>
    /// <param name="pObjectives"> the QuestObjective that is an objective for this quest</param>
    public Quest(string pTitle, string pInfo, QuestObjective pObjectives)
    {
        complete = false;
        title = pTitle;
        info = pInfo;
        List<QuestObjective> tempList = new List<QuestObjective>();
        tempList.Add(pObjectives);
        objectives = tempList;
        //set QuestObjective.ParentQuest for each objective
        foreach (QuestObjective Objective in tempList)
        {
            Objective.attachParent(this);
        }
        ID = ID_Counter;
        ID_Counter++;
    }

    public static KillQuest convertToKillQuest(QuestObjective pQuest)
    {
        try
        {
            KillQuest temp = (KillQuest)(pQuest);
            return temp;
        }
        catch(Exception)
        {
            return null;
        }
    }
    public static TalkQuest convertToTalkQuest(QuestObjective pQuest)
    {
        try
        {
            TalkQuest temp = (TalkQuest)(pQuest);
            return temp;
        }
        catch(Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// used to set a singular item as a reward to this quest
    /// </summary>
    /// <param name="pItem"> the item you want to add as a reward to the quest</param>
    public void setReward(Item pItem)
    {
        rewards = new Item[1] { pItem };
    }
    /// <summary>
    /// used to set multiple items as rewards to this quest
    /// </summary>
    /// <param name="pitems"> the items you want to add as a reward to the quest</param>
    public void setRewards(Item[] pitems)
    {
        rewards = pitems;
    }


    /// <summary>
    /// checks if all the questObjectives are complete, if so complete this quest
    /// </summary>
    public void UpdateQuestStatus()
    {
        bool isQuestComplete = true;
        foreach(QuestObjective objective in PlayerData.data.activeQuests[GetPlayerDataIndex()].objectives)
        {
            if(objective.objectiveComplete != true)
            {
                isQuestComplete = false;
                break;
            }
        }
        if (isQuestComplete)
        {
            complete = true; // sets LOCAL quest to complete :: Depreciated
            OnComplete();
        }
    }
    /// <summary>
    /// turns the quest in for rewards - can only be done once
    /// </summary>
    public void TurnInQuest()
    {
        // if quest is not already turned in, turn quest in
        if (!PlayerData.data.activeQuests[this.GetPlayerDataIndex()].TurnedIn && !this.isAlreadyCompleteAndTurnedIn())
        {
            GiveRewards();
            Debug.Log("Quest " + title + " turned in!");
            // add to players completed quest list
            PlayerData.data.completedQuests.Add(this);
            PlayerData.data.completedQuests[this.GetPlayerDataIndex()].TurnedIn = true;
            // delete from players Active quest list
            PlayerData.data.activeQuests.RemoveAt(this.GetPlayerDataIndex());
        }
        else
        {
            Debug.Log("tried to turn in a quest already turned in!!");
        }
    }
    //run when the quest is complete
    void OnComplete()
    {
        Debug.Log(title+" Completed");
        PlayerData.data.activeQuests[this.GetPlayerDataIndex()].complete = true;
    }
    // run once when the tuest is turned in
    void GiveRewards()
    {
        foreach( Item reward in rewards)
        {
            PlayerData.data.playerInv.AddItem(reward);
        }
    }
    public bool isAlreadyActive()
    {
        for (int i = 0; i < PlayerData.data.activeQuests.Count; i++)
        {
            if (PlayerData.data.activeQuests[i].ID == this.ID)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// checks if the quest has been completed and turned in
    /// </summary>
    /// <returns></returns>
    public bool isAlreadyCompleteAndTurnedIn()
    {
        for (int i = 0; i < PlayerData.data.completedQuests.Count; i++)
        {
            if (PlayerData.data.completedQuests[i].ID == this.ID)
            {
                return true;
            }
        }
        return false;
    }

    public bool isAlreadyTurnedIn()
    {
        for (int i = 0; i < PlayerData.data.activeQuests.Count; i++)
        {
            if (PlayerData.data.activeQuests[i].ID == this.ID)
            {
                if(PlayerData.data.activeQuests[i].TurnedIn)
                    return true;
            }
        }
       
        return false;
    }

    public bool isComplete()
    {
        for (int i = 0; i < PlayerData.data.activeQuests.Count; i++)
        {
            if (PlayerData.data.activeQuests[i].ID == this.ID)
            {
                if (PlayerData.data.activeQuests[i].complete)
                    return true;
            }
        }

        return false;
    }
    /// <summary>
    /// gets the index of this quest in the players activeQuest list (the quest log). returns -1 as rogue value
    /// </summary>
    /// <returns></returns>
    public int GetPlayerDataIndex()
    {
        for (int i = 0; i < PlayerData.data.activeQuests.Count; i++)
        {
            if (PlayerData.data.activeQuests[i].ID == this.ID)
            {
                return i;
            }
        }
        Debug.LogError("StrangeDev: GetPlayerDataIndex() Error");
        return -1;
    }
}
// referred to as "objectives" or "questSteps"
public class QuestObjective
{
    public Quest ParentQuestLocal = null; // the Quest that this Objective is a part of
    public bool objectiveComplete = false; // true when this objective is complete
    public string title = "task title"; // the title of the objective
    public bool showTitle = true; // whether or not the title should show on the UI
    public int objectiveID = -1;
    public static int nextID = 0;


    
    public QuestObjective(string pTitle)
    {
        title = pTitle;
        objectiveID= nextID;
        nextID++;
    }
    //run when the Quest is created, attaches the parent Quest to these objectives
    public void attachParent(Quest pParentQuest)
    {
        ParentQuestLocal = pParentQuest;
    }
}
public class KillQuest : QuestObjective
{
    public List<Enemy> targets = new List<Enemy>(); // the target or targets to be tracked for this killQuest
    public int killsNeeded = 1; // the amount of kills needed for completion
    public int amountKilled = 0; // the amount of kills the player has

    /// <summary>
    /// create a Killquest with a singular target
    /// </summary>
    /// <param name="pTitle"> the title of the KillQuest</param>
    /// <param name="pTarget"> the target to kill</param>
    /// <param name="pKillsNeeded"> the amount needed to kill</param>
    public KillQuest(string pTitle, Enemy pTarget, int pKillsNeeded) : base (pTitle)
    {
        targets.Clear();
        targets.Add(pTarget);
        killsNeeded = pKillsNeeded;
    }
    /// <summary>
    /// create a killquest with multiple targets
    /// </summary>
    /// <param name="pTitle"> the title of the KillQuest</param>
    /// <param name="pTargets"> the targets to kill</param>
    /// <param name="pKillsNeeded"> the amount needed to kill</param>
    public KillQuest(string pTitle, List<Enemy> pTargets, int pKillsNeeded) : base(pTitle)
    {
        targets = pTargets;
        killsNeeded = pKillsNeeded;
    }

    //run when a target is killed
    public void TargetKilled()
    {
        amountKilled++;
        if(amountKilled >= killsNeeded)
        {
            objectiveComplete = true;
            PlayerData.data.activeQuests[ParentQuestLocal.GetPlayerDataIndex()].UpdateQuestStatus();
        }
    }
}
public class TalkQuest : QuestObjective
{
    public Dialogue questedDialogue; // the dialogue to run to complete the quest objective

    /// <summary>
    /// creates a quest objectives that is completed when a dialogue is run
    /// </summary>
    /// <param name="pTitle"> the name of the objective (shown on the questhelper)</param>
    /// <param name="pQuestedDialogue"> the dialogue that completes this quest step when run</param>
    public TalkQuest(string pTitle, Dialogue pQuestedDialogue)  : base(pTitle)
    {
        questedDialogue = pQuestedDialogue;
    }

    public void QuestedDialogueRun()
    {
        objectiveComplete = true;
        PlayerData.data.activeQuests[ParentQuestLocal.GetPlayerDataIndex()].UpdateQuestStatus();
    }
}
