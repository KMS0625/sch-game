using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement
{
    private float moveDirection;
    public Rigidbody2D rb;

    public Movement(Rigidbody2D rb)
    {
        this.rb = rb;
    }

    public void MoveInput()
    {
        moveDirection = Input.GetAxisRaw("Horizontal");
    }
    
    public void Move(float speed)
    {
        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
    }
}
