using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Earth_District : District
{
    public int campFireBonus = 8;
    public float additionalEnergy=0f;
    //for adding 8% of the population as the fire energy 
    public override void setCamp()
    {
        removeYield();
        city = GetComponent<City>();
        
        type = Type.camp;
        resourcesYield = campResources;
        //adding additional fire energy based on population of the city
        setAdditionalEnergy();
        //resourcesYield.cityResources[cityResource.Fire]+=additionalEnergy;

        setYield();

        
    }
    public override void populationChanged(){
        //if(type==Type.camp)
            //setCamp();
    }

    //settting 8% bonus on fire
    public void setAdditionalEnergy(){
        //subtracting prevois bonus
        resourcesYield.cityResources[cityResource.Fire]-=additionalEnergy;
        city.resourcesYield.cityResources[cityResource.Fire]-=additionalEnergy;
        additionalEnergy = (int)((city.cityResources[cityResource.Fire]/100f)*(city.population*campFireBonus));

        //Setting both here and in city
        resourcesYield.cityResources[cityResource.Fire]+=additionalEnergy;
        city.resourcesYield.cityResources[cityResource.Fire]+=additionalEnergy;
    }

    public override void StartTurn()
    {
        base.StartTurn();
        setAdditionalEnergy();
    }
}
