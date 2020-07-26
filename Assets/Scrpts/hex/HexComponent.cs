﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexComponent : MonoBehaviour
{



    public Hex hex;
    public Building building;
    public Improvement improvement;
    public City city;
    public TextMesh text;
    public List<Unit> units = new List<Unit>();
    public List<Unit> enemies = new List<Unit>();
    private float radiusOfUnits=2.5f;
    public int production = 2;
    public Biome hexBiome;
    public Vector2 location;
    public int elevation=1;
    public UpdatePosition updatePos;


    //public HexComponent[] neighbors;
    // Start is called before the first frame update
    void Awake(){
        //AudioSettings default biome
        hexBiome = GetComponent<Biome>();
        setBiomAndTerrain(biome.water,terrain.water_deep_ocean);

        //getting updatePosition
        updatePos = GetComponent<UpdatePosition>();
        updatePos.location = location;
    }

    /*public void updatePosition(){
        //updating this hexes position
        this.transform.position = hex.positionFromCamera();
    }*/
    public void addUnit(Unit un){
        if(units==null)
            units = new List<Unit>();
        units.Add(un);
        changeUnitElevation(un);
        setUnitOffsets();
    }
    public void removeUnit(int id){
        for(int i=0;i<units.Count;i++){
            if(units[i].id==id){
                removeUnitOffsets(i);
                units.RemoveAt(i);
            }
        }
        setUnitOffsets();
    }
    public void addEnemy(Unit un){
        if(enemies==null)
            enemies = new List<Unit>();
        //changeUnitElevation(un);
        enemies.Add(un);
        changeUnitElevation(un);
        setEnemiesOffsets();
    }
    public bool containEnemies(){
        if(enemies!=null)
            return enemies.Count>0;
        return false;
    }
    public bool containUnits(){
        if(units!=null)
            return units.Count>0;
        return false;
    }
    public void removeEnemy(int id){
        for(int i=0;i<enemies.Count;i++){
            if(enemies[i].id==id){
                removeEnemyOffsets(i);
                enemies.RemoveAt(i);
                
            }
        }
    }
    //when there are no enemies,buildings, cities or units
    public bool isEmpty(){
        if(!containEnemies()){
            if(units!=null){
                if(units.Count==0){
                    if(city==null){
                        if(building==null && improvement==null)
                            return true;
                    }
                }
            }
        }
        return false;
    }
    public bool containStructure(){
        return building!=null || city!=null || improvement!=null; 
    }
    //setting units offset within this hex
    public void setUnitOffsets(){
        if((units!=null && units.Count>1)|| (containStructure())){
            for(int i=0 ;i<units.Count;i++){
                float angle = i*Mathf.PI*2f/units.Count;
                Vector3 newPos= new Vector3(Mathf.Cos(angle)*radiusOfUnits,0,Mathf.Sin(angle)*radiusOfUnits);
                units[i].setOffset(new Vector3(newPos.x,0,newPos.z));   
            }
            for(int i=0 ;i<units.Count;i++){
                units[i].updatePos.updateLocationFromCamera();
            }
            
        }
    }
    public void setEnemiesOffsets(){
        if((enemies!=null && enemies.Count>1)|| (containStructure())){
            for(int i=0 ;i<enemies.Count;i++){
                float angle = i*Mathf.PI*2f/enemies.Count;
                Vector3 newPos= new Vector3(Mathf.Cos(angle)*radiusOfUnits,0,Mathf.Sin(angle)*radiusOfUnits);
                enemies[i].setOffset(new Vector3(newPos.x,0,newPos.z));
                //Debug.Log(string.Format("x: {0}, z: {1}",newPos.z,newPos.z));   
            }
            for(int i=0 ;i<units.Count;i++){
                enemies[i].updatePos.updateLocationFromCamera();
            }
            
        }
    }
    //when the unit leaves
    public void removeUnitOffsets(int i){
        units[i].offset.x=0;
        units[i].offset.z=0;
        units[i].offset.y-=elevation*Hex.hexHeight;

    }
    public void removeEnemyOffsets(int i){
        enemies[i].offset.x=0;
        enemies[i].offset.z=0;
        enemies[i].offset.y-=elevation*Hex.hexHeight;

    }

    public void setBiomAndTerrain(biome b,terrain t ){
        hexBiome.setBiome(b);
        gameObject.GetComponentInChildren<MeshRenderer>().material = HexMapGenerator.Instance.mats[hexBiome.setTerrain(t)];
        elevation = hexBiome.setElevation();
        Vector3 y = new Vector3(0,elevation*Hex.hexHeight*2,0);
        
        //setting offset in updatePosition component of the hex
        updatePos = GetComponent<UpdatePosition>();
        updatePos.offset = y;

    }
    public void setHex(Hex h){
        hex = h;
        location = new Vector2(hex.Q,hex.R);

        //setting offset in updatePosition component of the hex
        updatePos = GetComponent<UpdatePosition>();
        updatePos.location = location;
    }
    public bool checkTerrain(terrain t){
        if(hexBiome.hexTerrain==t)
            return true;
        return false;
    }
    public bool checkBiome(biome b){
        if(hexBiome.hexBiome==b)
            return true;
        return false;
    }
    private void changeUnitElevation(Unit unit){
        unit.offset.y+=elevation*Hex.hexHeight;
    }

    public void BuildImprovement(Improvement imp){
        improvement = imp;
        setUnitOffsets();
    }

}
