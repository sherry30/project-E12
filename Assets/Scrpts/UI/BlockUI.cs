using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockUI : MonoBehaviour,IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data){
        Debug.Log(gameObject.name + ": I was clicked!");
    }
}
