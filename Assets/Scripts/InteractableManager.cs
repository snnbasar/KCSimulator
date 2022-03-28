using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SojaExiles;
using System;

public class InteractableManager : MonoBehaviour
{
    public static InteractableManager instance;

    [SerializeField] private float rayLenght;
    [SerializeField] private LayerMask ignoreMe;

    public GameObject hitObject;

    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private KeyCode inspectKey = KeyCode.F;
    [SerializeField] private KeyCode dragKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode collectItemKey = KeyCode.T;
    [SerializeField] private KeyCode inventoryKey = KeyCode.I;
    [SerializeField] private KeyCode marketKey = KeyCode.Tab;
    [SerializeField] private KeyCode eatingKey = KeyCode.C;
    public GameObject interactableObject;
    public bool hasInteractable;

    public bool isInspecting; //SetsOn ItemInteractions
    public bool isMoving; //Sets on GroundPlacementController


    RaycastHit hitPoint;
    private void Awake()
    {
        instance = this;
    }


    private void Update()
    {
        DrawRayCast();
        CheckForInteractable();
        CheckForInventory();
        CheckForMarket();
        if (!hitObject)
            return;
        //CheckForMoveable(); //Out Of Use
        CheckForDoors();
        CheckForDrawerAndDoors();
        CheckForInspecting();
        CheckForDraggable();
        CheckForItem();
        CheckForGuns();
        CheckForYemek();
    }

    

    private void CheckForInventory()
    {
        if(!isMoving && Input.GetKeyDown(inventoryKey))
        {
            InventoryUI.instance.ToogleInventoryUI();
        }
    }
    private void CheckForMarket()
    {
        if (!isMoving && Input.GetKeyDown(marketKey))
        {
            MarketUI.instance.ToogleMarketUI();
        }
    }

    private void DrawRayCast()
    {
        RaycastHit hit;
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayLenght, Color.red, 0.1f);
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward * rayLenght, out hit, ignoreMe))
        {
            hitObject = hit.transform.gameObject;
            hitPoint = hit;
        }
        else
            hitObject = null;
    }



    private void CheckForInteractable()
    {
        if (hitObject && !hasInteractable && hitObject.layer == 6)
        {
            interactableObject = hitObject;
            hasInteractable = true;
            Outline(true);
        }
        if (hasInteractable && hitObject != interactableObject)
        {
            Outline(false);
            interactableObject = null;
            hasInteractable = false;
        }
    }

    private void Outline(bool status)
    {
        switch (status)
        {
            case true:
                OutlineController.instance.OutlineDo(interactableObject);
                break;
            case false:
                OutlineController.instance.OutlineReset(interactableObject);
                break;
        }
    }



    private void CheckForMoveable()
    {
        if (interactableObject)
        {
            if (!isInspecting && interactableObject.CompareTag("TestTube") && Input.GetKeyDown(interactKey))
            {
                GroundPlacementController.instance.HandleNewObjectHotkey(interactableObject);
            }
        }
    }


    private void CheckForDoors()
    {
        if (interactableObject)
        {
            if (Input.GetKeyDown(interactKey) && interactableObject.TryGetComponent<DoorController>(out DoorController doorController))
            {
                if(!doorController.outOfUse)
                    doorController.DoAction();
            }
        }
    }

    private void CheckForDrawerAndDoors()
    {
        if (interactableObject)
        {
            if (interactableObject.CompareTag("opencloseDoor") && Input.GetKeyDown(interactKey))
            {
                opencloseDoor _opencloseDoor = interactableObject.GetComponent<opencloseDoor>();
                _opencloseDoor.DoAction();
            }
            if (interactableObject.CompareTag("opencloseDoor1") && Input.GetKeyDown(interactKey))
            {
                opencloseDoor1 _opencloseDoor1 = interactableObject.GetComponent<opencloseDoor1>();
                _opencloseDoor1.DoAction();
            }
            if (interactableObject.CompareTag("Drawer_Pull_X") && Input.GetKeyDown(interactKey))
            {
                Drawer_Pull_X _draver_Pull_X = interactableObject.GetComponent<Drawer_Pull_X>();
                _draver_Pull_X.DoAction();
            }
            if (interactableObject.CompareTag("Drawer_Pull_Z") && Input.GetKeyDown(interactKey))
            {
                Drawer_Pull_Z _draver_Pull_Z = interactableObject.GetComponent<Drawer_Pull_Z>();
                _draver_Pull_Z.DoAction();
            }
        }
    }

    private void CheckForInspecting()
    {
        if (interactableObject)
        {
            if (isMoving)
                return;
            if (Input.GetKeyDown(inspectKey) && interactableObject.GetComponent<ItemTransforms>())
            {
                ItemInteraction.instance.DoAction(interactableObject);
            }
        }
    }


    private void CheckForDraggable()
    {
        if (interactableObject)
        {
            if (isMoving || isInspecting)
                return;
            if (Input.GetKeyDown(dragKey) && interactableObject.GetComponent<DragObject>())
            {
                DraggableController.instance.DoAction(interactableObject);
            }


            /*if (interactableObject.CompareTag("TestTube") && Input.GetKeyDown(dragKey))
            {
                DraggableController.instance.DoAction(interactableObject);
            }
            if (interactableObject.CompareTag("Kutu") && Input.GetKeyDown(dragKey))
            {
                DraggableController.instance.DoAction(interactableObject);
            }*/
        }
    }

    private void CheckForItem()
    {
        if (interactableObject)
        {
            if (isMoving || isInspecting)
                return;
            if (Input.GetKeyDown(collectItemKey) && interactableObject.TryGetComponent<ItemObject>(out ItemObject item))
            {
                item.OnHandlePickupItem();
                SoundManager.instance.PlaySound(Soundlar.PickUp);
            }

            /*if (interactableObject.CompareTag("TestTube") && Input.GetKeyDown(collectItemKey))
            {
                if (interactableObject.TryGetComponent<ItemObject>(out ItemObject item))
                {
                    print("aldi");
                    item.OnHandlePickupItem();
                    //itemController.SetTargetPosition(item.transform);
                    //itemController.Pickup();
                }
            }
            if (interactableObject.CompareTag("Besin") && Input.GetKeyDown(collectItemKey))
            {
                if (interactableObject.TryGetComponent<ItemObject>(out ItemObject item))
                {
                    print("aldi");
                    item.OnHandlePickupItem();
                }
            }*/
        }
    }
    private void CheckForGuns()
    {
        if (interactableObject)
        {
            if (isMoving || isInspecting)
                return;
            if (Input.GetKeyDown(collectItemKey) && interactableObject.TryGetComponent<PickUpController>(out PickUpController pickUpController))
            {
                pickUpController.PickUp();
            }

        }
    }

    private void CheckForYemek()
    {
        if (interactableObject)
        {
            if (isMoving || isInspecting)
                return;
            if (Input.GetKeyDown(eatingKey) && interactableObject.TryGetComponent<Yiyecek>(out Yiyecek yiyecek))
            {
                yiyecek.YeBeni();
            }

        }
    }
}
