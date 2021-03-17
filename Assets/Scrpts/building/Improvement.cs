using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum improvement{
    animal_Trap,
    Farm
}
public class Improvement : Building
{
    public improvement imp;
    public int turnsTOBuild=2;
    public bool mustBeInTerritory=false;
    public string Source;
    
    /*public int turnsLeft=-1;
    public void startConstruction(){
        turnsLeft = turnsTOBuild;
    }
    protected override void StartTurn(){
        if(turnsLeft!=-1)
            turnsLeft--;
        if(turnsLeft==0)
            HexOperations.Instance.BuildImprovement(this);
    }*/
}
