using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*public enum unit{
    Fire_settler,
    Fire_scout,
    Fire_camper,
    Fire_hunter,
    Fire_gatherer,
    Fire_civillian,
    barbarian_shadow
}*/

public class Unit : MonoBehaviour
{
    public enum Class{
        camper,
        hero,
        worker,
        civilian,
        warrior,
        hunter,
        gatherer,
        scout,
        settler,
        knight,
        cavalry,
        tank,
        support,
        trader, 
        range,
        shadow,
        animal

    }
    public enum Type{
        Land,
        Naval

    }
    public string Name;
    public int attack;
    public int defense;
    [HideInInspector]
    public int id;
    protected static int currentID=0;
    public string description;
    public int maxHealth;
    public int currentHealth;
    //public int damage=100;
    public Type typeOfUnit;
    public Class classOfUnit;
    protected Energy kingdom;
    [SerializeField]
    public Cost cost;
    public List<GameObject> items;
    public int itemLimit=3;
    public Vector2 location;
    public int movement=2;
    public int player=-1;//player index; -1 if player controller else AIController; -2 for Shadows -3 for Animals
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
    public Sprite icon;
    public string source;

    public City city;
    protected virtual void Awake(){
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
        //GameState.onStartTurn+=StartTurn;
    }

    //called every loop in moveUnit
    protected IEnumerator moveUnitOneSpace(HexComponent newHex){
        //remove unit in the old location

        HexComponent temp = HexMap.Instance.getHexComponent(location);
        
        //removing units from the old location
        //if(!isEnemy){
        temp.removeUnit(this.id);
        //}
        /*else{
            temp.removeEnemy(this.id);
        } */ 

        
        /*if(skipAnimation){
            location = newHex.location;
            setUpdatePosition();
        }*/
        yield return StartCoroutine(onUnitMove(temp,newHex));

        location = newHex.location;
        setUpdatePosition();
        //adding the unit in the hexCOmponent after the animation
        //if(!isEnemy){
        HexMap.Instance.getHexComponent(location).addUnit(this);
        //}
        /*else{
            HexMap.Instance.getHexComponent(newHex.location).addEnemy(this);
        }*/
        
    }
    public IEnumerator moveUnit(List<HexComponent> travelPath){
        if(paralysed){
            travelPath.Clear();
        }
        exhausted = true;
        moving = true;
        setUpdatePosition();
        //condition for if this unit is selected or not
        for(int i=0;i<travelPath.Count;i++){
            yield return StartCoroutine(moveUnitOneSpace(travelPath[i]));
        }
        moving = false;
        setUpdatePosition();
        updatePos.updateLocationFromCamera();
        //updatePos.updateLocationFromCamera();//updating one last time after done moving
    }

    //no animation
    public void moveUnitFast(List<HexComponent> travelPath){
        if(paralysed)
            travelPath.Clear();
        exhausted = true;
        
        if(travelPath.Count>0){
            HexComponent temp = HexMap.Instance.getHexComponent(location);
            temp.removeUnit(this.id);

            //changing units location
            location = travelPath[travelPath.Count-1].location;
            setUpdatePosition();
            HexMap.Instance.getHexComponent(location).addUnit(this);

            //updating location 
            updatePos.updateLocationFromCamera();
        }

    }


    //call this function whenever changing any of these 3 variables in this cript
    private void setUpdatePosition(){
        updatePos.location = this.location;
        updatePos.moving = this.moving;
        updatePos.offset = this.offset;
    }  
    public virtual void StartTurn(){
        exhausted = false;
    }
    //TODO: add dealing damage functionality for building(city,improvament etc) as well
    public void dealDamage(Unit enemy){
        if(enemy!=null){
            int totalDamage = this.attack-enemy.defense;
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
    public void setElevation(float y){
        offset.y+=y;
        setUpdatePosition();
    }
    public void equipItem(GameObject item){
        if(items.Count>=itemLimit){
            Debug.Log("ItemLimit reached on this unit");
            return;
        }
        DragDrop dr = item.GetComponent<DragDrop>();
        dr.enabled=false;
        item.GetComponent<Item>().equip();


        items.Add(item);
    }

}
