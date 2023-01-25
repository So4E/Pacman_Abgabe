using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerationV2 : MonoBehaviour
{   
    public GameObject rootNode;
    // Generator settings 
    [Range(3,300)]public int levelsize = 3;
    [Range(1,5)] public int floorCount = 1;
    [Range(-.5f,.5f)]public float openess = 0f; 
    [Range(0f,1f)] public float globalScale = 1f;
    public int maxStairs = 30;
    public float xY_offset = 2f;
    public float z_offset = 1f;
    public string pathToWorldGenerationPrefabs = "WorldGenerationTileSetV1";
    public string floorFolderName = "Floors";
    public string borderFolderName = "Borders";
    public string stairFolderName = "Stairs";
    public string wallFolderName = "Walls";
    public string obstacleFolderName = "Obstacle";
    public string galleryFloorFolderName = "GalleryFloors";
    // Constants for spawning in elements
    private Vector3 n_posV3= new Vector3(-2,.5f,-1);
    private Vector3 s_posV3= new Vector3(0,.5f,-1);
    private Vector3 w_posV3= new Vector3(1,.5f,0);
    private Vector3 e_posV3= new Vector3(-1,.5f,0);
    private Vector3 n_rotV3= new Vector3(0,0,0);
    private Vector3 s_rotV3= new Vector3(0,0,0);
    private Vector3 w_rotV3= new Vector3(0,-90,0);
    private Vector3 e_rotV3= new Vector3(0,90,0);
    // Our world as 3d array
    private GameObject[,,] worldMesh;
    // Tilesets
    GameObject[] wall_options;
    GameObject[] border_options;
    GameObject[] stair_options;
    GameObject[] floor_options;
    GameObject[] opstacle_options;
    GameObject[] gallery_options;
    // ruleSet for our next Tile
    bool northWall,southWall,eastWall,westWall,northBorder,southBorder,eastBorder ,westBorder ,isObstacle, isStair;
    int stairLevel,stairHeading;

    private void Awake() {

        if(LoadPrefabs()){
            worldMesh= new GameObject[floorCount,levelsize,levelsize];
            //floorDensityMap = new float[5]{floorDensity_f1,floorDensity_f2,floorDensity_f3,floorDensity_f4,floorDensity_f5};
            GenerateWorld();
        }else{
            Debug.LogError("PAUL_ERROR: Can not generate world, there are either not enough prefab models or the once provided failed to load -> Tom check your folder structur");
        }

        // apply global scale
        rootNode.transform.localScale += new Vector3(globalScale,globalScale,globalScale);

        // pass mesh to root so we can refer to our level parts ingame more easily
        rootNode.GetComponent<LevelRoot>().worldMesh = worldMesh; 


        // clean up
        Destroy(this.gameObject);
        Destroy(this);
        
    }

    private void GenerateWorld(){
        var nextPosition = new Vector3(0,0,0);
        int x,y,z;
        z = x = y = 0;

        // Groundfloor
        bool[,][] mazeMap = Create2DMaze(); 
        bool[,] opsMap = CreateObstacleMap(mazeMap);
        int[,][] stairMap = CreateStairMap(opsMap,z);
        mazeMap = CleanUpWallsAroundObstacles(mazeMap,opsMap);
        (opsMap,mazeMap) = CleanUpObstaclesAndWallsBlockingStairs(opsMap,mazeMap,stairMap);

        for(x = 0; x < levelsize; x++){
            for(y = 0; y < levelsize; y++){
                GameObject nextTile = SpawnNewTile(nextPosition,$"({z}|{x}|{y})");
                worldMesh[z,x,y] = nextTile;
                AssignTileRules(nextTile,mazeMap[x,y],opsMap[x,y],stairMap[x,y],z,x,y);
                PopulateTile(nextTile.transform,nextPosition);
                nextPosition.z += xY_offset; 
            }
            nextPosition.z = 0;
            nextPosition.x += xY_offset;
        }
    
        // All following floors
        for(z = 1; z < floorCount; z++){
           List<(int,int)> listOfFloorTilesOnCurrentFloor = ConnectAllStairExitPoints(z,stairMap,opsMap);
            foreach((int,int)tile in listOfFloorTilesOnCurrentFloor){   
                if(worldMesh[z,tile.Item1,tile.Item2] == null){
                    nextPosition = new Vector3(tile.Item1*xY_offset,z*z_offset,tile.Item2*xY_offset);
                    GameObject nextTile = SpawnNewTile(nextPosition,$"({z}|{tile.Item1}|{tile.Item2})");
                    worldMesh[z,tile.Item1,tile.Item2] = nextTile;
                }
            }
        }
    }

    private void PopulateTile(Transform t,Vector3 position){
        // borders are prefered over walls
        if(northBorder){Instantiate(border_options[Random.Range(0,border_options.Length)], position + n_posV3, Quaternion.Euler(n_rotV3)).transform.SetParent(t);}
        else if(northWall){Instantiate(wall_options[Random.Range(0,wall_options.Length)], position + n_posV3, Quaternion.Euler(n_rotV3)).transform.SetParent(t);}
        if(southBorder){Instantiate(border_options[Random.Range(0,border_options.Length)], position + s_posV3, Quaternion.Euler(s_rotV3)).transform.SetParent(t);}
        else if(southWall){Instantiate(wall_options[Random.Range(0,wall_options.Length)], position + s_posV3, Quaternion.Euler(s_rotV3)).transform.SetParent(t);}
        if(eastBorder){Instantiate(border_options[Random.Range(0,border_options.Length)], position + e_posV3, Quaternion.Euler(e_rotV3)).transform.SetParent(t);}
        else if(eastWall){Instantiate(wall_options[Random.Range(0,wall_options.Length)], position + e_posV3, Quaternion.Euler(e_rotV3)).transform.SetParent(t);}
        if(westBorder){Instantiate(border_options[Random.Range(0,border_options.Length)], position + w_posV3, Quaternion.Euler(w_rotV3)).transform.SetParent(t);}
        else if(westWall){Instantiate(wall_options[Random.Range(0,wall_options.Length)], position + w_posV3, Quaternion.Euler(w_rotV3)).transform.SetParent(t);}
        // obstacles (with random rotation)
        if(isObstacle){
            int randomRotation = new int[]{0,90,180,270}[Random.Range(0,4)];
            Instantiate(opstacle_options[Random.Range(0,opstacle_options.Length)],position, Quaternion.Euler(new Vector3(-90,0,randomRotation))).transform.SetParent(t);
        }
        // stairs
        if(isStair){
            // heading 0 = N; 1 = S; 2= W; 3= E
            int stairRot = 0;
            switch (stairHeading){case 0 : stairRot = 0; break;case 1 : stairRot = 180; break;case 2 : stairRot = 270; break;case 3 : stairRot = 90; break;}
            Instantiate(stair_options[Random.Range(0,stair_options.Length)],position+new Vector3(0,stairLevel,0), Quaternion.Euler(new Vector3(-90,0,stairRot))).transform.SetParent(t);
        }
    }

    private void AssignTileRules(GameObject tile,bool[] mazeInfo,bool opsInfo,int[] stairInfo,int pos_z,int pos_x,int pos_y){
        
        ResetRules();

        // BORDERS :
        // first checking the outlines adding borders so it is not possible to jump of the map
        if(isNorthBorder(pos_x)){northBorder = true;}
        else if(isSouthBorder(pos_x)){southBorder = true;}
        if(isEastBorder(pos_y)){eastBorder = true;}
        else if(isWestBorder(pos_y)){westBorder = true;}

        // WALLS AND OBSTACLES :
        // order: {north,south,west,east} so a[2] = bool for west
        if(opsInfo){
            isObstacle = true;
        }else{
            northWall = mazeInfo[0];
            southWall = mazeInfo[1];
            eastWall = mazeInfo[2];
            westWall = mazeInfo[3];
        }

        // STAIRS
        if(stairInfo[0] != -1){
            isStair = true;
            stairHeading = stairInfo[0];
            stairLevel = stairInfo[1];
        }


        // edit the tile
        tile.GetComponent<V2TileScript>().north = northWall;
        tile.GetComponent<V2TileScript>().south = southWall;
        tile.GetComponent<V2TileScript>().east = eastWall;
        tile.GetComponent<V2TileScript>().west = westWall;
        tile.GetComponent<V2TileScript>().b_north = northBorder;
        tile.GetComponent<V2TileScript>().b_south = southBorder;
        tile.GetComponent<V2TileScript>().b_east = eastBorder;
        tile.GetComponent<V2TileScript>().b_west = westBorder;
        tile.GetComponent<V2TileScript>().isObstacle = isObstacle;
        tile.GetComponent<V2TileScript>().isStair = isStair;
        tile.GetComponent<V2TileScript>().stairLevel = stairLevel;
        tile.GetComponent<V2TileScript>().stairHeading = stairHeading;
    }

    private void ResetRules(){
        stairLevel=stairHeading = 0;
        northWall=southWall=westWall=eastWall=northBorder=southBorder=westBorder=eastBorder=isObstacle=isStair = false;
    }

    private GameObject SpawnNewTile(Vector3 position,string name){
        GameObject choice;
        if(position.y < 1){choice = floor_options[Random.Range(0,floor_options.Length)];}
        else{choice = gallery_options[Random.Range(0,gallery_options.Length)];}
        var newTile = Instantiate(choice, position, Quaternion.Euler(new Vector3(-90,0,0)), rootNode.transform); 
        newTile.name = name;
        return newTile;
    }

    private bool LoadPrefabs(){
        wall_options = Resources.LoadAll<GameObject>(pathToWorldGenerationPrefabs+"/"+wallFolderName);
        border_options = Resources.LoadAll<GameObject>(pathToWorldGenerationPrefabs+"/"+borderFolderName);
        stair_options = Resources.LoadAll<GameObject>(pathToWorldGenerationPrefabs+"/"+stairFolderName);
        floor_options = Resources.LoadAll<GameObject>(pathToWorldGenerationPrefabs+"/"+floorFolderName);
        opstacle_options = Resources.LoadAll<GameObject>(pathToWorldGenerationPrefabs+"/"+obstacleFolderName);
        gallery_options = Resources.LoadAll<GameObject>(pathToWorldGenerationPrefabs+"/"+galleryFloorFolderName);

        bool success = true;
        if(wall_options.Length < 1){success = false;}    
        if(border_options.Length < 1){success = false;}  
        if(stair_options.Length < 1){success = false;}  
        if(floor_options.Length < 1){success = false;}  
        if(opstacle_options.Length < 1){success = false;}
        if(gallery_options.Length < 1){success = false;}

        return success;
    }
    /*#################################### GROUNDFLOOR #################################################################################################*/
    // Ruleset.: 
    // 1. Random walls are build (affected by openess Bias) -> RandBool()
    // 2. We ensure every tile has at least one entrance and one exit, if not it gets corrected -> EnsureOpenToNeighbours()
    // 3. We ensure that we ar not blocking entrances/exits of neighbouring tiles, if not corrected -> EnsureOpenToNeighbours()
    // 4. matchingNeighbouringWalls() doubles all walls(in width) so we dont have ZigZag (This is mainly cosmetic)
    // returns a boolean map for a playable maze
    private bool[,][] Create2DMaze(){
        bool[,][] maze = new bool[levelsize,levelsize][];
        for(int x = 0; x < levelsize; x++){
            for(int y = 0; y < levelsize; y++){
                if(x == 0 || y == 0 || x == levelsize-1 || y == levelsize-1){
                    // order: {north,south,west,east} so a[2] = bool for west
                    maze[x,y] = new bool[]{false,false,false,false};
                }else{
                    bool[] current= new bool[]{RandBool(),RandBool(),RandBool(),RandBool()}; 
                    maze[x,y] = EnsureOpenToNeighbours(maze,EnsureTwoOpenMinimum(current),x,y);
                    current = MatchNeighbouringWalls(maze,current,x,y);
                }
            }  
        }  
        return maze;
    }

    private bool[] EnsureTwoOpenMinimum(bool[] current){
        while(SumAsInt(current) > 2){
            current[Random.Range(0,4)] = false;
        }
        return current;
    }

    private bool[] EnsureOpenToNeighbours(bool[,][] maze, bool[] current, int x, int y){
        bool[] leftNeighbour = maze[x,y-1];
        if(!leftNeighbour[3]){
            current[2] = false;
        }
        bool[] aboveNeighbour = maze[x-1,y];
        if(!aboveNeighbour[1]){
            current[0] = false;
        }
        return current;
    }

    private bool[] MatchNeighbouringWalls(bool[,][] maze, bool[] current, int x, int y){
        current[2] = maze[x,y-1][3]; // westWall - eastWall of Neighbour match
        current[0] = maze[x-1,y][1]; // northWall - southWall of Neighbour match
        return current;
    }

    private bool[,] CreateObstacleMap(bool[,][] mazeMap){
        bool[,] obsMap = new bool[levelsize,levelsize];
        for (int x = 0; x < levelsize; x++){
            for (int y = 0; y < levelsize; y++){
                if(SumAsInt(mazeMap[x,y]) >= 3){ obsMap[x,y] = true; }
                else{obsMap[x,y] = false;}
            }
        }
        return obsMap;
    }

    private int[,][]CreateStairMap(bool[,] opsMap,int currentFloor){
        int[,][] stairMap = new int[levelsize,levelsize][];
        // fill empty stairMap
        for (int x = 0; x < levelsize; x++){
            for (int y = 0; y < levelsize; y++){
                stairMap[x,y] = new int[]{-1,0};
            }
        }

        for (int x = 1; x < levelsize; x++){
            for (int y = 1; y < levelsize; y++){
                if(x%3 == 0 && y%3 ==0 && opsMap[x,y] && stairMap[x,y][0] == -1){
                    if(opsMap[x,y-1] && stairMap[x,y-1][0] == -1)     {stairMap[x,y][0] = stairMap[x,y-1][0]  = 3; stairMap[x,y][1] = 1; } // left option
                    else if(opsMap[x,y+1] && stairMap[x,y+1][0] == -1){stairMap[x,y][0] = stairMap[x,y+1][0]  = 2; stairMap[x,y][1] = 1; } //right option
                    else if(opsMap[x-1,y] && stairMap[x-1,y][0] == -1){stairMap[x,y][0] = stairMap[x-1,y][0]  = 1; stairMap[x,y][1] = 1; } //above option
                    else if(opsMap[x+1,y] && stairMap[x+1,y][0] == -1){stairMap[x,y][0] = stairMap[x+1,y][0]  = 0; stairMap[x,y][1] = 1; } //below option
                }
            }
        }

        return stairMap;
    }
    //For a better looking World
    private bool[,][] CleanUpWallsAroundObstacles(bool[,][] mazeMap,bool[,] obsMap){
        for (int x = 0; x < levelsize; x++){
            for (int y = 0; y < levelsize; y++){
                if(obsMap[x,y]){
                    mazeMap[x,y-1][3] = false;//left
                    mazeMap[x,y+1][2] = false;//right
                    mazeMap[x-1,y][1] = false;//above
                    mazeMap[x+1,y][0] = false;//below
                }
            }
        }
        return mazeMap;
    }
    private (bool[,], bool[,][]) CleanUpObstaclesAndWallsBlockingStairs(bool[,] opsMap,bool[,][] mazeMap,int[,][] stairMap){
        for (int x = 0; x < levelsize; x++){
            for (int y = 0; y < levelsize; y++){
                if(stairMap[x,y][0] != -1){ //at stair location
                    opsMap[x,y] = false;
                    // at the tile leading to the stair (only for bottom part of stair)
                    if(stairMap[x,y][1] == 0){ // is bottom part of stair?
                        mazeMap[x,y][OppositeWallIndex(stairMap[x,y][0])] = false;
                        (int,int) entranceTile = (0,0);
                        (int,int) exitTile = (0,0);
                        switch (stairMap[x,y][0]){ // find x,y of entranceTile
                            case 0 : entranceTile = (x+1,y); exitTile = (x-2,y); break;case 1 : entranceTile = (x-1,y); exitTile = (x+2,y); break;
                            case 2 : entranceTile = (x,y+1); exitTile = (x,y-2); break;case 3 : entranceTile = (x,y-1); exitTile = (x,y+2); break;}
                        // clear entrance
                        int e_x = entranceTile.Item1 ; int e_y = entranceTile.Item2;
                        opsMap[e_x,e_y] = false;
                        mazeMap[e_x,e_y][stairMap[x,y][0]] = false;
                        // clear exit
                        e_x = exitTile.Item1 ; e_y = exitTile.Item2;
                        opsMap[e_x,e_y] = false;
                        mazeMap[e_x,e_y][stairMap[x,y][0]] = false;
                    }
                }

            }
        }
        return (opsMap,mazeMap);
    }
    
    /*#################################### ALL OTHER FLOORS #################################################################################################*/
    private List<(int,int)> ConnectAllStairExitPoints(int currentFloor, int[,][] subLevelStairMap, bool[,] opsMap){
        bool[,] blockedForGeneration = opsMap;
        List<(int,int)>connectionsToSublevel = new List<(int, int)>{};
        // 1. Analyse Sub-level
        for (int x = 0; x < levelsize; x++){
            for (int y = 0; y < levelsize; y++){
                // find blocked tiles
                if(subLevelStairMap[x,y][0] != -1){ // all stairs + all obstacles
                    blockedForGeneration[x,y] = true;
                }
                // find connections
                if(subLevelStairMap[x,y][1] == 1){ // all upper stairhalfs
                    // take heading into account
                    if     (subLevelStairMap[x,y][0] == 0){connectionsToSublevel.Add((x-1,y));} // left option
                    else if(subLevelStairMap[x,y][0] == 1){connectionsToSublevel.Add((x+1,y));} //right option
                    else if(subLevelStairMap[x,y][0] == 2){connectionsToSublevel.Add((x,y-1));} //above option
                    else if(subLevelStairMap[x,y][0] == 3){connectionsToSublevel.Add((x,y+1));} //below option
                }

            }
        }
        // guard
        if(connectionsToSublevel.Count <= 0){return connectionsToSublevel;}
        // 2. sort connections by proximity, 
        //    This is very important i tried a lot of differnt solutions and this is the only way not cover half the map with galerys
        List<(int,int)> conectionSorted = new List<(int, int)>{};
        conectionSorted.Add(connectionsToSublevel[0]);
        while(true){
            (int,int)c1 = conectionSorted[conectionSorted.Count-1];
            connectionsToSublevel.Remove(c1);
            int c1_x = c1.Item1;
            int c1_y = c1.Item2;
            int smallestDist = int.MaxValue;
            (int,int) closest = (-1,-1);
            foreach ((int,int) c2 in connectionsToSublevel){
                int nextDist = Mathf.Abs(c1_x-c2.Item1) + Mathf.Abs(c1_y-c2.Item2);
                if(nextDist < smallestDist){
                    closest = c2;
                    smallestDist = nextDist;
                }
            }
            if(closest != (-1,-1)){conectionSorted.Add(closest);}else{break;}; // if no more closest was found we are finished
        }

        // 3. build Paths
        return RecursiveBFSPathfindingWithBacktracking(conectionSorted,blockedForGeneration,new List<(int, int)>{});
    }

    // Freely programmed after https://www.youtube.com/watch?v=KiCBXu4P-2Y&t=614s by WilliamFiset extended by 
    // backtracking algo to retrieve path and recursion for exploring multiple paths and chaining them  
    private List<(int,int)> RecursiveBFSPathfindingWithBacktracking(List<(int,int)> toVisit, bool[,] blocked, List<(int,int)> path){
        int start_x, start_y;
        (int,int)start = toVisit[0];
        (int,int)end = toVisit[1]; 
        (start_x,start_y) = start;
        Queue<int> column_queue = new Queue<int>{};
        Queue<int> row_queue = column_queue;
        int nodes_left_in_layer = 1;
        int nodes_in_next_layer = 0;
        bool[,] visited = new bool[levelsize,levelsize];
        // n,s,e,w
        int[] dir_row = {-1,+1,0,0};
        int[] dir_col = {0,0,+1,-1};
        // for backtracking
        Dictionary<(int, int), (int, int)> predecessors = new Dictionary<(int, int), (int, int)>();

        // Start bfs
        row_queue.Enqueue(start_x);
        column_queue.Enqueue(start_y);
        visited[start_x,start_y] = true;
        while(row_queue.Count > 0){
            int r = row_queue.Dequeue();
            int c = column_queue.Dequeue();

            //explore neighbours
            for (int i = 0; i < 4; i++){
                int rr = r + dir_row[i];
                int cc = c + dir_col[i];
                // map edge check
                if(rr < 0 || cc < 0){continue;}
                if(rr == levelsize || cc == levelsize){continue;}
                //skip visited or blocked
                if(visited[rr,cc]){ continue;}
                if(blocked[rr,cc]){continue;}
                row_queue.Enqueue(rr);
                column_queue.Enqueue(cc);
                visited[rr,cc] = true;
                nodes_in_next_layer++;
                predecessors[(rr, cc)] = (r, c);
            }

            if((r,c) == end){
                break;
            }

            nodes_left_in_layer--;
            if(nodes_left_in_layer == 0){
                nodes_left_in_layer = nodes_in_next_layer;
                nodes_in_next_layer = 0;
            }
        }

        // Reconstruct the path from the end to the start
        (int, int) current = end;
        while (current != start){
            path.Add(current);
            // if key does not exsist the stair couldnt be connected to the network (path blocked by obstacles edge of map etc..)
            if(predecessors.ContainsKey(current)){current = predecessors[current];}
            else{Debug.LogWarning($"Blocked Stair at: {current}");break;}     
        }
        path.Add(start);

        toVisit.Remove((start_x,start_y));
        if(!(toVisit.Count < 2)){
            return RecursiveBFSPathfindingWithBacktracking(toVisit,blocked,path);
        }else{
            return path;
        }
    }

    /*#################################### HELPER FUNCTIONS #################################################################################################*/
    // returns a random bool, biased by openess
    private bool RandBool(){
        return Random.value >= .5f + openess;
    }

    private int SumAsInt(bool[] b){
        int sum = 0;
        for (int i = 0; i < b.Length; i++){ sum += b[i] ? 1 : 0; }
        return sum;
    }

    private int OppositeWallIndex(int wIndex){
        int res = 0;
        switch(wIndex){case 0 : res = 1; break;case 1 : res = 0; break;case 2 : res = 3; break;case 3 : res = 2; break;} return res;
    }

    // Checks if at side of the map
    private bool isEastBorder(int x){return x == 0;}
    private bool isWestBorder(int x){return x == levelsize-1;}
    private bool isSouthBorder(int y){return y == levelsize-1;}
    private bool isNorthBorder(int y){return y == 0;}

    


}