using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GameState : MonoBehaviour
{
    public enum Turn{
        Player,
        NPC,
        AI    
    }    
    public static GameState Instance {get; private set;}
    public int currentTurn=0;
    public GameObject selectedObject;
    public GameObject mouseOverObject;

    public List<Vector2> occupiedHexes;

    public Kingdom[] kingdoms;

    public int turn=0;
    public Turn currentPlayerTurn;
    private Turn[] turnOrder={Turn.Player,Turn.NPC};//current turn order
    public GameObject NextTurnButton;
    public bool moving=false;
    
    //TODO: weather

    // Start is called before the first frame update
    private void Awake(){
        currentPlayerTurn = turnOrder[turn];
        //button for next turn
        Button but = NextTurnButton.GetComponent<Button>();
        but.onClick.AddListener(() => onEndTurn());

        if(Instance==null){
            Instance =this;
            return;
        }
        else{
            Destroy(gameObject);
        }
    }
    
    public void setOccupiedHexes(Vector2 location){
        occupiedHexes.Add(location);
    }
    public void HexOccupiedCheck(ref Vector2 location){
        //check if its occupied 
        //if it is then randomly select another
        Vector2 loc = new Vector2(location.x,location.y);
        bool selected = true;
        int checking =0;
        while(selected){
            bool notChanged = true;
            for(int i=0;i<occupiedHexes.Count;i++){
                if(occupiedHexes[i]==loc){
                    //repeat becuase occupied
                    selected = true;
                    notChanged = false;
                }
            }
            if(notChanged)
                selected=false;
            else{
                location = new Vector2
                (UnityEngine.Random.Range(0,HexMap.Instance.mapWidth)
                ,UnityEngine.Random.Range(0,HexMap.Instance.mapHeight));
            }
            checking++;
            if(checking>=1000)
                return;
        }
    }

    public bool HexOccupiedCheck(Vector2 location){
        return occupiedHexes.Contains(location);
    }
    public void LandHexOccupiedCheck(ref Vector2 location,List<HexComponent> LandTiles){
        //check if its occupied 
        //if it is then randomly select another from the landtiles list
        Vector2 loc = new Vector2(location.x,location.y);
        bool selected = true;
        int checking =0;
        while(selected){
            bool notChanged = true;
            for(int i=0;i<occupiedHexes.Count;i++){
                if(occupiedHexes[i]==loc){
                    //repeat becuase occupied
                    selected = true;
                    notChanged = false;
                }
            }
            if(notChanged)
                selected=false;
            else{
                HexComponent campTile = LandTiles[UnityEngine.Random.Range(0,LandTiles.Count)];
                location = campTile.location;
            }
            checking++;
            if(checking>=1000)
                return;
        }
    }

    
    public void onEndTurn(){
        //incrementing turn only if its the last player turn ended in turn order
        if(currentPlayerTurn==turnOrder[turnOrder.Length-1])
            currentTurn++;
        
        //settibng turn currentTurnPlayer
        turn= (turn+1)%turnOrder.Length;
        currentPlayerTurn = turnOrder[turn];

        //executing onStartMethod of the current TurnPlayer an onEndTurn for Npc and AIs to instantly skip turn
        if(currentPlayerTurn==Turn.Player)
            //onStartTurn();
            PlayerController.Instance.StartTurn();
        else if(currentPlayerTurn == Turn.NPC){
            NPCController.Instance.StartTurn();
            onEndTurn();
        }

    }
    
    public bool checkPlayerTurn(){
        return currentPlayerTurn==Turn.Player;
    }

    public void deSelectObject(){
        selectedObject = null;
        UIController.Instance.deSelectObject();
    }

}
