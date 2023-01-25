using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Range(3, 300)]
    public int levelsize = 3;
    [Range(1, 10)]
    public int floorCount = 1;
    [Range(-.5f, .5f)]
    public float openess = 0f;
    public float xY_offset = 2f;// offset between each tile -> toms tiles 4*6 = 24
    public float z_offset = 1f;

    // the tile which should be used for generation, we can use any tile as long it uses the BasicTileScript
    public Transform BasicTile;
    private Transform[,,] worldMesh;

    private float[] rotationOfTiles = { 90, -90, 180 }; // rotate tiles to get more variaty

    void Awake() {
        worldMesh = new Transform[floorCount, levelsize, levelsize];
        generateWorld();
    }

    // generates the world starting with the ground floor 
    private void generateWorld() {
        var nextPosition = new Vector3(0, 0, 0);
        for (int z = 0; z < floorCount; z++) {
            for (int x = 0; x < levelsize; x++) {
                for (int y = 0; y < levelsize; y++) {
                    Transform tile = spawnNewTile(nextPosition);
                    worldMesh[z, x, y] = tile;
                    setWalls(tile, z, x, y);
                    nextPosition.z += xY_offset;
                }
                nextPosition.z = 0;
                nextPosition.x += xY_offset;
            }
            nextPosition.z = 0;
            nextPosition.x = 0;
            nextPosition.y += z_offset;
        }
    }

    // spawns and returns a new tile at given position
    private Transform spawnNewTile(Vector3 position) {

        Vector3 rotationOffest = new Vector3(0, rotationOfTiles[Random.Range(0, rotationOfTiles.Length)], 0);
        Transform newTile = Instantiate(BasicTile, position, Quaternion.Euler(rotationOffest));

        // Transform newTile = Instantiate(BasicTile, position, Quaternion.identity);
        newTile.name = position.ToString();
        return newTile;
    }

    // set walls for tile
    private void setWalls(Transform tile, int pos_z, int pos_x, int pos_y) {


        // second randomWalls ensure at least two openings
        bool northWall, southWall, eastWall, westWall;
        bool[] options = { false, false, Random.value >= .5f + openess, Random.value >= .5f + openess };
        for (int i = 3; i > 0; i--) {
            int r = Random.Range(0, i);
            bool b = options[r];
            options[r] = options[i];
            options[i] = b;
        }
        northWall = options[0];
        southWall = options[1];
        westWall = options[2];
        eastWall = options[3];

        // first random walls no openings ensured
        // bool northWall =  Random.value >= .5f + openess;
        // bool southWall =  Random.value >= .5f + openess;
        // bool eastWall  =   Random.value >= .5f + openess;
        // bool westWall  =   Random.value >= .5f + openess;

        // assure map is playable by adding needed walls and removing those in the way
        // first checking the outlines adding walls so it is not possible to jump of the map
        if (isNorthBorder(pos_x)) {
            northWall = true;
        } else if (isSouthBorder(pos_x)) {
            southWall = true;
        }

        if (isEastBorder(pos_y)) {
            eastWall = true;
        } else if (isWestBorder(pos_y)) {
            westWall = true;
        }

        // check how the neighbouring tiles look like to avoid deadends. We check if they open towards us
        // At the time of generation each tile can have a maximum of two neighbours one to the west and
        // one to the north
        if (!isWestBorder(pos_y)) {
            Transform leftNeighbour = worldMesh[pos_z, pos_x, pos_y - 1];
            if (!leftNeighbour.gameObject.GetComponent<BasicTileScript>().east) {
                westWall = false;
            }
        }
        if (!isNorthBorder(pos_x)) {
            Transform aboveNeighbour = worldMesh[pos_z, pos_x - 1, pos_y];
            if (!aboveNeighbour.gameObject.GetComponent<BasicTileScript>().south) {
                northWall = false;
            }
        }

        Debug.Log($"{northWall},{southWall},{westWall},{eastWall}");

        // edit the tile
        tile.gameObject.GetComponent<BasicTileScript>().north = northWall;
        tile.gameObject.GetComponent<BasicTileScript>().south = southWall;
        tile.gameObject.GetComponent<BasicTileScript>().east = eastWall;
        tile.gameObject.GetComponent<BasicTileScript>().west = westWall;
    }

    // Checks if at side of the map
    private bool isEastBorder(int x) {
        return x == levelsize - 1;
    }
    private bool isWestBorder(int x) {
        return x == 0;
    }
    private bool isSouthBorder(int y) {
        return y == levelsize - 1;
    }
    private bool isNorthBorder(int y) {
        return y == 0;

    }


}
