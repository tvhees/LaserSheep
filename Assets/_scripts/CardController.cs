using UnityEngine;
using System.Collections;

public class CardController : MonoBehaviour
{

    public Sprite idleTexture;
    public Sprite hoverTexture;
    public Sprite clickTexture;
    public string sheepColour;
    public string direction;
	public GameObject pair;

	private bool active;
    private GameController gameScript;
    private SpriteRenderer cardTexture;
	private bool selected;
	private CardController pairController;

    void Start()
    {
        cardTexture = GetComponent<SpriteRenderer>();

		gameScript = GameObject.Find("Main Camera").GetComponent<GameController> ();

		pairController = pair.GetComponent<CardController> ();

		Deselect ();
    }

    void OnMouseEnter()
    {
		if (active)
	        cardTexture.sprite = hoverTexture;
    }

    void OnMouseExit()
    {
		if (active)
        	cardTexture.sprite = idleTexture;
    }

    void OnMouseDown()
    {
		if (active)
        	cardTexture.sprite = clickTexture;
    }

    void OnMouseUpAsButton()
    {
		if (active){
			selected = !selected;

			if (selected) {
				gameScript.ChangeCardSlot(direction, true);
				pairController.PairSelect();
			}

			if (!selected) {
				gameScript.ChangeCardSlot (direction, false);
				pairController.Deselect();
			}

		    cardTexture.sprite = idleTexture;
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
