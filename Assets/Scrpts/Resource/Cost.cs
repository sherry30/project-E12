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
        alchemy
    }

    public enum Resource{
        gold
    }
    
    //for making dictionary
[System.Serializable]
public class DictionaryResFloat: SerializableDictionary<Resource,float>{}  
[System.Serializable]
public class DictionaryRawFloat: SerializableDictionary<Raw_Material,float>{}  
[System.Serializable]
public class DictionaryEnergyFloat: SerializableDictionary<Energy,float>{}  
[System.Serializable]
public class DictionaryOtherResFloat:SerializableDictionary<OtherResource,float>{}  
[System.Serializable]
public class DictionaryCrystalFloat:SerializableDictionary<crystal,float>{}  
[System.Serializable]
public class Cost :Resources
{


    

    /*[SerializeField]
    private DictionaryResFloat costResource;
    [SerializeField]
    private DictionaryRawFloat costRaw;
    [SerializeField]
    private DictionaryEnergyFloat costEnergy;
    [SerializeField]
    private DictionaryOtherResFloat costOtherRes;
    [SerializeField]
    private DictionaryCrystalFloat costCrystal;*/
    /*Cost(Dictionary<Resource,float> res,
     Dictionary<Raw_Material,float> raw,
     Dictionary<Energy,float> ener)
     {
        doRes(res);
        doRaw(raw);
        doEnergy(ener);
    }

    Cost(Dictionary<Resource,float> res){
        doRes(res);
    }
    Cost(Dictionary<Raw_Material,float> raw){
        doRaw(raw);
    }
    Cost(Dictionary<Energy,float> energy){
        doEnergy(energy);
    }
    Cost(Dictionary<Resource,float> res,Dictionary<Raw_Material,float> raw){
        doRes(res);
        doRaw(raw);
    }

    Cost(Dictionary<Resource,float> res,Dictionary<Energy,float> energy){
        doRes(res);
        doEnergy(energy);
    }

    Cost(Dictionary<Raw_Material,float> raw,Dictionary<Energy,float> energy){
        doRaw(raw);
        doRaw(raw);
    }
    Cost(){
        
    }
    private void doRes(Dictionary<Resource,float> res){
        foreach(KeyValuePair<Resource, float> entry in res){
            this.costResource.Add(entry.Key,entry.Value);
        }
    }
    private void doRaw(Dictionary<Raw_Material,float> raw){
        foreach(KeyValuePair<Raw_Material,float> entry in raw){
            this.costRaw.Add(entry.Key,entry.Value);
        }
    }
    private void doEnergy(Dictionary<Energy,float> energy){
        foreach(KeyValuePair<Energy, float> entry in energy){
            this.costEnergy.Add(entry.Key,entry.Value);
        }
    }*/

    public float getProduction(){
        if(resources==null)
            Debug.Log("This unit does not have a cost");
        return cityResources[cityResource.production];
    }

    public bool checkCost(int player = -1, City city = null){
        Player tempPlayer = PlayerController.Instance.player;
        //checking if energy cost is met
        if(player==-1){
            tempPlayer = PlayerController.Instance.player;

        }
        else{
            tempPlayer = AIController.Instance.AIPlayers[player];
        }
        Dictionary<Resource,float> res = tempPlayer.resources.resources;
        Dictionary<Raw_Material,float> raw = tempPlayer.resources.RawMaterials;
        Dictionary<Energy,float> energy = tempPlayer.resources.Energies;
        Dictionary<OtherResource,float> otherRes = tempPlayer.resources.OtherResources;
        Dictionary<crystal,float> cryst = tempPlayer.resources.Crystals;
        Dictionary<cityResource,float> citRes = null;
        if(city!=null){
            citRes = city.cityResources;
        }

        //checking if food cost is met
        if(cityResources!=null){
            foreach(KeyValuePair<cityResource, float> entry in cityResources){
                if(entry.Key == cityResource.production)
                    continue;
                if(citRes[entry.Key]<cityResources[entry.Key]){
                    return false;
                }
            }
        }
        //checking if energy cost is met
        if(Energies!=null){
            foreach(KeyValuePair<Energy, float> entry in Energies){
                if(energy[entry.Key]<Energies[entry.Key]){
                    return false;
                }
            }
        }
        //checking if Resource cost is met
        if(resources!=null){
            foreach(KeyValuePair<Resource, float> entry in resources){
                if(res[entry.Key]<resources[entry.Key]){
                    return false;
                }
            }
        }
        //checking if Raw material cost is met
        if(RawMaterials!=null){
            foreach(KeyValuePair<Raw_Material, float> entry in RawMaterials){
                if(raw[entry.Key]<RawMaterials[entry.Key]){
                    return false;
                }
            }
        }
        //checking if otherResource cost is met
        if(OtherResources!=null){
            foreach(KeyValuePair<OtherResource, float> entry in OtherResources){
                if(entry.Key==OtherResource.Science)
                    continue;
                if(otherRes[entry.Key]<OtherResources[entry.Key]){
                    return false;
                }
            }
        }
        if(Crystals!=null){
            foreach(KeyValuePair<crystal, float> entry in Crystals){
                if(cryst[entry.Key]<Crystals[entry.Key]){
                    return false;
                }
            }
        }

        //spending all the things

        //spwnding food cost
        if(cityResources!=null){
            foreach(KeyValuePair<cityResource, float> entry in cityResources){
                if(entry.Key == cityResource.production)
                    continue;
                citRes[entry.Key] -=cityResources[entry.Key];
            }
        }
        if(Energies!=null){
            foreach(KeyValuePair<Energy, float> entry in Energies){
                tempPlayer.resources.Energies[entry.Key]-=Energies[entry.Key];
            }
        }

        //checking if Resource cost is met
        if(resources!=null){
            foreach(KeyValuePair<Resource, float> entry in resources){
                tempPlayer.resources.resources[entry.Key]-=resources[entry.Key];
            }
        }
        //checking if Raw material cost is met
        if(RawMaterials!=null){
            foreach(KeyValuePair<Raw_Material, float> entry in RawMaterials){
                tempPlayer.resources.RawMaterials[entry.Key]-=RawMaterials[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(OtherResources!=null){
            foreach(KeyValuePair<OtherResource, float> entry in OtherResources){
                if(entry.Key==OtherResource.Science)
                    continue;
                tempPlayer.resources.OtherResources[entry.Key]-=OtherResources[entry.Key];
            }
        }
        if(Crystals!=null){
            foreach(KeyValuePair<crystal, float> entry in Crystals){
                tempPlayer.resources.Crystals[entry.Key]-=Crystals[entry.Key];
            }
        }
        return true;   
    }

    public bool onlyCheckCost(int player = -1, City city =null){
        Player tempPlayer = PlayerController.Instance.player;
        //checking if energy cost is met
        if(player==-1){
            tempPlayer = PlayerController.Instance.player;

        }
        else{
            tempPlayer = AIController.Instance.AIPlayers[player];
        }
        Dictionary<Resource,float> res = tempPlayer.resources.resources;
        Dictionary<Raw_Material,float> raw = tempPlayer.resources.RawMaterials;
        Dictionary<Energy,float> energy = tempPlayer.resources.Energies;
        Dictionary<OtherResource,float> otherRes = tempPlayer.resources.OtherResources;
        Dictionary<crystal,float> cryst = tempPlayer.resources.Crystals;
        Dictionary<cityResource,float> citRes = null;
        if(city!=null){
            citRes = city.cityResources;
        }

        //checking if food cost is met
        if(cityResources!=null){
            foreach(KeyValuePair<cityResource, float> entry in cityResources){
                if(entry.Key == cityResource.production)
                    continue;
                if(citRes[entry.Key]<cityResources[entry.Key]){
                    return false;
                }
            }
        }
        
        //checking if energy cost is met
        if(Energies!=null){
            foreach(KeyValuePair<Energy, float> entry in Energies){
                if(energy[entry.Key]<Energies[entry.Key]){
                    return false;
                }
            }
        }
        //checking if Resource cost is met
        if(resources!=null){
            foreach(KeyValuePair<Resource,float> entry in resources){
                if(res[entry.Key]<resources[entry.Key]){
                    return false;
                }
            }
        }
        //checking if Raw material cost is met
        if(RawMaterials!=null){
            foreach(KeyValuePair<Raw_Material, float> entry in RawMaterials){
                if(raw[entry.Key]<RawMaterials[entry.Key]){
                    return false;
                }
            }
        }
        //checking if otherResource cost is met
        if(OtherResources!=null){
            foreach(KeyValuePair<OtherResource, float> entry in OtherResources){
                if(entry.Key==OtherResource.Science)
                    continue;
                if(otherRes[entry.Key]<OtherResources[entry.Key]){
                    return false;
                }
            }
        }
        if(Crystals!=null){
            foreach(KeyValuePair<crystal, float> entry in Crystals){
                if(cryst[entry.Key]<Crystals[entry.Key]){
                    return false;
                }
            }
        }
        return true;
    }

    public int spendProduction(City city, int player =-1){
        Player tempPlayer = PlayerController.Instance.player;

        if(player!=-1)
            tempPlayer = AIController.Instance.AIPlayers[player];
        Dictionary<cityResource,float> cit = city.cityResources;
        float var1 = cityResources[cityResource.production];
        float var2 = cit[cityResource.production];
        float days = var1/var2;

        //subtracting
        cit[cityResource.production]-=cityResources[cityResource.production];
        if(cit[cityResource.production]<0)
            cit[cityResource.production]=0;

        //int case have 0 sproduction
        if(var2==0)
            return (int)var1;
        if(var1%var2!=0)
            days++;
        if(days==0)
            days=1;
        return (int)days;
        
    }
    public int spendScience(int player =-1){
        Player tempPlayer = PlayerController.Instance.player;

        if(player!=-1)
            tempPlayer = AIController.Instance.AIPlayers[player];
        Dictionary<OtherResource,float> otherRes = tempPlayer.resources.OtherResources;
        float var1 = OtherResources[OtherResource.Science];
        float var2 = otherRes[OtherResource.Science];

        //subtracting
        otherRes[OtherResource.Science]-=OtherResources[OtherResource.Science];
        if(otherRes[OtherResource.Science]<0)
            otherRes[OtherResource.Science]=0;

        //int case have 0 sciencwe
        if(var2==0)
            return (int)var1;
        float days = var1/var2;
        if(var1%var2!=0)
            days++;
        if(days==0)
            days=1;
        return (int)days;
        
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
            if(Energies!=null){
                foreach(KeyValuePair<Energy, float> entry in Energies){
                    Debug.Log(string.Format("require {0} {1} Energy",Energies[entry.Key],entry.Key));
                }
            }
            //checking if energy cost is met
            if(resources!=null){
                foreach(KeyValuePair<Resource, float> entry in resources){
                    Debug.Log(string.Format("require {0} {1}",resources[entry.Key],entry.Key));
                }
            }
            //checking if Raw material cost is met
            if(RawMaterials!=null){
                foreach(KeyValuePair<Raw_Material, float> entry in RawMaterials){
                    Debug.Log(string.Format("require {0} {1}",RawMaterials[entry.Key],entry.Key));
                }
            }
            //checking if energy cost is met
            if(OtherResources!=null){
                foreach(KeyValuePair<OtherResource,float> entry in OtherResources){
                    Debug.Log(string.Format("require {0} {1}",OtherResources[entry.Key],entry.Key));
                }
            }
            if(Crystals!=null){
                foreach(KeyValuePair<crystal,float> entry in Crystals){
                    Debug.Log(string.Format("require {0} {1}",Crystals[entry.Key],entry.Key));
                }
            } 

        }
    
}
