using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1TechSkill : TechSkill
{
    void Awake(){
        Name = "Crafting";
        techCode = TechCode.P1;
        description = "unlocks the ability to craft items at a camp, and for Civillians to build the 'Animal Trap' Improvement. ";
    }
    public override void UnlockSkill(int player = -1){
        base.UnlockSkill(player);
        bool avail = checkPreRequisite();
        if(!avail)
            return;
        if(player==-1)
            PlayerController.Instance.player.availableImprovements.Add(improvement.animal_Trap);
        return;

    }
}
