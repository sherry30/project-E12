using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemProductionList : MonoBehaviour
{
    public GameObject unitInfo;
    private City city;
    private Transform available;
    private Transform current;
    public int currentIndex=0,availableIndex=1;
    List<int> itemInd = new List<int>();
    void Awake(){
        current = gameObject.transform.GetChild(currentIndex).GetChild(0);
        available = gameObject.transform.GetChild(availableIndex).GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(0);
        //available = gameObject.transform.Find("UnitFrame");
    }
    void OnEnable(){
        city = GameState.Instance.selectedObject.GetComponent<City>();
        int startIndex = city.itemStartIndex;
        if(itemInd==null)
            itemInd = new List<int>();//for producing

        //setting currentUnitInfo if theres a unit in production
        if(city!=null && city.itemProduction!=-1){
            GameObject obj = (GameObject) Instantiate(unitInfo);
            obj.transform.SetParent(current, false);
            obj.transform.localScale = new Vector3(1, 1, 1);
            Transform unitDesc = obj.transform.GetChild(0).GetChild(1);
            Item item = PlayerController.Instance.player.kingdom.items[city.itemProduction];

            //changing text on the UI
            unitDesc.Find("ItemName").GetComponent<Text>().text = item.name;
            unitDesc.Find("ItemClass").GetComponent<Text>().text = item.type.ToString();
        }

        //making all the available items to be produced    
        for(int i=0;i<city.numOfItems;i++){
            
            //instiating UnitInfo object
            GameObject obj = (GameObject) Instantiate(unitInfo);
            obj.transform.SetParent(available, false);
            obj.transform.localScale = new Vector3(1, 1, 1);
            Item item = PlayerController.Instance.player.kingdom.items[startIndex+i];
            Transform unitDesc = obj.transform.GetChild(0).GetChild(1);

            //changing text on the UI
            unitDesc.Find("ItemName").GetComponent<Text>().text = item.name;
            unitDesc.Find("ItemClass").GetComponent<Text>().text = item.type.ToString();

            //subscribing the function with the button
            Button tempButton = obj.GetComponent<Button>();
            int copy = i;
            tempButton.onClick.AddListener(delegate {Produce(startIndex+copy,item);});//() => Produce(startIndex+i,unit));
            

        }
    }
    void OnDisable(){
        for(int i=0;i<available.childCount;i++){
            Destroy(available.GetChild(i).gameObject);
        }
        if(city!=null && (city.itemProduction!=-1 || city.unitProduction!=-1))   
            Destroy(current.GetChild(0).gameObject);
        
    }
    private void Produce(int index,Item item){
        //checking if nothing is being produced rn
        if(city!=null && city.itemProduction==-1 && city.unitProduction==-1){
            if(!item.cost.checkCost()){
                Debug.Log("Not enough Resources");
                item.cost.printCost();
                return;
            }
            city.ProduceItem(index);
            GameObject obj = (GameObject) Instantiate(unitInfo);
            obj.transform.SetParent(current, false);
            obj.transform.localScale = new Vector3(1, 1, 1);
            Transform unitDesc = obj.transform.GetChild(0).GetChild(1);

            //changing text on the UI
            unitDesc.Find("ItemName").GetComponent<Text>().text = item.Name;
            unitDesc.Find("ItemClass").GetComponent<Text>().text = item.type.ToString();
        }

    }
}
