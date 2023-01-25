using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTileScript : MonoBehaviour
{   
    // open connections to other tiles  
    public bool north,east,west,south,top,bottom;
    // for ingame use keep track of state of tile
    public bool isEmpty,hasGhost,hasPlayer,hasCherry,hasCoin;

    public Transform BasicTile;

    void Start() {
        isEmpty = true;
        hasGhost = false;   
        hasCherry = false;
        hasCoin = false;
        hasPlayer = false;
        buildTile();
    }

    private void buildTile(){
        foreach(Transform nextWall in transform){
            // find wall and check its state
            string name = nextWall.name;
            bool exsits;
            switch(name){
                case "N_wall": exsits = north; break;
                case "W_wall": exsits = west; break;
                case "S_wall": exsits = south; break;
                case "E_wall": exsits = east; break;
                case "Top": exsits = top; break;
                case "Bottom": exsits = bottom; break;
                default: exsits = true; break;
            }
            // remove wall if there should be an opening
            if(!exsits){
                Destroy(nextWall.gameObject);
            }
        }
    }
}
