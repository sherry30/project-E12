using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : Building
{
    public enum Type{
        camp,
        village,
    }
    public int boarderLength;
    public bool capital=true;
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
    public void Campers(){
        if(typeOfCity==Type.camp){
            GameState.Instance.deSelectObject();
            HexOperations.Instance.DestroyCity(this);
            GameObject obj = HexOperations.Instance.spawnUnit(location,0);
            
        }
    }
    protected override void StartTurn(){
        camped = false;
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
    public void upgradeToVillage(){
        if(typeOfCity==Type.camp){
            typeOfCity = Type.village;
            boarderLength=2;
            maxPopulation=10;

        }
    }
    


}
