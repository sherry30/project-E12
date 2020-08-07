using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    
    public static HexMap Instance{get;private set;}
    void Awake(){
        Hex.hexHeight=HexPrefab.transform.localScale.y;
        hexes = new HexComponent[mapWidth,mapHeight];
        if(Instance==null){
            Instance =this;
            return;
        }
        else{
            Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //initializing before use in Start
        HexOperations.Instance.Initialize();
        //generate all water tiles
        GenerateMap();   

        //Raise terrain procedurely
        LandTiles = HexMapGenerator.Instance.createLand(mapHeight,mapWidth,hexes);

        //setting terrain of the map
        HexMapGenerator.Instance.setTerrain();

        //placing camp at a landtile on the hex map
        PlaceCamp();

        //spawn starting units of the  player
        spawnStartingUnits(); 

        //spawning AI camps
        PlaceAICamps();

        //spawning statrting units of all AIs
        spawnAIStartingUnits();

        //spawning shadows
        NPCController.Instance.spawnShadows();
        

        //setup camera once at the start
        CameraController.onCameraMove();

        //setupminicamComp at the start
        minimapCamCont.FixPos();
        
    }

    public GameObject HexPrefab;



    public int mapHeight=20;
    public int mapWidth= 30;

    public HexComponent[,] hexes;
    public float cameraOffset=5f;
    private List<HexComponent> LandTiles;


    public void GenerateMap(){
        StaticBatchingUtility.Combine(this.gameObject);
        //generating all the hexes

        for(int x= 0;x<mapWidth;x++){
            for(int y = 0; y<mapHeight;y++){
                //Hex object
                Hex hex = new Hex(x,y);
                
                Vector3 pos = hex.positionFromCamera(Camera.main.transform.position,mapWidth,mapHeight);
                //instantiate all the Hex
                GameObject HexGo = (GameObject)Instantiate(
                    HexPrefab,
                    pos,
                    Quaternion.identity,
                    this.transform
                );

                //changing name of the Hex Object
                HexGo.name = string.Format("Hex{0},{1}",x,y);



                //assigning hexclass in the HexComponent
                HexComponent hexGoComp = HexGo.GetComponent<HexComponent>();
                hexGoComp.setHex(hex);

                //changing material to water
                hexGoComp.setBiomAndTerrain(biome.water,terrain.water_deep_ocean);

                //changing text on the hex to coordinates
                //hexGoComp.text = hexGoComp.GetComponentInChildren<TextMesh>();
                //hexGoComp.text.text = string.Format("{0},{1}",x,y);

                //filling hexes array in this class
                hexes[x,y] = hexGoComp;


                
            }
        }

        
    }

    

    private void PlaceCamp(){
        
        HexComponent campTile = LandTiles[UnityEngine.Random.Range(0,LandTiles.Count)];
        Vector2 location = campTile.location;
        GameState.Instance.LandHexOccupiedCheck(ref location,LandTiles);

        //position of city
        Vector3 place = hexes[(int)location.x,(int)location.y].hex.positionFromCamera();

        //building it 
        HexOperations.Instance.BuildCity(location,0);

        //moving camera to the camp place
        CameraController cam = Camera.main.gameObject.GetComponent<CameraController>();
        Camera.main.transform.position = new Vector3(place.x, place.y+cameraOffset, place.z-cameraOffset);
        cam.adjusted=true;

    }
    private void PlaceAICamps(){
        for(int i=0;i<AIController.Instance.NumberOfAIs;i++){
            HexComponent campTile = LandTiles[UnityEngine.Random.Range(0,LandTiles.Count)];
            Vector2 location = campTile.location;
            GameState.Instance.LandHexOccupiedCheck(ref location,LandTiles);

            //position of city
            Vector3 place = hexes[(int)location.x,(int)location.y].hex.positionFromCamera();

            //building it 
            HexOperations.Instance.BuildAICity(location,0,i);
         
        }


    }

    public HexComponent getHexComponent(Vector2 location){
        return hexes[mod((int)location.x,mapWidth),mod((int)location.y,mapHeight)];
    }
    private int mod(int x, int m) {
    int r = x%m;
    return r<0 ? r+m : r;
    }
    private void spawnStartingUnits(){
        Kingdom king = PlayerController.Instance.player.kingdom;
        City camp = PlayerController.Instance.player.cities[0];
        for(int i=0;i<king.startingUnitIndexes.Count;i++){
            HexOperations.Instance.spawnUnit(camp.location,king.startingUnitIndexes[i]);
        }
        //set territory of distance 1
        PlayerController.Instance.player.setTerritory(HexOperations.Instance.getClosestNeighbours(camp.location));

    }
    private void spawnAIStartingUnits(){
        for(int j=0;j<AIController.Instance.NumberOfAIs;j++){
            Kingdom king = AIController.Instance.AIPlayers[j].kingdom;
            City camp = AIController.Instance.AIPlayers[j].cities[0];
            for(int i=0;i<king.startingUnitIndexes.Count;i++){
                HexOperations.Instance.spawnAIUnit(camp.location,king.startingUnitIndexes[i],j);
            }
            AIController.Instance.AIPlayers[j].setTerritory(HexOperations.Instance.getClosestNeighbours(camp.location));
        }
    }

}
