using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    RaycastHit theObject;
    private RectTransform rect;
    Vector2 lastMousePos;
    private void Awake(){
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        getCanvas(gameObject);
        
    }
    public void OnPointerDown(PointerEventData data){
    }
    public void OnDrag(PointerEventData data){
        rect.anchoredPosition += data.delta/canvas.scaleFactor;

         //raycast for checking if its over a unit
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayLength= (mouseRay.origin.y/mouseRay.direction.y);

        Vector3 hitPos = mouseRay.origin - (mouseRay.direction*rayLength*1000f);
        
        Debug.DrawRay(Camera.main.transform.position, hitPos, Color.white);
        
        /*Vector2 curretnMousePos = data.position;
        Vector2 diff = curretnMousePos-lastMousePos;
        Vector3 newPosition = rect.position+new Vector3(diff.x,diff.y,transform.position.z);
        rect.position = newPosition;///canvas.scaleFactor;
        lastMousePos = curretnMousePos;*/

    }
    public void OnEndDrag(PointerEventData data){
        rect.anchoredPosition = lastMousePos;

        //raycast for checking if its over a unit
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayLength= (mouseRay.origin.y/mouseRay.direction.y);

        Vector3 hitPos = mouseRay.origin - (mouseRay.direction*rayLength*1000f);
        GameObject hitObj;
        if(Physics.Raycast(Camera.main.transform.position,hitPos,out theObject,Mathf.Infinity)){
            hitObj = theObject.transform.parent.gameObject;


            if(hitObj.tag=="Unit"){   
                Unit unit = hitObj.GetComponent<Unit>();
                if(unit.player!=-1)
                    return;
                //its over a unit 
                
                Item item = GetComponent<Item>();
                /*Item temp = CopyComponent<Item>(item,hitObj);
                temp.equiped=true;
                temp.building=null;*/
                //item.equiped=true;
                unit.equipItem(item.building.getItem(item));
                item.building.removeItem(gameObject);
                item.building=null;

                Destroy(transform.parent.gameObject);
            }
        }
        canvasGroup.blocksRaycasts=true;
        canvasGroup.alpha=1f;

    }
    public void OnBeginDrag(PointerEventData data){
        lastMousePos = rect.anchoredPosition;
        getCanvas(gameObject);
        canvasGroup.blocksRaycasts=false;
        canvasGroup.alpha=0.8f;
    }
    private void getCanvas(GameObject g){
        if(g.GetComponent<Canvas>()!=null) {
            canvas=g.GetComponent<Canvas>();
            return;
        }else{
            if(g.transform.parent!=null){
                getCanvas (g.transform.parent.gameObject);
            }
        }
    }
}
