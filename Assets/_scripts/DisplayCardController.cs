using UnityEngine;
using System.Collections;

public class DisplayCardController : MonoBehaviour {
	
	public bool sheepCard;
	public string direction;
	public GameObject redSheeps;
	public GameObject blueSheeps;
	public GameObject activeLasers;

	private SheepManager redSheepsScript;
	private SheepManager blueSheepsScript;
	private LaserManager activeLasersScript;

	void Awake(){
		redSheeps = GameObject.Find ("RedSheeps");
		blueSheeps = GameObject.Find ("BlueSheeps");
		activeLasers = GameObject.Find ("ActiveLasers");

		redSheepsScript = redSheeps.GetComponent<SheepManager> ();
		blueSheepsScript = blueSheeps.GetComponent<SheepManager> ();
		activeLasersScript = activeLasers.GetComponent<LaserManager> ();
	}

	public IEnumerator ResolveCard(bool redCard){
		gameObject.SetActive (true);

		if (sheepCard) {
			if (redCard)
				yield return StartCoroutine(redSheepsScript.MoveSheep (direction));
			else
				yield return StartCoroutine(blueSheepsScript.MoveSheep (direction));
		} else
			yield return StartCoroutine(activeLasersScript.MoveLasers (direction));
	}
}
