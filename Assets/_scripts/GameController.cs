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
	public GameObject tutorialManager;
	public bool tutorial;
    public GameObject gameEndPanel;
    public Text gameEndText;
    public GameObject gameButtons;

	private GameObject boardHolder;
	private BoardController boardScript;
	private CardSlotManager cardSlotScript;
    private bool[] sheepColours = new bool[2]{true, false};
	private GameObject newObject;
	private bool addCard;
	private GameObject[] sheepList;
	private Collider2D[] laserList;
    private string otherColour;
	private string[] newLasers = new string[4]{"TOP","RIGHT","BOTTOM","LEFT"};
	private int laserIndex;
	private int numberLasers = 1;
	private TutorialController tutorialScript;

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
			PlayerColour.Instance.redSheep = sheepColours [Random.Range (0, 2)];
            // Update Cardslots to reflect player colours
            cardSlotScript.ChangeSlotColours(PlayerColour.Instance.redSheep);

            // Set global player scores
            PlayerColour.Instance.redScore = 5;
            PlayerColour.Instance.blueScore = 5;
		}

        // Set random start side for new lasers
        // --> LaserController
		laserIndex = Random.Range (0, 4);
	}

    // Resolve chosen cards, check for game end and prepare for the next turn.
	public IEnumerator ResolutionPhase(GameObject[] displaySheep){

		yield return StartCoroutine (cardSlotScript.ResolveCards (displaySheep, boardHolder));

        // Check if the game should end
		if (PlayerColour.Instance.redScore < 1 || PlayerColour.Instance.blueScore < 1)
        {
            EndGame();
            yield break;
        }

        //Change start player for next round
		PlayerColour.Instance.redSheep = !PlayerColour.Instance.redSheep;

		//Change card slots to match new player colours and reset them
        cardSlotScript.ChangeSlotColours(PlayerColour.Instance.redSheep);		

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

    // Display winner at end of game, deactivate all but main menu button
    void EndGame() {
        gameEndPanel.SetActive(true);
        if (PlayerColour.Instance.redScore == 0) {
			if (PlayerColour.Instance.blueScore == 0)
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
