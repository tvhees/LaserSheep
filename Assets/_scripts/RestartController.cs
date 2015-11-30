using UnityEngine;
using System.Collections;

public class RestartController : MonoBehaviour
{
	public Sprite idleSprite;
	public Sprite clickSprite;

	private BoardController boardController;
	private SpriteRenderer restartSprite;

	void Start(){

		boardController = GameObject.Find("Main Camera").GetComponent<BoardController> ();

		restartSprite = GetComponent<SpriteRenderer> ();
	}

    void OnMouseDown()
    {

		restartSprite.sprite = clickSprite; 

	}

	void OnMouseUpAsButton(){

		boardController.StartGame();

		restartSprite.sprite = idleSprite;

	}
}
