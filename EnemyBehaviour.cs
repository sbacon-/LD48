using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    float sightRange = 10f,speed = 5f;
    Vector2 worldBounds = new Vector2(2f,10f);

    Transform target;

    Vector3 direction = Vector3.zero;
    void Start(){
        target = GameObject.Find("Player").transform;
        InvokeRepeating("ChooseRandomDirection",0f,4f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position,target.position)<sightRange){
            Vector3 asCloseAsItCanGet = new Vector3(target.position.x,transform.position.y,target.position.z);
            transform.position = Vector3.MoveTowards(transform.position,asCloseAsItCanGet,speed*Time.deltaTime);
            transform.LookAt(asCloseAsItCanGet);
        }else{
        Vector3 targetPos = transform.position+direction;
        transform.position = Vector3.MoveTowards(transform.position,targetPos , (speed/2)*Time.deltaTime);
        Vector3 pos = transform.position;
        pos.z = Mathf.Clamp(pos.z,worldBounds.x,worldBounds.y);
            if (pos.z == worldBounds.x || pos.z == worldBounds.y) ChooseRandomDirection();
        transform.position = pos;
        transform.LookAt(targetPos);
        }        
    }

    void ChooseRandomDirection(){
        direction = new Vector3(Random.Range(-1f,1f),0,(Random.Range(-1f,1f)));
        direction = direction.normalized;
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("guard")){
            direction*=-1;
        }
    }
}
