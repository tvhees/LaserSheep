using UnityEngine;
using System.Collections;

public class LaserController : MovingObject {

	// Blocking method for laser objects
	protected override int Blocking(Vector2 start, Vector2 increment){
		
		Vector2 point = start + increment;

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
}