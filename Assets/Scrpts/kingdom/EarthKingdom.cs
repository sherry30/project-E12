using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthKingdom : Kingdom
{
    // Start is called before the first frame update
    public override void Start(){
        base.Start();
        Name = "Earth_kingdom";
        type = Energy.Earth;
        /*cities = new City[cityPrefabs.Length];
        units = new Unit[unitPrefabs.Length];
        setCities();*/
    }
}
