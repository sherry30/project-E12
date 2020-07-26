using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [HideInInspector]
    public int maxHealth;
    [HideInInspector]
    public int currentHealth;

    private Unit unit;
    private GameObject healthBarUI;
    private Image image;
    private Building building;
    

    private void Awake()
    {
        unit = gameObject.GetComponent<Unit>();
        if(unit!=null)
            maxHealth = unit.maxHealth;
        else{
            building = GetComponent<Building>();
            maxHealth = building.maxHealth;
        }
        healthBarUI = transform.Find("HealthBarUI").gameObject;
        image = healthBarUI.transform.GetChild(0).GetComponent<Image>();
        currentHealth = maxHealth;

        //hiding health bar at the start 
        hideHealth();

    }
    public void updateUI(){
        image.fillAmount = calculateHealth();
    }
    public void displayHealth(){
        healthBarUI.SetActive(true);
    }
    public void hideHealth(){
        healthBarUI.SetActive(false);
    }
    private float calculateHealth()
    {
        return currentHealth / maxHealth;
    }
    
}