using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserButtonScript : MonoBehaviour
{

    public bool isButtonSelected = false;

    UserManager userManager;
    string name,level;

    public void clickButton(){
        userManager.hideAllBorder();
        if(!isButtonSelected){
            userManager.setSelectedName(name);
            userManager.setSelectedLevel(level);

            userManager.isSelected = true;
            isButtonSelected = true;
            showBorder();
        }
        else{
            isButtonSelected = false;
            userManager.isSelected = false;
            hideBorder();
        }
    }
    public void showBorder(){
        gameObject.transform.Find("Border").gameObject.SetActive(true);
    }
    public void hideBorder(){
        gameObject.transform.Find("Border").gameObject.SetActive(false);
    }
    
    
    void Start()
    {
        userManager = GameObject.Find("ScriptObject").GetComponent<UserManager>();
        name = gameObject.transform.Find("Name").gameObject.GetComponent<Text>().text;
        level = gameObject.transform.Find("Level").gameObject.GetComponent<Text>().text;
    }

    void Update()
    {
        
    }
}
