using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public int NumberOfAIs;
    public List<Player> AIPlayers;
    public List<Energy> energies;
    public static AIController Instance;
    
    void Awake(){
        removeKingdoms();
        if(Instance==null){
            Instance =this;
            return;
        }
        else{
            Destroy(gameObject);
        }
    }

    private void removeKingdoms(){
        AirKingdom air = this.GetComponent<AirKingdom>();
        WaterKingdom water = this.GetComponent<WaterKingdom>();
        EarthKingdom earth = this.GetComponent<EarthKingdom>();
        FireKingdom fire = this.GetComponent<FireKingdom>();
        DarkKingdom dark = this.GetComponent<DarkKingdom>();
        LightKingdom light = this.GetComponent<LightKingdom>();
        if(AIPlayers==null)
            AIPlayers = new List<Player>();
        if(energies.Contains(Energy.Fire)){
            //adding a new AIPlayer
            AIPlayers.Add(new Player());
            //setting its kingdom
            AIPlayers[AIPlayers.Count-1].kingdom = gameObject.GetComponent<FireKingdom>();


        }

        else
            Destroy(fire);

        if(energies.Contains(Energy.Water)){
            AIPlayers.Add(new Player());
            AIPlayers[AIPlayers.Count-1].kingdom = gameObject.GetComponent<WaterKingdom>();

            
        }
        else
            Destroy(water);

        if(energies.Contains(Energy.Air)){
            AIPlayers.Add(new Player());
            AIPlayers[AIPlayers.Count-1].kingdom = gameObject.GetComponent<AirKingdom>();
            
        }
        else
            Destroy(air);

        if(energies.Contains(Energy.Earth)){
            AIPlayers.Add(new Player());
            AIPlayers[AIPlayers.Count-1].kingdom = gameObject.GetComponent<EarthKingdom>();
            
        }
        else
            Destroy(earth);

        if(energies.Contains(Energy.Dark)){
            AIPlayers.Add(new Player());
            AIPlayers[AIPlayers.Count-1].kingdom = gameObject.GetComponent<DarkKingdom>();
            
        }
        else
            Destroy(dark);

        if(energies.Contains(Energy.Light)){
            AIPlayers.Add(new Player());
            AIPlayers[AIPlayers.Count-1].kingdom = gameObject.GetComponent<LightKingdom>();
            
        }
        else
            Destroy(light);


        //destroying other kingdoms
    }
}
