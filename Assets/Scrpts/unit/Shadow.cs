using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : Unit
{
    new void Awake(){
        base.Awake();
        typeOfUnit = Type.Land;
        classOfUnit = Class.shadow;
        Name = "Shadow";
        id = currentID;
        currentID++;
        currentHealth=maxHealth;
        /*Vector3 temp = GetComponentInChildren<Collider>().bounds.size;
        offset = (temp.y/2f)+Hex.hexHeight;
        Debug.Log(string.Format("Unit Offset: {0}",offset));*/

    }
}
