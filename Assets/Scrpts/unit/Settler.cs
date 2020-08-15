using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settler : Unit
{
    public void Settle(int index){
        HexOperations.Instance.BuildCity(location,index);
    }
}
