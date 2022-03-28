using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    public static ItemInteraction instance;

    [SerializeField] private KeyCode inspectKey = KeyCode.F;
    [SerializeField] private float inspectSpeed = 5f;
    Vector3 Initial_position;
    Quaternion Rot;

    public Transform InspectZone;

    public GameObject currentObj;
    private Rigidbody rb;
    ItemTransforms itemTransforms;

   public bool isInspecting = false;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (isInspecting && Input.GetKeyDown(inspectKey))
        {
            GameManager.instance.IsInspecting = false;
            GameManager.instance.CanUseCursor(false);
            GameManager.instance.CanControlPlayer(true);
            InteractableManager.instance.isInspecting = false;
            StopAllCoroutines();
            BeslemeManager.instance.SetInpectedObje(null);
            itemTransforms.canMove = false;
            itemTransforms.enabled = false;
            //move.enabled = true;
            //rotate.enabled = true;
            if (currentObj.TryGetComponent<KarincaHub>(out KarincaHub karincaHub))
            {
                karincaHub.ChangeToInspectionMode(false);
                karincaHub.SetPislikParents(true);
                ObjectInfoUI.instance.ToggleKarincaHub(false);
            }
            currentObj.transform.rotation = Quaternion.Euler(Rot.eulerAngles);
            StartCoroutine(MoveToTarget(currentObj, Initial_position, inspectSpeed, false));
            if (rb != null)
            {
                StartCoroutine(TogglePhysics(rb, true, 0.5f));
            }
        }
    }
    public void DoAction(GameObject toInspect)
    {
        if (!isInspecting && currentObj != toInspect)
        {
            GameManager.instance.IsInspecting = true;
            GameManager.instance.CanUseCursor(true);
            GameManager.instance.CanControlPlayer(false);
            InteractableManager.instance.isInspecting = true;
            StopAllCoroutines();
            currentObj = toInspect.gameObject;
            BeslemeManager.instance.SetInpectedObje(currentObj);
            rb = currentObj.GetComponent<Rigidbody>();
            Initial_position = toInspect.transform.position;
            Rot = Quaternion.Euler(toInspect.transform.localEulerAngles);
            if (rb != null)
            {
                rb.isKinematic = true;
            }
            //move.enabled = false;
            //rotate.enabled = false;
            StartCoroutine(MoveToTarget(currentObj, InspectZone.position, inspectSpeed, true));
            if (currentObj.TryGetComponent<ItemTransforms>(out ItemTransforms item))
            {
                itemTransforms = item;
            }
            else
                currentObj.AddComponent<ItemTransforms>();

            if (currentObj.TryGetComponent<KarincaHub>(out KarincaHub karincaHub))
            {
                karincaHub.ChangeToInspectionMode(true);
                karincaHub.SetPislikParents(true);
                ObjectInfoUI.instance.SetKarincaHubObjectInfo(karincaHub);
            }
        }
        
    }
    IEnumerator MoveToTarget(GameObject MovedObj, Vector3 Target, float Speed, bool yakinlastir)
    {
        while (MovedObj.transform.position != Target)
        {
            MovedObj.transform.position = Vector3.MoveTowards(MovedObj.transform.position, Target, Time.deltaTime * Speed);
            yield return null;
        }
        if (yakinlastir)
        {

            itemTransforms.enabled = true;
            itemTransforms.canMove = true;
            isInspecting = true;
        }
        if (!yakinlastir)
        {

            isInspecting = false;
        }
    }
    IEnumerator TogglePhysics(Rigidbody rb, bool value, float TimeWait)
    {
        yield return new WaitForSeconds(TimeWait);
        rb.isKinematic = !value;
        currentObj = null;
    }

}
