using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : Building
{
    public enum Type{
        camp,
        village,
        city,
        capital
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
    public List<District> districts;//for now
    public bool camped=false;
    public void Campers(){
        if(typeOfCity==Type.camp){
            GameState.Instance.deSelectObject();
            HexOperations.Instance.DestroyCity(this);
            GameObject obj = HexOperations.Instance.spawnUnit(location,unitStartIndex);
            
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
                    unitProduction=-1;
                    daysTillProduced = -1;
                }

                //if the item is done being produced
                else{
                    //HexOperations.Instance.spawnUnit(location,unitProduction);
                    AddItem(getItem(itemProduction));
                    itemProduction=-1;
                    daysTillProduced = -1;
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
        if(player==-1)
            daysTillProduced= PlayerController.Instance.player.kingdom.items[index].daysToBeProduced;
        else
            daysTillProduced= AIController.Instance.AIPlayers[player].kingdom.items[index].daysToBeProduced;
        
    }

    private Unit getUnit(int index){
        Unit unit = null;
        if(player==-1)
            unit = PlayerController.Instance.player.kingdom.units[index];
        else
            unit = AIController.Instance.AIPlayers[player].kingdom.units[index];
        
        return unit;
    }

    private Item getItem(int index){
        Item item = null;
        if(player==-1)
            item = PlayerController.Instance.player.kingdom.items[index];
        else
            item = AIController.Instance.AIPlayers[player].kingdom.items[index];
        
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
