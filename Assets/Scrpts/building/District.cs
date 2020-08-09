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
    List<Building> buildings;
    public int limitOfBuildings=3;
    public Type type;
    public int maxPopulation;

    //the things it willprovide at the strat of your turn
    public DictionaryResInt resourcesYield;
    public DictionaryRawInt RawMaterialYield;

    public DictionaryEnergyInt energyYield;
    public DictionaryOtherResInt OtherResourcesYield;
    public virtual void SpecialSkill(){

    }
    
    protected override void StartTurn(){
        //adding all the yields tothe player

        Player tempPlayer = PlayerController.Instance.player;
        //checking if energy cost is met
        if(player!=-1){
            tempPlayer = AIController.Instance.AIPlayers[player];
        }

        if(energyYield!=null){
            foreach(KeyValuePair<Energy, int> entry in energyYield){
                tempPlayer.Energies[entry.Key]+=energyYield[entry.Key];
            }
        }
        //checking if Resource cost is met
        if(resourcesYield!=null){
            foreach(KeyValuePair<Resource, int> entry in resourcesYield){
                tempPlayer.Resources[entry.Key]+=resourcesYield[entry.Key];
            }
        }
        //checking if Raw material cost is met
        if(RawMaterialYield!=null){
            foreach(KeyValuePair<Raw_Material, int> entry in RawMaterialYield){
                tempPlayer.RawMaterials[entry.Key]+=RawMaterialYield[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(OtherResourcesYield!=null){
            foreach(KeyValuePair<OtherResource, int> entry in OtherResourcesYield){
                tempPlayer.OtherResources[entry.Key]+=OtherResourcesYield[entry.Key];
            }
        }
    }
    public void setCamp(){
        //setting up yields
        resourcesYield = new DictionaryResInt();
        resourcesYield.Add(Resource.production,2);
        resourcesYield.Add(Resource.food,2);
        energyYield= new DictionaryEnergyInt();
        energyYield.Add(Energy.Fire,1);

        maxPopulation = 7;

        //type
        type = Type.camp;
    }
    public void setVillage(){
        //setting up yields
        resourcesYield = new DictionaryResInt();
        resourcesYield.Add(Resource.production,2);
        resourcesYield.Add(Resource.food,2);
        energyYield= new DictionaryEnergyInt();
        energyYield.Add(Energy.Fire,1);
        OtherResourcesYield = new DictionaryOtherResInt();
        OtherResourcesYield.Add(OtherResource.approval,2);

        maxPopulation = 10;

        type = Type.chiefs_hut;
    }
}
