using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : Unit
{
    protected override void Awake(){
        base.Awake();
        typeOfUnit = Type.Land;
        classOfUnit = Class.shadow;
        Name = "Shadow";
        id = currentID;
        currentID++;
        currentHealth=maxHealth;
        player = -2;
        /*Vector3 temp = GetComponentInChildren<Collider>().bounds.size;
        offset = (temp.y/2f)+Hex.hexHeight;
        Debug.Log(string.Format("Unit Offset: {0}",offset));*/

    }
}
