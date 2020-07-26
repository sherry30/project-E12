using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{
    public Kingdom kingdom;
    public int approval;
    public Dictionary<Raw_Material,int> rawMaterials;
    public Dictionary<Raw_Material,int> rawMaterialYield;
    public Dictionary<Resource,int> resources;
    public Dictionary<Resource,int> resourcesYield;
    public Dictionary<Energy,int> energy;
    public Dictionary<Energy,int> energyYield;
    
    private int population{get;set;}
    public float populationGrowing;
    public List<City> cities;
    public List<Unit> units;
    public List<Improvement> improvements;
    public int productionYield;
    public List<HexComponent> territory;



    public Era era=Era.StoneAge;
    public void BuildCity(City cit,Vector2 location){
        cit.Build(location);
        if(cities==null)
            cities = new List<City>();
        cities.Add(cit);
    }

    public void BuildImprovement(Improvement imp,Vector2 location){
        imp.Build(location);
        if(improvements==null)
            improvements = new List<Improvement>();
        improvements.Add(imp);
    }
    public void SpawnUnit(Unit unit,Vector2 location){
        unit.spawnUnit(location);
        if(units==null)
            units = new List<Unit>();
        units.Add(unit);
    }
    public void RemoveBuilding(int id){
        int place=0;
        //destroying gameObject and removing from list
        for(int i=0;i<cities.Count;i++){
            if(cities[i].id==id){
                cities[i].Demolish();
                i= place;
            }

        }
        cities.RemoveAt(place);
        

    }
    public void RemoveUnit(int id){
        //removing from list
        for(int i=0;i<units.Count;i++){
            if(units[i].id==id){
                units.RemoveAt(i);
                return;
            }


        }
    }
    public void setTerritory(List<HexComponent> ter){
        territory = ter;
    }
    public void setTerritory(HexComponent[] ter){
        territory = ter.OfType<HexComponent>().ToList();
    }
}
