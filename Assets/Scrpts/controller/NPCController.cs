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
    public int numOfAnimals=3;
    public GameObject shadowPrefab;
    public GameObject animalPrefab;
    public Shadow[] shadows;
    public Animal[] animals;
    public static NPCController Instance;
    public void spawnShadows(){
        //instantiating shadow
        shadows = new Shadow[numOfShadows];
        animals = new Animal[numOfAnimals];
        for(int i=0;i<numOfShadows;i++){
            shadows[i]= HexOperations.Instance.spawnShadow(shadowPrefab);
        }
        for(int i=0;i<numOfAnimals;i++){
            animals[i]= HexOperations.Instance.spawnAnimal(animalPrefab);
        }
    }
    public void StartTurn(){
        //Debug.Log("NPC turn Sarted");
        /*Task t = new Task(moveShadow());
            t.Finished+=delegate (bool manual){
                //GameState.Instance.onEndTurn();
            };*/
        moveShadowsFast();
        moveAnimalsFast();
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
            
            yield return StartCoroutine(shadows[i].moveUnit(oneHex)); 
        }
    }

    private void moveShadowsFast(){
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
            
            shadows[i].moveUnitFast(oneHex); 
        }
    }

    private void moveAnimalsFast(){
        for(int i=0;i<animals.Length;i++){
            HexComponent[] hex = HexOperations.Instance.getClosestNeighbours(animals[i].location);
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
            
            animals[i].moveUnitFast(oneHex); 
        }
    }
    

}
