using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bruce : MonoBehaviour
{
	public Enemy Slime;
	public Enemy Warrior;
	Dialogue hello;
	Dialogue turnin;
	Dialogue aa;
	Dialogue ca;
	Quest q;
	// Start is called before the first frame update
	void Start()
	{

		KillQuest kill_slimes = new KillQuest("kill the slime", Slime, 1); // create the quest objectives
		KillQuest kill_Warrior = new KillQuest("kill the warrior", Warrior, 1); // create the quest objectives
		List<QuestObjective> obj = new List<QuestObjective> { kill_slimes, kill_Warrior};
		q = new Quest("Your First Quest","kill the slimes", obj); // from that create the quest
		
		Dialogue task = new Dialogue("can you kill that slime for me please?",q); // create the dialogue
		hello = new Dialogue("Hello there, i have a task for you.", task); // create the doialogue that triggers the quest and the next dialogue


		turnin = new Dialogue("thank you for completing my quest");

		aa = new Dialogue("i have already given you a task");
		ca = new Dialogue("you have already completed my task");

		Item sword = ItemDatabase.SearchDatabaseByName("example item");
		q.setReward(sword);
	}

	public void OnNearby()
	{
		UIController.ShowInteractionTooltip();
	}

	public void NoLongerNearby()
	{
		UIController.HideInteractionTooltip();
		hello.HideDialogue();
	}

	public void Use()
	{
		// if quest is not already complete or active
		if(!q.isAlreadyCompleteAndTurnedIn() && !q.isAlreadyActive())
		{
			hello.ShowDialogue();
			UIController.HideInteractionTooltip(); 
		}
		// quest is active, but complete and not turned in
		else if(q.isAlreadyActive() && !q.isAlreadyTurnedIn() && q.isComplete() && !q.isAlreadyCompleteAndTurnedIn())
		{
			q.TurnInQuest();
			turnin.ShowDialogue();
			UIController.HideInteractionTooltip();

		}
		else
		{
			//other
			if (q.isAlreadyCompleteAndTurnedIn())
			{
				ca.ShowDialogue();
				PlayerData.data.playerInv.AddItem(ItemDatabase.SearchDatabaseByID(0));
			}
			else
			{
				aa.ShowDialogue();
				UIController.HideInteractionTooltip();
			}
			
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
