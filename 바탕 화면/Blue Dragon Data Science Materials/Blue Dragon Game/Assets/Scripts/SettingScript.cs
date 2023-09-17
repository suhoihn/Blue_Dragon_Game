using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SettingScript : MonoBehaviour
{
    // Start is called before the first frame update

    Toggle BGMToggle, SEToggle;
    Slider BGMSlider, SESlider;

    public GameObject settingBoard;

    AudioSource bgm;

    public void quitApplication(){
        Application.Quit();
    }

    void Start()
    {
        BGMToggle = settingBoard.transform.Find("BGMToggle").gameObject.GetComponent<Toggle>();
        BGMSlider = settingBoard.transform.Find("BGMSlider").gameObject.GetComponent<Slider>();
        SEToggle = settingBoard.transform.Find("SEToggle").gameObject.GetComponent<Toggle>();
        SESlider = settingBoard.transform.Find("SESlider").gameObject.GetComponent<Slider>();

        //settingBoard = settingBoard.transform.Find("SettingBoard").gameObject;
        bgm = GameObject.Find("BGMAudio").gameObject.GetComponent<AudioSource>();
        // pauseBGM(false);
        BGMToggle.onValueChanged.AddListener(pauseBGM);
        BGMSlider.onValueChanged.AddListener(adjustVolume);

        SEToggle.onValueChanged.AddListener(muteSE);
        SESlider.onValueChanged.AddListener(adjustSEVolume); 

        if(PlayerPrefs.GetInt("BGM_Play",1) == 1){BGMToggle.isOn = true;}
        else{BGMToggle.isOn = false;}

        BGMSlider.value = PlayerPrefs.GetFloat("BGM_Volume",1.0f);

        if(PlayerPrefs.GetInt("SE_Play",1) == 1){SEToggle.isOn = true;}
        else{SEToggle.isOn = false;}

        SESlider.value = PlayerPrefs.GetFloat("SE_Volume",1.0f);

    }  

    void muteSE(bool flag){
        if(flag){PlayerPrefs.SetInt("SE_Play",1);}
        else{PlayerPrefs.SetInt("SE_Play",0);}

    }
    void adjustSEVolume(float v){
        PlayerPrefs.SetFloat("SE_Volume",v);
    }

    void pauseBGM(bool flag){
        if(flag){bgm.Play();PlayerPrefs.SetInt("BGM_Play",1);}
        else{bgm.Pause();PlayerPrefs.SetInt("BGM_Play",0);}
    }

    void adjustVolume(float v){
        bgm.volume = v;
        PlayerPrefs.SetFloat("BGM_Volume",v);
    }

    public void openSetting(){
        PlayerPrefs.SetInt("pause", 1);
        settingBoard.SetActive(true);
        Time.timeScale = 0;
    }

    public void closeSetting(){
        PlayerPrefs.SetInt("pause", 0);
        settingBoard.SetActive(false);
        Time.timeScale = 1;

    }

    public void gotoMain(){
        Time.timeScale = 1;
        PlayerPrefs.SetInt("pause", 0);
        SceneManager.LoadScene("mainScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
