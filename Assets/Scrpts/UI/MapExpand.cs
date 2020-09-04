using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapExpand : MonoBehaviour
{
    public RectTransform zoom;
    public Button mapExpandButton;

    void Awake()
    {
        mapExpandButton.onClick.AddListener(ExpandMap);
    }

    //Handle the onClick event
    public void ExpandMap()
    {
        zoom.position = new Vector3(299f, 295f, 0f);
        zoom.sizeDelta = new Vector2(1980, 1745);
        Debug.Log("Expanding Map...");
    }
    
}
