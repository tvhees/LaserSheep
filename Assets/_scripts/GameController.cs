using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public GameObject boardManager;
	public GameObject slotHolder;
	public GameObject laser;
	public GameObject tutorialManager;
	public bool tutorial;
    public GameObject gameEndPanel;
    public Text gameEndText;
    public GameObject gameButtons;

	private GameObject boardHolder;
	private BoardController boardScript;
	private CardSlotManager cardSlotScript;
	private TutorialController tutorialScript;

	void Awake() {
		// Making sure there's no existing board
		if (boardHolder != null)
			Destroy (boardHolder);

		// Create a new board and find required scripts
		boardHolder = Instantiate (boardManager);
		boardScript = boardHolder.GetComponent<BoardController>();
		cardSlotScript = slotHolder.GetComponent<CardSlotManager> ();

        // Determine if we're running a tutorial or a multiplayer game. Will eventually require support for multiple game modes
        // --> use switch() and unique game mode managers
		if (tutorial) {
			// Start the tutorial
			tutorialScript = tutorialManager.GetComponent<TutorialController>();
			tutorialScript.StartTutorial();
		}
		else {
			// Starting a new multiplayer game
			boardScript.StartGame ();

            // Update Cardslots to reflect player colours
            cardSlotScript.ChangeSlotColours(PlayerColour.Instance.redSheep);
		}
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
		StartCoroutine(boardScript.updateLasers ());
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
