using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    public static OutlineController instance;

    private void Awake()
    {
        instance = this;
    }

    public void OutlineDo(GameObject objToOutline)
    {
        Outline outline;
        if (objToOutline.GetComponent<Outline>())
            outline = objToOutline.GetComponent<Outline>();
        else
            outline = objToOutline.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 5f;
    }

    public void OutlineReset(GameObject objToOutline)
    {

        if (!objToOutline)
            return;

        if(objToOutline.TryGetComponent<Outline>(out Outline outline))
            outline.OutlineWidth = 0f;
    }
}
