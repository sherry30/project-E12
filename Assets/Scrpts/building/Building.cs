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
    [HideInInspector]
    public static int currentID=0;
    public Sprite icon;
    public string Name;
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
    /*public DictionaryResFloat resourcesYield;
    public DictionaryRawFloat RawMaterialYield;

    public DictionaryEnergyFloat energyYield;
    public DictionaryOtherResFloat OtherResourcesYield;*/
    public Resources resourcesYield;
    public City city;//city this building is under; mainly for districtsand improvements adn units

    //[HideInInspector]
    public float populationGrowthHelp = 0;
    //[HideInInspector]
    public float unitProductionHelp = 0;
    public Resources percentageExtraResources;



    void Awake(){
        Vector3 temp = GetComponentInChildren<Collider>().bounds.size;
        offset = new Vector3(0, (temp.y/2f)+Hex.hexHeight-GetComponentInChildren<Renderer>().bounds.center.y+0.5f,0);
        setUpdatePosition();
        //resourcesYield = new Resources();
        //resourcesYield.Initialize();
    }


    //protected abstract void setCost();
    public virtual void Build(Vector2 coordinate){

        //setting location on this class
        location = coordinate;
        offset.y += HexMap.Instance.getHexComponent(location).elevation * Hex.hexHeight*2;
        setUpdatePosition();
        //letting gamestate know this hex isoocupied
        if(!GameState.Instance.HexOccupiedCheck(coordinate))
            GameState.Instance.occupiedHexes.Add(coordinate);
        isBuilt = true;
        //setting up id for this building or city
        id = currentID;
        currentID++;

        setAdditionalHelp();

        setYield();
        setPercentYield();
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
        if(resourcesYield.Energies!=null){
            foreach(KeyValuePair<Energy, float> entry in resourcesYield.Energies){
                city.resourcesYield.Energies[entry.Key]+=resourcesYield.Energies[entry.Key];
            }
        }
        //checking if Resource cost is met
        if(resourcesYield.resources!=null){
            foreach(KeyValuePair<Resource, float> entry in resourcesYield.resources){
                city.resourcesYield.resources[entry.Key]+=resourcesYield.resources[entry.Key];
            }
        }
        //checking if Raw material cost is met
        if(resourcesYield.RawMaterials!=null){
            foreach(KeyValuePair<Raw_Material,float> entry in resourcesYield.RawMaterials){
                city.resourcesYield.RawMaterials[entry.Key]+=resourcesYield.RawMaterials[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(resourcesYield.OtherResources!=null){
            foreach(KeyValuePair<OtherResource,float> entry in resourcesYield.OtherResources){
                city.resourcesYield.OtherResources[entry.Key]+=resourcesYield.OtherResources[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(resourcesYield.Crystals!=null){
            foreach(KeyValuePair<crystal,float> entry in resourcesYield.Crystals){
                city.resourcesYield.Crystals[entry.Key]+=resourcesYield.Crystals[entry.Key];
            }
        }

        if(resourcesYield.cityResources!=null){
            foreach(KeyValuePair<cityResource,float> entry in resourcesYield.cityResources){
                city.resourcesYield.cityResources[entry.Key]+=resourcesYield.cityResources[entry.Key];
            }
        }
    }
    
    public virtual void removeYield(){
        //checking if energy cost is me

        if(resourcesYield.Energies!=null){
            foreach(KeyValuePair<Energy, float> entry in resourcesYield.Energies){
                city.resourcesYield.Energies[entry.Key]-=resourcesYield.Energies[entry.Key];
            }
        }
        //checking if Resource cost is met
        if(resourcesYield.resources!=null){
            foreach(KeyValuePair<Resource, float> entry in resourcesYield.resources){
                city.resourcesYield.resources[entry.Key]-=resourcesYield.resources[entry.Key];
            }
        }
        //checking if Raw material cost is met
        if(resourcesYield.RawMaterials!=null){
            foreach(KeyValuePair<Raw_Material,float> entry in resourcesYield.RawMaterials){
                city.resourcesYield.RawMaterials[entry.Key]-=resourcesYield.RawMaterials[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(resourcesYield.OtherResources!=null){
            foreach(KeyValuePair<OtherResource,float> entry in resourcesYield.OtherResources){
                city.resourcesYield.OtherResources[entry.Key]-=resourcesYield.OtherResources[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(resourcesYield.Crystals!=null){
            foreach(KeyValuePair<crystal,float> entry in resourcesYield.Crystals){
                city.resourcesYield.Crystals[entry.Key]-=resourcesYield.Crystals[entry.Key];
            }
        }
        if(resourcesYield.cityResources!=null){
            foreach(KeyValuePair<cityResource,float> entry in resourcesYield.cityResources){
                city.resourcesYield.cityResources[entry.Key]-=resourcesYield.cityResources[entry.Key];
            }
        }
    }

    //for setting help in % for unit production and population growth
    public virtual void setAdditionalHelp(){
        city.populationGrowthHelp+=populationGrowthHelp;
        city.unitProductionHelp += unitProductionHelp;
    }

    public Player getPlayer(){
        Player tempPlayer = PlayerController.Instance.player;
        if(player!=-1)
            tempPlayer = AIController.Instance.AIPlayers[player];
        
        return tempPlayer;
    }


    public virtual void setPercentYield(){
        if(percentageExtraResources.Energies!=null){
            foreach(KeyValuePair<Energy, float> entry in percentageExtraResources.Energies){
                city.percentageExtraResources.Energies[entry.Key]+=percentageExtraResources.Energies[entry.Key];
            }
        }
        //checking if Resource cost is met
        if(percentageExtraResources.resources!=null){
            foreach(KeyValuePair<Resource, float> entry in percentageExtraResources.resources){
                city.percentageExtraResources.resources[entry.Key]+=percentageExtraResources.resources[entry.Key];
            }
        }
        //checking if Raw material cost is met
        if(percentageExtraResources.RawMaterials!=null){
            foreach(KeyValuePair<Raw_Material,float> entry in percentageExtraResources.RawMaterials){
                city.percentageExtraResources.RawMaterials[entry.Key]+=percentageExtraResources.RawMaterials[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(percentageExtraResources.OtherResources!=null){
            foreach(KeyValuePair<OtherResource,float> entry in percentageExtraResources.OtherResources){
                city.percentageExtraResources.OtherResources[entry.Key]+=percentageExtraResources.OtherResources[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(percentageExtraResources.Crystals!=null){
            foreach(KeyValuePair<crystal,float> entry in percentageExtraResources.Crystals){
                city.percentageExtraResources.Crystals[entry.Key]+=percentageExtraResources.Crystals[entry.Key];
            }
        }
        if(percentageExtraResources.cityResources!=null){
            foreach(KeyValuePair<cityResource,float> entry in percentageExtraResources.cityResources){
                city.percentageExtraResources.cityResources[entry.Key]-=percentageExtraResources.cityResources[entry.Key];
            }
        }
    }

}
