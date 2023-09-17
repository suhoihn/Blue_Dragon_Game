using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mode2_Script : MonoBehaviour
{
    // Start is called before the first frame update

    char[] allAlphabets = new char[26];
      
    public GameObject renderAlphabetPrefab;
    public GameObject lifePrefab;

    int words = 0;
    bool gameOver = false;

    string targetWord;
    int maxLength = 0;
    int filledLength = 0;
    bool[] emptyPos;
    float emptyRatio = 0.3f;


    int life = 3;
    int completedWords = 0;
    int coinCount = 0;
    int status = 0;

    List<char> wrongAlphabets, correctAlphabets;


    float wordSpeed;
    int angleLevel;
    float wordInterval;
    float obstacleRate, coinRate, falseAlphabetRate;
    float elapsedTime = 0f;
    float nextElapsedTime = 0f;
    
    Image wordImage;
    Utils utilsScript;

    void Start()
    {
        PlayerPrefs.SetString("modeName", "Mode 2");
        words = PlayerPrefs.GetInt("completedWords",0);   

        utilsScript = GameObject.Find("Mode2_Manager").GetComponent<Utils>();

        wordImage = GameObject.Find("WordImage").GetComponent<Image>();

        status = 0;
        int idx = 0;
        for(char c = 'a'; c <= 'z'; c++){
            allAlphabets[idx++] = c;
        }  

        int speed_idx = PlayerPrefs.GetInt("map_speed_level",2);
        int delay_idx = PlayerPrefs.GetInt("map_delay_level",2);
        int obstacle_idx = PlayerPrefs.GetInt("map_obstacle_level",2);
        int angle_idx = PlayerPrefs.GetInt("map_angle_level",2);

        float[] arr_throwing_speed = {3.5f, 4.0f, 4.5f, 5.0f, 5.5f};
        float[] arr_throwing_interval = {1.0f, 0.8f, 0.6f, 0.4f, 0.2f};//{2.5f, 2.0f, 1.5f, 1.2f, 0.8f};
        float[] arr_obstacle_throwing_rate = {0.05f, 0.1f, 0.2f, 0.3f, 0.4f};
        int[] arr_throwing_angle_level = {1,2,2,3,3};

        wordSpeed = arr_throwing_speed[speed_idx];
        wordInterval = arr_throwing_interval[delay_idx];
        obstacleRate = arr_obstacle_throwing_rate[obstacle_idx];
        angleLevel = arr_throwing_angle_level[angle_idx];
        if(angleLevel == 3){
            wordSpeed *= 3;
        }
        coinRate = 0.1f;
        falseAlphabetRate = 0.5f;
        showLife();
        
        Debug.Log("=============================");
        Debug.Log("speed: " + speed_idx.ToString());
        Debug.Log("interval: " + delay_idx.ToString());
        Debug.Log("obstacle rate: " + obstacle_idx.ToString());
        Debug.Log("angle level: " + angle_idx.ToString());



    }

    void destroyRenderedWord(){
        foreach(Transform child in GameObject.Find("WordRenderer").transform){
            Destroy(child.gameObject);
        }

    }   

    void renderAlphabetAt(int idx){
        GameObject renderGroup = GameObject.Find("WordRenderer").gameObject;
        GameObject temp = Instantiate(renderAlphabetPrefab);
        if(emptyPos[idx]){
            temp.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("alphabets/" + "_");
        }
        else{
            temp.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("alphabets/" + targetWord[idx]);
        }

        temp.transform.SetParent(renderGroup.transform);
        temp.transform.localPosition = new Vector3(idx * 0.7f, 0f, 0f);
        temp.transform.localScale = new Vector3(2,2,2);
    } 

    void renderTargetWord(){
        GameObject renderGroup = GameObject.Find("WordRenderer").gameObject;
        for(int i = 0; i < maxLength; i++){
            //temp = AlphabetObjectPooling.alphaInstance.getObj();

            GameObject temp = Instantiate(renderAlphabetPrefab);
            if(emptyPos[i]){
                temp.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("alphabets/" + "_");
            }
            else{
                temp.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("alphabets/" + targetWord[i]);
            }

            temp.transform.SetParent(renderGroup.transform);
            temp.transform.localPosition = new Vector3(i * 0.7f, 0f, 0f);
            temp.transform.localScale = new Vector3(2,2,2);
        }

        Vector3 tempScale = utilsScript.resizeImage(
            Resources.Load<Sprite>("words/" + targetWord),
            wordImage
        );

        wordImage.transform.localScale = tempScale;

    }
    public void handleGameOver(){
        PlayerPrefs.SetInt("completedWords", completedWords);
        // int totalCoinCount = PlayerPrefs.GetInt("coinCount", 0);
        // PlayerPrefs.SetInt("coinCount", totalCoinCount + coinCount);
        PlayerPrefs.SetInt("coinGainCount", coinCount);

        PlayerPrefs.SetFloat("elapsedTime", elapsedTime);

        GameObject.Find("SceneLoadManager").GetComponent<SceneLoadManager>().GoToResultScene();
    }

    void assignNewWord(){

        targetWord = GameObject.Find("Mode2_Manager").GetComponent<Utils>().get_random_word();

        maxLength = targetWord.Length;
        emptyPos = new bool[maxLength];
        for(int i = 0; i < maxLength; i++){
            emptyPos[i] = false;
        }
        filledLength = 0;
        while(filledLength < maxLength * emptyRatio){
            int idx = Random.Range(0,maxLength);;
            while(emptyPos[idx]){
                idx = Random.Range(0,maxLength);
            }
            emptyPos[idx] = true;
            filledLength++;
        }

        wrongAlphabets = new List<char>();
        correctAlphabets = new List<char>();
        for(int i = 0; i < allAlphabets.Length; i++){
            bool isEmptyAlphabet = false;
            for(int j = 0; j < targetWord.Length; j++){
                if(targetWord[j] == allAlphabets[i] && emptyPos[j]){
                    isEmptyAlphabet = true;
                    correctAlphabets.Add(allAlphabets[i]);
                    break;
                }
            }

            if(!isEmptyAlphabet){
                wrongAlphabets.Add(allAlphabets[i]);
            }

        }

        // for(int i = 0; i < maxLength; i++){
        //     if(emptyPos[i]){Debug.Log("_");}
        //     else{Debug.Log(targetWord[i]);}
        // }

        //Debug.Log(wrongAlphabets);
        //Debug.Log(correctAlphabets);
    }

    void Update()
    {
        if(status == 0){
            assignNewWord();
            destroyRenderedWord();
            renderTargetWord();
            status = 1;
        }
        elapsedTime += Time.deltaTime;
        nextElapsedTime += Time.deltaTime;

        if(nextElapsedTime >= wordInterval){
            nextElapsedTime = 0f;
            throwObj();
        }

        GameObject.Find("CoinText").GetComponent<Text>().text = coinCount.ToString();
        GameObject.Find("WordCountText").GetComponent<Text>().text = completedWords.ToString();

        if(life < 0) { handleGameOver(); }
    }

    void showLife(){
        float x_gap = 1f;

        foreach(Transform child in GameObject.Find("LifeRenderer").transform){
            Destroy(child.gameObject);
        }

        for(int i = 0; i < life; i++){
            GameObject temp = Instantiate(lifePrefab);
            temp.transform.SetParent(GameObject.Find("LifeRenderer").transform);
            temp.transform.localPosition = new Vector3(x_gap * i, 4f, 0f);
        }


    }

    void throwObj(){
        float objRate = Random.Range(0f, 1f);
        GameObject obj = AlphabetObjectPooling.alphaInstance.getObj();

        if(objRate < obstacleRate){
            obj.gameObject.tag = "obstacle";
            obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("obstacle");
        }
        else if(objRate < obstacleRate + coinRate){
            obj.gameObject.tag = "coin";
            obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("coin");

        }
        else{
            objRate = Random.Range(0f, 1f);
            if(objRate < falseAlphabetRate){
                int idx = Random.Range(0, wrongAlphabets.Count);
                obj.gameObject.tag = "wrongAlphabet";   
                obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("alphabets/" + wrongAlphabets[idx]);
                obj.GetComponent<ThrownObjScript>().setAlphabet(wrongAlphabets[idx]);

            }
            else{
                int idx = Random.Range(0, correctAlphabets.Count);
                obj.gameObject.tag = "correctAlphabet";   
                obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("alphabets/" + correctAlphabets[idx]);
                obj.GetComponent<ThrownObjScript>().setAlphabet(correctAlphabets[idx]);


            }
        }

        float x = Random.Range(-8f,8f);
        obj.GetComponent<ThrownObjScript>().setPosition(new Vector3(x, 10f, 0f));
        obj.GetComponent<ThrownObjScript>().setAngle(angleLevel);
        obj.GetComponent<ThrownObjScript>().setSpeed(wordSpeed);
        obj.GetComponent<ThrownObjScript>().initialize();
    }

    public void handleWrongAlphabet(char c){
        life--;
        showLife();

    }

    public void handleCorrectAlphabet(char c){
        if(correctAlphabets.Contains(c)){
            correctAlphabets.Remove(c);
        }
        else{wrongAlphabets.Add(c);}

        for(int i = 0; i < targetWord.Length; i++){
            if(targetWord[i] == c){
                emptyPos[i] = false;
            }
        }
        
        if(isWordFinished()){
            assignNewWord();
            completedWords++;
        }

        destroyRenderedWord();
        renderTargetWord();

    }

    bool isWordFinished(){
        for(int i = 0; i < targetWord.Length; i++){
            if(emptyPos[i]){return false;}
        }
        return true;
    }

    public void handleCoin(){
        coinCount++;
    }   

    public void handleObstacle(){
        life--;
        showLife();
    }
}
