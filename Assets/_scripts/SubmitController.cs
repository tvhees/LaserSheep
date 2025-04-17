using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubmitController : MonoBehaviour {

	public GameObject displaySheepRed;
	public GameObject displaySheepBlue;
	public GameObject[] displaySheep;
    public GameObject gameManager;
    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    public GameObject slot4;

	private GameObject displayObject;
    private GameController gameScript;
	private GameObject[] actionCards;
	private GameObject[] displayCards;
	private GameObject instance;
    

    void Awake() {
        gameScript = gameManager.GetComponent<GameController>();
		actionCards = GameObject.FindGameObjectsWithTag ("ActionCard");
		displaySheep = new GameObject[4];
    }

	void OnMouseUpAsButton(){

        NextPlayer();

		if (slot2.transform.childCount > 0 && slot4.transform.childCount > 0) {
			instance = Instantiate(displayObject, slot2.transform.position, Quaternion.identity) as GameObject;
			displaySheep[1] = instance;
			instance = Instantiate(displayObject, slot4.transform.position, Quaternion.identity) as GameObject;
			displaySheep[3] = instance;

			StartCoroutine (gameScript.ResolutionPhase (displaySheep));
		}
		else {
			instance = Instantiate(displayObject, slot1.transform.position, Quaternion.identity) as GameObject;
			displaySheep[0] = instance;
			instance = Instantiate(displayObject, slot3.transform.position, Quaternion.identity) as GameObject;
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
		if (PlayerColour.Instance.redSheep)
			displayObject = displaySheepRed;
		else
			displayObject = displaySheepBlue;
			
		PlayerColour.Instance.redSheep = !PlayerColour.Instance.redSheep;

	}


}
