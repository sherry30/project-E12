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
        //return if already in the middle of building
        if(building || moving)
            return;
        
        //return if the civillian is out of territory
        if(player==-1){
            if(!PlayerController.Instance.player.territory.Contains(HexMap.Instance.getHexComponent(location))){
                Debug.Log("cant build here because out of territory");
                return;
            }
        }
        else{
            if(!AIController.Instance.AIPlayers[player].territory.Contains(HexMap.Instance.getHexComponent(location))){
                Debug.Log("cant build here because out of territory");
                return;
            }
        }


        if(player==-1)
            inProduction = PlayerController.Instance.player.kingdom.improvements[index];
        else
            inProduction = AIController.Instance.AIPlayers[player].kingdom.improvements[index];
        building = true;
        buildTimeLeft = inProduction.turnsTOBuild; 
        improvementIndex = index;
        paralysed = true;
        reasonForParalyzed = string.Format("building {0}",inProduction.name);
    }
    protected override void StartTurn(){
        base.StartTurn();

        if(buildTimeLeft!=-1)
            buildTimeLeft--;
        if(buildTimeLeft==0){
            HexOperations.Instance.BuildImprovement(location,improvementIndex);

            //setting everything to default once its done building
            inProduction = null;
            buildTimeLeft = -1;
            improvementIndex = -1;
            paralysed = false;
            reasonForParalyzed = "None";
        }
    }
}
