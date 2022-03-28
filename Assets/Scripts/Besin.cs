using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Besin : MonoBehaviour
{
    public float besinDegeri;
    public float destroyMeAfterEatingTime;
    public float destroyMeTime;


    public List<Transform> bones = new List<Transform>();
    private Rigidbody rb;
    private Collider col;


    ItemObject itemObject;
    bool envanterde;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        itemObject = GetComponent<ItemObject>();
        StartCoroutine(Destroy());
    }

    public void PlanToDestroyMe(float time)
    {
        Destroy(gameObject, time);
    }

    public async void GetMeReadyForEatingInKarincaHub(KarincaHub hub)
    {
        rb.isKinematic = true;
        col.enabled = false;
        PlanToDestroyMe(destroyMeAfterEatingTime + 1);
        if (bones.Count <= 0)
            return;
        for (int i = 0; i < bones.Count; i++)
        {
            await Task.Delay((int)(destroyMeAfterEatingTime / bones.Count) * 1000);
            bones[i].localScale = Vector3.zero;
            hub.SetCurrentAclik(besinDegeri / bones.Count);
        }
        hub.KarincalariDagit();
    }
    public void GetMeReadyForEating()
    {
        rb.isKinematic = true;

        PlanToDestroyMe(destroyMeAfterEatingTime);
    }

    /*private void DestroyMe()
    {

        if (itemObject.AmIInInventory)
            return;
        //InventorySystem.instance.Delete(itemObject.referenceItem, itemObject.inventoryItem);
        else
            Destroy(gameObject);
    }*/

    IEnumerator Destroy()
    {
        while (destroyMeTime > 0)
        {
            destroyMeTime -= Time.deltaTime;
            if (itemObject.AmIInInventory)
            {
                envanterde = true;
                StopAllCoroutines();
            }
            yield return null;
        }
        itemObject.DestroyMe();
    }
}
