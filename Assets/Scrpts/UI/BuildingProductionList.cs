using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingProductionList : MonoBehaviour
{
    public GameObject buildingInfo;
    private District district;
    private Transform available;
    private Transform current;
    public int currentIndex=0,availableIndex=1;
    List<int> buildingInd = new List<int>();
    void Awake(){
        current = gameObject.transform.GetChild(currentIndex).GetChild(0);
        available = gameObject.transform.GetChild(availableIndex).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0);
        //available = gameObject.transform.Find("UnitFrame");
    }
    void OnEnable(){
        district =  GameState.Instance.selectedObject.GetComponent<District>();

        //checking if selected building is acityornot
        if(district==null)
            return;

        int startIndex = district.buildingStartingIndex;
        if(buildingInd==null)
            buildingInd = new List<int>();//for producing

        //setting currentUnitInfo if theres a unit in production
        if(district.buildingProduction!=-1){
            //Debug.Log("ran");
            GameObject obj = (GameObject) Instantiate(buildingInfo);
            obj.transform.SetParent(current, false);
            obj.transform.localScale = new Vector3(1, 1, 1);
            Transform buildingDesc = obj.transform.GetChild(0).GetChild(1);
            Building building = PlayerController.Instance.player.kingdom.buildings[district.buildingProduction];

            //changing image of Unit
            obj.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = building.icon;

            //changing text on the UI
            buildingDesc.Find("BuildingName").GetComponent<Text>().text = building.name;
            //unitDesc.Find("BuildingClass").GetComponent<Text>().text = building.classOfUnit.ToString();
        }

        //making all the available units to be produced    
        for(int i=0;i<district.numOfBuildings;i++){
            
            //instiating UnitInfo object
            GameObject obj = (GameObject) Instantiate(buildingInfo);
            obj.transform.SetParent(available, false);
            obj.transform.localScale = new Vector3(1, 1, 1);
            Building building = PlayerController.Instance.player.kingdom.buildings[startIndex+i];
            Transform buildingDesc = obj.transform.GetChild(0).GetChild(1);
            

            //changing image of Unit
            obj.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = building.icon;
            

            //changing text on the UI
            buildingDesc.Find("BuildingName").GetComponent<Text>().text = building.name;
            //buildingDesc.Find("UnitClass").GetComponent<Text>().text = unit.classOfUnit.ToString();

            //subscribing the function with the button
            Button tempButton = obj.GetComponent<Button>();
            int copy = i;
            tempButton.onClick.AddListener(delegate {Produce(startIndex+copy,building,obj);});//() => Produce(startIndex+i,unit));
            

        }
    }
    void OnDisable(){
        for(int i=0;i<available.childCount;i++){
            Destroy(available.GetChild(i).gameObject);
        }
        if(district!=null && current.childCount>0)   
            Destroy(current.GetChild(0).gameObject);
        district =null;
        
    }
    private void Produce(int index,Building bil,GameObject objj){
        //if a unit or an item is already being produced then return
        if(district!=null && district.buildingProduction==-1 && !district.positionSelectingMode){

            //selectwhere it will be built first
            StartCoroutine(positionCheckSeq(index, bil,objj));
            
            
        }

    }

    private IEnumerator positionCheckSeq(int index,Building bil,GameObject objj){

        //check if itsunlocked for thsi player

        //uncomment when setting up availableBuildings in player
        /*if(!getPlayer().availableDistricts.Contains(dis.type)){
            Debug.Log("This district is not unlocked yet");
            Debug.Log(dis.source);
            yield break;
        }*/

        //checking cost
        if(!bil.cost.onlyCheckCost())
        {
            Debug.Log("Not enough Resources");
            bil.cost.printCost();
            yield break;
        }

        //start position selecting mode
        //couroutine ends when a new object is selected
        district.positionSelectingMode=true;
        yield return StartCoroutine(MouseController.Instance.positionSelectingModeOn());
        district.positionSelectingMode=false;

        GameObject sel = MouseController.Instance.positionSelected;
        HexComponent hex= sel.GetComponent<HexComponent>();
        
        if(sel.tag!="Hex"){
            Debug.Log("Selected object is not a hex");
            yield break;
        }
        else{
            if(GameState.Instance.HexOccupiedCheck(hex.location)){
                Debug.Log("This Hex is occupied");
                yield break;
            }
            else if(!district.city.teritory.Contains(hex)){
                Debug.Log("This Hex is out od this cities teritotry");
                yield break;
            }
        }
        MouseController.Instance.clearPositionSelected();

        bil.cost.checkCost();
        int days = bil.cost.spendProduction();
        district.ProduceBuilding(index,days,hex.location);
        GameObject obj = (GameObject) Instantiate(objj);
        obj.transform.SetParent(current, false);
        obj.transform.localScale = new Vector3(1, 1, 1);
        Transform unitDesc = obj.transform.GetChild(0).GetChild(1);

    }
    private Player getPlayer(int player = -1){
        Player tempPlayer = PlayerController.Instance.player;
        if(player!=-1)
            tempPlayer = AIController.Instance.AIPlayers[player];
        
        return tempPlayer;
    }
}
