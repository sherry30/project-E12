using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

//storing cities own resources here
public enum cityResource{
    food,
    production,
    approval
}

[System.Serializable]
public class DictionarycityResFloat: SerializableDictionary<cityResource,float>{}  
public class City : Building
{
    public enum Type{
        camp,
        village,
    }

    
        //for making dictionary
    
    [SerializeField]
    public  DictionarycityResFloat cityResources;

    public int boarderLength;
    public List<HexComponent> teritory;
    //public Resources ownResources;

    public bool capital=false;
    public int approvalThreshold;
    public int population;
    public int maxPopulation=4;
    public Type typeOfCity;
    //the things it can produce, their indexes in kingdom

    public int unitStartIndex,numOfUnits;//indesx in PlayerController.Instance.kingdom.units //units this city can produce
    public int itemStartIndex,numOfItems;//indesx in PlayerController.Instance.kingdom.items //itemss this city can produce
    public int districtStartIndex,numOfDistricts;//indesx in PlayerController.Instance.kingdom.items //itemss this city can produce
    
    //what it is currently producing
    public int unitProduction=-1;//unit being produced rn
    public int itemProduction=-1;//item being produced rn
    public int districtProduction=-1;//unit being produced rn
    private Vector2 districtLocation;//where a districtwill be produced after its production
    [HideInInspector]
    public bool positionSelectingMode=false;

    public int daysTillProduced=-1;//number of days unitl the unit in productionis produced
    public List<District> districts;
    public District thisDistrict;///for now
    public bool camped=false;


    public override void Build(Vector2 coordinate){
        //setting its resources to 0
        cityResources = new DictionarycityResFloat();
        cityResources.Add(cityResource.food,0);
        cityResources.Add(cityResource.production,0);
        cityResources.Add(cityResource.approval,0);
        base.Build(coordinate);
        city = this;
        setTeritory();

        //setting recources yield as 2 food and 1 production for now
        resourcesYield = new Resources();
        resourcesYield.Initialize();
        //resourcesYield.resources[Resource.food]=2;
        //resourcesYield.resources[Resource.production]=1;

        //setting up district
        District dis= GetComponent<District>();
        dis.Build(location);
        dis.buildingType = BuildingType.district;
        dis.player = player;
        dis.city = this;
        if(typeOfCity==City.Type.camp){
            capital=true;    
            dis.setCamp();
        }
        else if(typeOfCity==City.Type.village){
            dis.setVillage();
        }
        //add town later as well
        thisDistrict = dis;
        upgradeToVillage(getPlayer());
        //setYield();
    }
    public void Campers(){
        if(typeOfCity==Type.camp){
            GameState.Instance.deSelectObject();
            HexOperations.Instance.DestroyCity(this);
            GameObject obj = HexOperations.Instance.spawnUnit(location,0);
        }
    }
    public override void StartTurn(){
        camped = false;

        //checking if unit or item or District is under production
        if(unitProduction!=-1 || itemProduction!=-1 || districtProduction!=-1){
            daysTillProduced-=1;
            if(daysTillProduced==0){
                //if the unit is done being produced
                if(unitProduction!=-1){
                    HexOperations.Instance.spawnUnit(location,unitProduction);

                    //destroying the info object in the producing slot if this object is selected
                    
                    unitProduction=-1;
                    daysTillProduced = -1;
                    if(GameState.Instance.selectedObject==this.gameObject){
                        UIController.Instance.openBuildingHub();
                    }
                }

                //if the item is done being produced
                else if(itemProduction!=-1){
                    AddItem(getPlayer().kingdom.itemPrefabs[itemProduction]);

                    //destroying the info object in the producing slot if this object is selected
                    
                    itemProduction=-1;
                    daysTillProduced = -1;
                    if(GameState.Instance.selectedObject==this.gameObject){
                        UIController.Instance.openBuildingHub();
                    }
                }

                //if the District is done being produced
                else if(districtProduction!=-1){
                    //AddItem(getPlayer().kingdom.itemPrefabs[itemProduction]);
                    districts.Add(getPlayer().kingdom.districts[districtProduction]);
                    HexOperations.Instance.BuildDistrict(districtLocation,districtProduction,this);

                    //destroying the info object in the producing slot if this object is selected
                    
                    districtProduction=-1;
                    daysTillProduced = -1;
                    districtLocation=Vector2.zero;
                    if(GameState.Instance.selectedObject==this.gameObject){
                        UIController.Instance.openBuildingHub();
                    }
                }
            }
        }
    }
    public void ProduceUnit(int index,int days){
        unitProduction = index;
        
        daysTillProduced = days;
        
    }
    public void ProduceItem(int index,int days){
        itemProduction = index;


        daysTillProduced = days;
        
    }
    public void ProduceDistrict(int index,int days, Vector2 loc){
        districtProduction = index;
        districtLocation = loc;
        daysTillProduced = days;

        //this place is considered occupied even before it is built
        GameState.Instance.occupiedHexes.Add(loc);
        
    }

    private Unit getUnit(int index){
        Unit unit = null;
        if(player==-1)
            unit = PlayerController.Instance.player.kingdom.units[index];
        else
            unit = AIController.Instance.AIPlayers[player].kingdom.units[index];
        
        return unit;
    }

    private Player getPlayer(){
        Player tempPlayer = PlayerController.Instance.player;
        if(player!=-1)
            tempPlayer = AIController.Instance.AIPlayers[player];
        
        return tempPlayer;
    }
    public void upgradeToVillage(Player tempPlayer){
        if(typeOfCity==Type.camp){

            //adding Farm
            tempPlayer.availableImprovements.Add(improvement.Farm);
            //adding settler
            tempPlayer.availableUnits.Add(Unit.Class.settler);
            //adding district village green
            tempPlayer.availableDistricts.Add(District.Type.village_green);
            thisDistrict.setVillage();
            typeOfCity = Type.village;
            boarderLength=2;
            maxPopulation=10;
            setTeritory();

        }
    }
    
    private void setTeritory(){        
        teritory = HexOperations.Instance.getNeighbors(location,boarderLength).ToList();
    }
    //to add all the yield at the strat of the turn
    public void getYield(){

        Player tempPlayer = PlayerController.Instance.player;
        if(player!=-1){
            tempPlayer = AIController.Instance.AIPlayers[player];
        }

        if(resourcesYield.Energies!=null){
            foreach(KeyValuePair<Energy, float> entry in resourcesYield.Energies){
                tempPlayer.resources.Energies[entry.Key]+=resourcesYield.Energies[entry.Key];
            }
        }
        //checking if Resource cost is met
        if(resourcesYield.resources!=null){
            foreach(KeyValuePair<Resource, float> entry in resourcesYield.resources){
                tempPlayer.resources.resources[entry.Key]+=resourcesYield.resources[entry.Key];
            }
        }
        //checking if Raw material cost is met
        if(resourcesYield.RawMaterials!=null){
            foreach(KeyValuePair<Raw_Material, float> entry in resourcesYield.RawMaterials){
                tempPlayer.resources.RawMaterials[entry.Key]+=resourcesYield.RawMaterials[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(resourcesYield.OtherResources!=null){
            foreach(KeyValuePair<OtherResource, float> entry in resourcesYield.OtherResources){
                tempPlayer.resources.OtherResources[entry.Key]+=resourcesYield.OtherResources[entry.Key];
            }
        }
        //checking if Crystals cost is met
        if(resourcesYield.Crystals!=null){
            foreach(KeyValuePair<crystal, float> entry in resourcesYield.Crystals){
                tempPlayer.resources.Crystals[entry.Key]+=resourcesYield.Crystals[entry.Key];
            }
        }

        if(resourcesYield.cityResources!=null){
            foreach(KeyValuePair<cityResource, float> entry in resourcesYield.cityResources){
                cityResources[entry.Key]+=resourcesYield.cityResources[entry.Key];
            }
        }
        /*if(OtherResourcesYield!=null){
            foreach(KeyValuePair<OtherResource, float> entry in OtherResourcesYield){
                tempPlayer.resources.OtherResources[entry.Key]+=OtherResourcesYield[entry.Key];
            }
        }*/
    }

    //for growing population at the start of the turn
    public void populationGrowth(){
        while(true){
            int result = (int)(10+5*(population+Math.Pow(population,1.5)));
            if(cityResources[cityResource.food]>=result){
                population++;
                cityResources[cityResource.food]-=result;
            }
            else
                break;
        }
    }

   /* public override void setYield(){
        if(ownResources.Energies!=null){
            foreach(KeyValuePair<Energy, float> entry in ownResources.Energies){
                city.resourcesYield.Energies[entry.Key]+=ownResources.Energies[entry.Key];
            }
        }
        //checking if Resource cost is met
        if(ownResources.resources!=null){
            foreach(KeyValuePair<Resource, float> entry in ownResources.resources){
                city.resourcesYield.resources[entry.Key]+=ownResources.resources[entry.Key];
            }
        }
        //checking if Raw material cost is met
        if(ownResources.RawMaterials!=null){
            foreach(KeyValuePair<Raw_Material,float> entry in ownResources.RawMaterials){
                city.resourcesYield.RawMaterials[entry.Key]+=ownResources.RawMaterials[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(ownResources.OtherResources!=null){
            foreach(KeyValuePair<OtherResource,float> entry in ownResources.OtherResources){
                city.resourcesYield.OtherResources[entry.Key]+=ownResources.OtherResources[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(ownResources.Crystals!=null){
            foreach(KeyValuePair<crystal,float> entry in ownResources.Crystals){
                city.resourcesYield.Crystals[entry.Key]+=ownResources.Crystals[entry.Key];
            }
        }
    }
    public override void removeYield(){
        //checking if energy cost is me

        if(ownResources.Energies!=null){
            foreach(KeyValuePair<Energy, float> entry in ownResources.Energies){
                city.resourcesYield.Energies[entry.Key]-=ownResources.Energies[entry.Key];
            }
        }
        //checking if Resource cost is met
        if(ownResources.resources!=null){
            foreach(KeyValuePair<Resource, float> entry in ownResources.resources){
                city.resourcesYield.resources[entry.Key]-=ownResources.resources[entry.Key];
            }
        }
        //checking if Raw material cost is met
        if(ownResources.RawMaterials!=null){
            foreach(KeyValuePair<Raw_Material,float> entry in ownResources.RawMaterials){
                city.resourcesYield.RawMaterials[entry.Key]-=ownResources.RawMaterials[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(ownResources.OtherResources!=null){
            foreach(KeyValuePair<OtherResource,float> entry in ownResources.OtherResources){
                city.resourcesYield.OtherResources[entry.Key]-=ownResources.OtherResources[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(ownResources.Crystals!=null){
            foreach(KeyValuePair<crystal,float> entry in ownResources.Crystals){
                city.resourcesYield.Crystals[entry.Key]-=ownResources.Crystals[entry.Key];
            }
        }
    }*/


}
