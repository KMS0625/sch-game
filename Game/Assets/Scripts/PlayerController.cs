using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float initialJumpForce = 200.0f;
    public float holdJumpForce = 30.0f;
    public float maxJumpTime = 0.15f;
    public float jumpTimeCounter = 0.0f;
    public float moveDirection = 0;
    public bool isJumping = false;
    public bool isGrounded = false;

    public Rigidbody2D playerRb;
    public BoxCollider2D playerColider;

    public float lineSize = 16f;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerColider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        
        checkGround();
        MoveInput();
        JumpInput();
    }


    void FixedUpdate()
    {
        Move();
        JumpHold();
    }

    void MoveInput()
    {
        moveDirection = Input.GetAxisRaw("Horizontal");
    }
    
    void Move()
    {
        playerRb.velocity = new Vector2(moveDirection * moveSpeed, playerRb.velocity.y);
    }

    void checkGround()
    {
        float halfHeight = playerColider.size.y * transform.localScale.y * 0.5f;
        Vector2 footPosition = playerRb.position + new Vector2(0, -halfHeight);

        RaycastHit2D hit = Physics2D.Raycast(footPosition, Vector2.down, 0.1f, LayerMask.GetMask("Platform"));
        Debug.DrawRay(playerRb.position, Vector2.down * halfHeight, Color.red, 0.1f);

        if (hit.collider)
        {
            Debug.Log("hit platfrom");
            isGrounded = true;
        }
    }


    void JumpInput()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)    // 땅에 닿아있는 지도 확인 할것
        {
            isGrounded = false;
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            playerRb.velocity = Vector2.zero;
            playerRb.AddForce(new Vector2(0, initialJumpForce));
        }
        else if (Input.GetButtonUp("Jump") && isJumping)
        {
            isJumping = false;
        }
    }
    
    void JumpHold()
    {
        if (isJumping)
        {
            playerRb.AddForce(new Vector2(0, holdJumpForce));
            jumpTimeCounter -= Time.fixedDeltaTime;

            if (jumpTimeCounter <= 0)
            {
                isJumping = false;
                jumpTimeCounter = 0;
            }
        }
    }
}
