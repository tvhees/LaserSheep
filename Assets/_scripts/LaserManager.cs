using UnityEngine;
using System.Collections;

public class LaserManager : MonoBehaviour {

	public float beamWaitTime = 0.3f;
	public float beamFireTime = 0.5f;
	public LayerMask sheepMask;
	public LayerMask laserMask;
	public GameObject laserBeamGraphic;

	public IEnumerator MoveLasers(string direction){

		Collider2D[] laserList;

		switch(direction){
		case "UPLEFT":
			laserList = Physics2D.OverlapAreaAll(new Vector2(1.5f, 3), new Vector2(2.5f, 8));
			foreach(Collider2D laser in laserList){
				yield return StartCoroutine(laser.GetComponent<LaserController>().Move("UP"));
			}
			
			laserList = Physics2D.OverlapAreaAll(new Vector2(3, 1.5f), new Vector2(8, 2.5f));
			foreach(Collider2D laser in laserList){
				yield return StartCoroutine(laser.GetComponent<LaserController>().Move("LEFT"));
			}
			break;
		case "UPRIGHT":
			laserList = Physics2D.OverlapAreaAll(new Vector2(8.5f, 3), new Vector2(9.5f, 8));
			foreach(Collider2D laser in laserList){
				yield return StartCoroutine(laser.GetComponent<LaserController>().Move("UP"));
			}
			
			laserList = Physics2D.OverlapAreaAll(new Vector2(3, 1.5f), new Vector2(8, 2.5f));
			foreach(Collider2D laser in laserList){
				yield return StartCoroutine(laser.GetComponent<LaserController>().Move("RIGHT"));
			}
			break;
		case "DOWNLEFT":
			laserList = Physics2D.OverlapAreaAll(new Vector2(1.5f, 3), new Vector2(2.5f, 8));
			foreach(Collider2D laser in laserList){
				yield return StartCoroutine(laser.GetComponent<LaserController>().Move("DOWN"));
			}
			
			laserList = Physics2D.OverlapAreaAll(new Vector2(3, 8.5f), new Vector2(8, 9.5f));
			foreach(Collider2D laser in laserList){
				yield return StartCoroutine(laser.GetComponent<LaserController>().Move("LEFT"));
			}
			break;
		case "DOWNRIGHT":
			laserList = Physics2D.OverlapAreaAll(new Vector2(8.5f, 3), new Vector2(9.5f, 8));
			foreach(Collider2D laser in laserList){
				yield return StartCoroutine(laser.GetComponent<LaserController>().Move("DOWN"));
			}
			
			laserList = Physics2D.OverlapAreaAll(new Vector2(3, 8.5f), new Vector2(8, 9.5f));
			foreach(Collider2D laser in laserList){
				yield return StartCoroutine(laser.GetComponent<LaserController>().Move("RIGHT"));
			}
			break;
		}
		/*LaserController[] laserList = transform.GetComponentInChildren<LaserController> ();

		switch(direction){
		case "UPLEFT":


		foreach (LaserController laser in laserList)
			laser.moved = false;
		
		foreach (LaserController laser in laserList) {
			if (LaserController.side == sideOne
			yield return StartCoroutine (laser.Move (direction));
		}*/
	}

	public IEnumerator FireLasers(){
		GameObject[] activeLasers = GameObject.FindGameObjectsWithTag ("ActiveLaser");
		
		foreach (GameObject activeLaser in activeLasers) {
			Vector2 laserPosition = activeLaser.transform.position;
			if (laserPosition.x == 2)
				laserBeam(activeLaser, activeLasers, new Vector2(5.5f, laserPosition.y), new Vector2(1, 0));
			if (laserPosition.y == 2)
				laserBeam(activeLaser, activeLasers, new Vector2(laserPosition.x, 5.5f), new Vector2(0, 1)); 
		}
		
		yield return new WaitForSeconds (beamWaitTime);
		
	}

	// Display a laser beam, destroy sheep, decrease score appropriately
	// --> LaserController/SheepController
	void laserBeam(GameObject activeLaser, GameObject[] activeLasers, Vector2 position, Vector2 direction){
		Quaternion rotation;
		Collider2D[] sheepHit;
		Vector2 pointA;
		Vector2 pointB;
		
		if (direction.x == 1) {
			rotation = Quaternion.identity;
			pointA = new Vector2(2, position.y + 0.2f);
			pointB = new Vector2(9, position.y - 0.2f);
		} 
		else {
			rotation = Quaternion.Euler (0, 0, 90);
			pointA = new Vector2(position.x + 0.2f, 2);
			pointB = new Vector2(position.x - 0.2f, 9);
		}
		
		RaycastHit2D otherObject = Physics2D.Raycast(position, direction, Mathf.Infinity , laserMask);
		
		GameObject beamGraphic = null;
		
		if (otherObject.collider != null) {
			beamGraphic = Instantiate (laserBeamGraphic, position, rotation) as GameObject;
			sheepHit = Physics2D.OverlapAreaAll(pointA,pointB,sheepMask);
			foreach(Collider2D sheep in sheepHit){
				Destroy (sheep.gameObject);
				if (sheep.tag == "RedSheep")
					PlayerColour.Instance.redScore--;
				else
					PlayerColour.Instance.blueScore--;
			}
		}
		
		Destroy (beamGraphic, beamFireTime);
	}
}