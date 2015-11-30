using UnityEngine;
using System.Collections;

public class SheepController : MonoBehaviour {

	public float moveSpeed;
	public bool moved = false;

	private int moveFlag;
	private Vector2 increment;
    private Transform sheepTransform;
	private Collider2D otherCollider;
	private Vector2 end;
	private Vector2 sheepPosition;

    public IEnumerator Move(string direction) {
        sheepTransform = GetComponent<Transform>();
		sheepPosition = sheepTransform.position;
		Vector2 start = sheepPosition;

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

    }

	IEnumerator SmoothMovement(Vector2 end){

		float sqrDistance = (sheepPosition - end).sqrMagnitude;

		while (sqrDistance > float.Epsilon) {
			Vector2 newPosition = Vector2.MoveTowards(sheepPosition, end, moveSpeed * Time.deltaTime);

			sheepTransform.position = newPosition;

			sheepPosition = sheepTransform.position;

			sqrDistance = (sheepPosition - end).sqrMagnitude;

			yield return null;
		}

	}

	int Blocking(Vector2 start, string direction){

		Vector2 point = start + increment;

		while (point.x > 2 && point.x < 9 && point.y > 2 && point.y < 9){

			otherCollider = Physics2D.OverlapCircle(point, 0.2f);

			if(otherCollider == null)
				return 1;

			if(otherCollider.GetComponent<SheepController>().moved == true)
				break;

			if (otherCollider.gameObject.tag != this.tag)
				break;

			point += increment;
		}

		point = start - increment;

		while (point.x > 2 && point.x < 9 && point.y > 2 && point.y < 9)
        {

			otherCollider = Physics2D.OverlapCircle(point, 0.2f);

			if(otherCollider == null)
				return -1;

			if(otherCollider.GetComponent<SheepController>().moved == true)
				break;

			if (otherCollider.gameObject.tag != this.tag)
				break;

			point -=increment;
		}

		return 0;
	}
}