using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {

	public void LocalMultiplayer(){
		Application.LoadLevel ("LocalMultiplayer");
	}

	public void Tutorial(){
		Application.LoadLevel ("Tutorial");
	}

	public void MainMenu(){
		Application.LoadLevel ("MenuScene");
	}
}
