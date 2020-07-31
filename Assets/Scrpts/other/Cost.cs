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
    public enum OtherResource{
        Science,
        alchemy,
        crystal
    }

    public enum Resource{
        gold,
        production,
        food
    }
    //for making dictionary
[System.Serializable]
public class DictionaryResInt: SerializableDictionary<Resource,int>{}  
[System.Serializable]
public class DictionaryRawInt: SerializableDictionary<Raw_Material,int>{}  
[System.Serializable]
public class DictionaryEnergyInt: SerializableDictionary<Energy,int>{}  
[System.Serializable]
public class DictionaryOtherResInt:SerializableDictionary<OtherResource,int>{}  
[System.Serializable]
public class Cost 
{

    [SerializeField]
    private DictionaryResInt costResource;
    [SerializeField]
    private DictionaryRawInt costRaw;
    [SerializeField]
    private DictionaryEnergyInt costEnergy;
    [SerializeField]
    private DictionaryOtherResInt costOtherRes;
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

    public bool checkCost(int player = -1){
        Player tempPlayer = PlayerController.Instance.player;
        //checking if energy cost is met
        if(player==-1){
            tempPlayer = PlayerController.Instance.player;

        }
        else{
            tempPlayer = AIController.Instance.AIPlayers[player];
        }
        Dictionary<Resource,int> res = tempPlayer.Resources;
        Dictionary<Raw_Material,int> raw = tempPlayer.RawMaterials;
        Dictionary<Energy,int> energy = tempPlayer.Energies;
        Dictionary<OtherResource,int> otherRes = tempPlayer.OtherResources;
        //checking if energy cost is met
        if(costEnergy!=null){
            foreach(KeyValuePair<Energy, int> entry in costEnergy){
                if(energy[entry.Key]<costEnergy[entry.Key]){
                    return false;
                }
            }
        }
        //checking if Resource cost is met
        if(costResource!=null){
            foreach(KeyValuePair<Resource, int> entry in costResource){
                if(res[entry.Key]<costResource[entry.Key]){
                    return false;
                }
            }
        }
        //checking if Raw material cost is met
        if(costRaw!=null){
            foreach(KeyValuePair<Raw_Material, int> entry in costRaw){
                if(raw[entry.Key]<costRaw[entry.Key]){
                    return false;
                }
            }
        }
        //checking if otherResource cost is met
        if(costOtherRes!=null){
            foreach(KeyValuePair<OtherResource, int> entry in costOtherRes){
                if(otherRes[entry.Key]<costOtherRes[entry.Key]){
                    return false;
                }
            }
        }

        //spending all the things
        if(costEnergy!=null){
            foreach(KeyValuePair<Energy, int> entry in costEnergy){
                tempPlayer.Energies[entry.Key]-=costEnergy[entry.Key];
            }
        }
        //checking if Resource cost is met
        if(costResource!=null){
            foreach(KeyValuePair<Resource, int> entry in costResource){
                tempPlayer.Resources[entry.Key]-=costResource[entry.Key];
            }
        }
        //checking if Raw material cost is met
        if(costRaw!=null){
            foreach(KeyValuePair<Raw_Material, int> entry in costRaw){
                tempPlayer.RawMaterials[entry.Key]-=costRaw[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(costOtherRes!=null){
            foreach(KeyValuePair<OtherResource, int> entry in costOtherRes){
                    tempPlayer.OtherResources[entry.Key]-=costOtherRes[entry.Key];
            }
        }
        return true;   
    }

        //if it is AIPlayer
        /*else{
            Player tempPlayer = AIController.Instance.AIPlayers[player];
            Dictionary<Resource,int> res = tempPlayer.Resources;
            Dictionary<Raw_Material,int> raw = tempPlayer.RawMaterials;
            Dictionary<Energy,int> energy = tempPlayer.Energies;
            Dictionary<OtherResource,int> otherRes = tempPlayer.OtherResources;

            if(costEnergy!=null){
                foreach(KeyValuePair<Energy, int> entry in costEnergy){
                    if(energy[entry.Key]<costEnergy[entry.Key]){
                        return false;
                    }
                    else{
                        tempPlayer.Energies[entry.Key]-=costEnergy[entry.Key];
                    }
                }
            }
            //checking if Resource cost is met
            if(costResource!=null){
                foreach(KeyValuePair<Resource, int> entry in costResource){
                    if(res[entry.Key]<costResource[entry.Key]){
                        return false;
                    }
                    else{
                        tempPlayer.Resources[entry.Key]-=costResource[entry.Key];
                    }
                }
            }
            //checking if Raw material cost is met
            if(costRaw!=null){
                foreach(KeyValuePair<Raw_Material, int> entry in costRaw){
                    if(raw[entry.Key]<costRaw[entry.Key]){
                        return false;
                    }
                    else{
                        tempPlayer.RawMaterials[entry.Key]-=costRaw[entry.Key];
                    }
                }
            }
            //checking if oherResource cost is met
            if(costOtherRes!=null){
                foreach(KeyValuePair<OtherResource, int> entry in costOtherRes){
                    if(otherRes[entry.Key]<costOtherRes[entry.Key]){
                        return false;
                    }
                    else{
                        tempPlayer.OtherResources[entry.Key]-=costOtherRes[entry.Key];
                    }
                }
            }
        }
        return true;*/

    //}

    public void printCost(){
        //checking if energy cost is met
            if(costEnergy!=null){
                foreach(KeyValuePair<Energy, int> entry in costEnergy){
                    Debug.Log(string.Format("require {0} {1} Energy",costEnergy[entry.Key],entry.Key));
                }
            }
            //checking if energy cost is met
            if(costResource!=null){
                foreach(KeyValuePair<Resource, int> entry in costResource){
                    Debug.Log(string.Format("require {0} {1}",costResource[entry.Key],entry.Key));
                }
            }
            //checking if Raw material cost is met
            if(costRaw!=null){
                foreach(KeyValuePair<Raw_Material, int> entry in costRaw){
                    Debug.Log(string.Format("require {0} {1}",costRaw[entry.Key],entry.Key));
                }
            }
            //checking if energy cost is met
            if(costOtherRes!=null){
                foreach(KeyValuePair<OtherResource, int> entry in costOtherRes){
                    Debug.Log(string.Format("require {0} {1}",costOtherRes[entry.Key],entry.Key));
                }
            }   
        }
    
}
