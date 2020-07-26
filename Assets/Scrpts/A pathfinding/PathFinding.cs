using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding 
{
    private List<PathNode> openNodes;
    private List<PathNode> closedNodes;
    private PathNode currentNode;
    //private Unit unit;
    /*public PathFinding(Unit unit){
        this.unit = unit;
    }*/

    public List<HexComponent> shortesPath(HexComponent currentHex, HexComponent destHex){
        List<HexComponent> result = new List<HexComponent>();
        openNodes = new List<PathNode>();
        closedNodes = new List<PathNode>();

        //adding starting node to open nodes
        PathNode temp = new PathNode(currentHex);
        openNodes.Add(temp);
        currentNode = openNodes[0];
        bool found = false;
        while(!found){
            currentNode = openNodes[lowestFCost(openNodes)];
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);
            //Debug.Log(currentNode.hexComp.hex.Q + " , "+ currentNode.hexComp.hex.R);
            //Debug.Log("Gcost = "+ currentNode.gCost+" Hcost = "+ currentNode.hCost+" Fcost = "+ currentNode.fCost);
            //path has been found
            //Debug.Log("current:");
            if(currentNode.hexComp.hex.Q==destHex.hex.Q && currentNode.hexComp.hex.R==destHex.hex.R){
                
                found = true;
                continue;
            }
            foreach(HexComponent neighbour in HexOperations.Instance.getClosestNeighbours(new Vector2(currentNode.hexComp.hex.Q,currentNode.hexComp.hex.R))){
                PathNode temp2 = new PathNode(neighbour);
                if(closeCheck(temp2) && !temp2.hexComp.isEmpty())//TODO: add restrictions for dead tiles like mountains
                    continue;
                else if(getFCost(currentNode,temp2,destHex)<temp2.fCost || !openCheck(temp2)){
                    setFCost(currentNode,ref temp2,destHex);
                    temp2.cameFrom = currentNode;
                    //Debug.Log("neighbours:");
                    //Debug.Log(temp2.hexComp.hex.Q + " , "+ temp2.hexComp.hex.R);
                   // Debug.Log("Gcost = "+ temp2.gCost+" Hcost = "+ temp2.hCost+" Fcost = "+ temp2.fCost);
                    if(!openCheck(temp2))
                        openNodes.Add(temp2);

                }
            }

        }
        for(int i=0;currentNode.cameFrom!=temp;i++){
            result.Add(currentNode.hexComp);
            currentNode = currentNode.cameFrom;
            if(currentNode==null)
                break;
        }
        if(currentNode!=null)
            result.Add(currentNode.hexComp);
        result.Reverse();
        return result;
    }
    private int lowestFCost(List<PathNode> nodes){
        int lowest = 1000000000;
        int index=0;
        for(int i=0;i<nodes.Count;i++){
            if(nodes[i].fCost<=lowest){
                lowest = nodes[i].fCost;
                index = i;
            }
        }
        return index;
    }
    private bool closeCheck(PathNode node){
        foreach(PathNode n in closedNodes){
            if(n.hexComp.hex.Q==node.hexComp.hex.Q && n.hexComp.hex.R==node.hexComp.hex.R )
                return true;
        }
        return false;
    }
    private bool openCheck(PathNode node){
        foreach(PathNode n in openNodes){
            if(n.hexComp.hex.Q==node.hexComp.hex.Q && n.hexComp.hex.R==node.hexComp.hex.R )
                return true;
        }
        return false;
    }
    private int getFCost(PathNode current, PathNode temp2,HexComponent destHex){
        int g,h,f;
        g = current.gCost+1;
        h = temp2.hexComp.hex.Distance(destHex.hex);
        f = g+h;
        return f;

    }
    private void setFCost(PathNode current,ref PathNode temp2,HexComponent destHex){
        temp2.gCost = current.gCost+1;
        temp2.hCost = temp2.hexComp.hex.Distance(destHex.hex);
        temp2.fCost = temp2.gCost+temp2.hCost;

    }


}
