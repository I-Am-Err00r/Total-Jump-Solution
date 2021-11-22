using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script will manage different character states and scripts that inherit from the Character script can change the value as needed
public class Character : MonoBehaviour
{
    //A reference to the Rigidbody2D on the Character
    protected Rigidbody2D rb;
    //A reference to the Collider2D on the Character
    protected Collider2D col;
    //A reference to the Character script
    protected Character character;

    //Determines if the Character is grounded
    [HideInInspector]
    public bool isGrounded;
    //Determines if the Character is performing an itial jump and/or an additional jump
    [HideInInspector]
    public bool isJumping;

    //Instead of running Start() in each script, set it up this way so that each script can get all the values of Start() and add more logic for specific script that would need to at Start()
    void Start()
    {
        Initializtion();
    }

    //Virtual Start() method; every script will run this, and those that need to add more logic can override it and add more logic to it after the base.Initialization() line that pops up when overriding a virtual method
    protected virtual void Initializtion()
    {
        //Establishes the Rigidbody2D on the Character
        rb = GetComponent<Rigidbody2D>();
        //Establishes the Collider2D on the Character
        col = GetComponent<Collider2D>();
        //Establishes the Character on the Character
        character = GetComponent<Character>();
    }

    //A method that is used by child scripts of the Character to determine if the Character is touching a collider it should be aware of so those scripts can perform certain logic depending on what those scripts need to do
    protected virtual bool CollisionCheck(Vector2 direction, float distance, LayerMask collision)
    {
        //Sets up an array of hits so if the player is colliding with multiple objects, it can sort through each one to look for one it should
        RaycastHit2D[] hits = new RaycastHit2D[10];
        //An int to help sort the hits variable so the Character can run a for loop and check the values of each collision
        int numHits = col.Cast(direction, hits, distance);
        //For loop that sorts hits with the int value it receives based on the Collider2D.Cast() method
        for (int i = 0; i < numHits; i++)
        {
            //If there is at least 1 layer that has been setup by a child script of a layer it should look out for
            if ((1 << hits[i].collider.gameObject.layer & collision) != 0)
            {
                //Returns this method as true if the above if statement is true
                return true;
            }
        }
        //If the logic makes it to hear, then there aren't any layers that whatever child script called this method should be looking out for and returns false back to that child script
        return false;
    }
}
