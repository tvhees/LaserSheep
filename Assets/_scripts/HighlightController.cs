using UnityEngine;
using System.Collections;

public class HighlightController : MonoBehaviour {

	private bool flickerOn;
	private float flickerTime;
	private float flickerPause = 0.7f;

	public void setFlickerTime(){
		flickerTime = Time.time;
		flickerOn = false;
		changeRenderer();
	}

	void changeRenderer(){
		flickerOn = !flickerOn;

		gameObject.GetComponent<SpriteRenderer> ().enabled = flickerOn;

		flickerTime += flickerPause;
	}

	void Update () {

		if (Time.time > flickerTime) {
			changeRenderer();
		}
	}
}
