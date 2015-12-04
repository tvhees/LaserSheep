using UnityEngine;
using System.Collections;

public class LaserManager : MonoBehaviour {

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
}