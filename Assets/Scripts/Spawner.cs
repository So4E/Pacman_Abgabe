using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{   
    public GameObject rootNode;
    public Vector3 offset = new Vector3(1,0,0);
    public float globalScale = 1;

    [Range(0, 100)] public int portalCount = 10;
    [Range(0,100)]public int maxGhosts = 10;
    [Range(0f,60f)]public float ghostDelay = 0;
    [Range(0f,10f)]public float coinCherryDelay = 0;
    [Range(0,100)]public float coinDensity = 100;
    [Range(0,100)]public float cherryChance = 10;
    
    public string pathToSpawnables = "Spawnables";
    public string ghostFolderName = "Ghosts";
    public string coinFolderName = "Coins";
    public string cherryFolderName = "Cherrys";
    public string playerFolderName = "Player";
    public string portalFolderName = "Portals";
    private GameObject[] ghost_options;
    private GameObject[] cherry_options;
    private GameObject[] coin_options;
    private GameObject[] player_options;
    private GameObject[] portal_options;

    private float ghostDelayTimer, coinDelayTimer;

    private List<GameObject> emptyLocations = new List<GameObject>{};
    private List<GameObject> possibleLocations = new List<GameObject> {};

    private GameObject player,ghosts,coinsAndCherrys,portals;
    private int maxCoins;
    // Preparing Settings and Loading Prefabs
    private void Awake(){
        LoadPrefabs();
        coinsAndCherrys = Instantiate(new GameObject("!_Coins"), new Vector3(0,0,0), Quaternion.Euler(new Vector3(0,0,0)),rootNode.transform); 
        ghosts = Instantiate(new GameObject("!_Ghosts"), new Vector3(0,0,0), Quaternion.Euler(new Vector3(0,0,0)),rootNode.transform); 
        player = Instantiate(new GameObject("Player"), new Vector3(0,0,0), Quaternion.Euler(new Vector3(0,0,0)));
        portals = Instantiate(new GameObject("!_Portals"), new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0)),rootNode.transform);
        ghostDelayTimer = coinDelayTimer = 0f;
        //Instantiate(player, new Vector3(0,20,0), Quaternion.Euler(new Vector3(0,0,0)));
    }
    // Taking in the Map calculate all places suitable for spawning Objects
    private void Start() {
        GameObject[,,] worldMesh = rootNode.GetComponent<LevelRoot>().worldMesh;
        Debug.Log(worldMesh);
        for (int z = 0; z < worldMesh.GetLength(0); z++){
           for (int x = 0; x < worldMesh.GetLength(1); x++){
             for (int y = 0; y < worldMesh.GetLength(2); y++){
                bool isObstacle,isStair;
                GameObject tile = worldMesh[z,x,y];
                if(tile != null){// Upper floor do have null tiles "Air"
                    isObstacle = tile.GetComponent<V2TileScript>().isObstacle;
                    isStair = tile.GetComponent<V2TileScript>().isStair;
                    if(!isObstacle && !isStair){ // Everywhere where there is no Obstacle or Stair Coins/Cherrys and Ghosts can spawn
                        emptyLocations.Add(tile);
                    }
                }
            }
           }
        }
        possibleLocations = emptyLocations;
        maxCoins = (int)(emptyLocations.Count / 100 * coinDensity) - maxGhosts - 1;

        SpawnAll();
    }

    // The Delay is an easy way to stop Objects spawning on or right next to the Player
    private void Update() {
        if (ghostDelayTimer > ghostDelay)
        {
            SpawnGhost();
            ghostDelayTimer = 0f;
        }
        else if (ghosts.transform.childCount < maxGhosts)
        {
            ghostDelayTimer += Time.deltaTime;
        }
    }

    private void LoadPrefabs(){

        coin_options = Resources.LoadAll<GameObject>(pathToSpawnables+"/"+coinFolderName);
        cherry_options = Resources.LoadAll<GameObject>(pathToSpawnables+"/"+cherryFolderName);
        ghost_options = Resources.LoadAll<GameObject>(pathToSpawnables+"/"+ghostFolderName);
        player_options = Resources.LoadAll<GameObject>(pathToSpawnables+"/"+playerFolderName);
        portal_options = Resources.LoadAll<GameObject>(pathToSpawnables + "/" + portalFolderName);


    }

    private void SpawnAll(){
        SpawnPlayer();

        while (ghosts.transform.childCount < maxGhosts){
            SpawnGhost();
        }        
        while(coinsAndCherrys.transform.childCount < maxCoins){
            SpawnCoinOrCherry();
        }
        while (portals.transform.childCount <= portalCount)
        {
            SpawnPortal();
        }
    }

    private void SpawnObject(GameObject[] choices,Vector3 position,Transform t,string name){
        GameObject choice = choices[Random.Range(0,choices.Length)];
        var newObject = Instantiate(choice, position, Quaternion.Euler(new Vector3(0,0,0)), t); 
        newObject.transform.localScale += new Vector3(globalScale,globalScale,globalScale);
        newObject.name = name;
    }
    private void SpawnGhost(){
        int index = Random.Range(0,emptyLocations.Count);
        Vector3 nextPosition = possibleLocations[index].transform.position + offset + new Vector3(0,20,0); 
        SpawnObject(ghost_options,nextPosition,ghosts.transform,$"{nextPosition}Ghost");
    }
    private void SpawnCoinOrCherry(){
        int index = Random.Range(0,emptyLocations.Count);
        Vector3 nextPosition = emptyLocations[index].transform.position + offset;
        // Randomly pick Coin or Cherry
        GameObject[] cORc;
        string name;
        if(Random.Range(0,100) <= cherryChance){
            cORc = cherry_options; 
            name = $"{nextPosition}Cherry";
        }
        else{
            cORc = coin_options; 
            name = $"{nextPosition}Dot";
        }
        SpawnObject(cORc,nextPosition,coinsAndCherrys.transform,name);
        emptyLocations.RemoveAt(index);
    }
    private void SpawnPlayer(){
        int index = Random.Range(0,emptyLocations.Count);
        Vector3 nextPosition = emptyLocations[index].transform.position + offset + new Vector3(0,10,0); 
        SpawnObject(player_options,nextPosition,player.transform,$"Player");
        emptyLocations.RemoveAt(index);
    }

    private void SpawnPortal()
    {
        int index = Random.Range(0, emptyLocations.Count);
        Vector3 nextPosition = emptyLocations[index].transform.position + offset + new Vector3(0, 0, 0);
        SpawnObject(portal_options, nextPosition, portals.transform, $"Portal");
        emptyLocations.RemoveAt(index);
        Debug.Log("Portal Yippi");
    }
}

