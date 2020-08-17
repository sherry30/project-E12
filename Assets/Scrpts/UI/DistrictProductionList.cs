using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistrictProductionList : MonoBehaviour
{
    public GameObject districtInfo;
    private City city;
    private Transform available;
    private Transform current;
    public int currentIndex=0,availableIndex=1;
    List<int> districtInd = new List<int>();
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

        int startIndex = city.districtStartIndex;
        if(districtInd==null)
            districtInd = new List<int>();//for producing

        //setting currentUnitInfo if theres a unit in production
        if(city!=null && city.districtProduction!=-1){
            //Debug.Log("ran");
            GameObject obj = (GameObject) Instantiate(districtInfo);
            obj.transform.SetParent(current, false);
            obj.transform.localScale = new Vector3(1, 1, 1);
            Transform disDesc = obj.transform.GetChild(0).GetChild(1);
            District dis = PlayerController.Instance.player.kingdom.districts[city.districtProduction];

            //changing image of Unit
            obj.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = dis.icon;

            //changing text on the UI
            disDesc.Find("DistrictName").GetComponent<Text>().text = dis.Name;
            //unitDesc.Find("DistrictClass").GetComponent<Text>().text = dis.classOfUnit.ToString();
        }

        //making all the available units to be produced    
        for(int i=0;i<city.numOfDistricts;i++){
            
            //instiating UnitInfo object
            GameObject obj = (GameObject) Instantiate(districtInfo);
            obj.transform.SetParent(available, false);
            obj.transform.localScale = new Vector3(1, 1, 1);
            District dis = PlayerController.Instance.player.kingdom.districts[startIndex+i];
            Transform disDesc = obj.transform.GetChild(0).GetChild(1);

            //changing image of Unit
            obj.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = dis.icon;
            

            //changing text on the UI
            disDesc.Find("DistrictName").GetComponent<Text>().text = dis.name;
            //unitDesc.Find("UnitClass").GetComponent<Text>().text = unit.classOfUnit.ToString();

            //subscribing the function with the button
            Button tempButton = obj.GetComponent<Button>();
            int copy = i;
            tempButton.onClick.AddListener(delegate {Produce(startIndex+copy,dis,obj);});//() => Produce(startIndex+i,unit));
            

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
    private void Produce(int index,District dis,GameObject objj){
        //if a unit or an item is already being produced then return
        if(city!=null && city.unitProduction==-1 && city.itemProduction==-1 && city.districtProduction==-1 && !city.positionSelectingMode){

            city.positionSelectingMode=true;
            //selectwhere it will be built first
            StartCoroutine(positionCheckSeq(index, dis,objj));
            
            
        }

    }

    private IEnumerator positionCheckSeq(int index,District dis,GameObject objj){

        //check if itsunlocked for thsi player
        if(!getPlayer().availableDistricts.Contains(dis.type)){
            Debug.Log("This district is not unlocked yet");
            Debug.Log(dis.source);
            yield break;
        }

        //checking cost
        if(!dis.cost.onlyCheckCost())
        {
            Debug.Log("Not enough Resources");
            dis.cost.printCost();
            yield break;
        }

        //start position selecting mode
        //couroutine ends when a new object is selected
        yield return StartCoroutine(MouseController.Instance.positionSelectingModeOn());
        city.positionSelectingMode=false;

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
            else if(!city.teritory.Contains(hex)){
                Debug.Log("This Hex is out od this cities teritotry");
                yield break;
            }
        }
        MouseController.Instance.clearPositionSelected();

        dis.cost.checkCost();
        city.ProduceDistrict(index,dis.daysToBeProduced,hex.location);
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
