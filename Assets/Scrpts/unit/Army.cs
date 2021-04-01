using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Army 
{

    public int unitLimit = 6;
    public List<Unit> units;
    public Vector2 location;
    public int movement=0;
    public int player = -1;
    public int armyID = -1;
    public bool moving = false;


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

        //setting units tag to army
        unit.gameObject.tag = "Army";

        //settin glocation
        location = unit.location;
    }

    public void StartTurn()
    {
        exhausted = false;
    }

    public IEnumerator moveArmy(List<HexComponent> travelPath)
    {
        if (exhausted)
            travelPath.Clear();
        exhausted = true;
        moving = true;
        //removing this army from hex component
        HexMap.Instance.getHexComponent(location).removeArmy();

        int[] done = new int[units.Count];
        for(int i=0; i < done.Length; i++)
        {
            done[i] = 0;
        }
        int index = 0;
       // mappingForMoving = new Dictionary<Unit, int>();
        foreach(Unit u in units)
        {

            //mappingForMoving.Add(u, index);
            Task t = new Task(u.moveUnit(travelPath));
            t.Finished += delegate (bool manual)
             {

                 int copy = index;
                 done[copy] = 1;
                 index++;
             };
            
        }

        bool doneMoving = false;
        while (!doneMoving)
        {
            bool flag = false;
            for(int i = 0; i < done.Length; i++)
            {
                if (done[i] == 0)
                {
                    flag = false;
                    break;
                }
                else
                    flag = true;
            }

            if (flag)
                doneMoving = true;
            yield return null;

            
        }
        location = travelPath[travelPath.Count - 1].location;
        HexMap.Instance.getHexComponent(location).addArmy(this);
        travelPath.Clear();


    }
}
