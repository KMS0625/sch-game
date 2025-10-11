using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float initialJumpForce = 10f;
    public float holdJumpForce = 80.0f;
    public float maxJumpTime = 0.1f;
    public float maxClickTime = 0.1f;
    public float gravityForce = 4.0f;
    public bool isGrounded = false;

    public Rigidbody2D playerRb;
    public BoxCollider2D playerColider;
    public float lineSize = 16f;

    Movement movement;
    Jump jump;
    GroundChecker groundChecker;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerColider = GetComponent<BoxCollider2D>();

        playerRb.gravityScale = gravityForce;

        movement = new Movement(playerRb);
        jump = new Jump(playerRb);
        groundChecker = new GroundChecker(playerRb, playerColider);
    }

    void Update()
    {
        movement.MoveInput();
        groundChecker.checkGround(ref isGrounded);
        jump.JumpInput(isGrounded, initialJumpForce, maxJumpTime, maxClickTime);
    }

    void FixedUpdate()
    {
        movement.Move(moveSpeed);
        jump.JumpHold(holdJumpForce);
    }
}
