using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class minimapCamCont : MonoBehaviour
{
    public static minimapCamCont Instance;
    void Awake(){
        if(Instance==null){
            Instance =this;
            CameraController.onCameraMove+=FixPos;
            return;
        }
        else{
            Destroy(gameObject);
        }
    }

    public static float constantPos = -57f;
    /*void Update()
    {
        
        transform.position = new Vector3(transform.position.x, transform.position.y, constantPos);
    }*/
    public static void FixPos(){
        Instance.transform.position = new Vector3(Instance.transform.position.x, Instance.transform.position.y, constantPos);
        
    }
}
