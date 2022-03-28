using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yiyecek : MonoBehaviour
{
    public float besinDegeri;


    public void YeBeni()
    {
        YemekManager.instance.YemekYedim(besinDegeri);
        Destroy(gameObject);
    }
}
