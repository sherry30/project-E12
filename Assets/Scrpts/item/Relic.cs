using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic : MonoBehaviour
{

    public int player = -1;
    public void spawnRelic(Unit unit)
    {
        player = unit.player;
    }
}
