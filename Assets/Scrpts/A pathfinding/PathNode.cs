using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{
    public int gCost=0;
    public int hCost=0;
    public int fCost=0;
    public HexComponent hexComp;
    public PathNode cameFrom;
    public PathNode(HexComponent hex){
        hexComp = hex;
    }


}
