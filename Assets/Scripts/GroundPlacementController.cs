using UnityEngine;
using DG.Tweening;


public class GroundPlacementController : MonoBehaviour
{
    public static GroundPlacementController instance;
    [SerializeField]
    private Transform holdPoint;
    [SerializeField] float upOffset;

    [SerializeField]
    private KeyCode newObjectHotkey = KeyCode.E;
    [SerializeField] private float rayLenght;
    public GameObject currentPlaceableObject;

    private float mouseWheelRotation;

    public LayerMask ignoreMe;

    private Transform oldParent;
    bool moved;
    private void Awake()
    {
        instance = this;

    }

    private void Update()
    {

        if (currentPlaceableObject != null)
        {
            if (!moved)
                MoveCurrentObjectToMouse();
            RotateFromMouseWheel();
            ReleaseIfClicked();
        }
    }


    public void HandleNewObjectHotkey(GameObject placeableObject)
    {
        if (currentPlaceableObject)
            ReleaseMe();
        currentPlaceableObject = placeableObject;

        Rigidbody objerb = currentPlaceableObject.GetComponent<Rigidbody>();
        objerb.isKinematic = true;
        objerb.velocity = Vector3.zero;
        objerb.angularDrag = 0;
        objerb.angularVelocity = Vector3.zero;
        currentPlaceableObject.layer = 2;
        InteractableManager.instance.isMoving = true;
    }

    private void MoveCurrentObjectToMouse()
    {

        if(currentPlaceableObject.transform.parent != null)
            oldParent = currentPlaceableObject.transform.parent;
        currentPlaceableObject.transform.parent = holdPoint;
        currentPlaceableObject.transform.DOLocalMove(Vector3.zero, 1f);
        currentPlaceableObject.transform.DOLocalRotate(Vector3.zero, 1f);
        moved = true;
        print("girdi");
    }

    private void RotateFromMouseWheel()
    {

        mouseWheelRotation = Input.mouseScrollDelta.y;
        currentPlaceableObject.transform.Rotate(Vector3.forward, mouseWheelRotation * 10f);
    }

    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ReleaseMe();
        }
    }

    private void ReleaseMe()
    {
        currentPlaceableObject.GetComponent<Rigidbody>().isKinematic = false;
        currentPlaceableObject.layer = 6;
        if (!oldParent)
            currentPlaceableObject.transform.parent = oldParent;
        else
            currentPlaceableObject.transform.parent = null;
        currentPlaceableObject = null;
        moved = false;
        InteractableManager.instance.isMoving = false;
    }
}