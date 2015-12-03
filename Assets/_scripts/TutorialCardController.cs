using UnityEngine;
using System.Collections;

public class TutorialCardController : MonoBehaviour
{

    public Sprite idleTexture;
    public Sprite hoverTexture;
    public Sprite clickTexture;
    public string sheepColour;
    public string direction;
	public GameObject pair;
	public GameObject tutorialManager;
	public GameObject slotHolder;
	public GameObject gameManager;

	private bool active;
	private GameController gameScript;
    private CardSlotManager cardSlotScript;
    private SpriteRenderer cardTexture;
	private bool selected;
	private TutorialCardController pairController;

    void Start()
    {
        cardTexture = GetComponent<SpriteRenderer>();

		cardSlotScript = slotHolder.GetComponent<CardSlotManager> ();

		gameScript = gameManager.GetComponent<GameController> ();

		pairController = pair.GetComponent<TutorialCardController> ();

		Deselect ();
    }

    void OnMouseEnter()
    {
		if (active && direction == "LEFT")
	        cardTexture.sprite = hoverTexture;
    }

    void OnMouseExit()
    {
		if (active)
        	cardTexture.sprite = idleTexture;
    }

    void OnMouseDown()
    {
		if (active && direction == "LEFT")
        	cardTexture.sprite = clickTexture;
    }

    void OnMouseUpAsButton()
    {
		if (active && direction == "LEFT"){
			selected = !selected;

			if (selected) {
				StartCoroutine(gameScript.ResolveCard(direction, "RedSheep"));
				pairController.PairSelect();
			}

			if (!selected) {
				cardSlotScript.ChangeCardSlot (direction, false);
				pairController.Deselect();
			}

		    cardTexture.sprite = idleTexture;

			tutorialManager.GetComponent<TutorialController>().IncrementMessage();
		}
    }

	public void Deselect(){
		selected = false;
		active = true;
		cardTexture.color = new Color (1f, 1f, 1f, 1f);
	}

	public void PairSelect(){
		active = false;
		cardTexture.color = new Color (0.5f, 0.5f, 0.5f, 255f);
	}
}
