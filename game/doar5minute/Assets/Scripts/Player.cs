﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour
{
    //Movement constants
    public float jumpHeight = 4;            //Max height the player can jump
    public float timeToJumpApex = .4f;      //How much time the jump action takes to reach apex
    public float moveSpeed = 6;             //Player movement speed
    float accelerationTimeAirborne = .2f;   //In air acceleration
    float accelerationTimeGrounded = .1f;   //Grounded acceleration

    //Wall jump/leap constants
    public Vector2 wallJumpOff;             //Velocity when player jumps off the wall withouth moving
    public Vector2 wallLeap;                //Velocity when player wants to wall leap from curent wall to another in the opposite direction

    //Wall slide constatns
    public float wallSlideSpeedMax = 3;     //Max speed that player can have while sliding the wall without moving
    public float wallStickTime = .25f;      //Max time before player gets unstick from the wall (makes wall leaping easier to perform)
    float timeToWallUnstick;

    //Kinematic operation variables/constants
    Vector3 velocity;
    float gravity;
    float jumpVelocity = 8;
    float velocityXSmoothing;

    Controller2D controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);     //Kinematic operatin defining deltaMovement (gravity is treated as acceleartion here)
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;             //kinematic operation of jump velocity

        print("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);
    }

    // Update is called once per frame
    void Update()
    {

        //Movement Input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));  //Get Input from user
        int wallDirX = (controller.collisions.left) ? -1 : 1;                                       //Direction of the wall we colided with.

        //Horizontal movement smoothing
        float targetVelocityX = input.x * moveSpeed;    //Desired velocity we want to achieve when moving
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);    //Smooth the movement between initial velocity and desired velosity (acceleration is taken into account)

        //Wall sliding
        bool wallSliding = false;
        if((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)  //If player is touching the wall while mid air
        {
            wallSliding = true;

            if(velocity.y < -wallSlideSpeedMax) 
            {
                velocity.y = -wallSlideSpeedMax;    //if object falls faster than max wall slide speed while stuck to the wall then set it's speed to constant -wallSlideSpeedMax
            }
            if(timeToWallUnstick > 0)               //Time how much time before player can unstick from the wall (0.25 secs)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;
                if(input.x != wallDirX && input.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else{
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }

        //If player is on ground just set its velocity to 0
        if(controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

   
        //Jump Input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallSliding)
            {
                if(input.x == 0)
                {
                    velocity.x = -wallDirX * wallJumpOff.x; //If we are not moving and jump while wallsliding then jump off the wall 
                    velocity.y = wallJumpOff.y;
                }else if(-wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            if (controller.collisions.below)
            {
                velocity.y = jumpVelocity;
            }
            
        }

   
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
