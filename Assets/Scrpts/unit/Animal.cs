using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Unit
{
    public enum Species{
        walrox
    }
    public bool predator;
    public bool migratory;
    public Species species;
    protected override void Awake(){
        base.Awake();
        player = -3;
    }

}
