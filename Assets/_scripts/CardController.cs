using UnityEngine;
using System.Collections;

public class CardController : MonoBehaviour
{

    public Sprite idleTexture;
    public Sprite hoverTexture;
    public Sprite clickTexture;
    public string direction;
	public GameObject pair;
	public GameObject slotHolder;

	private bool active;
    private CardSlotManager cardSlotScript;
    private SpriteRenderer cardTexture;
	private bool selected;
	private CardController pairController;

    void Start()
    {
        cardTexture = GetComponent<SpriteRenderer>();

		cardSlotScript = slotHolder.GetComponent<CardSlotManager> ();

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
				cardSlotScript.ChangeCardSlot(direction, true);
				pairController.PairSelect();
			}

			if (!selected) {
				cardSlotScript.ChangeCardSlot (direction, false);
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
