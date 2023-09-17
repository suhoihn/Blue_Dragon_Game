using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMAudioScript : MonoBehaviour
{
    // Start is called before the first frame update

    public static BGMAudioScript instance;

    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
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
}
