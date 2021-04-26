using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineBehaviour : MonoBehaviour
{
    Vector3 pos;
    public bool active=false;
    float swingSpeed=15f;
    public float maxX, minX, target;

    GameObject[] connectedVines = new GameObject[4];


    void Start(){
        pos=transform.position;
        maxX=transform.position.x+5;
        minX=transform.position.x-5;
        target = maxX;
    }

    void Update()
    {
        if(active){
            Vector3 targetPos = new Vector3(target,pos.y,pos.z);
            transform.position = Vector3.MoveTowards(transform.position,targetPos,swingSpeed*Time.deltaTime);
            for(int i = 1; i<5; i++){
                float segMaxX=maxX-i;
                float segMinX=minX+i;
                float segTarget = target==maxX? segMaxX:segMinX;
                Vector3 segTargetPos = new Vector3(segTarget,pos.y+(2*i),pos.z);
                connectedVines[i-1].transform.position = Vector3.MoveTowards(connectedVines[i-1].transform.position,segTargetPos,swingSpeed*Time.deltaTime);
            }
            if(Mathf.Abs(transform.position.x-target)<0.05f){
                target = target==maxX? minX : maxX;
            }

        }
    }

    public void Activate(){
        active=true;
        for(int i = 0; i<4; i++) connectedVines[i]=GameObject.Find("plat"+(pos.y+(2*(i+1))+""+pos.x+""+pos.z));
    }
}
