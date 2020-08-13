using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalTrap : Improvement
{
    public int trappedTurns=3;
    private int trappedTurnsLeft=0;
    public bool trapped=false;
    public Unit trappedUnit;
    void Start(){
        trappedUnit = null;
        Source = "P1 must be unloacked first";
    }

    public override void unitAddedToTheHex(){
        HexComponent tempHex = HexMap.Instance.getHexComponent(location);
        if(tempHex.containEnemies()){
            //first enemy on the tile gets trapped
            //if theres already one on here
            trappedUnit = tempHex.getEnemy();
            trapped = true;
            trappedTurnsLeft = trappedTurns;

            //making the unit paralyzed
            trappedUnit.paralysed = true;
            trappedUnit.reasonForParalyzed = "trapped by an Animal Trap";
        }
    }
    public override void StartTurn(){
        base.StartTurn();

        //ifa unit istrapped
        if(trapped){
            trappedTurnsLeft--;
            if(trappedTurnsLeft==0){
                trapped = false;
                trappedUnit.paralysed = false;
                trappedUnit.reasonForParalyzed = "None";
                trappedUnit=null;
            }
        }


    }
}
