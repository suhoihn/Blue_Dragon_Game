using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mode3_Script : MonoBehaviour
{
    public GameObject alphabetPrefab;
    char[] allAlphabets = new char[26];

    int words = 0;
    bool gameOver = false;

    string targetWord;
    int maxLength = 0;
    int filledLength = 0;
    bool[] emptyPos;
    float emptyRatio = 0.3f;

    int completedWords = 0;
    int wrongCount = 0;
    int status = 0;

    List<char> wrongAlphabets, correctAlphabets;
    Image wordImage;
    Utils utilsScript;

    float elapsedTime = 0f;
    public void clickAlphabet(string str){
        int isPaused = PlayerPrefs.GetInt("pause", 0);
        if(isPaused == 1){return;}

        char alphabet = char.Parse(str);
        bool correct = false;
        for(int i = 0; i < correctAlphabets.Count; i++){
            if(alphabet == correctAlphabets[i]){
                correct = true;
                break;
            }
        }

        if(correct){
            handleCorrectAlphabet(alphabet);
        }
        else{
            wrongCount++;
            utilsScript.playWrongSFX();;
        }
    }


    void Start()
    {
        PlayerPrefs.SetString("modeName", "Mode 3");
        words = PlayerPrefs.GetInt("completedWords",0);   

        utilsScript = GameObject.Find("Mode3_Manager").GetComponent<Utils>();
        wordImage = GameObject.Find("WordImage").GetComponent<Image>();

        status = 0;
        int idx = 0;
        for(char c = 'a'; c <= 'z'; c++){
            allAlphabets[idx++] = c;
        }  
    }
    void renderAlphabetAt(int idx){
        GameObject renderGroup = GameObject.Find("WordRenderer").gameObject;
        GameObject temp = Instantiate(alphabetPrefab);
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
        GameObject renderGroup = GameObject.Find("AlphabetRenderer").gameObject;
        for(int i = 0; i < maxLength; i++){
            //temp = AlphabetObjectPooling.alphaInstance.getObj();

            GameObject temp = Instantiate(alphabetPrefab);
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
    void assignNewWord(){
        targetWord = GameObject.Find("Mode3_Manager").GetComponent<Utils>().get_random_word();

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

    }
    void renderAllAlphabets(){
        float x_interval = 1f;
        float y_interval = 1f;

        foreach(Transform child in GameObject.Find("AlphabetRenderer").transform){
            Destroy(child.gameObject);
        }

        for(int i = 0; i < allAlphabets.Length; i++){

            int x_idx = i % 26;
            int y_idx = i / 26;
            GameObject temp = Instantiate(alphabetPrefab);
            temp.transform.SetParent(GameObject.Find("AlphabetRenderer").transform);
            temp.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("alphabet/" + allAlphabets[i].ToString());
            temp.transform.localPosition = new Vector3(x_idx * x_interval, y_idx * y_interval, 0f);

            temp.GetComponent<AlphabetInfo>().setAlphabet(allAlphabets[i]);

        }
    }
    void destroyRenderedWord(){
        foreach(Transform child in GameObject.Find("AlphabetRenderer").transform){
            Destroy(child.gameObject);
        }

    }   

    bool isWordFinished(){
        for(int i = 0; i < targetWord.Length; i++){
            if(emptyPos[i]){return false;}
        }
        return true;
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

    
    
    public void quitGame(){
        PlayerPrefs.SetString("modeName", "Mode 3");
        PlayerPrefs.SetFloat("elapsedTime", elapsedTime);
        PlayerPrefs.SetInt("completedWords", completedWords);
        PlayerPrefs.SetInt("wrongCount", wrongCount);
        PlayerPrefs.SetInt("coinCount", 0);
        
        GameObject.Find("SceneLoadManager").GetComponent<SceneLoadManager>().GoToResultScene();
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

        GameObject.Find("WordCountText").GetComponent<Text>().text = completedWords.ToString();
    }



    
}
