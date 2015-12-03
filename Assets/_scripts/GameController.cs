using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public GameObject boardManager;
	public GameObject slotHolder;
	public List<GameObject> cardSlots;
    public string sheepColour;
	public GameObject laser;
	public GameObject laserBeamGraphic;
	public LayerMask laserMask;
	public LayerMask sheepMask;
	public float beamWaitTime;
	public float beamFireTime;
	public GameObject tutorialManager;
	public bool tutorial;
    public GameObject gameEndPanel;
    public Text gameEndText;
    public GameObject gameButtons;

	private GameObject boardHolder;
	private BoardController boardScript;
	private CardSlotManager cardSlotScript;
    private string[] sheepColours = new string[2]{"RedSheep", "BlueSheep"};
	private GameObject newObject;
	private bool addCard;
	private GameObject[] sheepList;
	private Collider2D[] laserList;
    private string otherColour;
	private string[] newLasers = new string[4]{"TOP","RIGHT","BOTTOM","LEFT"};
	private int laserIndex;
	private int numberLasers = 1;
	private TutorialController tutorialScript;
    private int redScore;
    private int blueScore;

	void Awake() {
		// Making sure there's no existing board
		if (boardHolder != null)
			Destroy (boardHolder);

		boardHolder = Instantiate (boardManager);

		// Finding script that controls dynamic components - sheep + lasers
		boardScript = boardHolder.GetComponent<BoardController>();

		// Finding script that controls card slots
		cardSlotScript = slotHolder.GetComponent<CardSlotManager> ();

		// Adding action queue slots to a list in order of resolution
		cardSlots = new List<GameObject> ();
		for (int i = 0; i < 4; i++) {
			cardSlots.Add (slotHolder.transform.GetChild(i).gameObject);
		}

        // Determine if we're running a tutorial or a multiplayer game. Will eventually require support for multiple game modes
        // --> use switch() and unique game mode managers
		if (tutorial) {
			// Start the tutorial
			tutorialScript = tutorialManager.GetComponent<TutorialController>();
			tutorialScript.StartTutorial();

			// Setting start player to red
			sheepColour = "RedSheep";
		}
		else {
			// Starting a new multiplayer game
			boardScript.StartGame ();

			// Selecting a random start player colour
			sheepColour = sheepColours [Random.Range (0, 2)];
            // Update Cardslots to reflect player colours
            cardSlotScript.ChangeSlotColours(PlayerColour.Instance.redSheep);

            // Set player scores
            redScore = 5;
            blueScore = 5;
		}

        // Set random start side for new lasers
        // --> LaserController
		laserIndex = Random.Range (0, 4);
	}

    // Resolve chosen action cards.
    // Could be here or on CardSlotController
	public IEnumerator ResolutionPhase(string[] actionList, GameObject[] displaySheep){

		//Begin with start player for this round
		string currentColour = sheepColour;

		// For each played action resolve sheep movement, fire lasers, then move to next player colour
		for (int i = 0; i < 4; i++) {

			Destroy (displaySheep[i]);

			cardSlots[i].transform.GetChild(0).gameObject.SetActive(true);

			yield return StartCoroutine(ResolveCard (actionList [i], currentColour));

			yield return StartCoroutine(FireLasers());

            // Switch to next player's card colour
			if (currentColour == "RedSheep")
				currentColour = "BlueSheep";
			else 
				currentColour = "RedSheep";
		}

        // Check if the game should end
        if (redScore == 0 || blueScore == 0)
        {
            EndGame();
            yield break;
        }

        //Change start player for next round
        if (sheepColour == "RedSheep")
            sheepColour = "BlueSheep";
        else
            sheepColour = "RedSheep";

		//Change card slots to match new player colours and reset them
        cardSlotScript.ChangeSlotColours(PlayerColour.Instance.redSheep);		
        foreach (GameObject cardSlot in cardSlots)
            cardSlotScript.ResetSlot(cardSlot);

        //Move any "waiting" Lasers in to active position
        // --> LaserController
        GameObject[] waitLasers = GameObject.FindGameObjectsWithTag("WaitLaser");
        foreach (GameObject waitLaser in waitLasers) {
			Vector2 laserPosition = waitLaser.transform.position;
			string direction;

			if (laserPosition.x == 1)
				direction = "RIGHT";
			else if (laserPosition.x == 10)
				direction = "LEFT";
			else if (laserPosition.y == 1)
				direction = "UP";
			else
				direction = "DOWN";

			LaserController laserController = waitLaser.GetComponent<LaserController>();
			yield return StartCoroutine(laserController.Move(direction) );
        }
		
		//Instantiate a new "waiting" Laser
        // --> BoardController
		if (laserIndex > 3)
			laserIndex = 0;
		
		string orientation = newLasers[laserIndex];

        // This could be fed GameObjects directly
		switch (orientation) {
		case "BOTTOM":
			boardScript.PlaceObject(laser, 3, 8, 1, 1, numberLasers, false, "BOTTOM", boardScript.waitLasers);
			break;
		case "TOP":
			boardScript.PlaceObject(laser, 3, 8, 10, 10, numberLasers, false, "TOP", boardScript.waitLasers);
			break;
		case "LEFT":
			boardScript.PlaceObject(laser, 1, 1, 3, 8, numberLasers, false, "LEFT", boardScript.waitLasers);
			break;
		case "RIGHT":
			boardScript.PlaceObject(laser, 10, 10, 3, 8, numberLasers, false, "RIGHT", boardScript.waitLasers);
			break;
		}

		laserIndex++;
	}

	//Resolve action cards one at a time
    // --> CardController
	public IEnumerator ResolveCard(string direction, string currentColour){
		if (direction == "LEFT" || direction == "RIGHT" || direction == "UP" || direction == "DOWN") {
			sheepList = GameObject.FindGameObjectsWithTag (currentColour);
			yield return StartCoroutine (MoveSheep (direction));

			foreach (GameObject sheep in sheepList)
				sheep.GetComponent<SheepController>().moved = false;
		}
		else {
			switch(direction){
			case "UPLEFT":
				laserList = Physics2D.OverlapAreaAll(new Vector2(1.5f, 3), new Vector2(2.5f, 8));
				yield return StartCoroutine(MoveLasers (laserList, "UP"));

				laserList = Physics2D.OverlapAreaAll(new Vector2(3, 1.5f), new Vector2(8, 2.5f));
				yield return StartCoroutine (MoveLasers(laserList, "LEFT"));
				break;
			case "UPRIGHT":
				laserList = Physics2D.OverlapAreaAll(new Vector2(8.5f, 3), new Vector2(9.5f, 8));
				yield return StartCoroutine(MoveLasers (laserList, "UP"));
					
				laserList = Physics2D.OverlapAreaAll(new Vector2(3, 1.5f), new Vector2(8, 2.5f));
				yield return StartCoroutine (MoveLasers(laserList, "RIGHT"));
				break;
			case "DOWNLEFT":
				laserList = Physics2D.OverlapAreaAll(new Vector2(1.5f, 3), new Vector2(2.5f, 8));
				yield return StartCoroutine(MoveLasers (laserList, "DOWN"));
					
				laserList = Physics2D.OverlapAreaAll(new Vector2(3, 8.5f), new Vector2(8, 9.5f));
				yield return StartCoroutine (MoveLasers(laserList, "LEFT"));
				break;
			case "DOWNRIGHT":
				laserList = Physics2D.OverlapAreaAll(new Vector2(8.5f, 3), new Vector2(9.5f, 8));
				yield return StartCoroutine(MoveLasers (laserList, "DOWN"));
					
				laserList = Physics2D.OverlapAreaAll(new Vector2(3, 8.5f), new Vector2(8, 9.5f));
				yield return StartCoroutine (MoveLasers(laserList, "RIGHT"));
				break;
			}
		}
	}

	// Move all the sheep relevant to one card, one at a time
    // --> SheepController or CardController?
	IEnumerator MoveSheep(string direction){
		foreach (GameObject sheep in sheepList) {
			SheepController sheepController = sheep.GetComponent<SheepController>();
			yield return StartCoroutine(sheepController.Move(direction) );

		}
	}

    // Move all the lasers relevant to one card
    // --> LaserController or CardController?
	IEnumerator MoveLasers(Collider2D[] laserList, string direction){
		foreach(Collider2D laserCollider in laserList){
			LaserController laserController = laserCollider.GetComponentInParent<LaserController>();
			yield return StartCoroutine (laserController.Move(direction));
		}
	}

	// Fire any relevant laser objects and remove sheep
    // --> LaserController
	IEnumerator FireLasers(){
		GameObject[] activeLasers = GameObject.FindGameObjectsWithTag ("ActiveLaser");

		foreach (GameObject activeLaser in activeLasers) {
			Vector2 laserPosition = activeLaser.transform.position;
			if (laserPosition.x == 2)
				laserBeam(activeLaser, activeLasers, new Vector2(5.5f, laserPosition.y), new Vector2(1, 0));
			if (laserPosition.y == 2)
				laserBeam(activeLaser, activeLasers, new Vector2(laserPosition.x, 5.5f), new Vector2(0, 1)); 
		}

		yield return new WaitForSeconds (beamWaitTime);

	}

    // Display a laser beam, destroy sheep, decrease score appropriately
    // --> LaserController/SheepController
	void laserBeam(GameObject activeLaser, GameObject[] activeLasers, Vector2 position, Vector2 direction){
		Quaternion rotation;
		Collider2D[] sheepHit;
		Vector2 pointA;
		Vector2 pointB;
		
		if (direction.x == 1) {
			rotation = Quaternion.identity;
			pointA = new Vector2(2, position.y + 0.2f);
			pointB = new Vector2(9, position.y - 0.2f);
		} 
		else {
			rotation = Quaternion.Euler (0, 0, 90);
			pointA = new Vector2(position.x + 0.2f, 2);
			pointB = new Vector2(position.x - 0.2f, 9);
		}

		RaycastHit2D otherObject = Physics2D.Raycast(position, direction, Mathf.Infinity , laserMask);

		GameObject beamGraphic = null;

		if (otherObject.collider != null) {
			beamGraphic = Instantiate (laserBeamGraphic, position, rotation) as GameObject;
			sheepHit = Physics2D.OverlapAreaAll(pointA,pointB,sheepMask);
			foreach(Collider2D sheep in sheepHit){
				Destroy (sheep.gameObject);
                if (sheep.tag == "RedSheep")
                    redScore--;
                else
                    blueScore--;
			}
		}

		Destroy (beamGraphic, beamFireTime);
	}

    // Display winner at end of game, deactivate all but main menu button
    void EndGame() {
        gameEndPanel.SetActive(true);
        if (redScore == 0) {
            if (blueScore == 0)
            {
                gameEndText.text = "It's a draw!";
            }
            else
                gameEndText.text = "Blue Player Wins!";
        }
        else
            gameEndText.text = "Red Player Wins!";

        gameButtons.SetActive(false);
    }

}
