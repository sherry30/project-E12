using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum biome {
    mountain,
    water,
    polar,
    tundra
}
public enum terrain{
    //water terrains
    water_coast,
    water_ocean,
    water_deep_ocean,
    water_trench,
    //polar terrains
    polar_rocky_coast,
    polar_permafrost,//also in tundra
    polar_snow, //also in tundra
    polar_packed_snow,
    //tundra terrain
    tundra_permafrost,
    tundra_prairie,
    tundra_snow,
    tundra_marsh,
    //mountain
    mountain,
    cliff
}

public class Biome : MonoBehaviour
{
    public biome hexBiome;
    public terrain hexTerrain;
    public void setBiome(biome b){
        hexBiome = b;
        /*if(hexBiome==biome.water){
            elevations.Add(-1);elevations.Add(-2);elevations.Add(-3);elevations.Add(-4);
        }
        else if(hexBiome==biome.polar){

            elevations.Add(0);elevations.Add(0);elevations.Add(0);elevations.Add(0);
        }
        else if(hexBiome==biome.tundra){

            elevations.Add(0);elevations.Add(0);elevations.Add(0);elevations.Add(0);
        }*/
    }
    public int setTerrain(terrain t){
        hexTerrain = t;
        //returning material index based on terrain
        switch(t){
            case terrain.water_coast:
                return 0;
            case terrain.water_ocean:
                return 1;
            case terrain.water_deep_ocean:
                return 2;
            case terrain.water_trench:
                return 3;
            case terrain.polar_rocky_coast:
                return 4;
            case terrain.polar_permafrost:
            case terrain.tundra_permafrost:
                return 5;
            case terrain.polar_snow:
            case terrain.tundra_snow:
                return 6;
            case terrain.polar_packed_snow:
                return 7;
            case terrain.tundra_prairie:
                return 8;
            case terrain.tundra_marsh:
                return 9;
            //for now mountain
            case terrain.mountain:
                return 4;
        }
        return 0;
        //return mats[terrainAvailable.IndexOf(t)];

    }
    public int setElevation(){
        //returning  elevation based on terrain
        switch(hexTerrain){
            case terrain.water_coast:
                return -1;
            case terrain.water_ocean:
                return -2;
            case terrain.water_deep_ocean:
                return -3;
            case terrain.water_trench:
                return -4;
            case terrain.polar_rocky_coast:
                return 0;
            case terrain.polar_permafrost:
            case terrain.tundra_permafrost:
                return 0;
            case terrain.polar_snow:
            case terrain.tundra_snow:
                return 0;
            case terrain.polar_packed_snow:
                return 0;
            case terrain.tundra_prairie:
                return 0;
            case terrain.tundra_marsh:
                return 0;
            case terrain.mountain:
                return 6;
            case terrain.cliff:
                return 5;
        }
        return 0;
    }



}
