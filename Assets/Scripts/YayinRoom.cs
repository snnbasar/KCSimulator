using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class YayinRoom : MonoBehaviour
{
    public List<Transform> yayinlanacakHayvanlar = new List<Transform>();

    public Transform Camera;
    public float changeCameraTime;

    public Vector3 offset;

    public Ease ease;

    private void Start()
    {
        InvokeRepeating("ChangeCameraView", changeCameraTime, changeCameraTime);
    }


    public void ChangeCameraView()
    {
        if (yayinlanacakHayvanlar.Count <= 0)
            return;
        Transform rndmHayvan = yayinlanacakHayvanlar[Random.Range(0, yayinlanacakHayvanlar.Count)];
        FokusOnAnimal(rndmHayvan);
    }

    public void FokusOnAnimal(Transform animal)
    {

        Camera.DOMove(animal.position + offset, 1f).SetEase(ease).OnComplete(() => 
        {
            Camera.DOLookAt(animal.position, 1f);
        });

        //Camera.position = animal.position + offset;
        //Camera.LookAt(animal.position, Vector3.up);
    }

    public void RegisterKarincaHub(KarincaHub hub)
    {
        yayinlanacakHayvanlar.Add(hub.transform);
        FokusOnAnimal(hub.transform);
    }

    public void UnRegisterKarincaHub(KarincaHub hub)
    {
        yayinlanacakHayvanlar.Remove(hub.transform);
        ChangeCameraView();
    }
}
