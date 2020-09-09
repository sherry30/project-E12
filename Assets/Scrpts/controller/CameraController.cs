using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public GameObject mouseOverObject;
    RaycastHit theObject;
    public float PanSpeed = 20f;
    public float BoarderThickness=10f;

    public LayerMask layermask=10;

    public float scrollSpeed=20f;
    public  bool adjusted=false;
    public delegate void cameraMoveDelegate();
    public static cameraMoveDelegate onCameraMove;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
        //move camera with mouse
        Vector3 pos= transform.position;
        bool changed = false;

        if(Input.mousePosition.y>= (Screen.height - BoarderThickness)){
            pos.z+=PanSpeed *  Time.deltaTime;
            changed = true;
            
        };

        if(Input.mousePosition.y<= BoarderThickness){
            pos.z-=PanSpeed *  Time.deltaTime;
            changed = true;
        };

        if(Input.mousePosition.x>= (Screen.width - BoarderThickness)){
            pos.x+=PanSpeed *  Time.deltaTime;
            changed = true;
        };

        if(Input.mousePosition.x<= BoarderThickness){
            pos.x-=PanSpeed *  Time.deltaTime;
            changed = true;
        };
    //return if mouse is over UI or if a unit is moving
        if(!EventSystem.current.IsPointerOverGameObject()){
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            pos.y -= scroll* 100f * scrollSpeed *Time.deltaTime;
        }


        //updating everything
        if(adjusted){
            transform.position = pos;
            if(changed)
                onCameraMove();
        }


    }
}
