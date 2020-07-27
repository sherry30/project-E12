using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class makingButton : MonoBehaviour
{
    public GameObject buttonPrefab;//button for camper and camp
    private GameObject but;
    void OnEnable(){
        GameObject temp = GameState.Instance.selectedObject;
        //making camper button
        if(temp.CompareTag("City")){
            City tempCity = temp.GetComponent<City>();
            if(tempCity.typeOfCity== City.Type.camp){
                but = (GameObject) Instantiate(buttonPrefab);
                but.transform.SetParent(transform, false);
                but.transform.localScale = new Vector3(1, 1, 1);

                Button tempButton = but.GetComponent<Button>();
                if(!tempCity.camped)
                    tempButton.onClick.AddListener(() => tempCity.Campers());
                else    
                    Debug.Log("Already camped");
            }
        }
        else if(temp.CompareTag("Unit")){
            Unit tempUnit = temp.GetComponent<Unit>();

            //if the selected unit is a camper
            if(tempUnit.classOfUnit== Unit.Class.camper){
                Camper camper = temp.GetComponent<Camper>();
                but = (GameObject) Instantiate(buttonPrefab);
                but.transform.SetParent(transform, false);
                but.transform.localScale = new Vector3(1, 1, 1);

                Button tempButton = but.GetComponent<Button>();
                
                tempButton.onClick.AddListener(() => camper.Camp()); //camper unit function
            }

            //if the selected unit is a worker or a civilian
            if(tempUnit.classOfUnit== Unit.Class.civilian || tempUnit.classOfUnit== Unit.Class.worker){
                Civilian civ = temp.GetComponent<Civilian>();
                Improvement[] imp = PlayerController.Instance.player.kingdom.improvements;
                for(int i=0;i<imp.Length;i++){
                    but = (GameObject) Instantiate(buttonPrefab);
                    but.transform.SetParent(transform, false);
                    //but.transform.localScale = new Vector3(1, 1, 1);
                    but.GetComponentInChildren<Text>().text=imp[i].name;
                    int copy = i;
                    Button tempButton = but.GetComponent<Button>();
                    tempButton.onClick.AddListener(delegate{civ.startBuilding(copy);});//() => civ.startBuilding(copy)); //camper unit function
                } 
            }
        }
    }

    void OnDisable(){
        for(int i=0;i<transform.childCount;i++){
            Destroy(transform.GetChild(i).gameObject);
        }
    }


}
