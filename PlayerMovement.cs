using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerSpeed = 3f, jumpForce = 3f, gravityMultiplier = 2;
    
    Vector3 input;
    Vector2 worldBounds = new Vector2(1f,11f);

    GlobalControlScript gcs;
    GameManager gm;
    CharacterController cc;
    Animator anim;
    bool latch = false,latchCooldown=false;
    Transform latched;

    void Start(){
        gcs = GameObject.Find("GlobalControl").GetComponent<GlobalControlScript>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        if (gm.sceneName == "SceneC") anim.SetBool("Swim", true);
    }

    void Update()
    {
        float inY = input.y;
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0 ,Input.GetAxisRaw("Vertical"));
        anim.SetBool("Running",Mathf.Abs(input.x)==1 || Mathf.Abs(input.z) == 1);
        anim.SetBool("Grounded",cc.isGrounded || gm.sceneName == "SceneC");
        input = input.normalized*playerSpeed;
        input.y = inY;
        if(latch){
            transform.position=latched.position;
            if(Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.Joystick1Button0)) {
                latchCooldown=true;
                Invoke("ResetLatch",0.2f);
                latch=!latch;
                input.y=jumpForce;
                gcs.PlaySound("Jump");
            }
            return;
        }
        if((cc.isGrounded|| gm.sceneName == "SceneC") && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0))) {
            gcs.PlaySound("Jump");
            input.y=jumpForce;
        }
        
        input.y += Physics.gravity.y*gravityMultiplier*Time.deltaTime;
        transform.LookAt(new Vector3(transform.position.x+input.x,transform.position.y,transform.position.z+input.z));
        cc.Move(input*Time.deltaTime);

        Vector3 pos = transform.position;
        pos.z = Mathf.Clamp(pos.z, worldBounds.x, worldBounds.y);
        pos.x = Mathf.Clamp(pos.x, 0, 200);
        transform.position = pos;
        //if(rb.velocity.y<0)rb.AddForce(Vector3.up*fallSpeed,ForceMode.Acceleration);
        
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("vine")&&!latchCooldown){
            Latch(other.transform);
            GameObject go = GameObject.Find("plat10" + other.name.Substring(6));
            if (go != null) go.GetComponent<VineBehaviour>().Activate();
            GameObject go2 = GameObject.Find("plat0" + other.name.Substring(5));
            if (go2 != null) go2.GetComponent<VineBehaviour>().Activate();

        }
        if (other.CompareTag("coin")) {
            gm.Coin();
            GameObject.Destroy(other.gameObject);
        }
        if (other.CompareTag("win")) {
            gm.Win();
        }
    }
    void OnControllerColliderHit(ControllerColliderHit hit){
        if(hit.collider.CompareTag("enemy")){
            gm.GameOver(hit.collider.gameObject.name);
        }
    }

    void Latch(Transform t){
        if(latch)return;
        latch=true;
        latched=t;
        gcs.PlaySound("Latch");
    }
    void ResetLatch(){
        latchCooldown = false;
    }
}
