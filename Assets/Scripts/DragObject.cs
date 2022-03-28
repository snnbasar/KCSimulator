using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragObject : MonoBehaviour
{
    private Vector3 mOffset;


    private float mZCoord;

    public bool canMove;

    private Rigidbody rb;
    private Collider col;

    private float mouseWheelRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }
    void OnMouseDown()

    {

        rb.isKinematic = true;
        mZCoord = Camera.main.WorldToScreenPoint(

            gameObject.transform.position).z;




        // Store offset = gameobject world pos - mouse world pos

        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();

    }



    private Vector3 GetMouseAsWorldPoint()

    {

        // Pixel coordinates of mouse (x,y)

        Vector3 mousePoint = Input.mousePosition;



        // z coordinate of game object on screen

        mousePoint.z = mZCoord;



        // Convert it to world points

        return Camera.main.ScreenToWorldPoint(mousePoint);

    }



    /*void OnMouseDrag()

    {
        if (!canMove)
            return;
        transform.position = GetMouseAsWorldPoint() + mOffset;
        print(transform.position.z);
        mouseWheelRotation = Input.mouseScrollDelta.y;
        transform.Rotate(Vector3.forward, mouseWheelRotation * 10f);
    }*/

    private void Update()
    {
        if (!canMove)
            return;

        if (Input.GetMouseButton(0))
        {
            transform.position = GetMouseAsWorldPoint() + mOffset;
            print(transform.position.z);
            mouseWheelRotation = Input.mouseScrollDelta.y;
            transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
        }

        if (Input.GetMouseButtonUp(0))
        {
            DraggableController.instance.CancelAction();
            rb.isKinematic = false;
        }
    }

}