using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    float rotationSpeed = 60f;
    void Update()
    {
        transform.Rotate(new Vector3(0,rotationSpeed*Time.deltaTime,0),Space.Self);
        
    }   


}
