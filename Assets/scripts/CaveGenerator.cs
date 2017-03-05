using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveGenerator : MonoBehaviour {

	// PUBLIC VARIABLES EXPOSED TO THE EDITOR
	public int width;
	public int height;

	[Range(0,8)]
	public int deathLimit;

	[Range(0,8)]
	public int birthLimit;

	[Range(0,8)]
	public int treasureLimit;

	[Range(0,25)]
	public int smoothing;

	[Range(0,100)]
	public int fillChance;

	public string seed;


	// Tile map
	GridPiece[,] map;

	// Wall Transform
	public Transform Wall;

	// Treasure Transform
	public Transform Treasure;

	// Use this for initialization
	void Start () {
		map = new GridPiece[width, height];
		map = initialiseMap (map);
		for(int i=0; i<smoothing; i++){
			map = doSmoothing (map);
		}
		//populateTreasure (map);
		populateGameObjects ();
	}

	// Update is called once per frame
	void Update () {

	}

	public enum GridPiece
    {
        None = 0,
        Wall = 1,
        Treasure = 2
    }

	System.Random checkForSeed(){
		System.Random rand;

		if (seed == "") {
			rand = new System.Random ();
		} else {
			rand = new System.Random (seed.GetHashCode ());
		}

		return rand;
	}

	GridPiece[,] initialiseMap(GridPiece[,] map) {
		System.Random rand = checkForSeed ();
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (rand.Next (0, 100) < fillChance) {
					map [x, y] = GridPiece.Wall;
				}
			}
		}
		return map;
	}

	GridPiece[,] doSmoothing(GridPiece[,] oldMap){
		GridPiece[,] newMap = new GridPiece[width,height];
		for (int x=0; x<oldMap.GetLength(0); x++){
			for (int y=0; y<oldMap.GetLength(1); y++){
				int neighbours = countNeighbours (oldMap, x, y);
				if (oldMap [x, y] == GridPiece.Wall) {
					if (neighbours < deathLimit) {
						newMap [x, y] = GridPiece.None;
					} else {
						newMap [x, y] = GridPiece.Wall;
					}
				} else {
					if (neighbours > birthLimit) {
						newMap [x, y] = GridPiece.Wall;
					} else {
						newMap [x, y] = GridPiece.None;
					}
				}
			}
		}
		return newMap;
	}

	int countNeighbours(GridPiece[,] map, int x, int y){
		int count = 0;
		for (int i=-1; i<2; i++){
			for (int j=-1; j<2; j++){
				int neighbour_x = x + i;
				int neighbour_y = y + j;
				if (i == 0 && j == 0) {
					// looking at middle point, do nothing
				} else if (x == 0 || y == 0 || neighbour_x >= (map.GetLength (0)-1) || neighbour_y >= (map.GetLength (1)-1)) {
					// neighbour is at edge of map
					count++;
				}
				else if (map[neighbour_x,neighbour_y] == GridPiece.Wall){
					// neighbour is alive
					count++;
				}
			}
		}
		return count;
	}

	void populateTreasure(GridPiece[,] map){
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (map [x, y] == GridPiece.None) {
					int nbrs = countNeighbours (map, x, y);
					if (nbrs >= treasureLimit) {
						map [x,y] = GridPiece.Treasure;
					}
				}
			}
		}
	}


	void populateGameObjects() {
		if (map != null) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					if (map [x,y] == GridPiece.Wall) {
						Vector3 pos = new Vector3 (-width / 2 + x + .5f, 5, -height / 2 + y + .5f);
						Instantiate (Wall, pos, Quaternion.identity);
					} else if (map [x,y] == GridPiece.Treasure){
						Vector3 pos = new Vector3 (-width / 2 + x + .5f, 0.5f, -height / 2 + y + .5f);
						Instantiate (Treasure, pos, Quaternion.identity);
					}
				}
			}
		}
	}

	/*void OnDrawGizmos() {
		if (map != null) {
			for (int x = 0; x < width; x ++) {
				for (int y = 0; y < height; y ++) {
					Gizmos.color = Color.black;
					if (map [x, y] == GridPiece.Wall) {
						Vector3 pos = new Vector3 (-width / 2 + x + .5f, 0.5f, -height / 2 + y + .5f);
						Gizmos.DrawCube (pos, Vector3.one);
					}
				}
			}
		}
	}*/
}
