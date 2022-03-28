using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTransforms : MonoBehaviour
{
    public float Speed;

    public Vector3 poslastFame;
    public bool canMove;

    KarincaHub karincaHub;
    //public float mouseWheelYaklastirma;
    //public Transform hoidPoint;

    private void Start()
    {
        Speed = GameManager.instance.inspectSensitivity;
        if(TryGetComponent<KarincaHub>(out KarincaHub hub))
        {
            karincaHub = hub;
        }
    }

    //Controls the way the selected object behaves

    private void Update()
    {
        //if (Input.GetMouseButton(1))
        //   Move();
        if (canMove && Input.GetMouseButtonDown(1))
        {
            if(karincaHub)
                karincaHub.SetPislikParents(true);
            poslastFame = Input.mousePosition;
        }
        
        if (canMove && Input.GetMouseButton(1))
        {
            var delta = Input.mousePosition - poslastFame;
            poslastFame = Input.mousePosition;
            var axis = Quaternion.AngleAxis(-90f, Vector3.forward) * delta;
            transform.rotation = Quaternion.AngleAxis(delta.magnitude * 0.1f, axis) * transform.rotation;
        }

        if (canMove && karincaHub && Input.GetMouseButtonUp(1))
        {
            karincaHub.SetPislikParents(false);
        }
        /*if (canMove)
        {
            mouseWheelYaklastirma = Input.mouseScrollDelta.y;
            transform.Translate(Vector3.forward * mouseWheelYaklastirma * 0.1f);
            print(mouseWheelYaklastirma);
            if(mouseWheelYaklastirma > 0)
            {
                if (karincaHub)
                    karincaHub.SetPislikParents(true);
            }
            else
            {
                karincaHub.SetPislikParents(false);
            }

        }*/
    }
    /*private void OnEnable()
    {
        mouseWheelYaklastirma = 0;
    }*/

    private void Move()
    {
        float x = Input.GetAxis("Mouse X") * Speed;
        float y = Input.GetAxis("Mouse Y") * Speed;
        transform.Rotate(-Vector3.up * x, Space.World);
        transform.Rotate(-Vector3.forward * y, Space.World);
    }
}