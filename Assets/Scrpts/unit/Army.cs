using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Army 
{

    public int unitLimit = 6;
    public int capacity=0;
    public List<Unit> units;
    public Vector2 location;
    public int movement=0;
    public int player = -1;
    public int armyID = -1;
    public bool moving = false;

    public List<Unit.Class> classRestrictedTo2;
    public List<Unit.subClass> subClassRestrictedTo2;
    public Dictionary<Unit.Class, int> capacityTaken; // by default 1

    private PathFinding pathFinder = new PathFinding();
    public bool exhausted = false;

    public Army(int id, int unitLimit=4)
    {
        armyID = id;
        this.unitLimit = unitLimit;

        //adding which units are restriceted to 2
        classRestrictedTo2 = new List<Unit.Class>();
        classRestrictedTo2.Add(Unit.Class.mage);

        //restriction to sub Classes
        subClassRestrictedTo2 = new List<Unit.subClass>();
        subClassRestrictedTo2.Add(Unit.subClass.range_longbowman);
        subClassRestrictedTo2.Add(Unit.subClass.range_crossbowman);

        //extra capacity taken by bigger units
        capacityTaken = new Dictionary<Unit.Class, int>();
        capacityTaken.Add(Unit.Class.siege, 2);
        capacityTaken.Add(Unit.Class.tank, 2);
    }
    public void addUnit(Unit unit)
    {

        //checking restrictions
        //checknig capacity
        if (capacity >= unitLimit)
        {

        }


        //if unit is already in an army
        if (unit.isInArmy())
        {
            Debug.Log("This unit is already in army");
            return;
        }

        //gettin ghex comp
        HexComponent hexComp = HexMap.Instance.getHexComponent(unit.location);

        if (hexComp.units.Count <= 1)
        {
            Debug.Log("this unit is alone in this hex");
            return;
        }

        //checking if nuits which are restricted to 2 are not already full
        if (classRestrictedTo2.Contains(unit.classOfUnit)) {
            foreach (Unit u in units)
            {
                if (u.classOfUnit == unit.classOfUnit)
                {
                    Debug.Log("This Army cannot contain 2 "+unit.classOfUnit.ToString());
                    return;
                }
            }
        }

        if (subClassRestrictedTo2.Contains(unit.subClassOfUnit))
        {
            foreach (Unit u in units)
            {
                if (u.subClassOfUnit == unit.subClassOfUnit)
                {
                    Debug.Log("This Army cannot contain 2 " + unit.subClassOfUnit.ToString());
                    return;
                }
            }
        }


        //adding the unit to army after checking all restrictions
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

        //increasin gcapacity
        if (capacityTaken.ContainsKey(unit.classOfUnit))
            capacity += capacityTaken[unit.classOfUnit];
        else
            capacity++;
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

    public IEnumerator startArmyMove(GameObject newSelectedObject)
    {

        //restrictions
        //if army is exhausted

        if (exhausted)
            yield break;


        bool attacking = false;
        Unit enemy = null;

        HexComponent source = HexMap.Instance.getHexComponent(location);
        HexComponent dest = null;
        bool doneMoving = false;
        if (newSelectedObject.tag == "Hex")
            dest = newSelectedObject.GetComponent<HexComponent>();
        else if (newSelectedObject.tag == "enemy")
        {   //if selected is enemy
            //Debug.Log("ran");
            dest = HexMap.Instance.getHexComponent(newSelectedObject.GetComponent<Unit>().location);
            //Debug.Log(string.Format("location: {0}",dest.location));
        }

        //add more in case of playerclicks on cities and buildings and stuff
        List<HexComponent> travelPath = new List<HexComponent>();
        travelPath = pathFinder.shortesPath(source, dest);


        //if it contains enemies
        /* if(dest.containEnemies()){
             Debug.Log("This Hex contans enemies");
             enemy = dest.getEnemy();
             travelPath.RemoveAt(travelPath.Count-1);
             if(travelPath!=null && travelPath.Count>0)
                 dest = travelPath[travelPath.Count-1];

             ///in case its right next to the player
             else{
                 unit.dealDamage(enemy);
                 travelPath.Clear();
                 moving = false;
                 return;
             }
             attacking=true;
         }*/
        //if nuit is too far
        if (travelPath.Count > movement)
        {
            Debug.Log("Too far");
            int tempINT = travelPath.Count;
            for (int i = movement; i < tempINT; i++)
            {
                travelPath.RemoveAt(movement);
            }
        }



        //moving the army
        Task t = new Task(moveArmy(travelPath));
        t.Finished += delegate (bool manual)
        {
            // if (attacking)
            //   unit.dealDamage(enemy);
            travelPath.Clear();
            moving = false;
            doneMoving = true;
        };

        while (!doneMoving)
        {
            yield return null;
        }
    }

    public bool isCapacityFull()
    {
        return capacity >= unitLimit;
    }
}

