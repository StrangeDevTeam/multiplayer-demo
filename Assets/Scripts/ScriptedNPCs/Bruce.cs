using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bruce : MonoBehaviour
{
	public Enemy Slime;
	Dialogue hello;
	Quest q;
	// Start is called before the first frame update
	void Start()
	{

		KillQuest kill_slimes = new KillQuest("kill the slime", Slime, 1); // create the quest objectives
		q = new Quest("Your First Quest","kill the slimes", kill_slimes); // from that create the quest
		
		Dialogue task = new Dialogue("can you kill that slime for me please?",q); // create the dialogue
		hello = new Dialogue("Hello there, i have a task for you.", task); // create the doialogue that triggers the quest and the next dialogue
		
		
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
		if (!q.started)
		{
			hello.ShowDialogue();
			UIController.HideInteractionTooltip();
		}
		if (q.complete && !q.isTurnedIn)
		{
			q.TurnInQuest();
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
