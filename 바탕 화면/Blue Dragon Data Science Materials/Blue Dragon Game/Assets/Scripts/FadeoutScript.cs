using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeoutScript : MonoBehaviour
{
    private bool startFadeout = false;
    Color color;
    float fadeoutTime = 0.9f;
    float elapsedTime = 0f;

    void OnEnable(){
        startFadeout = true;
        elapsedTime = 0f;
        if(gameObject.GetComponent<Image>() != null){
            color = gameObject.GetComponent<Image>().color;
        }
        else if(gameObject.GetComponent<Text>() != null){ 
            color = gameObject.GetComponent<Text>().color;
        }
    }

    void Start()
    {
    }

    void Update()
    {
        if(startFadeout){
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp((fadeoutTime - elapsedTime) / fadeoutTime, 0f, 1f);
            if(gameObject.GetComponent<Image>() != null){
                gameObject.GetComponent<Image>().color = color;
            }
            else if(gameObject.GetComponent<Text>() != null){ 
                gameObject.GetComponent<Text>().color = color;
            }
        }

        if(elapsedTime >= fadeoutTime){
            startFadeout = false;
            gameObject.SetActive(false);
        }
    }
}
