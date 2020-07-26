using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitProductionList : MonoBehaviour
{
    public GameObject unitInfo;
    private City city;
    private Transform available;
    private Transform current;
    public int currentIndex=0,availableIndex=1;
    List<int> unitInd = new List<int>();
    void Awake(){
        current = gameObject.transform.GetChild(currentIndex).GetChild(0);
        available = gameObject.transform.GetChild(availableIndex).GetChild(0);
    }
    void OnEnable(){
        city = GameState.Instance.selectedObject.GetComponent<City>();
        int startIndex = city.unitStartIndex;
        if(unitInd==null)
            unitInd = new List<int>();//for producing

        //setting currentUnitInfo if theres a unit in production
        if(city!=null && city.unitProduction!=-1){
            GameObject obj = (GameObject) Instantiate(unitInfo);
            obj.transform.SetParent(current, false);
            obj.transform.localScale = new Vector3(1, 1, 1);
            Transform unitDesc = obj.transform.GetChild(0).GetChild(1);
            Unit unit = PlayerController.Instance.player.kingdom.units[city.unitProduction];

            //changing text on the UI
            unitDesc.Find("UnitName").GetComponent<Text>().text = unit.name;
            unitDesc.Find("UnitClass").GetComponent<Text>().text = unit.classOfUnit.ToString();
        }

        //making all the available units to be produced    
        for(int i=0;i<city.numOfUnits;i++){
            
            //instiating UnitInfo object
            GameObject obj = (GameObject) Instantiate(unitInfo);
            obj.transform.SetParent(available, false);
            obj.transform.localScale = new Vector3(1, 1, 1);
            Unit unit = PlayerController.Instance.player.kingdom.units[startIndex+i];
            Transform unitDesc = obj.transform.GetChild(0).GetChild(1);

            //changing text on the UI
            unitDesc.Find("UnitName").GetComponent<Text>().text = unit.name;
            unitDesc.Find("UnitClass").GetComponent<Text>().text = unit.classOfUnit.ToString();

            //subscribing the function with the button
            Button tempButton = obj.GetComponent<Button>();
            int copy = i;
            tempButton.onClick.AddListener(delegate {Produce(startIndex+copy,unit);});//() => Produce(startIndex+i,unit));
            

        }
    }
    void OnDisable(){
        for(int i=0;i<available.childCount;i++){
            Destroy(available.GetChild(i).gameObject);
        }
        if(city!=null && city.unitProduction!=-1)   
            Destroy(current.GetChild(0).gameObject);
        
    }
    private void Produce(int index,Unit unit){
        if(city!=null && city.unitProduction==-1){
            city.ProduceUnit(index,unit);
            GameObject obj = (GameObject) Instantiate(unitInfo);
            obj.transform.SetParent(current, false);
            obj.transform.localScale = new Vector3(1, 1, 1);
            Transform unitDesc = obj.transform.GetChild(0).GetChild(1);

            //changing text on the UI
            unitDesc.Find("UnitName").GetComponent<Text>().text = unit.Name;
            unitDesc.Find("UnitClass").GetComponent<Text>().text = unit.classOfUnit.ToString();
        }

    }
}
