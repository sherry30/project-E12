using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : MonoBehaviour
{
    void Start(){
        oldPosition = newPosition = this.transform.position;
        unit = GetComponent<Unit>();
    }
    private Unit unit;
    private Vector3 oldPosition;
    private Vector3 newPosition;
    private Vector3 currentVelocity;
    public bool cameraMoving=false;
    private float smoothTime=0.5f;
    public float thresholdForDist=50f;

    public IEnumerator onUnitMove(HexComponent oldHex, HexComponent newHex){
        oldPosition = oldHex.hex.positionFromCamera();
        newPosition = newHex.hex.positionFromCamera();
        newPosition.y += unit.offset.y;
        if(newHex.getElevation()==oldHex.getElevation()){

        }
        //Debug.Log(string.Format("old: {0}, new{1} ", oldPosition,newPosition));
        while(Vector3.Distance(this.transform.position,newPosition)>0.1f){
            if(Vector3.Distance(this.transform.position,newPosition)>thresholdForDist)
                this.transform.position = newPosition;
            this.transform.position = Vector3.SmoothDamp(this.transform.position,newPosition,ref currentVelocity,smoothTime);
            yield return null;
        }
    }
    /*void Update(){
        if(moving){
            this.transform.position = Vector3.SmoothDamp(this.transform.position,newPosition,ref currentVelocity,smoothTime);
            if(this.transform.position==newPosition)
                moving =false;
        }
    }*/
    /*public void  updateLocationFromCamera(){
        if(moving==false)
            this.transform.position = positionfromCamera(Camera.main.transform.position,HexMap.Instance.mapWidth,HexMap.Instance.mapHeight);
        //newPosition
    }
    private Vector3 positionfromCamera(Vector3 cameraPosition, float mapWidth, float mapHeight){
        Unit temp = gameObject.GetComponent<Unit>();
        HexComponent comp = HexMap.Instance.getHexComponent(temp.location);
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
        pos.y +=HexMap.Instance.campOffset;
        return pos;
    }*/
}
