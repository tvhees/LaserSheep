using UnityEngine;
using System.Collections;

public class LaserController : MonoBehaviour {

	public float moveSpeed = 2.0f;
	public bool moved = false;

	private Transform laserTransform;
	private Vector2 laserPosition;
	private Vector2 increment;
	private Collider2D otherCollider;
	private int moveFlag;
	private Vector2 end;

	public IEnumerator Move (string direction){
		laserTransform = GetComponent <Transform>();
		laserPosition = laserTransform.position;
		Vector2 start = laserPosition;

		switch (direction) {
		case "RIGHT":
			increment = new Vector2(1, 0);
			break;
		case "LEFT":
			increment = new Vector2(-1, 0);
			break;
		case "UP":
			increment = new Vector2(0, 1);
			break;
		case "DOWN":
			increment = new Vector2(0, -1);
			break;
		}

		moveFlag = Blocking(start, direction);

		end = start + moveFlag * increment;
		
		yield return StartCoroutine (SmoothMovement(end));

		moved = true;

		if (moveFlag == 1) {
			this.tag = "ActiveLaser";
			this.gameObject.layer = 12;
		}
	}

	int Blocking(Vector2 start, string direction){
		
		Vector2 point = start + increment;

		while (true){
			
			otherCollider = Physics2D.OverlapCircle(point, 0.2f);
			
			if(otherCollider == null)
				return 1;

			if (this.tag == "WaitLaser")
				return 0;

			if (otherCollider.gameObject.tag != this.tag)
				break;
			
			point += increment;
		}

		point = start - increment;
		
		while (true)
		{
			
			otherCollider = Physics2D.OverlapCircle(point, 0.2f);
			
			if(otherCollider == null)
				return -1;
			
			if (otherCollider.gameObject.tag != this.tag)
				break;
			
			point -=increment;
		}
		return 0;
	}

	IEnumerator SmoothMovement(Vector2 end){
		
		float sqrDistance = (laserPosition - end).sqrMagnitude;
		
		while (sqrDistance > float.Epsilon) {
			Vector2 newPosition = Vector2.MoveTowards(laserPosition, end, moveSpeed * Time.deltaTime);
			
			laserTransform.position = newPosition;
			
			laserPosition = laserTransform.position;
			
			sqrDistance = (laserPosition - end).sqrMagnitude;
			
			yield return null;
		}
		
	}


}