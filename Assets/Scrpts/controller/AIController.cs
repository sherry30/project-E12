using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public int NumberOfAIs;
    public List<Player> AIPlayers;
    public List<Energy> energies;
    public static AIController Instance;
    private List<HexComponent> travelPath = new List<HexComponent>();
    private PathFinding pathFinder;

    void Awake(){
        pathFinder = new PathFinding();
        removeKingdoms();
        foreach(Player p in AIPlayers)
        {
            p.setVariables();
        }
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

        


        if (energies.Contains(Energy.Fire)){
            //adding a new AIPlayer
            AIPlayers.Add(new Player());
            //settin gits player count
            AIPlayers[AIPlayers.Count - 1].player = AIPlayers.Count - 1;
            //setting its kingdom
            AIPlayers[AIPlayers.Count-1].kingdom = gameObject.GetComponent<FireKingdom>();


        }

        else
            Destroy(fire);

        if(energies.Contains(Energy.Water)){
            //adding a new AIPlayer
            AIPlayers.Add(new Player());
            //settin gits player count
            AIPlayers[AIPlayers.Count - 1].player = AIPlayers.Count - 1;

            AIPlayers[AIPlayers.Count-1].kingdom = gameObject.GetComponent<WaterKingdom>();

            
        }
        else
            Destroy(water);

        if(energies.Contains(Energy.Air)){
            //adding a new AIPlayer
            AIPlayers.Add(new Player());
            //settin gits player count
            AIPlayers[AIPlayers.Count - 1].player = AIPlayers.Count - 1;

            AIPlayers[AIPlayers.Count-1].kingdom = gameObject.GetComponent<AirKingdom>();
            
        }
        else
            Destroy(air);

        if(energies.Contains(Energy.Earth)){
            //adding a new AIPlayer
            AIPlayers.Add(new Player());
            //settin gits player count
            AIPlayers[AIPlayers.Count - 1].player = AIPlayers.Count - 1;

            AIPlayers[AIPlayers.Count-1].kingdom = gameObject.GetComponent<EarthKingdom>();
            
        }
        else
            Destroy(earth);

        if(energies.Contains(Energy.Dark)){
            //adding a new AIPlayer
            AIPlayers.Add(new Player());
            //settin gits player count
            AIPlayers[AIPlayers.Count - 1].player = AIPlayers.Count - 1;
            AIPlayers[AIPlayers.Count-1].kingdom = gameObject.GetComponent<DarkKingdom>();
            
        }
        else
            Destroy(dark);

        if(energies.Contains(Energy.Light)){
            //adding a new AIPlayer
            AIPlayers.Add(new Player());
            //settin gits player count
            AIPlayers[AIPlayers.Count - 1].player = AIPlayers.Count - 1;
            AIPlayers[AIPlayers.Count-1].kingdom = gameObject.GetComponent<LightKingdom>();
            
        }
        else
            Destroy(light);


        
        
        //destroying other kingdoms
    }

    public void StartTurn()
    {
        foreach(Player p in AIPlayers)
        {
            p.StartTurn();

            //Basic AI, Moves randomly

            foreach(Unit u in p.units)
            {
                HexComponent[] temp = HexOperations.Instance.getNeighbors(u.location, u.movement);

                HexComponent dest = temp[Random.Range(0, temp.Length - 1)];

                //has to move somewhere
                while (dest == HexMap.Instance.getHexComponent(u.location))
                {
                    dest = temp[Random.Range(0, temp.Length - 1)];
                }


                travelPath = pathFinder.shortesPath(HexMap.Instance.getHexComponent(u.location), dest);
                u.moveUnitFast(travelPath);

            }
        }
    }
}
