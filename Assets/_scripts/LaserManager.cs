using UnityEngine;
using System.Collections;

public class LaserManager : MonoBehaviour {

	public float beamWaitTime = 0.3f;
	public float beamFireTime = 0.5f;
	public LayerMask sheepMask;
	public LayerMask laserMask;
	public GameObject laserBeamGraphic;

	private Vector2[] orientation = new Vector2[2];
	private Vector2[] direction = new Vector2[2];

	public IEnumerator MoveLasers(Vector2 actionDirection){

		int dotProduct = Mathf.RoundToInt(Vector2.Dot (actionDirection, PlayerColour.Instance.reference));

		LaserController[] laserList = GetComponentsInChildren<LaserController> ();

		switch(dotProduct){
		case -1:
			orientation = new Vector2[]{Vector2.right, Vector2.up};
			direction = new Vector2[]{Vector2.up, -Vector2.right};
			break;
		case 3:
			orientation = new Vector2[]{-Vector2.right, Vector2.up};
			direction = new Vector2[]{Vector2.up, Vector2.right};
			break;
		case -3:
			orientation = new Vector2[]{Vector2.right, -Vector2.up};
			direction = new Vector2[]{-Vector2.up, -Vector2.right};
			break;
		case 1:
			orientation = new Vector2[]{-Vector2.right, -Vector2.up};
			direction = new Vector2[]{-Vector2.up, Vector2.right};
			break;
		}

		foreach(LaserController laser in laserList){
			if(laser.orientation == orientation[0])
				yield return StartCoroutine(laser.GetComponent<LaserController>().Move(direction[0]));
			if(laser.orientation == orientation[1])
				yield return StartCoroutine(laser.GetComponent<LaserController>().Move(direction[1]));
		}

		//tutorial code
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
				laserBeam(activeLaser, activeLasers, new Vector2(5.5f, laserPosition.y), Vector2.right);
			if (laserPosition.y == 2)
				laserBeam(activeLaser, activeLasers, new Vector2(laserPosition.x, 5.5f), Vector2.up); 
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