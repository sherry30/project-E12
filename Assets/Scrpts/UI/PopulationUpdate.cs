﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationUpdate : MonoBehaviour
{
    public Player player;
    public Text populationText;
    void Start(){
        player = PlayerController.Instance.player;
    }
    void FixedUpdate()
    {
        populationText.text = string.Format("Population: {0}",player.cities[0].population);
    }
}
