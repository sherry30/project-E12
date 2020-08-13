using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Era{
        StoneAge,
        steampunk,
        
    }
public class PlayerController : MonoBehaviour
{

    public Energy selected;
    [SerializeField]
    public Player player;
    //TODO current state of techtree
    public static PlayerController Instance {get; private set;}
    void Awake(){
        //player = new Player();
        //Debug.Log(string.Format("productino {0}",player.Resources[Resource.production]));
        removeKingdoms();
        player.kingdom.setVariables();
        player.setVariables();
        //GameState.onStartTurn+=StartTurn;
        if(Instance==null){
            Instance =this;
            return;
        }
        else{
            Destroy(gameObject);
        }
        
        
        
    }
    //remove other kindom classes fromthe controller
    private void removeKingdoms(){
        if(selected==Energy.Fire){
            player.kingdom = gameObject.GetComponent<FireKingdom>();
            AirKingdom air = this.GetComponent<AirKingdom>();
            WaterKingdom water = this.GetComponent<WaterKingdom>();
            EarthKingdom earth = this.GetComponent<EarthKingdom>();
            DarkKingdom dark = this.GetComponent<DarkKingdom>();
            LightKingdom light = this.GetComponent<LightKingdom>();
            Destroy(water);
            Destroy(air);
            Destroy(earth);
            Destroy(dark);
            Destroy(light);
        }
        else if(selected==Energy.Water){
            player.kingdom = gameObject.GetComponent<WaterKingdom>();
            AirKingdom air = this.GetComponent<AirKingdom>();
            FireKingdom fire = this.GetComponent<FireKingdom>();
            EarthKingdom earth = this.GetComponent<EarthKingdom>();
            DarkKingdom dark = this.GetComponent<DarkKingdom>();
            LightKingdom light = this.GetComponent<LightKingdom>();
            Destroy(fire);
            Destroy(air);
            Destroy(earth);
            Destroy(dark);
            Destroy(light);
            
        }
        else if(selected==Energy.Air){
            player.kingdom = gameObject.GetComponent<AirKingdom>();
            FireKingdom fire = this.GetComponent<FireKingdom>();
            WaterKingdom water = this.GetComponent<WaterKingdom>();
            EarthKingdom earth = this.GetComponent<EarthKingdom>();
            DarkKingdom dark = this.GetComponent<DarkKingdom>();
            LightKingdom light = this.GetComponent<LightKingdom>();
            Destroy(water);
            Destroy(fire);
            Destroy(earth);
            Destroy(dark);
            Destroy(light);
            
        }
        else if(selected==Energy.Earth){
            player.kingdom = gameObject.GetComponent<EarthKingdom>();
            AirKingdom air = this.GetComponent<AirKingdom>();
            WaterKingdom water = this.GetComponent<WaterKingdom>();
            FireKingdom fire = this.GetComponent<FireKingdom>();
            DarkKingdom dark = this.GetComponent<DarkKingdom>();
            LightKingdom light = this.GetComponent<LightKingdom>();
            Destroy(water);
            Destroy(air);
            Destroy(fire);
            Destroy(dark);
            Destroy(light);
            
        }
        else if(selected==Energy.Dark){
            player.kingdom = gameObject.GetComponent<DarkKingdom>();
            AirKingdom air = this.GetComponent<AirKingdom>();
            WaterKingdom water = this.GetComponent<WaterKingdom>();
            EarthKingdom earth = this.GetComponent<EarthKingdom>();
            FireKingdom fire = this.GetComponent<FireKingdom>();
            LightKingdom light = this.GetComponent<LightKingdom>();
            Destroy(water);
            Destroy(air);
            Destroy(earth);
            Destroy(fire);
            Destroy(light);
            
        }
        else if(selected==Energy.Light){
            player.kingdom = gameObject.GetComponent<LightKingdom>();
            AirKingdom air = this.GetComponent<AirKingdom>();
            WaterKingdom water = this.GetComponent<WaterKingdom>();
            EarthKingdom earth = this.GetComponent<EarthKingdom>();
            FireKingdom fire = this.GetComponent<FireKingdom>();
            DarkKingdom dark = this.GetComponent<DarkKingdom>();
            Destroy(water);
            Destroy(air);
            Destroy(earth);
            Destroy(fire);
            Destroy(dark);
            
        }
    }

    public void StartTurn(){
        player.StartTurn();
    }
    /*public void getYield(){
        DictionaryEnergyInt energyYield = player.energyYield;
        DictionaryResInt resourcesYield = player.resourcesYield;
        DictionaryRawInt RawMaterialYield = player.RawMaterialYield;
        DictionaryOtherResInt OtherResourcesYield = player.OtherResourcesYield;

        if(energyYield!=null){
            foreach(KeyValuePair<Energy, int> entry in energyYield){
                player.Energies[entry.Key]+=energyYield[entry.Key];
            }
        }
        //checking if Resource cost is met
        if(resourcesYield!=null){
            foreach(KeyValuePair<Resource, int> entry in resourcesYield){
                player.Resources[entry.Key]+=resourcesYield[entry.Key];
            }
        }
        //checking if Raw material cost is met
        if(RawMaterialYield!=null){
            foreach(KeyValuePair<Raw_Material, int> entry in RawMaterialYield){
                player.RawMaterials[entry.Key]+=RawMaterialYield[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(OtherResourcesYield!=null){
            foreach(KeyValuePair<OtherResource, int> entry in OtherResourcesYield){
                player.OtherResources[entry.Key]+=OtherResourcesYield[entry.Key];
            }
        }
    }*/

    
    /*public void BuildCity(City cit,Vector2 location){
        cit.Build(location);
        if(cities==null)
            cities = new List<City>();
        cities.Add(cit);
    }
    public void SpawnUnit(Unit unit,Vector2 location){
        unit.spawnUnit(location);
        if(units==null)
            units = new List<Unit>();
        units.Add(unit);
    }
    //removing building belonging to this player
    //only fixes ids from this playerbutshould fix it overall
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
    }*/
}
