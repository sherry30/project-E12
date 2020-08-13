using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class City : Building
{
    public enum Type{
        camp,
        village,
    }
    public int boarderLength;
    public List<HexComponent> teritory;
    public bool capital=false;
    public int approvalThreshold;
    public int population;
    public int maxPopulation=4;
    public Type typeOfCity;
    public List<Building> buildings;

    public int unitStartIndex,numOfUnits;//indesx in PlayerController.Instance.kingdom.units //units this city can produce
    public int itemStartIndex,numOfItems;//indesx in PlayerController.Instance.kingdom.items //itemss this city can produce
    
    public int unitProduction=-1;//unit being produced rn
    public int daysTillProduced=-1;//number of days unitl the unit in productionis produced
    public int itemProduction=-1;//item being produced rn
    public List<District> districts;
    public District thisDistrict;///for now
    public bool camped=false;


    public override void Build(Vector2 coordinate){
        base.Build(coordinate);
        city = this;
        setTeritory();

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

        //checking if unit or item is under production
        if(unitProduction!=-1 || itemProduction!=-1){
            daysTillProduced-=1;
            if(daysTillProduced==0){
                //if the unit is done being produced
                if(unitProduction!=-1){
                    HexOperations.Instance.spawnUnit(location,unitProduction);

                    //destroying the info object in the producing slot if this object is selected
                    
                    unitProduction=-1;
                    daysTillProduced = -1;
                    if(GameState.Instance.selectedObject==this.gameObject){
                        UIController.Instance.openUnitHub();
                    }
                }

                //if the item is done being produced
                else{
                    AddItem(getItem(itemProduction));

                    //destroying the info object in the producing slot if this object is selected
                    
                    itemProduction=-1;
                    daysTillProduced = -1;
                    if(GameState.Instance.selectedObject==this.gameObject){
                        UIController.Instance.openBuildingHub();
                    }
                }
            }
        }
    }
    public void ProduceUnit(int index,int days){
        unitProduction = index;
        //check cost only for production now
        daysTillProduced = days;
        
    }
    public void ProduceItem(int index){
        itemProduction = index;
        //check cost only for production now
        GameObject item = getItem(index);
        Item it = item.GetComponent<Item>();
        it.player = player;
        daysTillProduced = it.daysToBeProduced;
        /*if(player==-1)
            daysTillProduced= PlayerController.Instance.player.kingdom.items[index].daysToBeProduced;
        else
            daysTillProduced= AIController.Instance.AIPlayers[player].kingdom.items[index].daysToBeProduced;*/
        
    }

    private Unit getUnit(int index){
        Unit unit = null;
        if(player==-1)
            unit = PlayerController.Instance.player.kingdom.units[index];
        else
            unit = AIController.Instance.AIPlayers[player].kingdom.units[index];
        
        return unit;
    }

    private GameObject getItem(int index){
        GameObject item = null;
        if(player==-1)
            item = PlayerController.Instance.player.kingdom.itemPrefabs[index];
        else
            item = AIController.Instance.AIPlayers[player].kingdom.itemPrefabs[index];
        
        return item;
    }
    public void upgradeToVillage(Player tempPlayer){
        if(typeOfCity==Type.camp){

            //adding Farm
            tempPlayer.availableImprovements.Add(improvement.Farm);
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


}
