using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    public static CombatManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitiateFight(Army army, Army opposingArmy)
    {

        //sorint by agility in descending order
        List<Unit> agiSortedArmy =sortByAgility(army.units);
        
        List<Unit> agiSortedOpposingArmy = sortByAgility(opposingArmy.units);

        //number of gihts
        int numOfFights = 0;
        if (agiSortedArmy.Count<agiSortedOpposingArmy.Count)
            numOfFights = agiSortedArmy.Count;
        else if (agiSortedArmy.Count > agiSortedOpposingArmy.Count)
            numOfFights = agiSortedOpposingArmy.Count;
        else
            numOfFights = agiSortedOpposingArmy.Count;

        //initiaitng fights between highest agility 
        for(int i = 0; i < numOfFights; i++)
        {
            InitiateFight(agiSortedArmy[i],agiSortedOpposingArmy[i] );
        }
    }


    //desxcending order
    private List<Unit> sortByAgility(List<Unit> list)
    {
        List<Unit> newList = new List<Unit>(list);
        for(int j=0; j < newList.Count - 1; j++)
        {
            for(int i=0; i < newList.Count - 1; i++)
            {
                if(newList[i].stats.agi< newList[i+1].stats.agi)
                {
                    Unit temp = newList[i];
                    newList[i] = newList[i + 1];
                    newList[i + 1] = temp;
                }
            }
        }

        return newList;
    }

    public void InitiateFight(Unit unit, Unit opposingUnit)
    {
        bool unitGoesFirst = false;
        //first compare agility
        if (unit.stats.agi > opposingUnit.stats.agi)
        {
            unitGoesFirst = true;
            

        }

        else if (opposingUnit.stats.agi > unit.stats.agi)
        {

            unitGoesFirst = false;
            

        }
        //choosing random if they have same agility
        else
        {
            if(Random.Range(0, 1) >= 0.5)
            {
                unitGoesFirst = true;
            }

        }

        if (unitGoesFirst)
        {
            //opposing unit takes damage first
            opposingUnit.takeDamage(unit.getTotalDamage(opposingUnit));

            //attacking unit takes damage
            unit.takeDamage(opposingUnit.getTotalDamage(unit));
        }
        else
        {
            //attacking unit takes damage
            unit.takeDamage(opposingUnit.getTotalDamage(unit));

            //opposing unit takes damage first
            opposingUnit.takeDamage(unit.getTotalDamage(opposingUnit));
        }
        
    }
}
