using UnityEngine;
using System.Collections;

public class PlayerColour : Singleton<PlayerColour> {
	protected PlayerColour () {}

	public bool redSheep = true;
	public int redScore;
	public int blueScore;
	public Vector2 reference = new Vector2(2,1);

}
