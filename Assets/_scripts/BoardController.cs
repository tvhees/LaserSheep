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
	public Sprite laserBottom;
	public Sprite laserTop;
	public Sprite laserLeft;
	public Sprite laserRight;

    private List<Vector3> gridPositions = new List<Vector3>();
	private GameObject boardHolder;

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

    public void PlaceObject(GameObject newObject, int colMin, int colMax, int rowMin, int rowMax, int numberObjects, bool delete, string orientation)
    {
        for (int i = 0; i < numberObjects; i++) {
            Vector3 randomGrid = new Vector3(-1, -1, -1);
            while (randomGrid.x < colMin || randomGrid.x > colMax || randomGrid.y < rowMin || randomGrid.y > rowMax) {
                randomGrid = RandomGrid(delete);
            }
            GameObject instance = Instantiate(newObject, randomGrid, Quaternion.identity) as GameObject;
			instance.transform.SetParent (boardHolder.transform);

			SpriteRenderer renderer = instance.GetComponent<SpriteRenderer>();

			switch(orientation){
			case "BOTTOM":
				renderer.sprite = laserBottom;
				break;
			case "TOP":
				renderer.sprite = laserTop;
				break;
			case "LEFT":
				renderer.sprite = laserLeft;
				break;
			case "RIGHT":
				renderer.sprite = laserRight;
				break;
			case null:
				break;
			}

        }
    }

    public void StartGame (){
		if (boardHolder != null)
			Destroy (boardHolder);

		boardHolder = new GameObject ("Board");
        InitialiseGrid();
		PlaceObject(laser, 3, 8, 2, 2, numberLasers, false, "BOTTOM");
		PlaceObject(laser, 3, 8, 9, 9, numberLasers, false, "TOP");
		PlaceObject(laser, 2, 2, 3, 8, numberLasers, false, "LEFT");
		PlaceObject(laser, 9, 9, 3, 8, numberLasers, false, "RIGHT");

        GameObject[] startLasers = GameObject.FindGameObjectsWithTag("WaitLaser");
        foreach (GameObject startLaser in startLasers) {
			startLaser.tag = "ActiveLaser";
			startLaser.layer = 12;
		}
		PlaceObject(blueSheep, 3, 8, 3, 8, numberSheep, true, null);
        PlaceObject(redSheep, 3, 8, 3, 8, numberSheep, true, null);

    }

}
