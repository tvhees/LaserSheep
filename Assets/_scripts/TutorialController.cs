using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialController : MonoBehaviour {

	public GameObject messageBox;
	public Text messageText;
	public GameObject tutorialSheep;
	public GameObject tutorialLasers;
	public GameObject cardSlots;
	public GameObject actionCards;
	public GameObject submitButton;
	public GameObject cardHighlightOne;
	public GameObject cardHighlightTwo;
	public GameObject cardHighlightThree;
	public GameObject cardHighlightFour;
    public GameObject tutorialBeam;
    public GameObject nextButton;
    public GameObject leftCard;
    public GameObject upLeftCard;
    public GameObject deadSheep;

	private int tutorialIndex;
	private string messageString;

	public void StartTutorial(){
		tutorialIndex = 0;
		ChangeMessage(tutorialIndex);
	}

	void ChangeMessage(int i){

		switch (i) {
		    case 0:
			    messageString = "Welcome to Laser Sheep!\n" +
				    "The object of the game is to laser your opponent's sheep before they laser yours.";
			    TutorialSwitches (false, false, false, false, false);
			    HighlightSwitches(false, false, false, false, false);
			    break;
		    case 1:
			    messageString =	"Players start with FIVE sheep each in a pen surrounded by FOUR lasers. The player with the last sheep left wins.";
			    TutorialSwitches(true, true, false, false, false);
			    HighlightSwitches(false, false, false, false, false);
			    break;
		    case 2:
			    messageString = "Sheep may be moved with the direction actions. Each player can only move their own sheep. Try moving the red sheep left with the highlighted card!";
			    TutorialSwitches(true, true, true, false, false);
			    HighlightSwitches(true, false, true, false, false);
			    cardHighlightThree.GetComponent<HighlightController>().setFlickerTime();
                nextButton.SetActive(false);
                leftCard.GetComponent<TutorialCardControllerLeft>().active = true;
                upLeftCard.GetComponent<TutorialCardControllerLeft>().active = false;
                break;
		    case 3:
			    messageString = "Notice that the Move Right action is greyed out - you can't choose actions that are next to each other in the same turn.";
			    TutorialSwitches(true, true, true, false, false);
			    HighlightSwitches(true, false, false, false, false);
                nextButton.SetActive(true);
                leftCard.GetComponent<TutorialCardControllerLeft>().active = false;
                break;
		    case 4:
			    messageString = "Lasers may be moved with the other four actions. Each action moves all lasers on two sides of the pen.";
			    TutorialSwitches(true, true, true, false, false);
			    HighlightSwitches(false, true, false, false, false);
			    break;
            case 5:
                messageString = "If a laser or sheep can't move further in the requested direction, it will \"bounce\" in the opposite direction. Try using the highlighted action card!";
                TutorialSwitches(true, true, true, false, false);
                HighlightSwitches(false, true, false, true, false);
                cardHighlightFour.GetComponent<HighlightController>().setFlickerTime();
                nextButton.SetActive(false);
                upLeftCard.GetComponent<TutorialCardControllerLeft>().active = true;
                break;
            case 6:
                messageString = "Lasers can't move further than the edges of the pen. Sheep can't move out of the pen, and are also blocked by sheep of the opposite colour.";
                nextButton.SetActive(true);
                HighlightSwitches(false, true, false, false, false);
                upLeftCard.GetComponent<TutorialCardControllerLeft>().active = false;
                break;
            case 7:
                messageString = "When a pair of lasers faces each other, they will fire across the pen and any sheep under the laser beam will be removed.";
                HighlightSwitches(false, false, false, false, true);
                Destroy(deadSheep);
                break;
            case 8:
                messageString = "The final twist: players don't select one action at a time. Each player will select two actions in the order they want, then press the Confirm button.";
                TutorialSwitches(true, true, true, true, true);
                HighlightSwitches(false, false, false, false, false);
                break;
            case 9:
                messageString = "Once both players have confirmed, the four selected actions will resolve from top to bottom as shown by the action queue on the left of the pen.";
                break;
            case 10:
                messageString = "Be careful when choosing your actions - the board might look very different when they resolve! If you want to remove an action from the queue just click the action card again.";
                break;
            case 11:
                messageString = "That's it! Good luck and have fun.";
                nextButton.SetActive(false);
                break;
        }

		messageText.text = messageString;
	}

	void TutorialSwitches(bool sheep, bool lasers, bool cards, bool slots, bool submit){
		tutorialSheep.SetActive (sheep);
		tutorialLasers.SetActive (lasers);
		actionCards.SetActive (cards);
		cardSlots.SetActive (slots);
		submitButton.SetActive (submit);
	}

	void HighlightSwitches(bool cardOne, bool cardTwo, bool cardThree, bool cardFour, bool tutBeam){
		cardHighlightOne.SetActive (cardOne);
		cardHighlightTwo.SetActive (cardTwo);
		cardHighlightThree.SetActive (cardThree);
		cardHighlightFour.SetActive (cardFour);
        tutorialBeam.SetActive(tutBeam);
	}

	void RedSheepLeft(){

	}

	public void IncrementMessage(){
		tutorialIndex++;
		ChangeMessage (tutorialIndex);
	}

	public void DecrementMessage(){
		if (tutorialIndex > 0)
			tutorialIndex--;
		else
			Application.LoadLevel ("MenuScene");
		ChangeMessage (tutorialIndex);
	}
}
