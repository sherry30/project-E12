using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUI : MonoBehaviour
{

    public GameObject joinArmyPanel;
    public void createArmy()
    {
        //getting unit
        GameObject currentlySelected = GameState.Instance.selectedObject;

        Unit unit = currentlySelected.GetComponent<Unit>();

        HexComponent hexComp = HexMap.Instance.getHexComponent(unit.location);

        if (hexComp.hasArmy())
        {
            joinArmyPanel.SetActive(true);
            return;
        }
        //creating army in the player object
        PlayerController.Instance.player.createArmy(hexComp.units);
        UIController.Instance.deSelectObject();
        UIController.Instance.openArmyHub();
        

    }

    public void joinArmy()
    {
        GameObject currentlySelected = GameState.Instance.selectedObject;

        Unit unit = currentlySelected.GetComponent<Unit>();

        HexComponent hexComp = HexMap.Instance.getHexComponent(unit.location);

        PlayerController.Instance.player.addUnitToArmy(unit, hexComp.army);

        joinArmyPanel.SetActive(false);
    }
}
