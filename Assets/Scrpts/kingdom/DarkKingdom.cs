using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkKingdom : Kingdom
{
    // Start is called before the first frame update
    public override void Start(){
        base.Start();
        Name = "Dark_kingdom";
        type = Energy.Water;
    }
}
