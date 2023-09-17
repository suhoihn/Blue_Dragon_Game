using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Utils : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource wrongSFX;
    public void playWrongSFX(){
        int play = PlayerPrefs.GetInt("SE_Play",1);
        float volume = PlayerPrefs.GetFloat("SE_Volume", 1.0f);

        if(play == 1){
            wrongSFX.volume = volume;
            wrongSFX.Play();
        }
       
    }
    void Start()
    {
        
    }

    // Update is called once per frame

    public void setAlphabetImage(GameObject obj, string alphabet){
        Sprite sprite;
        sprite = Resources.Load<Sprite>("alphabets/" + alphabet);// + ".png");
        obj.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public string get_random_word(){

        string path = Application.dataPath + "/Resources/words/words.txt";

        TextAsset textFile = Resources.Load("words/words") as TextAsset;
        //Debug.Log(textFile.text);
        StringReader reader = new StringReader(textFile.text);
        List<string> wordList = new List<string>();


        string temp = "";
        while((temp = reader.ReadLine()) != null){
            wordList.Add(temp.Trim());
        }
        int idx = Random.Range(0, wordList.Count);

        return wordList[idx];
    }

    public Vector3 resizeImage(Sprite s, Image img){
        img.sprite = s;
        float w = s.rect.size[0];
        float h = s.rect.size[1];
        w = w/h;
        h = 1.0f;
        return new Vector3(w, h, 1.0f);
    }

    void Update()
    {
        
    }
}
