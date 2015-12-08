using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {
	
	public float moveSpeed = 3.0f;
	public bool moved = false;
	public Vector2 orientation;

	protected Vector2 reference = new Vector2(2,1);
	protected Vector3 end;
	protected int moveFlag;
	protected SpriteRenderer spriteRenderer;

	public abstract void ChangeSprite ();

	// Check direction of movement, check if movement is blocked, move object
	// --> Change 'direction' to native Vector2
	public IEnumerator Move(Vector2 direction) {
		Vector2 start = transform.position;
		
		moveFlag = Blocking(start, direction);
		
		end = start + moveFlag * direction;
		
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
