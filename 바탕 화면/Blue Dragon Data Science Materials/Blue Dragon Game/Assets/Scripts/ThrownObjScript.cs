using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownObjScript : MonoBehaviour
{
    // Start is called before the first frame update

    float angleLevel, speed;
    char alphabet;
    float minX = -8f;
    float maxX = 8f;
    float groundY, posX, posY, slope;

    bool thrownAtLeft = false;
    float height,yv;

    public void setAngle(float a){angleLevel = (int)Random.Range(1, a + 1);}
    public void setSpeed(float s){speed = s;}
    public void setAlphabet(char c){alphabet = c;}
    public void setPosition(Vector3 pos){transform.position = pos;}

    public void initialize(){
        if(angleLevel == 2){
            groundY = GameObject.Find("Ground").transform.position.y;
            posX = transform.position.x;
            posY = transform.position.y;
            
            slope = Random.Range(-(posX - minX) / (posY - groundY), (maxX - posX) / (posY - groundY));
        }
        else if(angleLevel == 3){
            if(Random.Range(0,2) == 0){thrownAtLeft = false;}
            else{thrownAtLeft = true;}
            height = Random.Range(groundY + 1f, 4f);
            yv = Random.Range(5f,8f);
            Vector3 pos = transform.position;
            pos.x = thrownAtLeft? -12f : 12f;
            pos.y = height;
            transform.position = pos;

        }
    }

    void Start()
    {
        initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if(angleLevel == 1){
            Vector3 pos = transform.position;
            pos.y -= speed * Time.deltaTime;
            transform.position = pos;
        }
        else if(angleLevel == 2){
            Vector3 pos = transform.position;
            pos.y -= speed * Time.deltaTime;
            pos.x += speed * slope * Time.deltaTime;
            transform.position = pos;
        }
        else if(angleLevel == 3){
            Vector3 pos = transform.position;
            if(thrownAtLeft){pos.x += speed * Time.deltaTime;}
            else{pos.x -= speed * Time.deltaTime;}

            yv = Mathf.Max(-7f, yv - 0.2f);
            pos.y += yv * Time.deltaTime;
            transform.position = pos;


        }

    }

    private void OnTriggerEnter2D(Collider2D other){
        //Debug.Log("aaaa");
        if(other.gameObject.tag == "ground"){
            AlphabetObjectPooling.alphaInstance.putObj(gameObject);
        }
        else if(other.gameObject.tag == "Player"){
            if(gameObject.tag == "wrongAlphabet"){
                GameObject.Find("Mode2_Manager").GetComponent<Mode2_Script>().handleWrongAlphabet(alphabet);
            }
            else if(gameObject.tag == "correctAlphabet"){
                GameObject.Find("Mode2_Manager").GetComponent<Mode2_Script>().handleCorrectAlphabet(alphabet);
            }
            else if(gameObject.tag == "coin"){
                GameObject.Find("Mode2_Manager").GetComponent<Mode2_Script>().handleCoin();
            }
            else if(gameObject.tag == "obstacle"){
                GameObject.Find("Mode2_Manager").GetComponent<Mode2_Script>().handleObstacle();
            }

            AlphabetObjectPooling.alphaInstance.putObj(gameObject);
        }
    }
}
