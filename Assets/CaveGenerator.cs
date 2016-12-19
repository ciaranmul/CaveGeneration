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

	[Range(0,25)]
	public int smoothing;

	[Range(0,100)]
	public int fillChance;

	public string seed;


	// Tile map
	bool[,] map;

	// Use this for initialization
	void Start () {
		map = new bool[width, height];
		map = initialiseMap (map);
		for(int i=0; i<smoothing; i++){
			map = doSmoothing (map);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
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

	bool[,] initialiseMap(bool[,] map) {
		System.Random rand = checkForSeed ();
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (rand.Next (0, 100) < fillChance) {
					map [x, y] = true;
				}
			}
		}
		return map;
	}

	bool[,] doSmoothing(bool[,] oldMap){
		bool[,] newMap = new bool[width,height];
		for (int x=0; x<oldMap.GetLength(0); x++){
			for (int y=0; y<oldMap.GetLength(1); y++){
				int neighbours = countNeighbours (oldMap, x, y);
				if (oldMap [x, y]) {
					if (neighbours < deathLimit) {
						newMap [x, y] = false;
					} else {
						newMap [x, y] = true;
					}
				} else {
					if (neighbours > birthLimit) {
						newMap [x, y] = true;
					} else {
						newMap [x, y] = false;
					}
				}
			}
		}
		return newMap;
	}

	int countNeighbours(bool[,] map, int x, int y){
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
				else if (map[neighbour_x,neighbour_y]){
					// neighbour is alive
					count++;
				}
			}
		}
		return count;
	}

	void OnDrawGizmos() {
		if (map != null) {
			for (int x = 0; x < width; x ++) {
				for (int y = 0; y < height; y ++) {
					Gizmos.color = Color.black;
					if (map [x, y] == true) {
						Vector3 pos = new Vector3 (-width / 2 + x + .5f, 0.5f, -height / 2 + y + .5f);
						Gizmos.DrawCube (pos, Vector3.one);
					}
				}
			}
		}
	}
}
