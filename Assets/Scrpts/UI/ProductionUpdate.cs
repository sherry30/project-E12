using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionUpdate : MonoBehaviour
{
    public Player player;
    public Text productionText;
    void Start(){
        player = PlayerController.Instance.player;
    }
    void Update()
    {
        productionText.text = string.Format("Production: {0}" ,player.resources.resources[Resource.production]);
    }
}
