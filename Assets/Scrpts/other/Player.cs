using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]

//has current stuff
public class Player 
{
    public int player = -1;
    public Kingdom kingdom;
    /*[SerializeField]
    public DictionaryResFloat Resources;
    [SerializeField]
    public DictionaryRawFloat RawMaterials;
    [SerializeField]
    public DictionaryEnergyFloat Energies;
    [SerializeField]
    public DictionaryOtherResFloat OtherResources;*/
    [SerializeField]
    public Resources resources;
    public int population;
    public float populationGrowing;
    public List<City> cities;
    public List<Unit> units;
    public List<Army> armies;
    public List<Unit> singleUnits; //units not in armies
    public List<Improvement> improvements;
    public List<District> districts;

    //available stuff
    public List<improvement> availableImprovements;
    public List<Unit.Class> availableUnits;
    public List<District.Type> availableDistricts;

    public List<HexComponent> territory;
    public delegate void startOfTurn();
    public event startOfTurn onStartTurn;


    private int armyIDCounter = 0;

    public void setVariables(){
        kingdom.setVariables();
        availableImprovements = kingdom.initialAvailableImprovments;
        availableUnits = kingdom.initialAvailableUnits;
        availableDistricts = kingdom.initialAvailableDistricts;
        resources = new Resources();
        resources.Initialize();

        //for testing
        //resources.resources[Resource.production]=3;
        resources.OtherResources[OtherResource.Science]=30;
    }

    public void unlockUnit(Unit.Class un){
        availableUnits.Add(un);
        //Debug.Log(string.Format("num of cities: {0}", cities.Count));
        foreach(City c in cities){
            //Debug.Log("ran on " + c.Name);
            c.availableUnits.Add(un);
        }
    }

    public void unlockImprovement(improvement im){
        availableImprovements.Add(im);
        foreach(City c in cities){
            c.availableImprovements.Add(im);
        }
    }
    public void unlockDistrict(District.Type di){
        availableDistricts.Add(di);
        foreach(City c in cities){
            c.availableDistricts.Add(di);
        }
    }


    public Era era=Era.StoneAge;
    public void BuildCity(City cit,Vector2 location){
        
        if(cities==null)
            cities = new List<City>();
        cities.Add(cit);
        onStartTurn+=cit.StartTurn;
        cit.Build(location);

        //Debug.Log(string.Format("num of cities: {0}", cities.Count));

    }

    public void BuildImprovement(Improvement imp,Vector2 location){
        imp.Build(location);
        /*if(improvements==null)
            improvements = new List<Improvement>();
        improvements.Add(imp);*/
        onStartTurn+=imp.StartTurn;
    }
    public void BuildDistrict(District dis,Vector2 location){
        dis.Build(location);
        /*if(districts==null)
            districts = new List<District>();
        districts.Add(dis);*/
        onStartTurn+=dis.StartTurn;
        Debug.Log("Start turn set");
    }
    public void BuildBuilding(Building bil,Vector2 location){
        bil.Build(location);
        /*if(districts==null)
            districts = new List<District>();
        districts.Add(dis);*/
        onStartTurn+=bil.StartTurn;
    }
    public void SpawnUnit(Unit unit,Vector2 location){
        unit.spawnUnit(location);
        if(units==null)
            units = new List<Unit>();
        if (singleUnits == null)
            singleUnits = new List<Unit>();
        units.Add(unit);
        singleUnits.Add(unit);
        onStartTurn+=unit.StartTurn;
    }

    public void createArmy(List<Unit> armyUnits)
    {
        if (armies == null)
            armies = new List<Army>();
        Army army = new Army(armyIDCounter);
        armyIDCounter++;
        foreach (Unit u in armyUnits)
        {
            if (singleUnits.Contains(u))
                singleUnits.Remove(u);
            else
            {
                Debug.LogError("singleUnits does not contain unit(id): " + u.id);
            }

            army.addUnit(u);
        }

        onStartTurn += army.StartTurn;

        //adding army to the hex component
        HexMap.Instance.getHexComponent(armyUnits[0].location).addArmy(army);
        armies.Add(army);
    }

    public void addUnitToArmy(Unit unit, Army army)
    {
        if (singleUnits.Contains(unit))
            singleUnits.Remove(unit);
        else
            Debug.LogError("singleUnits does not contain unit(id): " + unit.id);

        army.addUnit(unit);

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

    //getting yields from all the cities
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

        //refreshing UI after setting all the new variabkles
        UIController.Instance.refreshUI();
        //populationGrowth();

    }
}
