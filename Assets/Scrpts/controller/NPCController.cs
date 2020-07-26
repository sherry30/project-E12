using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    void Awake(){
        if(Instance==null){
            Instance =this;
            return;
        }
        else{
            Destroy(gameObject);
        }
    }
    public int numOfShadows=3;
    public GameObject shadowPrefab;
    public Shadow[] shadows;
    public static NPCController Instance;
    public void spawnShadows(){
        //instantiating shadow
        shadows = new Shadow[numOfShadows];
        for(int i=0;i<numOfShadows;i++){
            /*Vector2 location = new Vector2(Random.Range(0,mapWidth),Random.Range(0,mapHeight));
            GameState.Instance.HexOccupiedCheck(ref location);
            HexComponent hex = hexes[(int)location.x,(int)location.y];
            Vector3 place = hex.hex.positionFromCamera();
            GameObject obj = (GameObject)Instantiate(
                shadowPrefab,
                new Vector3(place.x,0,place.z),
                Quaternion.identity,
                this.transform         
            );
            shadows[i] = obj.GetComponent<Shadow>();
            shadows[i].spawnUnit(location);
            //setting ya xis for shadows
            Vector3 pos = obj.transform.position;
            obj.transform.position = new Vector3(pos.x,pos.y+shadows[i].offset.y,pos.z);
            //subscribing for moving
            shadows[i].onUnitMove += obj.GetComponent<UnitMove>().onUnitMove;
            //adding enemy to thw hexCompoenty
            hexes[(int)location.x,(int)location.y].addEnemy(shadows[i]);
            //TODO put these locations in Gamestate occupied locations  
            GameState.Instance.setOccupiedHexes(location);*/
            shadows[i]= HexOperations.Instance.spawnShadow(shadowPrefab);


        }
    }
    public void StartTurn(){
        //Debug.Log("NPC turn Sarted");
        Task t = new Task(moveShadow());
            t.Finished+=delegate (bool manual){
                GameState.Instance.onEndTurn();
            };
    }
    private IEnumerator moveShadow(){
        for(int i=0;i<shadows.Length;i++){
            HexComponent[] hex = HexOperations.Instance.getClosestNeighbours(shadows[i].location);
            int j=0;
            bool blocked= false;
            List<HexComponent> oneHex = new List<HexComponent>();
            oneHex.Add(hex[Random.Range(0,6)]);
            while(!oneHex[0].isEmpty()){
                oneHex.RemoveAt(0);
                oneHex.Add(hex[Random.Range(0,6)]);
                if(j>12){
                    blocked = true;
                    break;
                }
            }
            if(blocked)
                continue;
            
            yield return StartCoroutine(shadows[i].moveUnit(oneHex,true));
        }
    }

}
