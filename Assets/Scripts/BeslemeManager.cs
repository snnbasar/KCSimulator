using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeslemeManager : MonoBehaviour
{
    public static BeslemeManager instance;

    public GameObject inspectedObje;


    private void Awake()
    {
        instance = this;
    }
    

    public void SetInpectedObje(GameObject obje)
    {
        inspectedObje = obje;
    }

    public void CheckForBesle(Besin besin)
    {
        if (!inspectedObje)
            return;
        if (inspectedObje.CompareTag("TestTube"))
        {
            KarincaHub karincaHub = inspectedObje.GetComponent<KarincaHub>();
            karincaHub.KarincaBesle(besin);
            print("Karincalar Beslendi");
        }
    }

}
