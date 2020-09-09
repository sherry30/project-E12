using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
                        visited.Add(loc);
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
            foreach(Vector2 loc in visited){
                HexComponent[] nei = HexOperations.Instance.getNeighbors(loc,1);    
                foreach(HexComponent n in nei){
                    Vector2 loc2 = new Vector2(n.hex.Q,n.hex.R);
                    if(visited.Contains(n.location))
                        continue;
                    else{
                        if(unVisited.Contains(loc)){
                            break;
                        }
                        else{
                            newUnvisited.Add(loc);
                            break;
                        }
                    }
                }
                time++;
                if(time>20)
                    break;
            }
            

        }
        return unVisited;
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
