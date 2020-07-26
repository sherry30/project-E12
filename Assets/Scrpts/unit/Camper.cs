using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camper : MonoBehaviour
{
    public void Camp(){
        Unit unit = GetComponent<Unit>();
        HexComponent[,] hexes = HexMap.Instance.hexes;
        Vector2 loc = unit.location;
        //destroying unit and creating camp
        GameObject obj = HexOperations.Instance.BuildCity(loc,0);
        obj.GetComponent<City>().camped = true;
        //return;
        HexOperations.Instance.DestroyUnit(unit);
    }
}
