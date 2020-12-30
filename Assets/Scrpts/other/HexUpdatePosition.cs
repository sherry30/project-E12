using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexUpdatePosition : UpdatePosition
{
    private int counter=100;
    private int count=101;
   public  override void  updateLocationFromCamera(){
        if(count<counter){
            count++;
            return;
        }
        count=0;
        base.updateLocationFromCamera();
    }
}
