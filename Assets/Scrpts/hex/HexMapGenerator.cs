using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
 public class ListWrapperMaterial
 {
      public List<Material> variations;
 }
public class HexMapGenerator : MonoBehaviour
{

    public static HexMapGenerator Instance;
    void Awake(){
        if(Instance==null){
            Instance =this;
            directions = new Dictionary<int, Direction>();
            directions.Add(0,Direction.NW);
            directions.Add(1,Direction.NE);
            directions.Add(2,Direction.E);
            directions.Add(3,Direction.SE);
            directions.Add(4,Direction.SW);
            directions.Add(5,Direction.W);  

            //setting objects
            mountainSides = new Dictionary<Direction,string>();
            mountainMidSides = new Dictionary<Direction, string>();
            cliffSides = new Dictionary<Direction, string>();

            mountainSides.Add(Direction.NW,"mountain_top_side_NW");
            mountainSides.Add(Direction.NE,"mountain_top_side_NE");
            mountainSides.Add(Direction.E,"mountain_top_side_E");
            mountainSides.Add(Direction.SE,"mountain_top_side_SE");
            mountainSides.Add(Direction.SW,"mountain_top_side_SW");
            mountainSides.Add(Direction.W,"mountain_top_side_W");

            mountainMidSides.Add(Direction.NW,"mountain_mid_side_NW");
            mountainMidSides.Add(Direction.NE,"mountain_mid_side_NE");
            mountainMidSides.Add(Direction.E,"mountain_mid_side_E");
            mountainMidSides.Add(Direction.SE,"mountain_mid_side_SE");
            mountainMidSides.Add(Direction.SW,"mountain_mid_side_SW");
            mountainMidSides.Add(Direction.W,"mountain_mid_side_W");

            cliffSides.Add(Direction.NW,"cliff_NW");
            cliffSides.Add(Direction.NE,"cliff_NE");
            cliffSides.Add(Direction.E,"cliff_E");
            cliffSides.Add(Direction.SE,"cliff_SE");
            cliffSides.Add(Direction.SW,"cliff_SW");
            cliffSides.Add(Direction.W,"cliff_W");          
            return;
        }
        else{
            Destroy(gameObject);
        }
    }
    [SerializeField]
    public int chunks=1;
    [SerializeField]
    public int chunkSize=5;
    private List<Vector2> chunkLocs;
    [Range(1,7)]
    public int chunkiness;
    //[Range(0,0.5)]
    public float mutateProbability=0.2f;
    public List<ListWrapperMaterial> mats;
    private List<HexComponent> LandTiles;
    private Dictionary<int,Direction> directions;
    public int noOfMountain=1;
    public GameObject mountainGeomtery;
    private Dictionary<Direction,string> mountainSides;
    private Dictionary<Direction,string> mountainMidSides;
    private Dictionary<Direction,string> cliffSides;

    public List<HexComponent> createLand(int mapHeight, int mapWidth,HexComponent[,] hexes){        
        int size = mapHeight*mapWidth;

        //creating land
        for(int i=0;i<chunks;i++){
            int x = Random.Range(0,mapWidth);
            int y = Random.Range(0,mapHeight);
            int test = 0;

            //making sure its away from other chunks
            while(test<20){
                bool found = false;
                x = Random.Range(0,mapWidth);
                y = Random.Range(0,mapHeight);
                Vector2 previousChunk = new Vector2(0,0);
                if(i>0){
                    previousChunk = chunkLocs[chunkLocs.Count-1];
                    for(int k =0;k<chunkLocs.Count-1;k++){
                        if(hexes[x,y].hex.Distance(hexes[(int)chunkLocs[k].x,(int)chunkLocs[k].y].hex)>chunkSize){
                        //last condition for keeping each chunk away dfrom eavh other for now
                            found = true;
                        }
                        else{
                            found = false;
                            break;
                        }
                    }
                    if(found)
                        break;
                }
            test++;
            if(test>20)
                Debug.Log("Couldnt find chunk far away");
                
            }
            if(chunkLocs == null)
                chunkLocs = new List<Vector2>();
            chunkLocs.Add(new Vector2(x,y));
            
            //creating jitter
            Vector2 currentHex = new Vector2(x,y);
            List<Vector2> visited = new List<Vector2>();
            Vector2 nextHex = new Vector2(x,y);

            for(int j=0;j<chunkSize;j++){
                currentHex = nextHex;
                HexComponent[] neighbours = HexOperations.Instance.getNeighbors(currentHex,chunkiness);
                List<Vector2> unVisited = new List<Vector2>();
                visited.Add(currentHex);
                foreach(HexComponent n in neighbours){
                    Vector2 loc = new Vector2(n.hex.Q,n.hex.R);
                    if(checkVisited(loc,visited))
                        continue;
                    else{
                        unVisited.Add(loc);
                        //visited.Add(loc);
                    }
                }
                raiseTerrain(neighbours);
                raiseTerrain(hexes[(int)currentHex.x,(int)currentHex.y]);

                //mutation probability
                List<Vector2> newUnvisited = mutate(visited,unVisited);
                if(newUnvisited.Count>0){
                    nextHex = newUnvisited[Random.Range(0,newUnvisited.Count-1)];
                }
                else if(unVisited.Count>0)
                    nextHex = unVisited[Random.Range(0,unVisited.Count-1)];
                else{
                    Debug.LogError("unVisited is empty");
                }
            }
        }

        //raising mountains
        /*for(int i=0;i<noOfMountain;i++){
            HexComponent hex = LandTiles[Random.Range(0,LandTiles.Count)];
            raiseMountain(hex,1);
        }*/

        //creating north and southpoles
        createPoles(mapWidth,mapHeight,hexes);
        return LandTiles;
    }
    private void raiseTerrain(HexComponent[] hexes){
        foreach(HexComponent hex in hexes){
            //adding in landtiles to send back to hex map
            if(LandTiles==null)
                LandTiles = new List<HexComponent>();
            if(!LandTiles.Contains(hex)){
                LandTiles.Add(hex);
            }
            
            hex.setBiomAndTerrain(biome.polar,terrain.polar_rocky_coast);
        }
    }

    private void raiseMountain(HexComponent hex, int size){
        
        List<HexComponent> hexes = HexOperations.Instance.getNeighbors(hex.location,size).ToList();
        for(int i=0;i<hexes.Count;i++){
            
            if(hex.hex.Distance(hexes[i].hex)>1){
                hexes[i].setBiomAndTerrain(biome.mountain,terrain.mountain_cliff);
            }
            else if(hex== hexes[i]){
                hexes[i].setBiomAndTerrain(biome.mountain,terrain.mountain);
            }
            else{
                //chance of spawning at lower range of distance 1 mountain
                if(Random.Range(0,100)<50)
                    hexes[i].setBiomAndTerrain(biome.mountain,terrain.mountain);
                else{
                    hexes[i].setBiomAndTerrain(biome.mountain,terrain.mountain_cliff);    

                    //chance of spawning one more adjecent tile as mountain
                    HexComponent[] nei = HexOperations.Instance.getClosestNeighbours(hexes[i].location);
                    for(int j=0;j<6;j++){
                        if(Random.Range(0,100)<20){
                            if(hexes[i].elevation-nei[j].elevation>2){                                
                                nei[j].setBiomAndTerrain(biome.mountain,terrain.mountain_4);
                                
                                //setting cliff here
                                addAdditioanlMountainGeometry(nei[j]);


                            }
                        }
                    }
                    
                }
            }

            
            
        }
        //fixing additional geometry
        for(int i=0;i<hexes.Count;i++){
            addAdditioanlMountainGeometry(hexes[i]);
        }

    }

    private void addAdditioanlMountainGeometry(HexComponent hex){
        HexComponent[] nei = HexOperations.Instance.getClosestNeighbours(hex.location);
        List<Direction> dir = new List<Direction>();
        List<Direction> sideDir = new List<Direction>();
        List<Direction> cliffDir = new List<Direction>();

        GameObject geometry = (GameObject)Instantiate(
                    mountainGeomtery,
                    hex.transform.position,
                    Quaternion.identity,
                    hex.transform
                );
        
        for(int j=0;j<6;j++){
            if(hex.elevation-nei[j].elevation==1){
                dir.Add(directions[j]);
                
            }
            else if(hex.elevation-nei[j].elevation==2){
                sideDir.Add(directions[j]);
            }
            else if(hex.elevation-nei[j].elevation>2){
                cliffDir.Add(directions[j]);
            }
        }

        
        setMountain(dir,geometry);
        setMidMountain(sideDir,geometry);
        setCliff(cliffDir,geometry);

        GeometryCleanup(geometry);

        //instantiating
        /*GameObject geometry = (GameObject)Instantiate(
                    mountainGeomtery,
                    hex.transform.position,
                    Quaternion.identity,
                    hex.transform
                );*/
    }

    private void setMountain(List<Direction> dir, GameObject geo){

        for(int i=0;i<dir.Count;i++){
            geo.transform.Find(mountainSides[dir[i]]).gameObject.SetActive(true);
        }
    }

    private void setMidMountain(List<Direction> dir, GameObject geo){
        for(int i=0;i<dir.Count;i++){
            geo.transform.Find(mountainMidSides[dir[i]]).gameObject.SetActive(true);
        }
    }

    public void setCliff(List<Direction> dir, GameObject geo){
        for(int i=0;i<dir.Count;i++){
            geo.transform.Find(cliffSides[dir[i]]).gameObject.SetActive(true);
        }
    }

    public void GeometryCleanup(GameObject geo){
        foreach(KeyValuePair<Direction, string> entry in mountainSides)
        {
            GameObject temp = geo.transform.Find(entry.Value).gameObject;
            if(!temp.activeSelf){
                Destroy(temp.gameObject);
            }
        }

        foreach(KeyValuePair<Direction, string> entry in mountainMidSides)
        {
            GameObject temp = geo.transform.Find(entry.Value).gameObject;
            if(!temp.activeSelf){
                Destroy(temp.gameObject);
            }
        }
        foreach(KeyValuePair<Direction, string> entry in cliffSides)
        {
            GameObject temp = geo.transform.Find(entry.Value).gameObject;
            if(!temp.activeSelf){
                Destroy(temp.gameObject);
            }
        }
    }

    public void createPoles(int mapWidth,int mapHeight, HexComponent[,] hexes){
        int strength=3;
        HexComponent startingPoint = hexes[(mapWidth-1)/2,0];
        HexComponent startingPoint2 = hexes[(mapWidth-1),mapHeight-1];
        //startingPoint.setBiomAndTerrain(biome.polar, terrain.polar_snow);
        Debug.Log(string.Format("loc: {0}",startingPoint.location));
        for(int i=0;i<mapWidth;i+=strength){
            HexComponent[] neighbours = HexOperations.Instance.getNeighbors(hexes[i,0].location,strength);
            foreach(HexComponent n in neighbours){
                n.setBiomAndTerrain(biome.polar, terrain.polar_snow);
                
            }
        }
        
        //testing
        /*Vector2 currentHex = startingPoint.location;
        List<Vector2> visited = new List<Vector2>();
        Vector2 nextHex = startingPoint.location;

        for(int j=0;j<4;j++){
            currentHex = nextHex;
            HexComponent[] neighbours = HexOperations.Instance.getNeighbors(currentHex,4);
            List<Vector2> unVisited = new List<Vector2>();
            visited.Add(currentHex);
            foreach(HexComponent n in neighbours){
                Vector2 loc = n.location;
                if(visited.Contains(loc))
                    continue;
                else{
                    unVisited.Add(loc);

                }
            }
            for(int i=0;i<neighbours.Length;i++){
                neighbours[i].setBiomAndTerrain(biome.polar, terrain.polar_snow);
            }
            hexes[(int)currentHex.x,(int)currentHex.y].setBiomAndTerrain(biome.polar, terrain.polar_snow);

            if(unVisited.Count>0)
                nextHex = unVisited[Random.Range(0,unVisited.Count-1)];
            else{
                Debug.LogError("unVisited is empty");
            }
        }*/
    }

    private void raiseTerrain(HexComponent n){
        //adding in landtiles to send back to hex map
            if(LandTiles==null)
                LandTiles = new List<HexComponent>();
            if(!LandTiles.Contains(n)){
                LandTiles.Add(n);
            }
        n.setBiomAndTerrain(biome.polar,terrain.polar_rocky_coast);
    }

    private bool checkVisited(Vector2 loc,List<Vector2> visited){
        for(int i=0;i<visited.Count;i++){
            if(visited[i].x==loc.x && visited[i].y==loc.y)
                return true;
        }
        return false;
    }

    private List<Vector2> mutate(List<Vector2> visited, List<Vector2> unVisited){
        List<Vector2> newUnvisited = new List<Vector2>();
        if(Random.value<mutateProbability){
            int time = 0;
            foreach(Vector2 loc in unVisited){
                HexComponent[] nei = HexOperations.Instance.getNeighbors(loc,1);    
                foreach(HexComponent n in nei){
                    Vector2 loc2 =n.location;
                    if(visited.Contains(n.location))
                        continue;
                    else{
                        newUnvisited.Add(loc);
                        break;
                    }
                }
                time++;
                if(time>20)
                    break;
            }
            

        }
        return newUnvisited;
    }

    public void setTerrain(){
        int oceanSize = 4;
        List<HexComponent> coastTiles = new List<HexComponent>();
        List<HexComponent> oceanTiles = new List<HexComponent>();
        //setting coast 
        foreach(HexComponent hex in LandTiles){
            foreach(HexComponent neighbor in HexOperations.Instance.getClosestNeighbours(hex.location)){
                if(!neighbor.checkBiome(biome.water))
                    continue;
                if(neighbor.checkTerrain(terrain.water_coast))
                    continue;
                coastTiles.Add(neighbor);
                neighbor.setBiomAndTerrain(biome.water,terrain.water_coast);
            }
        }

        foreach(HexComponent hex in coastTiles){
            foreach(HexComponent neighbor in HexOperations.Instance.getNeighbors(hex.location,oceanSize)){
                if(!neighbor.checkBiome(biome.water))
                    continue;
                if(neighbor.checkTerrain(terrain.water_ocean) || neighbor.checkTerrain(terrain.water_coast))
                    continue;
                oceanTiles.Add(neighbor);
                neighbor.setBiomAndTerrain(biome.water,terrain.water_ocean);
            }
        }


    }
}
