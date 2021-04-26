using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    Vector3 offset;
    void Start()
    {
        offset = target.position-transform.position;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {

        float smoothSpeed = 0.50f;
        Vector3 desiredPosition = target.position-offset;
        //desiredPosition.y = transform.position.y;
        Vector3 smoothPostion = Vector3.Lerp(transform.position,desiredPosition,smoothSpeed);
        
        
        transform.position = smoothPostion;
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, 17, 183);
        pos.y = Mathf.Clamp(pos.y, 8, 17);
        transform.position = pos;

        transform.LookAt(target);
        
    }
}
