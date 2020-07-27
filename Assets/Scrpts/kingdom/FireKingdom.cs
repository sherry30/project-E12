using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireKingdom : Kingdom
{
    //public string working;
    
    public override void  Start()
    {
        base.Start();
        Name = "Burning Brood";
        type = Energy.Fire;
       
        
    }
}
