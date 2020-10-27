using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Jump : Character
{
    public int maxJumps;
    public float jumpForce;
    public float maxButtonHoldTime;
    public float holdForce;
    public float distanceToCollider;
    public float maxJumpSpeed;
    public float maxFallSpeed;
    public float fallSpeed;
    public float gravityMultipler;
    public LayerMask collisionLayer;

    private bool jumpPressed;
    private bool jumpHeld;
    private float buttonHoldTime;
    private float originalGravity;
    private int numberOfJumpsLeft;

    protected override void Initializtion()
    {
        base.Initializtion();
        buttonHoldTime = maxButtonHoldTime;
        originalGravity = rb.gravityScale;
        numberOfJumpsLeft = maxJumps;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPressed = true;
        }
        else
            jumpPressed = false;
        if (Input.GetKey(KeyCode.Space))
        {
            jumpHeld = true;
        }
        else
            jumpHeld = false;
        CheckForJump();
        GroundCheck();
    }

    private void FixedUpdate()
    {
        IsJumping();
    }

    private void CheckForJump()
    {
        if (jumpPressed)
        {
            if (!character.isGrounded && numberOfJumpsLeft == maxJumps)
            {
                character.isJumping = false;
                return;
            }
            numberOfJumpsLeft--;
            if (numberOfJumpsLeft >= 0)
            {
                rb.gravityScale = originalGravity;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                buttonHoldTime = maxButtonHoldTime;
                character.isJumping = true;
            }
        }
    }

    private void IsJumping()
    {
        if(character.isJumping)
        {
            rb.AddForce(Vector2.up * jumpForce);
            AdditionalAir();
        }
        if (rb.velocity.y > maxJumpSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxJumpSpeed);
        }
        Falling();
    }

    private void AdditionalAir()
    {
        if (jumpHeld)
        {
            buttonHoldTime -= Time.deltaTime;
            if (buttonHoldTime <= 0)
            {
                buttonHoldTime = 0;
                character.isJumping = false;
            }
            else
                rb.AddForce(Vector2.up * holdForce);
        }
        else
        {
            character.isJumping = false;
        }
    }

    private void Falling()
    {
        if(!character.isJumping && rb.velocity.y < fallSpeed)
        {
            rb.gravityScale = gravityMultipler;
        }
        if(rb.velocity.y < maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }
    }

    private void GroundCheck()
    {
        if (CollisionCheck(Vector2.down, distanceToCollider, collisionLayer) && !character.isJumping)
        {
            character.isGrounded = true;
            numberOfJumpsLeft = maxJumps;
            rb.gravityScale = originalGravity;
        }
        else
        {
            character.isGrounded = false;
        }
    }
}
