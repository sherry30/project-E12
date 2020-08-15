using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class District : Building
{
    public enum Type{
        camp,
        chiefs_hut,
        village_green,
        growth,
        military,
        knowledge,
        Forge,
        market_place,
        shipyard,
        air_shipyard
    }
    //List<Building> buildings;
    public int buildingStartingIndex, numOfBuildings;
    public int limitOfBuildings=3;
    public Type type;
    public int level;
    public int daysToBeProduced=2;
    //public int maxPopulation;


    
    public override void StartTurn(){
    }
    public void setCamp(){
        city = GetComponent<City>();
        //setting up yields
        resourcesYield = new DictionaryResInt();
        resourcesYield.Add(Resource.production,2);
        resourcesYield.Add(Resource.food,2);
        energyYield= new DictionaryEnergyInt();
        energyYield.Add(Energy.Fire,1);

        //maxPopulation = 7;

        //type
        type = Type.camp;
        setYield();
    }
    public void setVillage(){
        removeYield();
        //setting up yields
        resourcesYield = new DictionaryResInt();
        resourcesYield.Add(Resource.production,2);
        resourcesYield.Add(Resource.food,2);
        energyYield= new DictionaryEnergyInt();
        energyYield.Add(Energy.Fire,1);
        OtherResourcesYield = new DictionaryOtherResInt();
        OtherResourcesYield.Add(OtherResource.approval,2);

        //maxPopulation = 10;

        type = Type.chiefs_hut;
        setYield();
    }

    public override void setYield(){
        if(energyYield!=null){
            foreach(KeyValuePair<Energy, int> entry in energyYield){
                city.energyYield[entry.Key]+=energyYield[entry.Key];
            }
        }
        //checking if Resource cost is met
        if(resourcesYield!=null){
            foreach(KeyValuePair<Resource, int> entry in resourcesYield){
                city.resourcesYield[entry.Key]+=resourcesYield[entry.Key];
            }
        }
        //checking if Raw material cost is met
        if(RawMaterialYield!=null){
            foreach(KeyValuePair<Raw_Material, int> entry in RawMaterialYield){
                city.RawMaterialYield[entry.Key]+=RawMaterialYield[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(OtherResourcesYield!=null){
            foreach(KeyValuePair<OtherResource, int> entry in OtherResourcesYield){
                city.OtherResourcesYield[entry.Key]+=OtherResourcesYield[entry.Key];
            }
        }
    }
    public override void removeYield(){
        //checking if energy cost is me

        if(energyYield!=null){
            foreach(KeyValuePair<Energy, int> entry in energyYield){
                city.energyYield[entry.Key]-=energyYield[entry.Key];
            }
        }
        //checking if Resource cost is met
        if(resourcesYield!=null){
            foreach(KeyValuePair<Resource, int> entry in resourcesYield){
                city.resourcesYield[entry.Key]-=resourcesYield[entry.Key];
            }
        }
        //checking if Raw material cost is met
        if(RawMaterialYield!=null){
            foreach(KeyValuePair<Raw_Material, int> entry in RawMaterialYield){
                city.RawMaterialYield[entry.Key]-=RawMaterialYield[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(OtherResourcesYield!=null){
            foreach(KeyValuePair<OtherResource, int> entry in OtherResourcesYield){
                city.OtherResourcesYield[entry.Key]-=OtherResourcesYield[entry.Key];
            }
        }
    }
}
