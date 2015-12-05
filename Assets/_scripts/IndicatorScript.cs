using UnityEngine;
using System.Collections;

public class IndicatorScript : MonoBehaviour {

	public Sprite redSheep;
	public Sprite blueSheep;

	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();	
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerColour.Instance.redSheep)
			spriteRenderer.sprite = redSheep;
		else
			spriteRenderer.sprite = blueSheep;
	}
}
