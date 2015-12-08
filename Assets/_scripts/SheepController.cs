using UnityEngine;
using System.Collections;

public class SheepController : MovingObject {

	private Animator animator;
	private float blinkTime;

	void Awake(){
		spriteRenderer = GetComponent<SpriteRenderer> ();
		animator = GetComponent<Animator> ();

		blinkTime = Time.time + Random.Range (1.5f, 10.5f);
	}

	void Update(){
		if (Time.time > blinkTime) {
			animator.SetTrigger("sheepBlink");
			blinkTime = Time.time + Random.Range (4.5f, 6.5f);
		}
	}

	public override void ChangeSprite (){

		int dotProduct = Mathf.RoundToInt(Vector2.Dot (orientation, reference));

		if (dotProduct == 2 || dotProduct == 1)
			transform.rotation = Quaternion.Euler (0, 0, 0);
		else
			transform.rotation = Quaternion.Euler (0, 180f, 0);
	}

	// blocking method for sheep objects
	protected override int Blocking(Vector2 start, Vector2 direction){

		Vector2 point = start + direction;

		Collider2D otherCollider;

		while (point.x > 2 && point.x < 9 && point.y > 2 && point.y < 9){

			otherCollider = Physics2D.OverlapCircle(point, 0.2f);

			if(otherCollider == null){
				orientation = direction;
				ChangeSprite();
				return 1;
			}

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

			if(otherCollider == null){
				orientation = -direction;
				ChangeSprite();
				return -1;
			}

			if(otherCollider.GetComponent<SheepController>().moved == true)
				break;

			if (otherCollider.gameObject.tag != this.tag)
				break;

			point -=direction;
		}

		return 0;
	}

	public void LaserHit(){
		if (this.CompareTag("RedSheep"))
			PlayerColour.Instance.redScore--;
		else
			PlayerColour.Instance.blueScore--;

		Destroy (this.gameObject);
	}
}