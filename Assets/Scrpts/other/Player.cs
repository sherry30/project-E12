using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player 
{
    public int player = -1;
    public Kingdom kingdom;
    [SerializeField]
    public DictionaryResInt Resources;
    [SerializeField]
    public DictionaryRawInt RawMaterials;
    [SerializeField]
    public DictionaryEnergyInt Energies;
    public DictionaryOtherResInt OtherResources;
    /*public DictionaryResInt resourcesYield;
    public DictionaryRawInt RawMaterialYield;

    public DictionaryEnergyInt energyYield;
    public DictionaryOtherResInt OtherResourcesYield;*/
    
    public int population;
    public float populationGrowing;
    public List<City> cities;
    public List<Unit> units;
    public List<Improvement> improvements;
    public List<District> districts;
    public List<improvement> availableImprovements;
    //public int productionYield;
    public List<HexComponent> territory;
    public delegate void startOfTurn();
    public event startOfTurn onStartTurn;


    public void setVariables(){
    }
    public Era era=Era.StoneAge;
    public void BuildCity(City cit,Vector2 location){
        cit.Build(location);
        if(cities==null)
            cities = new List<City>();
        cities.Add(cit);
        onStartTurn+=cit.StartTurn;
        
    }

    public void BuildImprovement(Improvement imp,Vector2 location){
        imp.Build(location);
        if(improvements==null)
            improvements = new List<Improvement>();
        improvements.Add(imp);
        onStartTurn+=imp.StartTurn;
    }
    public void BuildDistrict(District dis,Vector2 location){
        dis.Build(location);
        if(districts==null)
            districts = new List<District>();
        districts.Add(dis);
        onStartTurn+=dis.StartTurn;
    }
    public void SpawnUnit(Unit unit,Vector2 location){
        unit.spawnUnit(location);
        if(units==null)
            units = new List<Unit>();
        units.Add(unit);
        onStartTurn+=unit.StartTurn;
    }
    public void RemoveBuilding(int id){
        
        //destroying gameObject and removing from list
        for(int i=0;i<cities.Count;i++){
            if(cities[i].id==id){
                cities[i].Demolish();
                onStartTurn-=cities[i].StartTurn;
                cities.RemoveAt(i);
                return;
            }

        }

    }
    public void RemoveUnit(int id){
        //removing from list
        for(int i=0;i<units.Count;i++){
            if(units[i].id==id){
                onStartTurn-=units[i].StartTurn;
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
    public void getYield(){
        //adding allthe yields at the start of turn
        /*if(energyYield!=null){
            foreach(KeyValuePair<Energy, int> entry in energyYield){
                Energies[entry.Key]+=energyYield[entry.Key];
            }
        }
        //checking if Resource cost is met
        if(resourcesYield!=null){
            foreach(KeyValuePair<Resource, int> entry in resourcesYield){
                Resources[entry.Key]+=resourcesYield[entry.Key];
            }
        }
        //checking if Raw material cost is met
        if(RawMaterialYield!=null){
            foreach(KeyValuePair<Raw_Material, int> entry in RawMaterialYield){
                RawMaterials[entry.Key]+=RawMaterialYield[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(OtherResourcesYield!=null){
            foreach(KeyValuePair<OtherResource, int> entry in OtherResourcesYield){
                OtherResources[entry.Key]+=OtherResourcesYield[entry.Key];
            }
        }*/
        for(int i=0;i<cities.Count;i++){
            cities[i].getYield();
        }
    }

    public void StartTurn(){
        getYield();
        onStartTurn();
    }
}
