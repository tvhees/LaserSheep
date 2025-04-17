using UnityEngine;
using System.Collections;

public class LaserController : MovingObject {

	public Sprite spriteOne;
	public Sprite spriteTwo;
	public Sprite spriteThree;
	public Sprite spriteFour;

	void Awake(){
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	public override void ChangeSprite(){
		
		int dotProduct = Mathf.RoundToInt(Vector2.Dot (orientation, reference));
		
		switch (dotProduct) {
		case 1:
			spriteRenderer.sprite = spriteOne;
			break;
		case -1:
			spriteRenderer.sprite = spriteTwo;
			break;
		case 2:
			spriteRenderer.sprite = spriteThree;
			break;
		case -2:
			spriteRenderer.sprite = spriteFour;
			break;
		}
	}

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