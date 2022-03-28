using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KarincaSingle : MonoBehaviour
{
    public Animator anim;
    [HideInInspector] public float karincaSpeed;
    public KarincaHub karincaHub;
    public float karincaDestroyTime;

    public bool isKralice;
    private void Start()
    {
        
    }
    public void SendMeSomewhere(Vector3 goPos)
    {
        transform.DOKill();
        anim.SetBool("walk", true);
        //transform.LookAt(transform.TransformPoint(goPos), Vector3.right);
        Vector3 fixedPos = karincaHub.transform.TransformPoint(goPos);
        float distance = Vector3.Distance(fixedPos, transform.position);
        float animSpeed = distance * 10;
        if (animSpeed > 1)
            anim.speed = 1;
        else
            anim.speed = animSpeed;
        transform.DOLookAt(fixedPos, 0.5f);
        transform.DOLocalMove(goPos, karincaSpeed).OnComplete(() => { 
            anim.SetBool("walk", false);
            anim.speed = 1f;
        });
    }

    public void KillMe()
    {
        if (isKralice)
            return;
        karincaHub.UnRegisterMe(this);
        Destroy(anim);
        transform.rotation = Quaternion.Euler(Vector3.forward * 180);
        Invoke("DestroyThis", karincaDestroyTime);
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
