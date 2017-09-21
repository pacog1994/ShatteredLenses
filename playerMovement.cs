using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour {

    public Animator anim; //Animation Controller for character
    public Animator anim2; //Animation Controller for negative character
    public float speed;
    private bool walkIsPlaying; //Boolean check for moving
    //the direction the player faces
    private int direction = 0; //Rotational indicator
    private GameObject mainCamera;
    private bool isInteracting; //if you want to interact
    private Collider collide;


    // Use this for initialization
    void Start()
    {
    }

    //For physics
    private void FixedUpdate()
    {
        if (GameManager.isPlayerFrozen) //if true you cant move
        {
            return;
        }
        //MOVEMENT ACTIONS
        if (walkIsPlaying == true) {
            float hForce = Input.GetAxis("Horizontal"); //Horizontal Axis
            float vForce = Input.GetAxis("Vertical"); //Vertical Axis

            anim.SetBool("isWalking", true);
            anim2.SetBool("isWalking", true);

            if (direction == 0) //ROTATIONAL 0
            {
                transform.Translate(transform.right * hForce * speed * Time.deltaTime); //Player moves along the x-axis
                if ((hForce > 0) && transform.localScale.x < 0) //If player faces left but moves in the + x-axis, flip.
                {
                    Flip();
                }
                else if ((hForce < 0) && transform.localScale.x > 0) //If player faces right but moves in the + x-axis, flip.
                {
                    Flip();
                }
            }
            if (direction == 1) //ROTATIONAL 90
            {
                transform.Translate(transform.forward * vForce * speed * Time.deltaTime); //Player moves along the z-axis
                if ((vForce > 0) && transform.localScale.x < 0) {
                    Flip();
                }
                else if ((vForce < 0) && transform.localScale.x > 0) {
                    Flip();
                }
            }
            if (direction == 2) {
                transform.Translate(transform.right * -hForce * speed * Time.deltaTime); //Player moves along the - x-axis

                if ((hForce < 0) && transform.localScale.x > 0) {
                    Flip();
                }
                else if ((hForce > 0) && transform.localScale.x < 0) {
                    Flip();
                }

            }
            if (direction == 3) {
                transform.Translate(transform.forward * -vForce * speed * Time.deltaTime); //Player moves along the -y-axis

                if ((vForce < 0) && transform.localScale.x > 0) {
                    Flip();
                }
                else if ((vForce > 0) && transform.localScale.x < 0) {
                    Flip();
                }

            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(GameManager.isPlayerFrozen) //if true you cant move
        {
            anim.SetBool("isWalking", false);
            anim2.SetBool("isWalking", false);
            return;
        }
        //MOVEMENT CHECK 
        if (Input.GetAxis("Horizontal") != 0) //Player is moving
        {
            walkIsPlaying = true;
        }
        else if (Input.GetAxis("Horizontal") == 0) //Player is not moving
        {
            anim.SetBool("isWalking", false);
            anim2.SetBool("isWalking", false);
            walkIsPlaying = false;
        }

    }

    //ROTATES the player object so their forward vector faces a certain way based on the Rotational indicator (direction var)
    //@param tempDir is the direction var
    public void lookDir(int tempDir)
    {
        switch (tempDir)
        {
            case 0:  //Player faces posiive x 
                lerpPosition(transform.forward, Vector3.forward, .2f);
                break;

            case 1: //Player faces positive z 
                lerpPosition(transform.forward, Vector3.right, .2f);
                break;

            case 2: //Player faces negative x
                lerpPosition(transform.forward, -Vector3.forward, .2f);
                break;

            case 3: //Player faces negative z
                lerpPosition(transform.forward, Vector3.left, .2f);
                break;
        }
    }

    //BUGGED
    //transforms the player's forward to be aligned to an indicated Vector3 rotation smoothly
    //@param startRot, the player's transform.forward
    //@param endRot, the desired rotation
    //@param LerpTime, speed of which to smoothly rotate
    void lerpPosition(Vector3 startRot, Vector3 endRot, float LerpTime)
    {
        transform.forward = Vector3.Slerp(startRot, endRot, 1f);
    }

    //Player's scale transforms to -x or +x
    void Flip()
    {
        //facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1f;
        transform.localScale = theScale;
    }
}
