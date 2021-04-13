using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public enum Inventories
    {
        unit,
        city,
        district,
        army
    }
    public GameObject currentlyOpened = null;
    public Inventories opened;
    public static UIController  Instance{get;private set;}
    private HealthBar healthBar;
    public int unitInv,cityInv,districtInv, armyInv;
    private bool armyOpen = false;
    private Army armySelected;
    void Awake(){   
        if(Instance==null){
            Instance =this;
            return;
        }
        else{
            Destroy(gameObject);
        }
    }
    public void openCityHub(){
        opened = Inventories.city;
        if(currentlyOpened!=null)
            hideUI();   
        else if(currentlyOpened==gameObject.transform.GetChild(0).transform.GetChild(cityInv).gameObject){
            hideUI();   
            displayUI();   
            return; 
        }
        objectChanged(gameObject.transform.GetChild(0).transform.GetChild(cityInv).gameObject);
        displayUI();
    }
    public void openDistrictHub(){
        opened = Inventories.district;
        if (currentlyOpened!=null)
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
        opened = Inventories.unit;
        if (currentlyOpened!=null){
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

    public void openArmyHub()
    {
        opened = Inventories.army;
        //hiding whatever is already open
        if (currentlyOpened != null)
        {
            hideUI();
        }

        //in case of refresh
        else if (currentlyOpened == gameObject.transform.GetChild(0).transform.GetChild(armyInv).gameObject)
        {
            hideUI();
            displayUI();
            return;
        }
        objectChanged(gameObject.transform.GetChild(0).transform.GetChild(armyInv).gameObject);
        displayArmyUI();
        armyOpen = true;
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
    private void displayArmyUI()
    {
        currentlyOpened.SetActive(true);
        healthBar.displayHealth();

        //displaying all healthbar in the army
        Army a = GameState.Instance.selectedObject.GetComponent<Unit>().getArmy();
        armySelected = a;
        foreach(Unit u in a.units)
        {
            u.gameObject.GetComponent<HealthBar>().displayHealth();
        }
    }
    private void hideUI(){
        if(currentlyOpened==null)
            return;
        currentlyOpened.SetActive(false);
        currentlyOpened=null;
        //healthBar = current.GetComponent<HealthBar>();
        if(healthBar!=null){
            

            if (armyOpen)
            {
                armyOpen = false;
                foreach(Unit u in armySelected.units) {
                    u.gameObject.GetComponent<HealthBar>().hideHealth();
                }
                armySelected = null; 
            }

            else
            {
                healthBar.hideHealth();
                
            }
            healthBar = null;
        }
    }
    private void objectChanged(GameObject curr){
        currentlyOpened = curr;
        healthBar = GameState.Instance.selectedObject.GetComponent<HealthBar>();
    }

    
    
    public void deSelectObject(){
        hideUI();
    }

    public void refreshUI()
    {
        if (opened == Inventories.unit)
            openUnitHub();
        if (opened == Inventories.army)
            openArmyHub();
        if (opened == Inventories.city)
            openCityHub();
        if (opened == Inventories.district)
            openDistrictHub();
    }
}
