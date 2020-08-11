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
        available = gameObject.transform.GetChild(availableIndex).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0);
        //available = gameObject.transform.Find("UnitFrame");
    }
    void OnEnable(){
        city =  GameState.Instance.selectedObject.GetComponent<City>();

        //checking if selected building is acityornot
        if(city==null)
            return;

        int startIndex = city.unitStartIndex;
        if(unitInd==null)
            unitInd = new List<int>();//for producing

        //setting currentUnitInfo if theres a unit in production
        if(city!=null && city.unitProduction!=-1){
            //Debug.Log("ran");
            GameObject obj = (GameObject) Instantiate(unitInfo);
            obj.transform.SetParent(current, false);
            obj.transform.localScale = new Vector3(1, 1, 1);
            Transform unitDesc = obj.transform.GetChild(0).GetChild(1);
            Unit unit = PlayerController.Instance.player.kingdom.units[city.unitProduction];

            //changing image of Unit
            obj.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = unit.icon;

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

            //changing image of Unit
            obj.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = unit.icon;
            

            //changing text on the UI
            unitDesc.Find("UnitName").GetComponent<Text>().text = unit.name;
            unitDesc.Find("UnitClass").GetComponent<Text>().text = unit.classOfUnit.ToString();

            //subscribing the function with the button
            Button tempButton = obj.GetComponent<Button>();
            int copy = i;
            tempButton.onClick.AddListener(delegate {Produce(startIndex+copy,unit,obj);});//() => Produce(startIndex+i,unit));
            

        }
    }
    void OnDisable(){
        for(int i=0;i<available.childCount;i++){
            Destroy(available.GetChild(i).gameObject);
        }
        if(city!=null && current.childCount>0)   
            Destroy(current.GetChild(0).gameObject);
        city =null;
        
    }
    private void Produce(int index,Unit unit,GameObject objj){
        //if a unit or an item is already being produced then return
        if(city!=null && city.unitProduction==-1 && city.itemProduction==-1){
            int days=1;
            if(unit.cost.checkCost())
                days = unit.cost.spendProduction();
            else{
                Debug.Log("Not enough Resources");
                unit.cost.printCost();
                return;
            }
            city.ProduceUnit(index,days);
            GameObject obj = (GameObject) Instantiate(objj);
            obj.transform.SetParent(current, false);
            obj.transform.localScale = new Vector3(1, 1, 1);
            Transform unitDesc = obj.transform.GetChild(0).GetChild(1);

            //changing text on the UI
            /*unitDesc.Find("UnitName").GetComponent<Text>().text = unit.Name;
            unitDesc.Find("UnitClass").GetComponent<Text>().text = unit.classOfUnit.ToString();*/
        }

    }
}
