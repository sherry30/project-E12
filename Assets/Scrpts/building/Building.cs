using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum BuildingType{
    improvement,
    city,
    district,
    building
}
public class Building : MonoBehaviour
{
    public BuildingType buildingType;
    public static int currentID=0;
    public string Name;
    [HideInInspector]
    public int id;
    public string description;

    public int maxHealth;

    protected int currentHealth{get;set;}
    [HideInInspector]
    public int player=-1;

    //public Type typeOfBuilding; //not sure yet

    public Cost cost;

    public Vector2 location;
    public List<GameObject> itemInventory;
    public int ItemInventoryLimit=6;

    [HideInInspector]
    public bool isBuilt=false;
    [HideInInspector]
    //offset is just for y axis unlinke in units because buildings dontneed adjusting with repect to other objects in the hex
    public Vector3 offset;

    void Awake(){
        Vector3 temp = GetComponentInChildren<Collider>().bounds.size;
        offset = new Vector3(0, (temp.y/2f)+Hex.hexHeight-GetComponentInChildren<Renderer>().bounds.center.y,0);
        setUpdatePosition();
    }


    //protected abstract void setCost();
    public virtual void Build(Vector2 coordinate){
        //setting location on this class
        location = coordinate;
        offset.y += HexMap.Instance.getHexComponent(location).elevation * Hex.hexHeight*2;
        setUpdatePosition();
        //letting gamestate know this hex isoocupied
        GameState.Instance.occupiedHexes.Add(coordinate);
        isBuilt = true;
        //setting up id for this building or city
        id = currentID;
        currentID++;
        GameState.onStartTurn+=StartTurn;

        //setting up district
        if(buildingType==BuildingType.city){
            City cit = GetComponent<City>();
            District dis= GetComponent<District>();
            dis.Build(location);
            dis.buildingType = BuildingType.district;
            dis.player = player;
            if(cit.typeOfCity==City.Type.camp){    
                dis.setCamp();
            }
            else if(cit.typeOfCity==City.Type.village){
                dis.setVillage();
            }
            //add town later as well
            cit.thisDistrict = dis;
        }
    }
    public void Demolish(){
        GameState.Instance.occupiedHexes.Remove(location);
        isBuilt=false;

    }
    private void setUpdatePosition(){
        UpdatePosition temp = GetComponent<UpdatePosition>();
        temp.location = this.location;
        temp.offset = this.offset;
    }
    protected virtual void StartTurn(){
        Debug.Log("New turn started");
    }
    public virtual void unitAddedToTheHex(){

    }

    public virtual void unitRemovedFromTheHex(){

    }
    public void AddItem(GameObject item){
        if(itemInventory.Count>=ItemInventoryLimit){
            Debug.Log("Inventory full");
            return;
        }
        if(itemInventory==null){
            itemInventory= new List<GameObject>();
        } 
        GameObject obj = Instantiate(item,Vector3.zero,Quaternion.identity) as GameObject;
        //obj.SetActive(false);
        obj.GetComponent<Item>().building = this.GetComponent<Building>();
        obj.GetComponent<DragDrop>().enabled=true;
        itemInventory.Add(obj);
        
    }
    public void removeItem(GameObject item){
        if(itemInventory==null){
            return;
        }
        for(int i=0;i<itemInventory.Count;i++){
            Item temp = itemInventory[i].GetComponent<Item>();
            if(temp.Name==item.GetComponent<Item>().Name){
                itemInventory.RemoveAt(i);
                return;
            }
        }
    }
    public GameObject getItem(Item item){
        for(int i=0;i<itemInventory.Count;i++){
            Item temp = itemInventory[i].GetComponent<Item>();
            if(temp.Name==item.Name){
                
                return itemInventory[i];
            }
        }
        return null;
    }

}
