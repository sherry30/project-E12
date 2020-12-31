using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fire_Earth_City : City
{
    public override void populationGrowth()
    {
        while(true){
            int result = (int)(10+5*(population+Math.Pow(population,1.5)));
            if(cityResources[cityResource.Fire] + cityResources[cityResource.Fire]*populationGrowthHelp>=result){
                population++;
                cityResources[cityResource.Fire]-=result;
                if(cityResources[cityResource.Fire]<=0)
                    cityResources[cityResource.Fire]=0;
                //changing the yield because of additional 8% energy based on population
                //telling all districts that population changed
                thisDistrict.populationChanged();
                foreach(District d in districts){
                    d.populationChanged();
                    
                }
            }
            else
                break;
        }

    }

    //for fixing Fire energy as a city's own resource
   /* public override void getYield()
    {
        Player tempPlayer = PlayerController.Instance.player;
        if(player!=-1){
            tempPlayer = AIController.Instance.AIPlayers[player];
        }
        float fireYield = 0;
        if(resourcesYield.Energies!=null){
            foreach(KeyValuePair<Energy, float> entry in resourcesYield.Energies){
                if(Energy.Fire==entry.Key){
                    fireYield= resourcesYield.Energies[entry.Key];
                    continue;
                }
                tempPlayer.resources.Energies[entry.Key]+=resourcesYield.Energies[entry.Key];
            }
        }
        //checking if Resource cost is met
        if(resourcesYield.resources!=null){
            foreach(KeyValuePair<Resource, float> entry in resourcesYield.resources){
                tempPlayer.resources.resources[entry.Key]+=resourcesYield.resources[entry.Key];
            }
        }
        //checking if Raw material cost is met
        if(resourcesYield.RawMaterials!=null){
            foreach(KeyValuePair<Raw_Material, float> entry in resourcesYield.RawMaterials){
                tempPlayer.resources.RawMaterials[entry.Key]+=resourcesYield.RawMaterials[entry.Key];
            }
        }
        //checking if otherResource cost is met
        if(resourcesYield.OtherResources!=null){
            foreach(KeyValuePair<OtherResource, float> entry in resourcesYield.OtherResources){
                tempPlayer.resources.OtherResources[entry.Key]+=resourcesYield.OtherResources[entry.Key];
            }
        }
        //checking if Crystals cost is met
        if(resourcesYield.Crystals!=null){
            foreach(KeyValuePair<crystal, float> entry in resourcesYield.Crystals){
                tempPlayer.resources.Crystals[entry.Key]+=resourcesYield.Crystals[entry.Key];
            }
        }

        if(resourcesYield.cityResources!=null){
            foreach(KeyValuePair<cityResource, float> entry in resourcesYield.cityResources){
                cityResources[entry.Key]+=resourcesYield.cityResources[entry.Key];
            }
        }
*/
}
