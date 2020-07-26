using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterKingdom : Kingdom
{
    // Start is called before the first frame update
    public override void Start(){
        base.Start();
        Name = "Water_kingdom";
        type = Energy.Water;
        /*cities = new City[cityPrefabs.Length];
        units = new Unit[unitPrefabs.Length];
        setCities();*/
    }
}
