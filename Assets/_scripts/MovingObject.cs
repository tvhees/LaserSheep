using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {
	
	public float moveSpeed = 3.0f;
	public bool moved = false;

	private Vector2 increment;
	private Vector3 end;
	private int moveFlag;

	// Check direction of movement, check if movement is blocked, move object
	// --> Change 'direction' to native Vector2
	public IEnumerator Move(string direction) {
		Vector2 start = transform.position;
		
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
		
		moveFlag = Blocking(start, increment);
		
		end = start + moveFlag * increment;
		
		yield return StartCoroutine (SmoothMovement(end));
		
		moved = true;
		
	}

	// Smoothly move gameobjects over time
	protected IEnumerator SmoothMovement(Vector3 end){
		
		float sqrDistance = (transform.position - end).sqrMagnitude;
		
		while (sqrDistance > float.Epsilon) {
			Vector2 newPosition = Vector2.MoveTowards(transform.position, end, moveSpeed * Time.deltaTime);
			
			transform.position = newPosition;
			
			sqrDistance = (transform.position - end).sqrMagnitude;
			
			yield return null;
		}
		
	}

	// Declare function to check blocking, will be defined in inherited classes
	protected abstract int Blocking(Vector2 start, Vector2 increment);

}
