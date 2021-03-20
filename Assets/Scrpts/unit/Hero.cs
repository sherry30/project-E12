using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Unit
{
    public GameObject relic;

    public override void takeDamage(int totalDamage)
    {
        if (armour != null)
        {
            float damage = totalDamage - armour.armour;
            armour.armour -= totalDamage;
            if (armour.armour < 0)
            {
                armour.armour = 0;
                currentHealth -= (int)damage;
            }
        }
        else
            currentHealth -= totalDamage;

        if (currentHealth <= 0) {
            //spawn relic on the location
            HexOperations.Instance.spawnRelic(this);
            HexOperations.Instance.removeUnit(this);
        }
        healthBar.updateUI();
    }
}
