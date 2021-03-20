using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction{
    NW,
    NE,
    E,
    SE,
    SW,
    W
}

public class HexComponent : MonoBehaviour
{



    public Hex hex;
    public Building building;
    public Improvement improvement;
    public City city;
    public TextMesh text;
    public List<Unit> units = new List<Unit>();
    //public List<Unit> enemies = new List<Unit>();
    private float radiusOfUnits=2.5f;
    public int production = 2;
    public Biome hexBiome;
    public Vector2 location;
    public int elevation=1;
    public UpdatePosition updatePos;
    private Dictionary<Direction,Transform> mountainSides;
    private Dictionary<Direction,Transform> mountainMidSides;
    private Dictionary<Direction,Transform> cliffSides;
    //private Transform mountainTop;
    public Relic relic;

    void Update(){
        Vector3 y = new Vector3(0,getElevation(),0);
        
        //setting offset in updatePosition component of the hex
        updatePos = GetComponent<UpdatePosition>();
        updatePos.offset = y;
    }


    void Awake(){
        //AudioSettings default biome
        hexBiome = GetComponent<Biome>();
        setBiomAndTerrain(biome.water,terrain.water_deep_ocean);

        //getting updatePosition
        updatePos = GetComponent<UpdatePosition>();
        updatePos.location = location;

        //setting mountain sides additional geomtery
        //GeometryCleanup();
        /*mountainSides = new Dictionary<Direction, Transform>();
        mountainMidSides = new Dictionary<Direction, Transform>();
        cliffSides = new Dictionary<Direction, Transform>();

        mountainSides.Add(Direction.NW,transform.Find("mountain_top_side_NW"));
        mountainSides.Add(Direction.NE,transform.Find("mountain_top_side_NE"));
        mountainSides.Add(Direction.E,transform.Find("mountain_top_side_E"));
        mountainSides.Add(Direction.SE,transform.Find("mountain_top_side_SE"));
        mountainSides.Add(Direction.SW,transform.Find("mountain_top_side_SW"));
        mountainSides.Add(Direction.W,transform.Find("mountain_top_side_W"));

        mountainMidSides.Add(Direction.NW,transform.Find("mountain_mid_side_NW"));
        mountainMidSides.Add(Direction.NE,transform.Find("mountain_mid_side_NE"));
        mountainMidSides.Add(Direction.E,transform.Find("mountain_mid_side_E"));
        mountainMidSides.Add(Direction.SE,transform.Find("mountain_mid_side_SE"));
        mountainMidSides.Add(Direction.SW,transform.Find("mountain_mid_side_SW"));
        mountainMidSides.Add(Direction.W,transform.Find("mountain_mid_side_W"));

        cliffSides.Add(Direction.NW,transform.Find("cliff_NW"));
        cliffSides.Add(Direction.NE,transform.Find("cliff_NE"));
        cliffSides.Add(Direction.E,transform.Find("cliff_E"));
        cliffSides.Add(Direction.SE,transform.Find("cliff_SE"));
        cliffSides.Add(Direction.SW,transform.Find("cliff_SW"));
        cliffSides.Add(Direction.W,transform.Find("cliff_W"));*/

        //mountainTop = transform.Find("mountain_top");

    }

    /*public void updatePosition(){
        //updating this hexes position
        this.transform.position = hex.positionFromCamera();
    }*/
    public void addUnit(Unit un){
        if(units==null)
            units = new List<Unit>();
        units.Add(un);
        un.setElevation(getElevation());
        setUnitOffsets();

        //letting the building obn this hex know that it got a new Unit
        if(building)
            building.unitAddedToTheHex();
    }
    public void removeUnit(int id){
        for(int i=0;i<units.Count;i++){
            if(units[i].id==id){
                removeUnitOffsets(i);
                units.RemoveAt(i);
            }
        }
        setUnitOffsets();

        //letting the building obn this hex know that it got a new Unit
        if(building)
            building.unitRemovedFromTheHex();
        
    }
    /*public void addEnemy(Unit un){
        if(enemies==null)
            enemies = new List<Unit>();
        //changeUnitElevation(un);
        enemies.Add(un);
        changeUnitElevation(un);
        setEnemiesOffsets();

        //letting the building obn this hex know that it got a new Unit
        if(building)
            building.unitAddedToTheHex();
    }*/

    /*public void removeEnemy(int id){
        for(int i=0;i<enemies.Count;i++){
            if(enemies[i].id==id){
                removeEnemyOffsets(i);
                enemies.RemoveAt(i); 
            }
        }
        
        //letting the building obn this hex know that it got a new Unit
        if(building)
            building.unitRemovedFromTheHex();
    }*/

    public bool containEnemies(int player = -1){
        for(int i=0;i<units.Count;i++){
            if(units[i].player!=player){
                return true;
            }
        }
        return false;
    /*if(enemies!=null)
            return enemies.Count>0;
        return false;*/
    }

    public List<Unit> getEnemies(int player=-1){
        List<Unit> enemies = new List<Unit>();
        for(int i=0;i<units.Count;i++){
            if(units[i].player!=player){
                enemies.Add(units[i]);
            }
        }
        return enemies;
    }

    //for retuning one enemy ata specific index
    public Unit getEnemy(int index=0,int player=-1){
        List<Unit> enemies = getEnemies(player);
        return enemies[0];
    }
    public bool containUnits(){
        if(units!=null)
            return units.Count>0;
        return false;
    }
    
    //when there are no enemies,buildings, cities or units
    public bool isEmpty(){
        if(!containEnemies()){
            if(units!=null){
                if(units.Count==0){
                    if(city==null){
                        if(building==null && improvement==null)
                            return true;
                    }
                }
            }
        }
        return false;
    }
    public bool containStructure(){
        return building!=null || city!=null || improvement!=null; 
    }
    //setting units offset within this hex
    public void setUnitOffsets(){
        if((units!=null && units.Count>1)|| (containStructure())){
            for(int i=0 ;i<units.Count;i++){
                float angle = i*Mathf.PI*2f/units.Count;
                Vector3 newPos= new Vector3(Mathf.Cos(angle)*radiusOfUnits,0,Mathf.Sin(angle)*radiusOfUnits);
                units[i].setOffset(new Vector3(newPos.x,0,newPos.z));   
            }
            for(int i=0 ;i<units.Count;i++){
                units[i].updatePos.updateLocationFromCamera();
            }
            
        }
    }
    /*public void setEnemiesOffsets(){
        if((enemies!=null && enemies.Count>1)|| (containStructure())){
            for(int i=0 ;i<enemies.Count;i++){
                float angle = i*Mathf.PI*2f/enemies.Count;
                Vector3 newPos= new Vector3(Mathf.Cos(angle)*radiusOfUnits,0,Mathf.Sin(angle)*radiusOfUnits);
                enemies[i].setOffset(new Vector3(newPos.x,0,newPos.z));
                //Debug.Log(string.Format("x: {0}, z: {1}",newPos.z,newPos.z));   
            }
            for(int i=0 ;i<units.Count;i++){
                enemies[i].updatePos.updateLocationFromCamera();
            }
            
        }
    }*/
    //when the unit leaves
    public void removeUnitOffsets(int i){
        units[i].offset.x=0;
        units[i].offset.z=0;
        units[i].offset.y-=getElevation();

    }
    /*public void removeEnemyOffsets(int i){
        enemies[i].offset.x=0;
        enemies[i].offset.z=0;
        enemies[i].offset.y-=elevation*Hex.hexHeight;

    }*/

    public void setBiomAndTerrain(biome b,terrain t ){
        hexBiome.setBiome(b);
        ListWrapperMaterial temp = HexMapGenerator.Instance.mats[hexBiome.setTerrain(t)];
        gameObject.GetComponentInChildren<MeshRenderer>().material = temp.variations[Random.Range(0,temp.variations.Count)];
        elevation = hexBiome.setElevation();
        Vector3 y = new Vector3(0,getElevation(),0);
        
        //setting offset in updatePosition component of the hex
        updatePos = GetComponent<UpdatePosition>();
        updatePos.offset = y;

    }
    public void setHex(Hex h){
        hex = h;
        location = new Vector2(hex.Q,hex.R);

        //setting offset in updatePosition component of the hex
        updatePos = GetComponent<UpdatePosition>();
        updatePos.location = location;
    }
    public bool checkTerrain(terrain t){
        if(hexBiome.hexTerrain==t)
            return true;
        return false;
    }
    public bool checkBiome(biome b){
        if(hexBiome.hexBiome==b)
            return true;
        return false;
    }
    public float getElevation(){
        return elevation*Hex.hexHeight*2;
        //unit.setupdate
    }

    public void BuildBuilding(Building imp){
       building = imp;
        setUnitOffsets();
    }

    public void addRelic(Relic rel)
    {
        relic = rel;
    }

    //setting additional geometry for mountain
    /*public void setMountain(List<Direction> dir){

        for(int i=0;i<dir.Count;i++){
            mountainSides[dir[i]].gameObject.SetActive(true);
        }
    }

    public void setCliff(List<Direction> dir){
        for(int i=0;i<dir.Count;i++){
            cliffSides[dir[i]].gameObject.SetActive(true);
        }
    }
    public void setMidMountain(Direction dir){
        mountainMidSides[dir].gameObject.SetActive(true);
    }
    public void GeometryCleanup(){
        foreach(KeyValuePair<Direction, Transform> entry in mountainSides)
        {
            if(!entry.Value.gameObject.activeSelf){
                Destroy(entry.Value.gameObject);
            }
        }

        foreach(KeyValuePair<Direction, Transform> entry in mountainMidSides)
        {
            if(!entry.Value.gameObject.activeSelf){
                Destroy(entry.Value.gameObject);
            }
        }
        foreach(KeyValuePair<Direction, Transform> entry in cliffSides)
        {
            if(!entry.Value.gameObject.activeSelf){
                Destroy(entry.Value.gameObject);
            }
        }
    }*/

}
