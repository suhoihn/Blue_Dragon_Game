using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    float SPEED = 8f;
    float JUMPFORCE = 12f;
    bool onGround = false;
    bool jumping = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.RightArrow)){
            Vector3 pos = transform.position;
            pos.x += SPEED * Time.deltaTime;
            transform.position = pos;
        }
        else if(Input.GetKey(KeyCode.LeftArrow)){
            Vector3 pos = transform.position;
            pos.x -= SPEED * Time.deltaTime;
            transform.position = pos;
        }

        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        if(Input.GetKeyDown(KeyCode.UpArrow) && onGround){
            onGround = false;
            rb.AddForce(new Vector3(0, 1, 0) * JUMPFORCE, ForceMode2D.Impulse);
            jumping = true;
        }

        if(Input.GetKeyUp(KeyCode.UpArrow) && jumping && rb.velocity.y > 0){
            jumping = false;
            Vector3 v = rb.velocity;
            v.y = 3f;
            rb.velocity = v;
        }


    }

    private void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "ground"){
            onGround = true;
        }
    }
}
