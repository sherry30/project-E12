using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : Unit
{
    [HideInInspector]
    public Improvement inProduction;
    [HideInInspector]
    public int improvementIndex=-1;
    [HideInInspector]
    public bool building=false;
    [HideInInspector]
    private int buildTimeLeft=-1;


    public void startBuilding(int index){

        //return if already in the middle of building or while moving
        if(building || moving){
            Debug.Log("Building or moving rn so cant move");
            return;
        }

        Improvement imp=null; 

        Player tempPlayer = PlayerController.Instance.player;
        if(player!=-1){
            tempPlayer = AIController.Instance.AIPlayers[player];
        }
        imp = tempPlayer.kingdom.improvements[index];
        //return if this improvement is not unlocked
        if(!city.availableImprovements.Contains(imp.imp)){
            Debug.Log(imp.Source);
            return;
        }
        
        //return if the civillian is out of territory
        if(imp.mustBeInTerritory){
            if(!tempPlayer.territory.Contains(HexMap.Instance.getHexComponent(location))){
                Debug.Log("cant build here because out of territory");
                return;
            }
        }

        //return if hex is alreadyoccupied
        if(GameState.Instance.HexOccupiedCheck(location)){
            Debug.Log("This hex is already occupied");
            return;
        }

        /*//return if Cost not met
        if(imp.cost.checkCost()){
            imp.cost.spendProduction();
        }
        else{
            Debug.Log("not enough resources");
            imp.cost.printCost();
        }*/


        inProduction = imp;
        building = true;
        buildTimeLeft = inProduction.turnsTOBuild; 
        improvementIndex = index;
        paralysed = true;
        reasonForParalyzed = string.Format("building {0}",inProduction.name);
        Debug.Log(string.Format("building {0}",inProduction.name));
    }
    public override void StartTurn(){
        base.StartTurn();

        if(buildTimeLeft!=-1)
            buildTimeLeft--;
        if(buildTimeLeft==0){
            //finding out what territory this improbvement is on default is null in case it is on no mans land, animal trap for instance
            City city = null;
            Player tempPlayer = PlayerController.Instance.player;
            
            if(player!=-1){
                tempPlayer = AIController.Instance.AIPlayers[player];
            }

            for(int i=0;i<tempPlayer.cities.Count;i++){
                if(tempPlayer.cities[i].teritory.Contains(HexMap.Instance.getHexComponent(location))){
                    city = tempPlayer.cities[i];
                }
            }
            HexOperations.Instance.BuildImprovement(location,improvementIndex,city);

            //setting everything to default once its done building
            building = false;
            inProduction = null;
            buildTimeLeft = -1;
            improvementIndex = -1;
            paralysed = false;
            reasonForParalyzed = "None";
        }
    }
}
