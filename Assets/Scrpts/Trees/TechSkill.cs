using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TechCode{

    P1,
    P2,
    P3,
    P4,
    P5,
    H1,
    E1,
    K1,
    K2,
    K3,
    R1,
    R2,
    F1,
    F2,
    F3,
    M1,
    M2,
    M3,
    A1,
    A2
}
public class TechSkill : MonoBehaviour{
    public TechCode techCode;
    public List<TechSkill> preRequisites;
    public bool unlocked = false;
    public string Name;
    public string description;
    public Cost cost;
    public int daysTillUnlock=-1;
    public int player = -1;
    public TechTree techTree;
    void Awake(){
        unlocked = false;
    }
    public void setPreRequisite(List<TechSkill> pre){
        preRequisites = pre;
        unlocked=false;
    }
    public virtual void UnlockSkill(int player = -1){
        this.player = player;
        if(unlocked){
            Debug.Log(string.Format("{0} is unlocked",techCode));
            return;
        }

        //check if preRequisitesare met or not
        if(!checkPreRequisite()){
            Debug.Log("Not all preRequisites aquired yet");
            for(int i=0;i<preRequisites.Count;i++){
                Debug.Log(preRequisites[i].techCode.ToString());
            }
            return;
        }

        //check if it is already unlocked or not
        if(techTree.underResearch){
            Debug.Log("Under Research");
            return;
        }
        //check if have enough science to pay
        if(!cost.checkCost(player)){
            Debug.Log("Not enough Scienece to research this tech");
            cost.printCost();
            return;
        }
        techTree.underResearch=true;
        daysTillUnlock=cost.spendScience(player);
        if(player==-1)
            PlayerController.Instance.player.onStartTurn+=StartTurn;
        else
            AIController.Instance.AIPlayers[player].onStartTurn+=StartTurn;
        Debug.Log(string.Format("days till research {0}",daysTillUnlock));
    }
    
    protected bool checkPreRequisite(){
        if(preRequisites ==null)
            return true;
        for(int i=0;i<preRequisites.Count;i++){
            if(!preRequisites[i].unlocked){
                return false;
            }
        }
        return true;
    }
    public virtual void StartTurn(){
        if(daysTillUnlock!=-1){
            daysTillUnlock--;
            if(daysTillUnlock==0){
                //unsubscribing to this metyhod whenever player turns start
                if(player==-1)
                    PlayerController.Instance.player.onStartTurn-=StartTurn;
                else
                    AIController.Instance.AIPlayers[player].onStartTurn-=StartTurn;

                unlocked=true;
                daysTillUnlock=-1;
                techTree.underResearch=false;
                UnlockSkill(player);
            }
        }
    }

}
