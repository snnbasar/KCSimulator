using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PislikSingle : MonoBehaviour
{
    public bool karincaPislik;
    public KarincaHub karincaHub;

    

    public void RegisterKarincaHub(KarincaHub hub)
    {
        karincaPislik = true;
        karincaHub = hub;
        karincaHub.karincaPislikler.Add(this);
        karincaHub.currentPislik++;
    }

    private void OnMouseEnter()
    {
        OutlineController.instance.OutlineDo(this.gameObject);
    }

    private void OnMouseExit()
    {
        OutlineController.instance.OutlineReset(this.gameObject);
    }

    void OnMouseDown()
    {
        if (karincaPislik)
        {
            karincaHub.currentPislik--;
            karincaHub.karincaPislikler.Remove(this);
            Destroy(gameObject);
        }
    }

}
