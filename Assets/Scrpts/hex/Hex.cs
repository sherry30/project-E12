using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex 
{
    //Q +R +S = 0
    //S =-(Q+R)
    public Hex(int q, int r,float radius = 4f){
        this.Q = q;
        this.R = r;
        this.S = -(q+r);
        this.radius = radius;

    }
    //x
    public int Q;
    public static float hexHeight=1f;
    //y
    public  int R;
    public  int S;
    public  float radius;
    static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3)/2;

    public void setQandR(int q, int r,float radius = 2f){
        this.Q = q;
        this.R = r;
        this.S = -(q+r);
        this.radius = radius;
    }
    /*public Vector2[] getLayer(int layer){
            int size = layer*6;
            for(int i=0;i<6;i++){
                for(int j=0;j<layer;j++){

                }
           }

    }*/

    public Vector2 cube_to_oddr(int x,int z){
        int col = x + (z + (z&1)) / 2;
        int row = z;
        return new Vector2(col, row);
    }
    public Vector2 oddr_to_cube(Hex hex){
        int x = hex.Q - (hex.R + (hex.R&1)) / 2;
        int z = hex.R;
        int y = -x-z;
        return new Vector2(x, y);
    }
    //get distance between 2 hexes
    public int Distance(Hex hex){
        return Mathf.Max(Mathf.Abs(this.Q - hex.Q),Mathf.Abs(this.R - hex.R),Mathf.Abs(this.S - hex.S));
    }
    public float getHeight(){
        return this.radius*2;
    }
    public float getWidth(){
        return WIDTH_MULTIPLIER* getHeight();
    }

    public Vector3 Position(){
        
        float width = getWidth();

        float hori = width;
        float vert = getHeight()*0.75f;
        //int row=0;
        //if(R%2==0)
            //row= 1;

       //- sign indicates moving downwards
        return new Vector3(
            hori *(Q + ((R%2))/2f),
            0,
            -vert * R
        );
    }
    public Vector3 DiagonalPosition(){
        
        float width = getWidth();

        float hori = width;
        float vert = getHeight()*0.75f;

       //- sign indicates moving downwards
        return new Vector3(
            hori *(Q + (R/2f)),
            0,
            -vert * R
        );
    }
    public float HexVerticalSpacing(){
        return getHeight() *0.75f;
    }
    public Vector3 positionFromCamera(){
        return HexOperations.Instance.positionFromCamera(Q,R);
    }
    public Vector3 positionFromCamera(Vector3 cameraPosition, float mapWidth, float mapHeight,Vector3? offset=null){
        //default offset value
        

        float width = mapWidth*getWidth();
        //float height = mapHeight * HexVerticalSpacing();
        Vector3 pos= DiagonalPosition();
        float howManyWidthsFromCamera = (pos.x - cameraPosition.x)/width;
        //should be -0.5 to 0.5

        if(howManyWidthsFromCamera>0)
            howManyWidthsFromCamera += 0.5f;
        else
            howManyWidthsFromCamera -=0.5f;
        int howManyWidthsToFix = (int)howManyWidthsFromCamera;
        pos.x -= howManyWidthsToFix* width;
        return pos; 

    }

}
