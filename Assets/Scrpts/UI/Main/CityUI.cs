using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class CityUI : MonoBehaviour
{
    public GameObject populationIndicator;
    public GameObject growthIndicator;
    public GameObject Name;

    //settlement stats
    public GameObject Food;
    public GameObject Production;
    public GameObject Alchemy;
    public GameObject Science;
    public GameObject Pantheon;
    public GameObject House;

    //Element stats
    public GameObject Air;
    public GameObject Fire;
    public GameObject Water;
    public GameObject Earth;
    public GameObject Light;
    public GameObject Dark;

    private void OnEnable()
    {
        City city = GameState.Instance.selectedObject.GetComponent<City>();
        //chaning population indicator
        populationIndicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = city.population.ToString() + "/" + city.maxPopulation;
        float foodNeeded = requiredFood(city) ;

        //changing growth Indiactor
        growthIndicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = foodNeeded.ToString();

        //Cahnign Name
        Name.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = city.Name;

        //setting Settlement Stats
        Food.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ((int)city.cityResources[cityResource.food]).ToString();
        Production.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ((int)city.cityResources[cityResource.production]).ToString();
        Alchemy.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ((int)city.resourcesYield.OtherResources[OtherResource.Science]).ToString();
        Science.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ((int)city.resourcesYield.OtherResources[OtherResource.alchemy]).ToString();
        //TODO: pantheon and house

        //energies
        Air.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ((int)city.resourcesYield.Energies[Energy.Air]).ToString();
        Fire.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ((int)city.resourcesYield.Energies[Energy.Fire]).ToString();
        Water.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ((int)city.resourcesYield.Energies[Energy.Water]).ToString();
        Earth.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ((int)city.resourcesYield.Energies[Energy.Earth]).ToString();
        Light.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ((int)city.resourcesYield.Energies[Energy.Light]).ToString();
        Dark.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ((int)city.resourcesYield.Energies[Energy.Dark]).ToString();


        //TODO: for now, have to change UI for burning brood
        //for burning brood
        if(city.getPlayer().kingdom.type==Energy.Fire)
            Fire.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ((int)city.resourcesYield.cityResources[cityResource.Fire]).ToString();



    }


    private int requiredFood(City city)
    {
        //using fire for Burning Brood for now
        double result =(int)( 10 + 5 * (city.population + Math.Pow(city.population, 1.5)));
        result = result - city.cityResources[cityResource.Fire];
        result = Math.Ceiling(result / city.resourcesYield.cityResources[cityResource.Fire]);
        return (int)result;

    }
}
