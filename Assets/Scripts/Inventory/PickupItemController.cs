using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItemController : MonoBehaviour
{
    public float ikWeight;
    //FullBodyBipedIK ik;

    private void Awake()
    {
        //ik = GetComponent<FullBodyBipedIK>();
    }

    private void Update()
    {
        //ik.solver.IKPositionWeight = ikWeight;
    }

    [ContextMenu("Pickup")]
    public void Pickup()
    {
        StartCoroutine(AnimatePickup());
    }
    IEnumerator AnimatePickup()
    {
        while (ikWeight < 1f)
        {
            ikWeight += 0.05f;
            yield return new WaitForSeconds(0.025f);
        }
        while (ikWeight > 0f)
        {
            ikWeight -= 0.05f;
            yield return new WaitForSeconds(0.025f);
        }
    }
}
