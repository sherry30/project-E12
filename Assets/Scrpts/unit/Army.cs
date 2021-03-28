using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Army 
{

    public int unitLimit = 6;
    public List<Unit> units;
    public int movement=0;
    public int player = -1;
    public int armyID = -1;

    public bool exhausted = false;

    public Army(int id)
    {
        armyID = id;
    }
    public void addUnit(Unit unit)
    {
        if (units == null)
            units = new List<Unit>();
        units.Add(unit);
        unit.joinArmy(armyID);

        //setting movement
        if (unit.movement < movement)
            movement = unit.movement;
        if (movement == 0)
            movement = unit.movement;

        exhausted = true;
    }

    public void StartTurn()
    {
        exhausted = false;
    }
}
