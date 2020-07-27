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
    public int approvalThreshold;
    public int population;
    public Type typeOfCity;
    public List<Building> buildings;

    public int unitStartIndex,numOfUnits;//indesx in PlayerController.Instance.kingdom.units //units this city can produce
    
    public int unitProduction=-1;//unit being produced rn
    public int daysTillProduced=-1;//number of days unitl the unit in productionis produced
    private List<District> districts{get;set;}//for now
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
        if(unitProduction!=-1){
            daysTillProduced-=1;
            if(daysTillProduced==0){
                HexOperations.Instance.spawnUnit(location,unitProduction);
                unitProduction=-1;
                daysTillProduced = -1;
            }
        }
    }
    public void ProduceUnit(int index,Unit unit){
        unitProduction = index;
        //check cost only for production now
        int var1 = unit.cost.getProduction();
        int var2 = PlayerController.Instance.player.Resources[Resource.production];
        int days = var1/var2;
        if(var1%var2!=0)
            days++;
        daysTillProduced = days;

        
        //TODO: check cost for rest ofrequirements
        
    }
    


}
