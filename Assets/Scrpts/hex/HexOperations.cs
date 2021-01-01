using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexOperations : MonoBehaviour
{
    //private float campOffset;
    private HexComponent[,] hexes ;
    private int mapWidth, mapHeight;
    private int[,] neighbours;
    public static HexOperations Instance {get;private set;}
    public Transform playerController;
    void Awake(){
        neighbours = new int[,]{ {0,-1},{1,-1},{1,0},{0,1},{-1,1},{-1,0} };
        //playerController = transform.Find("PlayerController");
        //CameraController.onCameraMove+=UpdateHexPositions;
        if(Instance==null){
            Instance =this;
            return;
        }
        else{
            Destroy(gameObject);
        }
    }
    //called in HexMap inSTart
    //Has to be called before usage in start function
    public void Initialize(){
        mapHeight = HexMap.Instance.mapHeight;
        mapWidth = HexMap.Instance.mapWidth;
        
        //campOffset = HexMap.Instance.campOffset;
        hexes = HexMap.Instance.hexes;
    }
    //only for player controller(the one playeing the game)
    public GameObject spawnUnit(Vector2 location,int unitIndex){
        Vector3 place = hexes[(int)location.x,(int)location.y].hex.positionFromCamera();
        GameObject obj = (GameObject)Instantiate(
            PlayerController.Instance.player.kingdom.unitPrefabs[unitIndex],
            new Vector3(place.x,0,place.z),
            Quaternion.identity,
            playerController
        );
        Unit temp = obj.GetComponent<Unit>();

        //updating in Player.Instance.units and calling unit.spawnUnit()
        PlayerController.Instance.player.SpawnUnit(temp,location);

        //letting unit know which player it belongs to
        temp.player = -1;

        //setting its location on y axis right
        Vector3 pos = obj.transform.position;
        obj.transform.position = new Vector3(pos.x,pos.y+temp.offset.y,pos.z);

        //subscribing for moving
        temp.onUnitMove += obj.GetComponent<UnitMove>().onUnitMove;
        //updating in hexcomponent
        hexes[(int)location.x,(int)location.y].addUnit(temp);

        return obj;
    }

    //have to update city script in 2 places after instantiation
    //in PLayerController.Instance.cities
    //in hexes array in this calss
    public GameObject BuildCity(Vector2 location,int cityIndex){
        Vector3 place = hexes[(int)location.x,(int)location.y].hex.positionFromCamera();
        GameObject obj = (GameObject)Instantiate(
            PlayerController.Instance.player.kingdom.cityPrefabs[cityIndex],
            new Vector3(place.x,place.y,place.z),
            Quaternion.identity,
            playerController          
        );
        City temp = obj.GetComponent<City>();

        //setting its possiton right on y axis
        Vector3 pos = obj.transform.position;
        obj.transform.position = new Vector3(pos.x,pos.y+temp.offset.y,pos.z);
        
        //letting unit know which player it belongs to
        temp.player = -1;

        //updating in the hexComponent city
        hexes[(int)location.x,(int)location.y].BuildBuilding(temp);
        //updating in the Player.Instance.cities
        PlayerController.Instance.player.BuildCity(temp,location);
        return obj;

    }
    public GameObject BuildBuilding(Vector2 location,int buildingIndex, City city){
        Vector3 place = hexes[(int)location.x,(int)location.y].hex.positionFromCamera();
        GameObject obj = (GameObject)Instantiate(
            PlayerController.Instance.player.kingdom.buildingPrefabs[buildingIndex],
            new Vector3(place.x,place.y,place.z),
            Quaternion.identity,
            playerController          
        );
        Building temp = obj.GetComponent<Building>();
        //setting up cityfor this building
        temp.city = city;

        //setting its possiton right on y axis
        Vector3 pos = obj.transform.position;
        obj.transform.position = new Vector3(pos.x,pos.y+temp.offset.y,pos.z);
        
        //letting unit know which player it belongs to
        temp.player = -1;

        //updating in the hexComponent city
        hexes[(int)location.x,(int)location.y].BuildBuilding(temp);
        //updating in the Player.Instance.cities
        PlayerController.Instance.player.BuildBuilding(temp,location);
        return obj;

    }

    public GameObject BuildImprovement(Vector2 location,int improvementIndex,City city){
        Vector3 place = hexes[(int)location.x,(int)location.y].hex.positionFromCamera();
        GameObject obj = (GameObject)Instantiate(
            PlayerController.Instance.player.kingdom.improvementPrefabs[improvementIndex],
            new Vector3(place.x,place.y,place.z),
            Quaternion.identity,
            playerController

        );
        Improvement temp = obj.GetComponent<Improvement>();

        temp.city = city;
        //setting its possiton right on y axis
        Vector3 pos = obj.transform.position;
        obj.transform.position = new Vector3(pos.x,pos.y+temp.offset.y,pos.z);
        
        //letting unit know which player it belongs to
        temp.player = -1;

        //updating in the hexComponent city
        hexes[(int)location.x,(int)location.y].BuildBuilding(temp);
        //updating in the Player.Instance.cities
        PlayerController.Instance.player.BuildImprovement(temp,location);
        return obj;

    }
    public GameObject BuildDistrict(Vector2 location,int disIndex,City city){
        Vector3 place = hexes[(int)location.x,(int)location.y].hex.positionFromCamera();
        GameObject obj = (GameObject)Instantiate(
            PlayerController.Instance.player.kingdom.districtPrefabs[disIndex],
            new Vector3(place.x,place.y,place.z),
            Quaternion.identity,
            playerController    
        );
        District temp = obj.GetComponent<District>();
        temp.city = city;

        //setting its possiton right on y axis
        Vector3 pos = obj.transform.position;
        obj.transform.position = new Vector3(pos.x,pos.y+temp.offset.y,pos.z);
        
        //letting unit know which player it belongs to
        temp.player = -1;

        //updating in the hexComponent building
        hexes[(int)location.x,(int)location.y].BuildBuilding(temp);
        //updating in the Player.Instance.district
        PlayerController.Instance.player.BuildDistrict(temp,location);
        return obj;

    }

    public void DestroyCity(City city){
        UpdatePosition temp = city.gameObject.GetComponent<UpdatePosition>();
        CameraController.onCameraMove-=temp.updateLocationFromCamera;
        Vector2 location = city.location;
        PlayerController.Instance.player.RemoveBuilding(city.id);
        hexes[(int)location.x,(int)location.y].city=null;

        //removing this object from selected objects and closing hub from UICOntroller
        GameState.Instance.selectedObject=null;
        UIController.Instance.CloseHub();

        //destroy after removing from hexes and PlayerCOntroller
        
        Destroy(city.gameObject);
        

    }
    public void DestroyUnit(Unit unit){
        UpdatePosition tempPos = unit.GetComponent<UpdatePosition>();
        //UnitMove temp = unit.gameObject.GetComponent<UnitMove>();
        CameraController.onCameraMove-=tempPos.updateLocationFromCamera;
        Vector2 location = unit.location;
        PlayerController.Instance.player.RemoveUnit(unit.id);
        hexes[(int)location.x,(int)location.y].removeUnit(unit.id);
        //removing this object from selected objects and closing hub from UICOntroller
        GameState.Instance.selectedObject=null;
        UIController.Instance.CloseHub();
        //destroy after removing from hexes and PlayerCOntroller
        Destroy(unit.gameObject);

    }
    //wrapping on the horizontal axis
    /*public void UpdateHexPositions(){
        for(int i=0;i<mapWidth;i++){
            for(int j=0;j<mapHeight;j++){
                hexes[i,j].updatePosition();
            }
        }
    }*/
    //for getting all in range ofvariable distance
    public HexComponent[] getNeighbors(Vector2 location, int distance){
        HexComponent hex = hexes[(int)location.x,(int)location.y];
        int q = hex.hex.Q;
        int r =hex.hex.R;
        int size = 0;
        for(int i=1;i<=distance;i++){
            size+=i*6;
        }
        size++;
        HexComponent[] temp = new HexComponent[size];
        int x,z;
        int t=0;
        for(int i =0-distance;i<mapWidth+distance;i++){
            x = q-i;
            if(x>=-distance &&x<=distance){
                for(int j=0-distance;j<mapHeight+distance;j++){
                    z = r-j;
                    if((-x-z)>=Mathf.Max(-distance,-x-distance) 
                    && (-x-z)<=Mathf.Min(distance,-x+distance)){
                        int y=-(x+z);
                        if(x+y+z==0){
                            if(q>mapWidth ||q<0 || r>mapHeight || r<0){
                                Debug.LogError("GteNeighbor function, out ofbound");
                                return temp;
                            }

                            temp[t] = hexes[mod(q+x,mapWidth),modY(r+z,mapHeight)];
                            t++;
                            if(t>=size){
                                return temp;
                                
                            }
                            
                        }

                    }
                    
                }
            }
        }
        return temp;
    }
    
    private int mod(int x, int m) {
        int r = x%m;
        return r<0 ? r+m : r;
    }
    private int modY(int x, int m){
        int r = x%m;
        if(x>=m)
            return m-1;
        else if(x==0)
            return 0;
        return r<0 ? r+m : r;
    }
        
    
    public Vector3 positionFromCamera(int q, int r){
      return hexes[q,r].hex.positionFromCamera(Camera.main.transform.position,mapWidth,mapHeight);
      
  }
    public HexComponent[] getClosestNeighbours(Vector2 location){
        HexComponent hex = hexes[(int)location.x,(int)location.y];
        HexComponent[] result=new HexComponent[6];
        for(int i=0;i<6;i++){
            result[i] = hexes[mod(hex.hex.Q+neighbours[i,0],mapWidth),mod(hex.hex.R+neighbours[i,1],mapHeight)];
        }
        return result;

    }

    

    public GameObject BuildAICity(Vector2 location,int cityIndex, int AIIndex){
        Vector3 place = hexes[(int)location.x,(int)location.y].hex.positionFromCamera();
        GameObject obj = (GameObject)Instantiate(
            AIController.Instance.AIPlayers[AIIndex].kingdom.cityPrefabs[cityIndex],
            new Vector3(place.x,place.y,place.z),
            Quaternion.identity,
            AIController.Instance.transform   
        );
        //setting layer and tage
        obj.tag = "enemyCity";
        obj.layer = 8;


        City temp = obj.GetComponent<City>();

        //setting its possiton right on y axis
        Vector3 pos = obj.transform.position;
        obj.transform.position = new Vector3(pos.x,pos.y+temp.offset.y,pos.z);
        
        //letting unit know which player it belongs to
        temp.player = AIIndex;

        //updating in the hexComponent city
        hexes[(int)location.x,(int)location.y].city = temp;
        //updating in the Player.Instance.cities
        AIController.Instance.AIPlayers[AIIndex].BuildCity(temp,location);
        return obj;

    }


    public GameObject spawnAIUnit(Vector2 location,int unitIndex,int AIIndex){
        Vector3 place = hexes[(int)location.x,(int)location.y].hex.positionFromCamera();
        GameObject obj = (GameObject)Instantiate(
            AIController.Instance.AIPlayers[AIIndex].kingdom.unitPrefabs[unitIndex],
            new Vector3(place.x,0,place.z),
            Quaternion.identity,
            AIController.Instance.transform   
        );
        //setting tag and layer of the unit
        obj.tag = "enemy";
        obj.layer = 9; //Unit

        Unit temp = obj.GetComponent<Unit>();

        //updating in Player.Instance.units and calling unit.spawnUnit()
        AIController.Instance.AIPlayers[AIIndex].SpawnUnit(temp,location);

        //letting unit know which player it belongs to
        temp.player = AIIndex;

        //setting its location on y axis right
        Vector3 pos = obj.transform.position;
        obj.transform.position = new Vector3(pos.x,pos.y+temp.offset.y,pos.z);
        //subscribing for moving
        temp.onUnitMove += obj.GetComponent<UnitMove>().onUnitMove;
        //updating in hexcomponent
        hexes[(int)location.x,(int)location.y].addUnit(temp);

        return obj;
    }
    public GameObject BuildAIDistrict(Vector2 location,int disIndex, int player=0){
        Vector3 place = hexes[(int)location.x,(int)location.y].hex.positionFromCamera();
        GameObject obj = (GameObject)Instantiate(
            AIController.Instance.AIPlayers[player].kingdom.districtPrefabs[disIndex],
            new Vector3(place.x,place.y,place.z),
            Quaternion.identity         
        );
        District temp = obj.GetComponent<District>();

        //setting its possiton right on y axis
        Vector3 pos = obj.transform.position;
        obj.transform.position = new Vector3(pos.x,pos.y+temp.offset.y,pos.z);
        
        //letting unit know which player it belongs to
        temp.player = player;

        //updating in the hexComponent city
        hexes[(int)location.x,(int)location.y].BuildBuilding(temp);
        //updating in the Player.Instance.cities
        AIController.Instance.AIPlayers[player].BuildDistrict(temp,location);
        return obj;

    }


    //spawning NPCs
    public Shadow spawnShadow(GameObject shadowPrefab){
        Vector2 location = new Vector2(Random.Range(0,mapWidth),Random.Range(0,mapHeight));
            GameState.Instance.HexOccupiedCheck(ref location);
            HexComponent hex = hexes[(int)location.x,(int)location.y];
            Vector3 place = hex.hex.positionFromCamera();
            GameObject obj = (GameObject)Instantiate(
                shadowPrefab,
                new Vector3(place.x,0,place.z),
                Quaternion.identity,
                NPCController.Instance.transform         
            );
            Shadow temp  = obj.GetComponent<Shadow>();
            temp.spawnUnit(location);
            //setting ya xis for shadows
            Vector3 pos = obj.transform.position;
            obj.transform.position = new Vector3(pos.x,pos.y+temp.offset.y,pos.z);
            //subscribing for moving
            temp.onUnitMove += obj.GetComponent<UnitMove>().onUnitMove;
            //adding enemy to thw hexCompoenty
            hexes[(int)location.x,(int)location.y].addUnit(temp);
            //TODO put these locations in Gamestate occupied locations  
            GameState.Instance.setOccupiedHexes(location);
            return temp;
    }
    public Animal spawnAnimal(GameObject animalPrefab){
        Vector2 location = new Vector2(Random.Range(0,mapWidth),Random.Range(0,mapHeight));
            GameState.Instance.HexOccupiedCheck(ref location);
            HexComponent hex = hexes[(int)location.x,(int)location.y];
            Vector3 place = hex.hex.positionFromCamera();
            GameObject obj = (GameObject)Instantiate(
                animalPrefab,
                new Vector3(place.x,0,place.z),
                Quaternion.identity,
                NPCController.Instance.transform         
            );
            Animal temp  = obj.GetComponent<Animal>();
            temp.spawnUnit(location);
            //setting ya xis for shadows
            Vector3 pos = obj.transform.position;
            obj.transform.position = new Vector3(pos.x,pos.y+temp.offset.y,pos.z);
            //subscribing for moving
            temp.onUnitMove += obj.GetComponent<UnitMove>().onUnitMove;
            //adding enemy to thw hexCompoenty
            hexes[(int)location.x,(int)location.y].addUnit(temp);
            //TODO put these locations in Gamestate occupied locations  
            GameState.Instance.setOccupiedHexes(location);
            return temp;
    }
}

