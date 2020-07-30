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
    public void setPreRequisite(List<TechSkill> pre){
        preRequisites = pre;
        unlocked=false;
    }
    public virtual void UnlockSkill(int player = -1){
        //check if preRequisitesare met or not
        if(!checkPreRequisite()){
            Debug.Log("Not all preRequisites aquired yet");
            return;
        }

        //check if it is already unlocked or not
        if(unlocked){
            Debug.Log("Already Unlocked");
            return;
        }
        //check if have enough science to pay
        if(!cost.checkCost(player)){
            Debug.Log("Not enough Scienece to research this tech");
            cost.printCost();
            return;
        }
        

        Debug.Log(string.Format("{0} is unlocked",techCode));
        unlocked=true;
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

}
