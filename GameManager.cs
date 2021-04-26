using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public string sceneName;
    public string nextScene;
    public string sceneMusic;
    public Sprite[] level;
    Color32[] levelColors; 
    public GameObject blockPrefab,platPrefab,bgPrefab,coinPrefab,enemyPrefab,guardPrefab,sharkPrefab;

    public int bgDensity;

    public int coinCount=0;

    GlobalControlScript gcs;

    GameObject uI_Coin, uI_Time, uI_Splash;

    void Start() {
        uI_Coin = GameObject.Find("UI_Coin");
        uI_Time = GameObject.Find("UI_Time");
        uI_Splash = GameObject.Find("UI_Splash");
        uI_Splash.GetComponent<TextMeshProUGUI>().CrossFadeAlpha(0, 3f, true);
        Invoke("SplashHotfix", 3f);
        gcs = GameObject.Find("GlobalControl").GetComponent<GlobalControlScript>();
        Color32 black = new Color32(0,0,0,255);
        Color32 gray4 = new Color32(68,68,68,255);
        Color32 yellow = new Color32(255,255,0,255);
        Color32 cyan = new Color32(0,255,255,255);
        Color32 red = new Color32(255, 0, 0, 255);
        Color32 green = new Color32(0, 255, 0, 255);
        float layerZ = 0;
        foreach(Sprite s in level){
            layerZ++;
            levelColors=s.texture.GetPixels32();
            for(int y = 0; y<s.rect.height; y++){
                for(int x = 0; x<s.rect.width; x++){
                Color32 c = levelColors[(int)(x+y*s.rect.width)];
                Vector3 pos = new Vector3(x*2,y*2,layerZ*2);
                    if(CompareColor(c,black)){
                        GameObject go = Instantiate(blockPrefab,pos,Quaternion.identity);
                        go.transform.parent = this.transform;
                        go.name="GROUND"+pos.x+"_"+pos.y+"_"+pos.z;
                    }
                    else if(CompareColor(c,gray4)){
                        GameObject go = Instantiate(platPrefab,pos,Quaternion.identity);
                        go.transform.parent = this.transform;
                        go.name="plat"+pos.y+""+pos.x+pos.z;
                        if (pos.y == 10 || pos.y==0) go.AddComponent<VineBehaviour>();
                    } else if(CompareColor(c,cyan)){
                        GameObject go = Instantiate(guardPrefab,pos,Quaternion.identity);
                        go.transform.parent = this.transform;
                        go.name="Guard"+pos.x+"_"+pos.y+"_"+pos.z;
                    }
                    else if(CompareColor(c,yellow)){
                        GameObject go = Instantiate(coinPrefab,pos,Quaternion.identity);
                        go.transform.parent = this.transform;
                    } 
                    else if (CompareColor(c, red)) {
                        GameObject go = Instantiate(enemyPrefab, pos, Quaternion.identity);
                        go.transform.parent = this.transform;
                    } 
                    else if (CompareColor(c, green)) {
                        GameObject go = Instantiate(sharkPrefab, pos, Quaternion.identity);
                        go.transform.parent = this.transform;
                    }
                }
            }
        }
        for(int i = 0; i<bgDensity; i++){
            Vector3 pos;
            GameObject go;
            switch (sceneName) {
                case "SceneA":
                    pos = new Vector3(Random.Range(0, 200), 0, 14);
                    go = Instantiate(bgPrefab, pos, Quaternion.identity, this.transform);
                    go.transform.Rotate(Vector3.up * Random.Range(0, 360));
                    break;
                case "SceneB":
                    pos = new Vector3(Random.Range(0, 200), Random.Range(0, 15), 22);
                    go = Instantiate(bgPrefab, pos, Quaternion.identity, this.transform);
                    go.transform.Rotate(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
                    break;

            }
        }
        gcs.PlaySound(sceneMusic);
    }

    // Update is called once per frame
    void Update() {
        uI_Coin.GetComponent<TextMeshProUGUI>().text = "COIN: " + coinCount;
        string timeString = Time.timeSinceLevelLoad.ToString();
        if (timeString.Length > 4) timeString = timeString.Substring(0, 4);
        uI_Time.GetComponent<TextMeshProUGUI>().text = ("TIME: " + timeString);
    }

    bool CompareColor(Color32 a, Color32 b){
        return (a.r == b.r && a.g == b.g && a.b == b.b);
    }
    public void GameOver(string cause){
        if(cause.StartsWith("gator"))gcs.PlaySound("Chomp");
        else gcs.PlaySound("Death");
        SceneManager.LoadScene(sceneName);

    }
    public void Coin() {
        gcs.PlaySound("Coin");
        coinCount++;
    }
    public void Win() {
        gcs.PlaySound("Win");
        switch (sceneName) {
            case "SceneA":
                gcs.l1 = Time.timeSinceLevelLoad;
                break;
            case "SceneB":
                gcs.l2 = Time.timeSinceLevelLoad;
                break;
            case "SceneC":
                gcs.l3 = Time.timeSinceLevelLoad;
                break;
            case "SceneD":
                gcs.l4 = Time.timeSinceLevelLoad;
                break;
        }
        gcs.l6 += coinCount;
        SceneManager.LoadScene(nextScene);
    }
    void SplashHotfix() {
        uI_Splash.SetActive(false);
    }
}
