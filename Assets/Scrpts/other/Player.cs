using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player 
{
    public int player = -1;
    public Kingdom kingdom;
    public int approval;
    [SerializeField]
    public DictionaryResInt Resources;
    [SerializeField]
    public DictionaryRawInt RawMaterials;
    [SerializeField]
    public DictionaryEnergyInt Energies;
    public DictionaryOtherResInt OtherResources;
    public DictionaryResInt resourcesYield;
    public DictionaryRawInt RawMaterialYield;

    public DictionaryEnergyInt energyYield;
    public DictionaryOtherResInt OtherResourcesYield;
    
    public int population;
    public float populationGrowing;
    public List<City> cities;
    public List<Unit> units;
    public List<Improvement> improvements;
    public List<improvement> availableImprovements;
    //public int productionYield;
    public List<HexComponent> territory;


    public void setVariables(){
        //availableImprovements = new List<bool>();
        
    }
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
