using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum Class{
        camper,
        hero,
        worker,
        civilian,
        warrior,
        scout,
        knight,
        cavalry,
        tank,
        support,
        trader, 
        range,
        shadow

    }
    public enum Type{
        Land,
        Naval

    }
    public string Name;
    public int attack;
    public int defense;
    public int id;
    protected static int currentID=0;
    public string description;
    public int maxHealth;
    public int currentHealth;
    private int damage=100;
    public Type typeOfUnit;
    public Class classOfUnit;
    protected Energy kingdom;
    [SerializeField]
    public Cost cost;
    public Item[] items;
    public Vector2 location;
    public int movement=2;
    [HideInInspector]
    public int player=-1;//player index; -1 if player controller else AIController; is +1 of AIController index
    public bool exhausted=false,moving=false;
    public delegate IEnumerator UnitMovedDelegate(HexComponent oldHex, HexComponent newHex);
    public event UnitMovedDelegate onUnitMove;
    [HideInInspector]
    public UpdatePosition updatePos;
    [HideInInspector]
    public Vector3 offset;//offset used for when multiple objects on the same hex or the hex also ha s building also use y for y axis
    [HideInInspector]
    public bool paralysed = false;
    public string reasonForParalyzed = "None";
    [HideInInspector]
    public HealthBar healthBar;
    protected void Awake(){
        currentHealth = maxHealth;

        healthBar = GetComponent<HealthBar>();

        //offset for y axis
        Vector3 temp = GetComponentInChildren<Collider>().bounds.size;
        offset = new Vector3(0,(temp.y/2f)+Hex.hexHeight-GetComponentInChildren<Renderer>().bounds.center.y,0);

        updatePos = GetComponent<UpdatePosition>();
        setUpdatePosition();


    }
    public void spawnUnit(Vector2 location){
        this.location = location;
        //offset.y += HexMap.Instance.getHexComponent(location).elevation;
        setUpdatePosition();
        id = currentID;
        currentID++;
        GameState.onStartTurn+=StartTurn;
    }
    protected IEnumerator moveUnitOneSpace(HexComponent newHex,bool enemy){
        //remove unit in the old location

        HexComponent temp = HexMap.Instance.getHexComponent(location);
        if(!enemy)
            temp.removeUnit(this.id);
        else
            temp.removeEnemy(this.id);

        Vector2 newLocation = new Vector2(newHex.hex.Q,newHex.hex.R);

        //add unit in hexcomponent in new location
        if(!enemy)
            HexMap.Instance.getHexComponent(newLocation).addUnit(this);
        else    
            HexMap.Instance.getHexComponent(newLocation).addEnemy(this);
        

        //change location in this class
        location.x = newHex.hex.Q;
        location.y = newHex.hex.R;
        //setUpdatePosition();
        yield return StartCoroutine(onUnitMove(temp,newHex));
        
    }
    public IEnumerator moveUnit(List<HexComponent> travelPath,bool enemy =false){
        if(paralysed)
            yield return null;
        exhausted = true;
        moving = true;
        setUpdatePosition();
        //condition for if this unit is selected or not
        for(int i=0;i<travelPath.Count;i++){
            yield return StartCoroutine(moveUnitOneSpace(travelPath[i],enemy));
        }
        moving = false;
        setUpdatePosition();
        updatePos.updateLocationFromCamera();//updating one last time after done moving
    }
    //call this function whenever changing any of these 3 variables in this cript
    private void setUpdatePosition(){
        updatePos.location = this.location;
        updatePos.moving = this.moving;
        updatePos.offset = this.offset;
    }  
    protected virtual void StartTurn(){
        exhausted = false;
    }
    //TODO: add dealing damage functionality for building(city,improvament etc) as well
    public void dealDamage(Unit enemy){
        if(enemy!=null){
            int totalDamage = this.damage-enemy.defense;
            enemy.takeDamage(totalDamage);
            Debug.Log(string.Format("Damage dealt: {0}",totalDamage));
        }
        else
            Debug.Log("This is not a Unit");
    }
    public void takeDamage(int totalDamage){
        currentHealth-=totalDamage;
        healthBar.updateUI();
    }
    public void setOffset(Vector3 off){
        offset.x = 0;
        offset.z = 0;
        this.offset += off;
        setUpdatePosition();
    }

}
