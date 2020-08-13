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
    public DictionaryResInt resourcesYield;
    public DictionaryRawInt RawMaterialYield;

    public DictionaryEnergyInt energyYield;
    public DictionaryOtherResInt OtherResourcesYield;
    public City city;

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
    }
    public virtual void Demolish(){
        GameState.Instance.occupiedHexes.Remove(location);
        isBuilt=false;

    }
    private void setUpdatePosition(){
        UpdatePosition temp = GetComponent<UpdatePosition>();
        temp.location = this.location;
        temp.offset = this.offset;
    }
    public virtual void StartTurn(){
        //Debug.Log("New turn started");
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
    public virtual void setYield(){

        /*Player tempPlayer = PlayerController.Instance.player;
        //checking if energy cost is met
        if(player!=-1){
            tempPlayer = AIController.Instance.AIPlayers[player];
        }

        if(energyYield!=null){
            foreach(KeyValuePair<Energy, int> entry in energyYield){
                tempPlayer.energyYield[entry.Key]+=energyYield[entry.Key];
            }
        }
        //checking if Resource cost is met
        if(resourcesYield!=null){
            foreach(KeyValuePair<Resource, int> entry in resourcesYield){
                tempPlayer.resourcesYield[entry.Key]+=resourcesYield[entry.Key];
            }
        }
        //checking if Raw material cost is met
        if(RawMaterialYield!=null){
            foreach(KeyValuePair<Raw_Material, int> entry in RawMaterialYield){
                tempPlayer.RawMaterialYield[entry.Key]+=RawMaterialYield[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(OtherResourcesYield!=null){
            foreach(KeyValuePair<OtherResource, int> entry in OtherResourcesYield){
                tempPlayer.OtherResourcesYield[entry.Key]+=OtherResourcesYield[entry.Key];
            }
        }*/
    }
    
    public virtual void removeYield(){
        /*Player tempPlayer = PlayerController.Instance.player;
        //checking if energy cost is met
        if(player!=-1){
            tempPlayer = AIController.Instance.AIPlayers[player];
        }

        if(energyYield!=null){
            foreach(KeyValuePair<Energy, int> entry in energyYield){
                tempPlayer.energyYield[entry.Key]-=energyYield[entry.Key];
            }
        }
        //checking if Resource cost is met
        if(resourcesYield!=null){
            foreach(KeyValuePair<Resource, int> entry in resourcesYield){
                tempPlayer.resourcesYield[entry.Key]-=resourcesYield[entry.Key];
            }
        }
        //checking if Raw material cost is met
        if(RawMaterialYield!=null){
            foreach(KeyValuePair<Raw_Material, int> entry in RawMaterialYield){
                tempPlayer.RawMaterialYield[entry.Key]-=RawMaterialYield[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(OtherResourcesYield!=null){
            foreach(KeyValuePair<OtherResource, int> entry in OtherResourcesYield){
                tempPlayer.OtherResourcesYield[entry.Key]-=OtherResourcesYield[entry.Key];
            }
        }*/
    }


}
