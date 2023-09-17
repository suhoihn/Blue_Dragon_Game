using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject obj;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        obj = GameObject.Find("big_m_jump");
        gameObject.transform.position = obj.transform.position;
    }
}
