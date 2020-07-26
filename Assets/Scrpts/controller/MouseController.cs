﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MouseController : MonoBehaviour
{
    private GameObject mouseOverObject;
    RaycastHit theObject;
    public static MouseController Instance;
    public GameObject selectedObject;
    private PathFinding pathFinder; //testing A* pathfinding
    private Unit unit;//testing A*pathfinding
    
    public bool moving=false;
    private List<HexComponent> travelPath= new List<HexComponent>();//testing
    void Awake(){
        pathFinder = new PathFinding();
        if(Instance==null){
            Instance =this;
            return;
        }
        else{
            Destroy(gameObject);
        }
    }
    void Start(){
        //moving = GameState.Instance.moving;
    }
    


    // Update is called once per frame
    void Update()
    {

        //setting selected and hovering object on mouse

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayLength= (mouseRay.origin.y/mouseRay.direction.y);

        Vector3 hitPos = mouseRay.origin - (mouseRay.direction*rayLength*1000f);
        
        //finding which object mouse is hovering on
        if(Physics.Raycast(Camera.main.transform.position,hitPos,out theObject,Mathf.Infinity)){
                mouseOverObject = theObject.transform.gameObject;
                GameState.Instance.mouseOverObject=mouseOverObject.transform.parent.gameObject;
            }
        
        
        //if mouse isclicked,change selected object in GameState
        if(Input.GetMouseButtonDown(0) && GameState.Instance.checkPlayerTurn()){
            GameObject newSelectedObject=null;
            if(EventSystem.current.IsPointerOverGameObject() || moving)
                return;
            if(Physics.Raycast(Camera.main.transform.position,hitPos,out theObject,Mathf.Infinity)){
                
                bool moveUnit = false;
                newSelectedObject = theObject.transform.gameObject.transform.parent.gameObject;
                //check if new selected object is a hex or not
                if(selectedObject!=null && selectedObject.tag=="Unit" && (newSelectedObject.tag == "enemy" || newSelectedObject.tag=="Hex")){
                    unit= selectedObject.GetComponent<Unit>();
                    if(!unit.exhausted && !unit.paralysed)
                        moveUnit = true;
                }
                //selectedObject = newSeletcedObject;
                //upating selected object if the unit is not moving 
                //if its moving then the selected objbject will remain the same as before(unit)
                if(!moveUnit){
                    //Close UI based on what object was cliskced
                    selectedObject = newSelectedObject;
                    GameState.Instance.selectedObject = newSelectedObject;
                    GameObject obj = selectedObject;//GameState.Instance.slectedObject;
                    if(obj.CompareTag("Unit")){
                    UIController.Instance.openUnitHub();
                    }
                    else if(obj.CompareTag("City")){
                        UIController.Instance.openBuildingHub();
                    }
                    else{
                        UIController.Instance.CloseHub();
                    }
                }
                
                
                //testing A* pathfinde
                if(newSelectedObject.tag !="Unit" && moveUnit){
                    moving=true;
                    HexComponent source = HexMap.Instance.getHexComponent(unit.location);
                    HexComponent dest=null;
                    bool attacking =false;
                    Unit enemy=null;
                    if(newSelectedObject.tag =="Hex")
                        dest= newSelectedObject.GetComponent<HexComponent>();
                    else if(newSelectedObject.tag == "enemy"){   //if selected is enemy
                        //Debug.Log("ran");
                        dest = HexMap.Instance.getHexComponent(newSelectedObject.GetComponent<Unit>().location);
                        //Debug.Log(string.Format("location: {0}",dest.location));
                    }
                    
                    //add more in case of playerclicks on cities and buildings and stuff
                    
                    
                    travelPath = pathFinder.shortesPath(source,dest);

                    //if it contains enemies
                    if(dest.containEnemies()){
                        Debug.Log("This Hex contans enemies");
                        enemy = dest.enemies[0];
                        travelPath.RemoveAt(travelPath.Count-1);
                        if(travelPath!=null && travelPath.Count>0)
                            dest = travelPath[travelPath.Count-1];

                        ///in case its right next to the player
                        else{
                            unit.dealDamage(enemy);
                            travelPath.Clear();
                            moving = false;
                            return;
                        }
                        attacking=true;
                    }
                    //in case the dest is out of range
                    if(travelPath.Count>unit.movement){
                        Debug.Log("Too far");
                        int tempINT = travelPath.Count;
                        for(int i=unit.movement;i<tempINT;i++){
                            travelPath.RemoveAt(unit.movement);
                        }
                    }
                    
                    Task t = new Task(unit.moveUnit(travelPath));
                    t.Finished+=delegate (bool manual){
                        if(attacking)
                            unit.dealDamage(enemy);
                            
                        travelPath.Clear();
                        moving = false;
                    };
                    
                }
                
            }
        } 
    }
}