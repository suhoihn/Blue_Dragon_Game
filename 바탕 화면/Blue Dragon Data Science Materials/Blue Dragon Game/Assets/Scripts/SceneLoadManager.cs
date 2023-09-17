using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static SceneLoadManager sceneInstance = null;
    void Awake(){
        if(sceneInstance == null){
            sceneInstance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToMainScene(){
        SceneManager.LoadScene("mainScene");
    }

    public void GoToUserScene(){
        SceneManager.LoadScene("UserScene");
    }

    public void GoToModeSelectScene(){
        SceneManager.LoadScene("ModeSelectScene");
    }
    
    public void GoToMode1_Scene(){
        SceneManager.LoadScene("Mode1_Scene");
    }
    
    public void GoToMode2_Scene(){
        SceneManager.LoadScene("Mode2_Scene");
    }
        
    public void GoToMode3_Scene(){
        SceneManager.LoadScene("Mode3_Scene");
    }

    public void GoToResultScene(){
        SceneManager.LoadScene("ResultScene");
    }
    public void GoToMyInfoScene(){
        SceneManager.LoadScene("MyInfoScene");
    }


}
