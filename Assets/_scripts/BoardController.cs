using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardController : MonoBehaviour {
	
    public int numberSheep = 5;
	public int numberLasers = 1;
    public GameObject blueSheep;
    public GameObject redSheep;
	public GameObject laser;
	public GameObject activeLasers;
	public GameObject waitLasers;
	public GameObject redSheeps;
	public GameObject blueSheeps;

	private int columns = 10;
	private int rows = 10;
    private List<Vector3> gridPositions = new List<Vector3>();
	private bool[] sheepColours = new bool[2]{true, false};
	private Vector2[] newLasers = new Vector2[4]{-Vector2.up,-Vector2.right,Vector2.up,Vector2.right};
	private int laserIndex;

	// Called when starting a local multiplayer game
	public void StartGame (){
		
		// Selecting a random start player colour
		PlayerColour.Instance.redSheep = sheepColours [Random.Range (0, 2)];
		
		// Set global player scores
		PlayerColour.Instance.redScore = 5;
		PlayerColour.Instance.blueScore = 5;
		
		// Place and activate initial lasers
		InitialiseGrid();
		PlaceObject(laser, 3, 8, 1, 1, numberLasers, false, Vector2.up, waitLasers);
		PlaceObject(laser, 3, 8, 10, 10, numberLasers, false, -Vector2.up, waitLasers);
		PlaceObject(laser, 1, 1, 3, 8, numberLasers, false, Vector2.right, waitLasers);
		PlaceObject (laser, 10, 10, 3, 8, numberLasers, false, -Vector2.right, waitLasers);
		StartCoroutine (updateLasers ());
		
		// Place initial sheep without overlap
		PlaceObject(blueSheep, 3, 8, 3, 8, numberSheep, true, Vector2.up, blueSheeps);
		PlaceObject(redSheep, 3, 8, 3, 8, numberSheep, true, Vector2.up, redSheeps);
		
		// Set random start side for new lasers
		laserIndex = Random.Range (0, 4);
	}

	// Create list of grid positions. 3-8 are inside the pen for both axes
    void InitialiseGrid() {
        gridPositions.Clear();

        for (int x = 1; x < columns + 1; x++) {
            for (int y = 1; y < rows + 1; y++) {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

	// Select a random grid with the option to remove that grid from the list, avoiding duplication
    Vector3 RandomGrid(bool delete) {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomGrid = gridPositions[randomIndex];
        if (delete)
			gridPositions.RemoveAt(randomIndex);
        return randomGrid;
    }

	// Place new sheep or lasers
    public void PlaceObject(GameObject newObject, int colMin, int colMax, int rowMin, int rowMax, int numberObjects, bool delete, Vector2 orientation, GameObject parentObject)
    {
        for (int i = 0; i < numberObjects; i++) {
            Vector3 randomGrid = new Vector3(-1, -1, -1);
            while (randomGrid.x < colMin || randomGrid.x > colMax || randomGrid.y < rowMin || randomGrid.y > rowMax) {
                randomGrid = RandomGrid(delete);
            }
            GameObject instance = Instantiate(newObject, randomGrid, Quaternion.identity) as GameObject;
			instance.transform.SetParent (parentObject.transform);
			instance.GetComponent<MovingObject>().orientation = orientation;
			instance.GetComponent<MovingObject>().ChangeSprite();
        }
    }

	// Move waiting lasers forward and activate them, then make a new waiting laser
	public IEnumerator updateLasers(){
		LaserController[] waitLaserList = waitLasers.transform.GetComponentsInChildren<LaserController> ();

		foreach (LaserController waitLaser in waitLaserList) {
			Vector2 direction = waitLaser.orientation;
			yield return StartCoroutine(waitLaser.Move(direction));
			waitLaser.transform.SetParent(activeLasers.transform);
			waitLaser.tag = "ActiveLaser";
			waitLaser.gameObject.layer = 12;
		}
		
		//Instantiate a new "waiting" Laser
		if (laserIndex > 3)
			laserIndex = 0;

		Vector2 orientation = newLasers[laserIndex];

		int dotProduct = Mathf.RoundToInt(Vector2.Dot (orientation, PlayerColour.Instance.reference));

		// This could be fed GameObjects directly
		switch (dotProduct) {
		case 1:
			PlaceObject(laser, 3, 8, 1, 1, numberLasers, false, Vector2.up, waitLasers);
			break;
		case -1:
			PlaceObject(laser, 3, 8, 10, 10, numberLasers, false, -Vector2.up, waitLasers);
			break;
		case 2:
			PlaceObject(laser, 1, 1, 3, 8, numberLasers, false, Vector2.right, waitLasers);
			break;
		case -2:
			PlaceObject(laser, 10, 10, 3, 8, numberLasers, false, -Vector2.right, waitLasers);
			break;
		}
		
		laserIndex++;
	}

}
