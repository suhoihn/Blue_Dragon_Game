using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[System.Serializable]
public class UserDataList// : MonoBehaviour
{
    public List<UserData> users;
    public UserDataList(){
        users = new List<UserData>(); 
        // addUser("test user", 1000);
    }

    public void addUser(string name, int level){
        UserData temp = new UserData();
        temp.setUsername(name);
        users.Add(temp);

    }

    public void deleteUser(string name){
        foreach(UserData user in users){
            if(user.username == name){
                users.Remove(user);
                break;
            }
        }

    }

    void Start(){}
    void Update(){}
}
