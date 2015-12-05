using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardController : MonoBehaviour {

    public int columns = 10;
    public int rows = 10;
    public int numberSheep = 5;
	public int numberLasers = 1;
    public GameObject blueSheep;
    public GameObject redSheep;
	public GameObject laser;
	public GameObject activeLasers;
	public GameObject waitLasers;
	public GameObject redSheeps;
	public GameObject blueSheeps;

    private List<Vector3> gridPositions = new List<Vector3>();
	private GameObject instance;
	private bool[] sheepColours = new bool[2]{true, false};
	private string[] newLasers = new string[4]{"TOP","RIGHT","BOTTOM","LEFT"};
	private int laserIndex;

    void InitialiseGrid() {
        gridPositions.Clear();

        for (int x = 1; x < columns + 1; x++) {
            for (int y = 1; y < rows + 1; y++) {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    Vector3 RandomGrid(bool delete) {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomGrid = gridPositions[randomIndex];
        if (delete)
			gridPositions.RemoveAt(randomIndex);
        return randomGrid;
    }

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

    public void StartGame (){

		// Selecting a random start player colour
		PlayerColour.Instance.redSheep = sheepColours [Random.Range (0, 2)];

		// Set global player scores
		PlayerColour.Instance.redScore = 5;
		PlayerColour.Instance.blueScore = 5;

		// Place initial lasers
        InitialiseGrid();
		PlaceObject(laser, 3, 8, 2, 2, numberLasers, false, Vector2.up, activeLasers);
		PlaceObject(laser, 3, 8, 9, 9, numberLasers, false, -Vector2.up, activeLasers);
		PlaceObject(laser, 2, 2, 3, 8, numberLasers, false, -Vector2.right, activeLasers);
		PlaceObject(laser, 9, 9, 3, 8, numberLasers, false, Vector2.right, activeLasers);

		// Activate initial lasers
        GameObject[] startLasers = GameObject.FindGameObjectsWithTag("WaitLaser");
        foreach (GameObject startLaser in startLasers) {
			startLaser.tag = "ActiveLaser";
			startLaser.layer = 12;
		}

		// Place initial sheep
		PlaceObject(blueSheep, 3, 8, 3, 8, numberSheep, true, Vector2.up, blueSheeps);
		PlaceObject(redSheep, 3, 8, 3, 8, numberSheep, true, Vector2.up, redSheeps);

		// Set random start side for new lasers
		laserIndex = Random.Range (0, 4);
    }

	public IEnumerator updateLasers(){
		LaserController[] waitLaserList = waitLasers.transform.GetComponentsInChildren<LaserController> ();

		foreach (LaserController waitLaser in waitLaserList) {
			Vector2 laserPosition = waitLaser.transform.position;
			Vector2 direction;
			
			if (laserPosition.x == 1)
				direction = Vector2.right;
			else if (laserPosition.x == 10)
				direction = -Vector2.right;
			else if (laserPosition.y == 1)
				direction = Vector2.up;
			else
				direction = -Vector2.up;

			yield return StartCoroutine(waitLaser.Move(direction) );
		}
		
		//Instantiate a new "waiting" Laser
		// --> BoardController
		if (laserIndex > 3)
			laserIndex = 0;

		string orientation = newLasers[laserIndex];
		
		// This could be fed GameObjects directly
		switch (orientation) {
		case "BOTTOM":
			PlaceObject(laser, 3, 8, 1, 1, numberLasers, false, Vector2.up, waitLasers);
			break;
		case "TOP":
			PlaceObject(laser, 3, 8, 10, 10, numberLasers, false, -Vector2.up, waitLasers);
			break;
		case "LEFT":
			PlaceObject(laser, 1, 1, 3, 8, numberLasers, false, -Vector2.right, waitLasers);
			break;
		case "RIGHT":
			PlaceObject(laser, 10, 10, 3, 8, numberLasers, false, Vector2.right, waitLasers);
			break;
		}
		
		laserIndex++;
	}

}
