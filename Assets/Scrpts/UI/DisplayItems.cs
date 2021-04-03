using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItems : MonoBehaviour
{
    private City city;
    private Unit unit;
    public bool cityList;
    public GameObject itemSlot;
    void OnEnable(){
        if(cityList){
            city =  GameState.Instance.selectedObject.GetComponent<City>();

            //checking if selected building is acityornot
            if(city==null)
                return;
            for(int i=0;i<city.itemInventory.Count;i++){
                Debug.Log("Displayting Item");
                GameObject obj = (GameObject) Instantiate(itemSlot);
                obj.transform.SetParent(this.transform, false);
                obj.transform.localScale = new Vector3(1, 1, 1);

                GameObject item = (GameObject) Instantiate(city.itemInventory[i]);
                item.transform.SetParent(obj.transform, false);
                item.transform.localScale = new Vector3(0.34f, 0.34f, 0.34f);

            }
        }
        else{
            unit =  GameState.Instance.selectedObject.GetComponent<Unit>();

            //checking if selected building is acityornot
            if(unit==null)
                return;
            for(int i=0;i<unit.items.Count;i++){

                GameObject obj = (GameObject) Instantiate(itemSlot);
                obj.transform.SetParent(this.transform, false);
                obj.transform.localScale = new Vector3(1, 1, 1);

                GameObject item = (GameObject) Instantiate(unit.items[i]);
                item.transform.SetParent(obj.transform, false);
                item.transform.localScale = new Vector3(0.34f, 0.34f, 0.34f);

            }
        }
    }
    void OnDisable(){
        for(int i=0;i<transform.childCount;i++){
            Destroy(transform.GetChild(i).gameObject);
        }
        city =null;
        
    }
}
