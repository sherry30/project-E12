using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armour : Item
{

    public float armour = 0f;
    private void Awake()
    {
        type = Type.Armour;
    }
}
