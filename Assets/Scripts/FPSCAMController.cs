using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCAMController : MonoBehaviour
{
    public static FPSCAMController instance;

    public bool canMove;

    public float mouseSpeed = 100f;
    public Transform playerBody;
    float xRotation = 0f;
    float yRotation = 0f;

    
    void Awake()
    {
        instance = this;
    }

    
    void Update()
    {
        if (canMove)
            Move();
    }

    private void Move()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;


        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;
        if (playerBody)
        {
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);

        }
        else
        {
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }
}
