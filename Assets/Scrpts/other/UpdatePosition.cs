using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePosition : MonoBehaviour
{
    void Awake(){
        offset = new Vector3(0,0,0);
        CameraController.onCameraMove+=updateLocationFromCamera;
    }
    public Vector2 location;
    public Vector3 offset;
    public bool moving = false; 
    public  void  updateLocationFromCamera(){
        if(!moving)
            this.transform.position = positionfromCamera(Camera.main.transform.position,HexMap.Instance.mapWidth,HexMap.Instance.mapHeight);
        //newPosition
    }
    private Vector3 positionfromCamera(Vector3 cameraPosition, float mapWidth, float mapHeight){
        HexComponent comp = HexMap.Instance.getHexComponent(location);
        float width = mapWidth*comp.hex.getWidth();
        //float height = mapHeight * HexVerticalSpacing();
        Vector3 pos= comp.hex.DiagonalPosition();
        float howManyWidthsFromCamera = (pos.x - cameraPosition.x)/width;
        //should be -0.5 to 0.5

        if(howManyWidthsFromCamera>0)
            howManyWidthsFromCamera += 0.5f;
        else
            howManyWidthsFromCamera -=0.5f;
        int howManyWidthsToFix = (int)howManyWidthsFromCamera;
        pos.x -= howManyWidthsToFix* width;
        //pos.y +=HexMap.Instance.campOffset;
        pos+=offset;
        return pos;
    }
}
