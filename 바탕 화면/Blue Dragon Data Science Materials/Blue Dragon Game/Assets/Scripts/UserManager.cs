using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class UserManager : MonoBehaviour
{
    public GameObject userObj;
    public GameObject registerWindow;
    public GameObject deleteWindow;
    public GameObject emptyText;


    public string selectedUsername, selectedUserlevel;
    public bool isSelected = false;

    float userListHeight = 174.1842f; // 149.1689f; // 

    List<string> userNameList = new List<string>();
    
    UserDataList userDataList;

    GameObject scrollViewContent;
    string jsonpath;

    public static UserManager userInstance = null;
    void Awake(){
        if(userInstance == null){
            userInstance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    void Start()
    {
        jsonpath = Application.dataPath + "/data/userdata.json";
        userDataList = new UserDataList();
        scrollViewContent = GameObject.Find("Content");
        initialize();

        

        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void initialize(){
        scrollViewContent = GameObject.Find("Content");

        if(File.Exists(jsonpath)){
            loadJson();
            showUserList();
        }
        else{
            saveJson();
        }
        if(userNameList.Count <= 0){
            showEmptyText();
        }
    }

    void OnDestroy(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        initialize();
    }

    public void updateData(){
        string name = PlayerPrefs.GetString("nickname", "");

        foreach(UserData user in userDataList.users){
            if(user.username == name){
                user.coin = PlayerPrefs.GetInt("coinCount", 0);
                user.level = PlayerPrefs.GetInt("level", 1);
                user.exp = PlayerPrefs.GetInt("exp", 0);
                for(int i = 0; i < 3; i++){
                    user.scoreCnt[i] = PlayerPrefs.GetInt("ScoreCnt" + i.ToString(), 0);
                    user.scoreAvr[i] = PlayerPrefs.GetFloat("ScoreAvr" + i.ToString(), 0);
                }
                user.setRLData(RL_Script.instance.value,RL_Script.instance.reward,RL_Script.instance.policy,RL_Script.instance.iter_cnt);
                break;
            }
        }

    }
    
    public void saveJson(){
        string jsonText = JsonUtility.ToJson(userDataList);
        File.WriteAllText(jsonpath,jsonText);

    }

    public void loadJson(){
        string jsonText = File.ReadAllText(jsonpath);
        userDataList = JsonUtility.FromJson<UserDataList>(jsonText);

        foreach(UserData temp in userDataList.users){
            Debug.Log(temp.username + ", " + temp.level.ToString());
        }
    }

    void showUserList(){
        foreach(UserData user in userDataList.users){
            GameObject obj = Instantiate(userObj);
            obj.transform.SetParent(scrollViewContent.transform);

            Text nameText = obj.transform.Find("Name").gameObject.GetComponent<Text>();
            Text levelText = obj.transform.Find("Level").gameObject.GetComponent<Text>();

            nameText.text = user.username;
            levelText.text = "LV " + user.level.ToString();

            Vector2 v = scrollViewContent.GetComponent<RectTransform>().sizeDelta;
            scrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(v[0],v[1] + userListHeight);

            userNameList.Add(user.username);

            hideEmptyText();
        }
    }

    public void showEmptyText(){ emptyText.SetActive(true); }
    public void hideEmptyText(){ emptyText.SetActive(false); } 

    public void addUser(string name, int level){
        GameObject obj = Instantiate(userObj);
        obj.transform.SetParent(scrollViewContent.transform);

        Text nameText = obj.transform.Find("Name").gameObject.GetComponent<Text>();
        Text levelText = obj.transform.Find("Level").gameObject.GetComponent<Text>();

        nameText.text = name;
        levelText.text = "LV " + level.ToString();

        Vector2 v = scrollViewContent.GetComponent<RectTransform>().sizeDelta;
        scrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(v[0],v[1] + userListHeight);

        userNameList.Add(name);

        userDataList.addUser(name, level);

        hideEmptyText();
        saveJson();

    }

    public bool isUniqueName(string name){
        if(userNameList.Contains(name)){return false;}
        return true;
    }

    public void openRegisterWindow(){
        registerWindow.SetActive(true);
        registerWindow.transform.Find("InputField").GetComponent<InputField>().text = "";
    }
    public void closeRegisterWindow(){ registerWindow.SetActive(false); }

    public void openDeleteWindow(){ 
        if(isSelected){ 
            deleteWindow.SetActive(true); 
            deleteWindow.transform.Find("UsernameText").GetComponent<Text>().text = selectedUsername + ", " + selectedUserlevel;

        }
    }

    public void closeDeleteWindow(){ deleteWindow.SetActive(false); }

    public void handleNameConfirm(){
        string s = registerWindow.transform.Find("InputField").transform.Find("text").GetComponent<Text>().text;
        if(s == ""){return;}

        if(isUniqueName(s)){
            addUser(s, 1);
            closeRegisterWindow();
        }
        else{
            registerWindow.transform.Find("ErrorText").gameObject.SetActive(true);  
        }

    }

    public void loadUserList(){}

    public void deleteUser(){
        userNameList.Remove(selectedUsername);
        foreach(Transform child in scrollViewContent.transform){
            Text nameText = child.Find("Name").gameObject.GetComponent<Text>();
            
            if(nameText.text == selectedUsername){

                child.transform.SetParent(null);

                Vector2 v = scrollViewContent.GetComponent<RectTransform>().sizeDelta;
                scrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(v[0],v[1] - userListHeight);
                Destroy(child.gameObject);
                break;
            }
        }
        closeDeleteWindow();
        
        userDataList.deleteUser(selectedUsername);

        if(userNameList.Count <= 0){
            showEmptyText();
        }
        saveJson();
    }

    public void hideAllBorder(){
        foreach(Transform child in scrollViewContent.transform){
            child.gameObject.GetComponent<UserButtonScript>().hideBorder();
        }
    }

    public void setSelectedName(string name){
        selectedUsername = name;
    }

    public void setSelectedLevel(string level){
        selectedUserlevel = level;
    }

    public void selectUserAndStart(){
        if(!isSelected){return;}

        PlayerPrefs.SetString("nickname", selectedUsername);
        
        foreach(UserData user in userDataList.users){
            if(user.username == selectedUsername){
                PlayerPrefs.SetInt("coinCount", user.coin);

                PlayerPrefs.SetInt("level", user.level);
                PlayerPrefs.SetInt("exp", user.exp);

                for(int i = 0; i < 3; i++){
                    //Debug.Log("User scoreCnt " + user.scoreCnt[i])
                    PlayerPrefs.SetInt("ScoreCnt" + i.ToString(), user.scoreCnt[i]);
                    PlayerPrefs.SetFloat("ScoreAvr" + i.ToString(), user.scoreAvr[i]);
                }
            }
        }

       

        SceneManager.LoadScene("mainScene");

    }

    void Update()
    {
        
    }
}
