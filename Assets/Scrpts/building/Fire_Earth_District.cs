using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Earth_District : District
{
    //for adding 8% of the population as the fire energy 
    public override void setCamp()
    {
        removeYield();
        city = GetComponent<City>();
        
        type = Type.camp;
        resourcesYield = campResources;
        //adding additional fire energy based on population of the city
        int additionalEnergy = (int)((city.population/100f)*8);
        resourcesYield.cityResources[cityResource.Fire]+=additionalEnergy;

        setYield();

        
    }
    public override void populationChanged(){
        if(type==Type.camp)
            setCamp();
    }
}
