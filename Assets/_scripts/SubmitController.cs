using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubmitController : MonoBehaviour {

	public GameObject displaySheepRed;
	public GameObject displaySheepBlue;
	public GameObject[] displaySheep;
    public GameObject gameManager;
    public GameObject slotOne;
    public GameObject slotTwo;
    public GameObject slotThree;
    public GameObject slotFour;

	private GameObject displayObject;
    private GameController gameScript;
	private GameObject[] actionCards;
	private GameObject[] displayCards;
	private SlotController slot1;
	private SlotController slot2;
	private SlotController slot3;
	private SlotController slot4;
	private GameObject instance;
    

    void Awake() {
        gameScript = gameManager.GetComponent<GameController>();
		actionCards = GameObject.FindGameObjectsWithTag ("ActionCard");
		displaySheep = new GameObject[4];

		slot1 = slotOne.GetComponent<SlotController>();
		slot2 = slotTwo.GetComponent<SlotController>();
        slot3 = slotThree.GetComponent<SlotController>();
        slot4 = slotFour.GetComponent<SlotController>();
    }

	void OnMouseUpAsButton(){

        NextPlayer();

		if (slot2.action != "EMPTY" && slot4.action != "EMPTY") {
			instance = Instantiate(displayObject, slot2.gameObject.transform.position, Quaternion.identity) as GameObject;
			displaySheep[1] = instance;
			instance = Instantiate(displayObject, slot4.gameObject.transform.position, Quaternion.identity) as GameObject;
			displaySheep[3] = instance;

			string[] actionList = new string[]{slot1.action, slot2.action, slot3.action, slot4.action};
			StartCoroutine (gameScript.ResolutionPhase (actionList, displaySheep));
		}
		else {
			instance = Instantiate(displayObject, slot1.gameObject.transform.position, Quaternion.identity) as GameObject;
			displaySheep[0] = instance;
			instance = Instantiate(displayObject, slot3.gameObject.transform.position, Quaternion.identity) as GameObject;
			displaySheep[2] = instance;
		}
	}

	void NextPlayer(){
	
		displayCards = GameObject.FindGameObjectsWithTag ("DisplayCard");
		foreach (GameObject displayCard in displayCards) {
			displayCard.SetActive(false);
		}

		// Deselect all action cards
		foreach(GameObject card in actionCards){
			card.GetComponent<CardController>().Deselect();
		}

		// Change player colour
		if (gameScript.sheepColour == "RedSheep") {
			displayObject = displaySheepRed;
			gameScript.sheepColour = "BlueSheep";
		}
		else{
			displayObject = displaySheepBlue;
			gameScript.sheepColour = "RedSheep";
		}
			

	}


}
