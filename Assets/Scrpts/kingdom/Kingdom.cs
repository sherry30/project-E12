﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Kingdom : MonoBehaviour
{
    public string Name;
    public string description;

    //its energies
    public Energy type;
    public Energy type2;

    //all the stuff this kingdm has
    public Unit[] units;
    public Building[] buildings;
    public Spell[] spells;
    public City[] cities;
    public Item[] items;
    public Improvement[] improvements;
    public District[] districts;

    //GameoBjects 
    public GameObject[] buildingPrefabs;
    public GameObject[] cityPrefabs;
    public GameObject[] unitPrefabs;
    public GameObject[] improvementPrefabs;
    public GameObject[] itemPrefabs;
    public GameObject[] districtPrefabs;
    public GameObject[] TechObjects;
    
    [HideInInspector]
    public TechTree techTree;

    //its starting uits
    public List<int> startingUnitIndexes;

    //things available at the start
    public List<improvement> initialAvailableImprovments;
    public List<Unit.Class> initialAvailableUnits;

    public virtual void Start(){
        
    }

    public void setVariables(){
        cities = new City[cityPrefabs.Length];
        units= new Unit[unitPrefabs.Length];
        improvements = new Improvement[improvementPrefabs.Length];
        items = new Item[itemPrefabs.Length];
        districts = new District[districtPrefabs.Length];
        buildings = new Building[buildingPrefabs.Length];

        
        for(int i=0;i<cities.Length;i++){
            cities[i]=cityPrefabs[i].GetComponent<City>();
        }
        for(int i=0;i<units.Length;i++){
            units[i] = unitPrefabs[i].GetComponent<Unit>();
        }
        for(int i=0;i<improvements.Length;i++){
            improvements[i] = improvementPrefabs[i].GetComponent<Improvement>();
        }

        for(int i=0;i<items.Length;i++){
            items[i] = itemPrefabs[i].GetComponent<Item>();
        }
        for(int i=0;i<districts.Length;i++){
            districts[i] = districtPrefabs[i].GetComponent<District>();
        }
        for(int i=0;i<buildings.Length;i++){
            buildings[i] = buildingPrefabs[i].GetComponent<Building>();
        }
        //setting up tech Tree from techObjects
        setTechTree();
    }
    public int getCityIndex(City obj){
        return Array.IndexOf(cities, obj);
    }
    //return building prefab by giving building
    public GameObject getPrefabOfBuilding(Building build){
        for(int i=0 ; i<buildings.Length;i++){
            if(buildings[i]==build)
                return buildingPrefabs[i];
        }
        Debug.LogError("Didint find building");
        return buildingPrefabs[0];
    }
    private void setTechTree(){
        for(int i=0;i<TechObjects.Length;i++){
            TechSkill tempSkill = TechObjects[i].GetComponent<TechSkill>();
            tempSkill.unlocked = false;
            techTree.techSkills.Add(tempSkill.techCode,tempSkill);
            tempSkill.techTree=techTree;
        }
    }
}
