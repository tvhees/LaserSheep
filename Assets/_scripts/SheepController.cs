using UnityEngine;
using System.Collections;

public class SheepController : MovingObject {

	// blocking method for sheep objects
	protected override int Blocking(Vector2 start, Vector2 direction){

		Vector2 point = start + direction;

		Collider2D otherCollider;

		while (point.x > 2 && point.x < 9 && point.y > 2 && point.y < 9){

			otherCollider = Physics2D.OverlapCircle(point, 0.2f);

			if(otherCollider == null)
				return 1;

			if(otherCollider.GetComponent<SheepController>().moved == true)
				break;

			if (otherCollider.gameObject.tag != this.tag)
				break;

			point += direction;
		}

		point = start - direction;

		while (point.x > 2 && point.x < 9 && point.y > 2 && point.y < 9)
        {

			otherCollider = Physics2D.OverlapCircle(point, 0.2f);

			if(otherCollider == null)
				return -1;

			if(otherCollider.GetComponent<SheepController>().moved == true)
				break;

			if (otherCollider.gameObject.tag != this.tag)
				break;

			point -=direction;
		}

		return 0;
	}
}