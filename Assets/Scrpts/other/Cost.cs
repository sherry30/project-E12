using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Raw_Material{
        stick,
        stone,
        fibre,
        hide,
        copper_ore,
        coal,
        iron_ore,

    }
    public enum Energy{
        None,
        Earth,
        Fire,
        Water,
        Air,
        Light,
        Dark

    }

    public enum Resource{
        gold,
        production,
        food
    }
    //for making dictionary
[System.Serializable]
public class DictionaryRes: SerializableDictionary<Resource,int>{}  
[System.Serializable]
public class DictionaryRaw: SerializableDictionary<Raw_Material,int>{}  
[System.Serializable]
public class DictionaryEnergy: SerializableDictionary<Energy,int>{}  
[System.Serializable]
public class Cost 
{

    [SerializeField]
    private DictionaryRes costResource;
    [SerializeField]
    private DictionaryRaw costRaw;
    [SerializeField]
    private DictionaryEnergy costEnergy;
    Cost(Dictionary<Resource,int> res,
     Dictionary<Raw_Material,int> raw,
     Dictionary<Energy,int> ener)
     {
        doRes(res);
        doRaw(raw);
        doEnergy(ener);
    }

    Cost(Dictionary<Resource,int> res){
        doRes(res);
    }
    Cost(Dictionary<Raw_Material,int> raw){
        doRaw(raw);
    }
    Cost(Dictionary<Energy,int> energy){
        doEnergy(energy);
    }
    Cost(Dictionary<Resource,int> res,Dictionary<Raw_Material,int> raw){
        doRes(res);
        doRaw(raw);
    }

    Cost(Dictionary<Resource,int> res,Dictionary<Energy,int> energy){
        doRes(res);
        doEnergy(energy);
    }

    Cost(Dictionary<Raw_Material,int> raw,Dictionary<Energy,int> energy){
        doRaw(raw);
        doRaw(raw);
    }
    Cost(){
        
    }
    private void doRes(Dictionary<Resource,int> res){
        foreach(KeyValuePair<Resource, int> entry in res){
            this.costResource.Add(entry.Key,entry.Value);
        }
    }
    private void doRaw(Dictionary<Raw_Material,int> raw){
        foreach(KeyValuePair<Raw_Material, int> entry in raw){
            this.costRaw.Add(entry.Key,entry.Value);
        }
    }
    private void doEnergy(Dictionary<Energy,int> energy){
        foreach(KeyValuePair<Energy, int> entry in energy){
            this.costEnergy.Add(entry.Key,entry.Value);
        }
    }

    public int getProduction(){
        if(costResource==null)
            Debug.Log("This unit does not have a cost");
        return costResource[Resource.production];
    }

    
}
