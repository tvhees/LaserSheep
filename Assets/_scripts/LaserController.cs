using UnityEngine;
using System.Collections;

public class LaserController : MovingObject {

	// Blocking method for laser objects
	protected override int Blocking(Vector2 start, Vector2 direction){
		
		Vector2 point = start + direction;

		Collider2D otherCollider;

		while (true){
			
			otherCollider = Physics2D.OverlapCircle(point, 0.2f);
			
			if(otherCollider == null){
				this.tag = "ActiveLaser";
				this.gameObject.layer = 12;
				return 1;
			}

			if (this.tag == "WaitLaser")
				return 0;

			if (otherCollider.gameObject.tag != this.tag)
				break;
			
			point += direction;
		}

		point = start - direction;
		
		while (true)
		{
			
			otherCollider = Physics2D.OverlapCircle(point, 0.2f);
			
			if(otherCollider == null)
				return -1;
			
			if (otherCollider.gameObject.tag != this.tag)
				break;
			
			point -=direction;
		}
		return 0;
	}
}