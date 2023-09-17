using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    int level, exp, coin;
    string nickname;

    Text levelText, nicknameText, expText, moneyText;
    InputField nicknameInput;

    public GameObject NicknamePopup, expFill;

    void Start(){
        // SceneManager.sceneLoaded -= OnSceneLoaded;
        // SceneManager.sceneLoaded += OnSceneLoaded;
        initialize_new();
    }
    // void OnSceneLoaded(Scene scene, LoadSceneMode mode){
    //     if(scene.name != "mainScene"){return;}
    //     initialize();
    // }

    void initialize_new()
    {
        level = PlayerPrefs.GetInt("level", -1);
        if(level == -1){initialize();}
        
        nickname = PlayerPrefs.GetString("nickname", "dummy");
        exp = PlayerPrefs.GetInt("exp", 0);
        coin = PlayerPrefs.GetInt("coinCount", 0);


        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        nicknameText = GameObject.Find("NameText").GetComponent<Text>();
        expText = GameObject.Find("ExpText").GetComponent<Text>();
        moneyText = GameObject.Find("MoneyText").GetComponent<Text>();

        levelText.text = level.ToString();
        nicknameText.text = nickname;
        expText.text = exp.ToString() + " / 100";
        moneyText.text = "$ " + coin.ToString();   

        expFill = GameObject.Find("ExpFill").gameObject;
        Vector3 temp = expFill.transform.localScale;
        temp.x = (float) (exp / 100f);

        expFill.transform.localScale = temp;

    }

    void initialize(){
        PlayerPrefs.SetInt("level", 1);
        PlayerPrefs.SetInt("exp", 0);
        PlayerPrefs.SetInt("coinCount", 0);

        level = 1;
        

        NicknamePopup.SetActive(true);
    }

    public void confirmNickname(){
        nicknameInput = GameObject.Find("NicknameInput").gameObject.GetComponent<InputField>();
        PlayerPrefs.SetString("nickname", nicknameInput.text);
        nickname = PlayerPrefs.GetString("nickname", "dummy");
        nicknameText.text = nickname;   
        NicknamePopup.SetActive(false);
    }


    void Update()
    {
    }
}
