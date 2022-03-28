using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableController : MonoBehaviour
{
    public static DraggableController instance;

    DragObject dragObject;
    private void Awake()
    {
        instance = this;
    }


    public void DoAction(GameObject obje)
    {
        if(obje.GetComponent<DragObject>())
            dragObject = obje.GetComponent<DragObject>();
        else
            dragObject = obje.AddComponent<DragObject>();
        dragObject.canMove = true;

    }

    public void CancelAction()
    {
        dragObject.canMove = false;
    }

}
