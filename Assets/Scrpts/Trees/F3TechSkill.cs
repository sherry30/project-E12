using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F3TechSkill : TechSkill
{
    public override void UnlockSkill(int player = -1){
        base.UnlockSkill(player);
        if(unlocked){
            Player tempPlayer = PlayerController.Instance.player;
            //checking if energy cost is met
            if(player!=-1){
                tempPlayer = AIController.Instance.AIPlayers[player];
            }
            tempPlayer.cities[0].upgradeToVillage(tempPlayer);
        }

    }
}
