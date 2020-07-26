using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirKingdom :Kingdom
{
    // Start is called before the first frame update
    public override void Start(){
        base.Start();
        //numberOfCities= cityPrefabs.Length;
        Name = "Air_kingdom";
        type = Energy.Air;
        /*cities = new City[cityPrefabs.Length];
        units = new Unit[unitPrefabs.Length];
        setCities();*/
    }
}
