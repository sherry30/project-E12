using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{

    public void QuitGame()
    {
        Debug.Log("Closing Game...");   //Just for testing
        Application.Quit();
    }
}
