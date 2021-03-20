using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item :MonoBehaviour
{
    public enum Type{
        Armour,
        Weapon,
        Tome,
        Artifact,
        Relic

    }
    public string Name;
    public int player;
    public string description;
    public Type type;
    public Cost cost;
    public bool equiped=false;
    public int daysToBeProduced=2;
    [HideInInspector]
    public Building building;
    public void equip(){
        equiped = true;
    }

}
