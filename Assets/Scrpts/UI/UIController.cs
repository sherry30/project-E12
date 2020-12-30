using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject currentlyOpened = null;
    public static UIController  Instance{get;private set;}
    private HealthBar healthBar;
    public int unitInv,buildingInv,districtInv;
    void Awake(){   
        if(Instance==null){
            Instance =this;
            return;
        }
        else{
            Destroy(gameObject);
        }
    }
    public void openBuildingHub(){
        if(currentlyOpened!=null)
            hideUI();   
        else if(currentlyOpened==gameObject.transform.GetChild(0).transform.GetChild(buildingInv).gameObject){
            hideUI();   
            displayUI();   
            return; 
        }
        objectChanged(gameObject.transform.GetChild(0).transform.GetChild(buildingInv).gameObject);
        displayUI();
    }
    public void openDistrictHub(){
        if(currentlyOpened!=null)
            hideUI();   
        else if(currentlyOpened==gameObject.transform.GetChild(0).transform.GetChild(districtInv).gameObject){
            hideUI();   
            displayUI();   
            return; 
        }
        objectChanged(gameObject.transform.GetChild(0).transform.GetChild(districtInv).gameObject);
        displayUI();
    }

    public void openUnitHub(){
        if(currentlyOpened!=null){
            hideUI();   
        }
        else if(currentlyOpened==gameObject.transform.GetChild(0).transform.GetChild(unitInv).gameObject){
            hideUI();  
            displayUI();
            return;
        }
        objectChanged(gameObject.transform.GetChild(0).transform.GetChild(unitInv).gameObject);
        displayUI();
    }
    public void CloseHub(){
        if(currentlyOpened!=null)
            hideUI();
    }
    private void displayUI(){
        currentlyOpened.SetActive(true);
        //healthBar = current.GetComponent<HealthBar>();
        healthBar.displayHealth();
    }
    private void hideUI(){
        if(currentlyOpened==null)
            return;
        currentlyOpened.SetActive(false);
        currentlyOpened=null;
        //healthBar = current.GetComponent<HealthBar>();
        if(healthBar!=null){
            healthBar.hideHealth();
            healthBar = null;
        }
    }
    private void objectChanged(GameObject curr){
        currentlyOpened = curr;
        healthBar = GameState.Instance.selectedObject.GetComponent<HealthBar>();
    }
    
    public void deSelectObject(){
        hideUI();
        healthBar = null;
    }

}
