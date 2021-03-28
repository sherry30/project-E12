using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{

    public static Utility Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    public Player getPlayer(int player)
    {
        Player tempPlayer = PlayerController.Instance.player;
        if (player != -1)
            tempPlayer = AIController.Instance.AIPlayers[player];

        return tempPlayer;
    }
}
