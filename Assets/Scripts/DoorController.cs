using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorController : MonoBehaviour
{
    public Transform pivot;
    public bool kapiStatus;
    [SerializeField] private float duration;
    [SerializeField] private Vector3 openVector;
    [SerializeField] private Ease ease;

    public bool outOfUse;

    private void Start()
    {
        outOfUse = false;
        kapiStatus = false;
    }


    private void OnTriggerStay(Collider other)
    {
        if (!outOfUse && Input.GetKeyDown(KeyCode.F) && other.CompareTag("Player"))
            DoAction();
    }

    public void DoAction()
    {
        if (!kapiStatus)
            OpenDoor();
        if (kapiStatus)
            CloseDoor();
    }

    private void CloseDoor()
    {
        outOfUse = true;
        SoundManager.instance.PlaySound(Soundlar.DoorClose);
        pivot.DOLocalRotate(Vector3.zero, duration).SetEase(ease).OnComplete(() => { 
            outOfUse = false;
            kapiStatus = false;
        });
    }

    private void OpenDoor()
    {
        outOfUse = true;
        SoundManager.instance.PlaySound(Soundlar.DoorOpen);
        pivot.DOLocalRotate(openVector, duration).SetEase(ease).OnComplete(() => {
            outOfUse = false;
            kapiStatus = true;
        }); ;
    }
}
