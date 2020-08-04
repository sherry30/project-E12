using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F3TechSkill : TechSkill
{
    public override void UnlockSkill(int player = -1){
        base.UnlockSkill(player);
        if(unlocked){
            if(player==-1){
                PlayerController.Instance.player.availableImprovements.Add(improvement.Farm);
                PlayerController.Instance.player.cities[0].typeOfCity = City.Type.village;
            }
            else{
                AIController.Instance.AIPlayers[player].availableImprovements.Add(improvement.Farm);
                AIController.Instance.AIPlayers[player].cities[0].typeOfCity = City.Type.village;
            }
        }

    }
}
