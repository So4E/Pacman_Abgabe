using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V2TileScript : MonoBehaviour
{   
    // open connections to other tiles will be set inside the world generator
    public bool b_north,b_east,b_west,b_south,north,east,west,south,isObstacle,isStair;
    public int stairLevel,stairHeading;

    // for ingame use keep track of state of tile
    public bool isEmpty,hasGhost,hasPlayer,hasCherry,hasCoin;

    void Start() {
        isEmpty = true;
        hasGhost = false;   
        hasCherry = false;
        hasCoin = false;
        hasPlayer = false;
    }

}
