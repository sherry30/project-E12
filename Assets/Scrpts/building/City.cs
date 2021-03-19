using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

//storing cities own resources here
public enum cityResource{
    food,
    production,
    approval,
    Fire
}

public enum FireEarthCityResource{
    food,
    production,
    approval,
    Fire
}
[System.Serializable]
public class DictionarycityResFloat: SerializableDictionary<cityResource,float>{}  
[System.Serializable]
public class DictionaryFireEarthCityResFloat: SerializableDictionary<FireEarthCityResource,float>{} 
public class City : Building
{
    public enum Type{
        camp,
        village,
    }

    
        //for making dictionary
    
    [SerializeField]
    public  DictionarycityResFloat cityResources;
    [HideInInspector]

    public int boarderLength;
    [HideInInspector]
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
    [HideInInspector]
    //what it is currently producing
    public int unitProduction=-1;//unit being produced rn
    [HideInInspector]
    public int itemProduction=-1;//item being produced rn
    [HideInInspector]
    public int districtProduction=-1;//unit being produced rn
    [HideInInspector]
    public float productionConsumption=0;
    [HideInInspector]
    public float ProductionLeftAtEnd=0;

    [HideInInspector]
    private Vector2 districtLocation;//where a districtwill be produced after its production
    [HideInInspector]
    public bool positionSelectingMode=false;

    public int daysTillProduced=-1;//number of days unitl the unit in productionis produced
    public List<District> districts;
    public District thisDistrict;///for now
    public bool camped=false;

    //available stuff in this city

    public List<improvement> availableImprovements;
    public List<Unit.Class> availableUnits;
    public List<District.Type> availableDistricts;

    
    //TODO: add extrapercentages to recourcesYield before getYield


    public override void Build(Vector2 coordinate){
        //setting its resources to 0
        city = this;
        cityResources = new DictionarycityResFloat();
        cityResources.Add(cityResource.food,0);
        cityResources.Add(cityResource.production,0);
        cityResources.Add(cityResource.approval,0);
        cityResources.Add(cityResource.Fire,0);

        base.Build(coordinate);

        populationGrowthHelp=0f;
        unitProductionHelp = 0f;
        
        setTeritory();

        //setting up available stuff in this city
        availableImprovements =  getPlayer().availableImprovements;
        availableDistricts = getPlayer().availableDistricts;
        availableUnits = getPlayer().availableUnits;

        //setting recources yield 
        resourcesYield = new Resources();
        resourcesYield.Initialize();

        percentageExtraResources = new Resources();
        percentageExtraResources.Initialize();

        //setting up district
        District dis= GetComponent<District>();
        dis.city = this;
        dis.Build(location);
        dis.buildingType = BuildingType.district;
        dis.player = player;
        

        //setting stratturn on this district
        getPlayer().onStartTurn+=dis.StartTurn;
        if(typeOfCity==City.Type.camp){
            capital=true;    
            dis.setCamp();
        }
        else if(typeOfCity==City.Type.village){
            dis.setVillage();
        }
        //add town later as well
        thisDistrict = dis;
        //upgradeToVillage(getPlayer());
    }
    public void Campers(){
        if(typeOfCity==Type.camp){
            GameState.Instance.deSelectObject();
            HexOperations.Instance.DestroyCity(this);
            GameObject obj = HexOperations.Instance.spawnUnit(city,0);
        }
    }
    public override void StartTurn(){
        camped = false;

        //checking if unit or item or District is under production
        if(unitProduction!=-1 || itemProduction!=-1 || districtProduction!=-1){
            daysTillProduced-=1;
            cityResources[cityResource.production] -= productionConsumption;


            if(daysTillProduced==0){
                productionConsumption = 0;
                cityResources[cityResource.production] += ProductionLeftAtEnd;
                ProductionLeftAtEnd = 0;
                //if the unit is done being produced
                if (unitProduction!=-1){
                    
                    HexOperations.Instance.spawnUnit(city,unitProduction);

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

        //after checking the production of unit, item or district, checking poopulation growth
        populationGrowth();

    }
    public void ProduceUnit(int index,int days){
        unitProduction = index;
        
        daysTillProduced = days;

        productionConsumption = resourcesYield.cityResources[cityResource.production];
        
    }
    public void ProduceItem(int index,int days){
        itemProduction = index;


        daysTillProduced = days;
        
    }
    public void ProduceDistrict(int index,int days, Vector2 loc){
        productionConsumption = resourcesYield.cityResources[cityResource.production];
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

    
    public void upgradeToVillage(Player tempPlayer){
        if(typeOfCity==Type.camp){

            //adding Farm
            availableImprovements.Add(improvement.Farm);
            //adding settler
            availableUnits.Add(Unit.Class.settler);

            //adding all districts this city is capable of producing
            for(int i=districtStartIndex;i<numOfDistricts;i++){
                availableDistricts.Add(getPlayer().kingdom.districts[i].type);
            }
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
    public virtual void getYield(){

        Player tempPlayer = PlayerController.Instance.player;
        if(player!=-1){
            tempPlayer = AIController.Instance.AIPlayers[player];
        }

        if(resourcesYield.Energies!=null){
            foreach(KeyValuePair<Energy, float> entry in resourcesYield.Energies){
                tempPlayer.resources.Energies[entry.Key]+=resourcesYield.Energies[entry.Key] + tempPlayer.resources.Energies[entry.Key]*percentageExtraResources.Energies[entry.Key];
            }
        }
        //checking if Resource cost is met
        if(resourcesYield.resources!=null){
            foreach(KeyValuePair<Resource, float> entry in resourcesYield.resources){
                tempPlayer.resources.resources[entry.Key]+=resourcesYield.resources[entry.Key] + tempPlayer.resources.resources[entry.Key]*percentageExtraResources.resources[entry.Key];
            }
        }
        //checking if Raw material cost is met
        if(resourcesYield.RawMaterials!=null){
            foreach(KeyValuePair<Raw_Material, float> entry in resourcesYield.RawMaterials){
                tempPlayer.resources.RawMaterials[entry.Key]+=resourcesYield.RawMaterials[entry.Key] + tempPlayer.resources.RawMaterials[entry.Key]*percentageExtraResources.RawMaterials[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(resourcesYield.OtherResources!=null){
            foreach(KeyValuePair<OtherResource, float> entry in resourcesYield.OtherResources){
                tempPlayer.resources.OtherResources[entry.Key]+=resourcesYield.OtherResources[entry.Key] + tempPlayer.resources.OtherResources[entry.Key]*percentageExtraResources.OtherResources[entry.Key];
            }
        }
        //checking if Crystals cost is met
        if(resourcesYield.Crystals!=null){
            foreach(KeyValuePair<crystal, float> entry in resourcesYield.Crystals){
                tempPlayer.resources.Crystals[entry.Key]+=resourcesYield.Crystals[entry.Key] + tempPlayer.resources.Crystals[entry.Key]* percentageExtraResources.Crystals[entry.Key];
            }
        }

        if(resourcesYield.cityResources!=null){
            foreach(KeyValuePair<cityResource, float> entry in resourcesYield.cityResources){
                //approval will not be added every turn
                if(entry.Key==cityResource.approval){
                    cityResources[cityResource.approval] = entry.Value;
                    continue;
                }
                cityResources[entry.Key]+=resourcesYield.cityResources[entry.Key] + cityResources[entry.Key]*percentageExtraResources.cityResources[entry.Key];
            }
        }
        /*if(OtherResourcesYield!=null){
            foreach(KeyValuePair<OtherResource, float> entry in OtherResourcesYield){
                tempPlayer.resources.OtherResources[entry.Key]+=OtherResourcesYield[entry.Key];
            }
        }*/
    }

    //for growing population at the start of the turn
    public virtual void populationGrowth(){
        while(true){
            int result = (int)(10+5*(population+Math.Pow(population,1.5)));
            if(cityResources[cityResource.food]+cityResources[cityResource.food]*populationGrowthHelp>=result){
                population++;
                cityResources[cityResource.food]-=result;
                if(cityResources[cityResource.food]<=0)
                    cityResources[cityResource.food]=0;
                //telling all districts that population changed
                thisDistrict.populationChanged();
                foreach(District d in districts){
                    d.populationChanged();
                }
            }
            else
                break;
        }
    }


    //wont do anything for city
    public override void setYield(){

    }
    public override void removeYield(){

    }

    public override void setPercentYield(){

    }
    /*public override void removeYield(){

    }*/

    public void setAdditionalSciencePercent(float extra){

    }


}
