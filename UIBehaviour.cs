using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIBehaviour : MonoBehaviour
{
    //UI SLIDING
    [SerializeField] AnimationCurve slideCurve;
    Transform slideTransform, optionsTransform, controlsTransform, panelTransform, pauseTransform;
    TextMeshProUGUI l1,l2,l3,l4,l5,l6;
    bool sliding=false;
    float slideStartX = 0, slideTargetX = -600, animTime = 0, slideSpeed = 0.25f;
    
    GlobalControlScript gcs;

    void Start() {
        gcs = GameObject.Find("GlobalControl").GetComponent<GlobalControlScript>();
        Transform[] childTransforms = GetComponentsInChildren<Transform>(true);
        if (SceneManager.GetActiveScene().name == "Win") {
            foreach (Transform t in childTransforms) {
                if (t.name == "LVL1") l1 = t.GetComponent<TextMeshProUGUI>();
                if (t.name == "LVL2") l2 = t.GetComponent<TextMeshProUGUI>();
                if (t.name == "LVL3") l3 = t.GetComponent<TextMeshProUGUI>();
                if (t.name == "LVL4") l4 = t.GetComponent<TextMeshProUGUI>();
                if (t.name == "TOT") l5 = t.GetComponent<TextMeshProUGUI>();
                if (t.name == "COIN") l6 = t.GetComponent<TextMeshProUGUI>();
            }
            gcs.PlaySound("Kazoo");
            gcs.l5 = gcs.l1 + gcs.l2 + gcs.l3 + gcs.l4;
            l1.text += "" + gcs.l1;
            l2.text += "" + gcs.l2;
            l3.text += "" + gcs.l3;
            l4.text += "" + gcs.l4;
            l5.text += "" + gcs.l5;
            l6.text += "" + gcs.l6;
        }
        foreach(Transform t in childTransforms){
            if(t.name == "UI_Slide")slideTransform=t;
            if(t.name == "UI_Options")optionsTransform=t;
            if(t.name == "UI_Controls")controlsTransform=t;
            if(t.name == "UI_Panel")panelTransform=t;
            if(t.name == "UI_Pause")pauseTransform=t;
        }
    }

    void Update() {
        if(sliding)Slide();
        if(Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.Joystick1Button7) && gcs.sProfile==1) gcs.Pause();
    }

    
    //BUTTONS
    public void StartGame(){
        CallSlide(gcs.GetSProfile().x,gcs.GetSProfile().y);
        Time.timeScale = 1;
        Invoke("StartGameLater",slideSpeed);
        gcs.sProfile = 1;
    }
    public void Resume(){
        gcs.Pause();
    }

    public void Options(){
        CallSlide(gcs.GetSProfile().x,gcs.GetSProfile().y);
        panelTransform.gameObject.SetActive(true);
        optionsTransform.gameObject.SetActive(true);
        DisplayAudio();
    }
    public void Controls(){
        CallSlide(gcs.GetSProfile().x,gcs.GetSProfile().y);
        panelTransform.gameObject.SetActive(true);
        controlsTransform.gameObject.SetActive(true);
    }

    public void ClosePanel() {
        optionsTransform.gameObject.SetActive(false);
        controlsTransform.gameObject.SetActive(false);
        panelTransform.gameObject.SetActive(false);
        CallSlide(gcs.GetSProfile().y,gcs.GetSProfile().x);
    }
    public void Quit(){
        gcs.Pause();
        SceneManager.LoadScene("MainMenu");
    }

    void StartGameLater(){
        SceneManager.LoadScene("SceneA");
    }
    //TWEENING   
    void CallSlide(float startX,float targetX){
        animTime=0;
        slideStartX=startX;
        slideTargetX=targetX;
        sliding=true;
    }
    void Slide(){
        print("SLIDE TO THE LEF");
        animTime+=0.005f;
        if(animTime>slideSpeed)sliding=false;
        float anim_X = animTime/slideSpeed;
        float eval_X = slideCurve.Evaluate(anim_X);
        slideTransform.localPosition = new Vector3(slideStartX*(1-eval_X)+(eval_X*slideTargetX),0,0);
    }

    //AUDIO
    void DisplayAudio(){
        GameObject.Find("Music").GetComponent<Slider>().SetValueWithoutNotify(gcs.musicVol);
        GameObject.Find("SFX").GetComponent<Slider>().SetValueWithoutNotify(gcs.sfxVol);
        GameObject.Find("Mute").GetComponent<Toggle>().SetIsOnWithoutNotify(gcs.mute);
    }
    public void UpdateAudio(){
        gcs.musicVol = GameObject.Find("Music").GetComponent<Slider>().value;
        gcs.sfxVol = GameObject.Find("SFX").GetComponent<Slider>().value;
        gcs.mute = GameObject.Find("Mute").GetComponent<Toggle>().isOn;
        gcs.UpdateAudio();
        DisplayAudio();
    }

    public void SetPauseTransform(bool active) {
        if (SceneManager.GetActiveScene().name == "Win") return;
        pauseTransform.gameObject.SetActive(active);
    }
    public void PCHOTFIX() {
        Application.Quit();
    }
}
