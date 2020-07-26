using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public enum Type{
        Armour,
        Weapon,
        Tome,
        Artifact

    }
    public string Name;
    public string description;
    public Type type;
    public Cost cost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
