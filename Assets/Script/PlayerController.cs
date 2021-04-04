using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public float speed;
    public float jumpforce;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate() 
    {
        Movement();
    }

    void Movement()
    {
        float horizontalMove=Input.GetAxis("Horizontal");
        float facedirection=Input.GetAxisRaw("Horizontal"); //保留-1，0，1

        if(horizontalMove!=0.0f)
        {
            rb.velocity=new Vector2(horizontalMove*speed*Time.deltaTime,rb.velocity.y);
            anim.SetFloat("running",Mathf.Abs(facedirection));
        }
        if(facedirection!=0.0f)
        {
            transform.localScale=new Vector3(facedirection,1,1);
        }

        if(Input.GetButtonDown("Jump"))
        {
            print("Jump");
            rb.velocity=new Vector2(rb.velocity.x,jumpforce*Time.deltaTime);
        }
    }
}
