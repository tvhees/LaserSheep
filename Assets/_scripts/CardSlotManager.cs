using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardSlotManager : MonoBehaviour {
	
	public GameObject leftCard;
	public GameObject rightCard;
	public GameObject upCard;
	public GameObject downCard;
	public GameObject upRightCard;
	public GameObject downRightCard;
	public GameObject upLeftCard;
	public GameObject downLeftCard;

	private List<GameObject> cardSlots;
	private LaserManager activeLasersScript;

	void Awake(){
		// Adding action queue slots to a list in order of resolution
		cardSlots = new List<GameObject> ();
		for (int i = 0; i < 4; i++) {
			cardSlots.Add (transform.GetChild(i).gameObject);
		}
	}


	// Function to add or remove action cards from the action queue
	public void ChangeCardSlot(GameObject displayCard, bool addCard){
		
		// Run when adding a new action card
		if (addCard){
			// Find next empty card slot - should all be on its own script
			GameObject cardSlot = GetCardSlot(null);
			
			// Add a clone of the selected action as a child of the queue slot
			if(cardSlot){
				GameObject instance = Instantiate (displayCard, cardSlot.transform.position, Quaternion.identity) as GameObject;
				instance.transform.SetParent (cardSlot.transform);
			}
		}
		
		// Run when removing a card from the queue
		else {
			
			// Delete any child clones and return queue slot to empty state
			GameObject cardSlot = GetCardSlot(displayCard);
			
			if(cardSlot){
				cardSlot.GetComponent<SlotController>().ResetSlot();
			}
		}
	}

	// Update card slot colours for player colour
	public void ChangeSlotColours(bool redPlayer)
	{
		foreach (GameObject cardSlot in cardSlots) {
			SlotController slotScript = cardSlot.GetComponent<SlotController>();
			slotScript.SetMaterial(redPlayer);
			slotScript.ResetSlot();
			redPlayer = !redPlayer;
		}
	}
	
	// Find a slot matching request
	public GameObject GetCardSlot(GameObject displayCard){
		for (int i = 0; i < 4; i++) {
			bool slotColour = cardSlots[i].GetComponent<SlotController>().redCard;
			GameObject slotAction = null;
			if (cardSlots[i].transform.childCount > 0)
				slotAction = cardSlots[i].transform.GetChild(0).gameObject;
			if(PlayerColour.Instance.redSheep == slotColour && displayCard == slotAction)
				return(cardSlots[i]);
		}
		return(null);
	}

	public IEnumerator ResolveCards(GameObject[] displaySheep, GameObject boardHolder){

		activeLasersScript = boardHolder.transform.GetChild (0).GetComponent<LaserManager>();

		// For each played action resolve sheep movement, fire lasers, then move to next player colour
		for (int i = 0; i < 4; i++) {
			
			Destroy (displaySheep[i]);
			
			yield return StartCoroutine(cardSlots[i].GetComponent<SlotController>().ResolveSlot());
			
			yield return StartCoroutine(activeLasersScript.FireLasers());
		}
	}
}
