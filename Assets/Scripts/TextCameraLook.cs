using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextCameraLook : MonoBehaviour
{
    Transform character;
    public float damping;
    void Start()
    {
        character = KarakterController.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        var lookPos = character.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }
}
