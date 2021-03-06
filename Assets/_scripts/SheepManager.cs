﻿using UnityEngine;
using System.Collections;

public class SheepManager : MonoBehaviour {

	public IEnumerator MoveSheep(Vector2 direction){
		SheepController[] sheepList = transform.GetComponentsInChildren<SheepController> ();
			
		foreach (SheepController sheep in sheepList)
			sheep.moved = false;

		foreach (SheepController sheep in sheepList) {
			yield return StartCoroutine(sheep.Move(direction));
		}
	}
}