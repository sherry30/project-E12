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

        if (unit.isInArmy())
        {
            Debug.Log("This unit is already in army");
            return;
        }

        //gettin ghex comp
        HexComponent hexComp = HexMap.Instance.getHexComponent(unit.location);

        if (hexComp.units.Count <= 1)
        {
            Debug.Log("this unit is alone in this hex");
            return;
        }

        if (hexComp.hasArmy())
        {
            joinArmyPanel.SetActive(true);
            return;
        }

        
        //creating army in the player object
        PlayerController.Instance.player.createArmy(hexComp.units);

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
