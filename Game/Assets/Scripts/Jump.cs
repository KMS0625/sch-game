using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Jump
{
    private float jumpTimeCounter = 0.0f;
    private bool isJumping;
    private Rigidbody2D rb;
    public Jump(Rigidbody2D rb)
    {
        this.rb = rb;
        isJumping = false;
    }

    public void JumpInput(bool isGrounded, float jumpForce, float maxJumpTime, float minPressTime)
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            rb.velocity = Vector2.zero;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if (Input.GetButtonUp("Jump") && isJumping)
        {
            isJumping = false;
        }
    }


    public void JumpHold(float jumpForce)
    {
        if (isJumping)
        {
            rb.AddForce(new Vector2(0, jumpForce));
            jumpTimeCounter -= Time.fixedDeltaTime;
            Debug.Log(jumpTimeCounter);

            if (jumpTimeCounter <= 0)
            {
                isJumping = false;
                jumpTimeCounter = 0;
            }
        }
    }
}
