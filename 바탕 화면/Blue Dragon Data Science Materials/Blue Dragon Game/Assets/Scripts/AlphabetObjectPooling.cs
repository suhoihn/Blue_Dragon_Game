using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphabetObjectPooling : MonoBehaviour
{
    // Start is called before the first frame update
    int maxObjs = 300;
    Queue<GameObject> q;

    public static AlphabetObjectPooling alphaInstance = null;
    public GameObject alphabetPrefab;

    void Awake(){
        if(alphaInstance == null){
            alphaInstance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }
    void Start()
    {
        q = new Queue<GameObject>();
        for(int i = 0; i < maxObjs; i++){
            GameObject obj = Instantiate(alphabetPrefab);
            putObj(obj);
        }

    }

    public void putObj(GameObject obj){
        obj.SetActive(false);
        obj.transform.SetParent(gameObject.transform);
        q.Enqueue(obj);
    }

    public GameObject getObj(){
        GameObject obj = q.Dequeue();
        obj.SetActive(true);
        obj.transform.SetParent(null);

        return obj;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
