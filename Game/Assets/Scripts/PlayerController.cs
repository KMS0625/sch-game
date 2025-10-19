using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool isSpriteLeft = false;
    private bool isGrounded = false;
    private bool isDashing = false;
    private bool isInvicible = false;
    private bool canDoubleJump = false;
    private bool canDash = false;
    private bool isDead = false;

    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float gravityForce = 4.0f;
    [SerializeField] private float JumpForce = 15.0f;
    [SerializeField] private float DoubleJumpForce = 13.0f;
    [SerializeField] private float dashForce = 15.0f;

    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float dashTime = 0.3f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float invicibletime = 2f;
    [SerializeField] private float dashCoolTime = 0.3f;

    private float jumpBufferCounter = 0f;
    private float coyoteTimeCounter = 0f;

    [SerializeField] private float fallLimit = -7f;
    [SerializeField] private Vector2 spawnPosition;

    [SerializeField] private int playerLife = 0;

    private float facingDirection = 0f;
    private float moveDirection = 0f;
    private Color playerColor;

    private GameObject respawnPlatform;
    private Color respawnPlatformColor;

    public Rigidbody2D playerRb;
    public BoxCollider2D playerColider;
    public SpriteRenderer playerRender;
    private TrailRenderer playerTrail;
    private Animator animator;
    private SpriteRenderer spawnPlatformRender;

    GroundChecker groundChecker;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerColider = GetComponent<BoxCollider2D>();
        playerRender = GetComponent<SpriteRenderer>();
        playerTrail = GetComponent<TrailRenderer>();
        animator = GetComponent<Animator>();

        groundChecker = new GroundChecker(playerRb, playerColider);

        respawnPlatform = GameObject.Find("Respawn Platform");
        respawnPlatform.SetActive(false);

        playerRb.gravityScale = gravityForce;
        playerColor = playerRender.color;

        GameManager.instance.UpdateLife(playerLife);
    }


    /* -------------------------------------------------------------------*/
    void Update()
    {
        if (isDead && GameManager.instance.checkGameover())
        {
            return;
        }

        Fall();

        groundChecker.checkGround(ref isGrounded);
        animator.SetBool("isGrounded", isGrounded);

        if (isDashing)
        {
            return;
        }

        if (isGrounded)
        {
            canDoubleJump = true;
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
            if (jumpBufferCounter < 0f)
            {
                jumpBufferCounter = 0f;
            }
        }

        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f)
            {
                StartJump();

                jumpBufferCounter = 0f;
                coyoteTimeCounter = 0f;
            }
            else if (canDoubleJump)
            {
                DoubleJump();
                jumpBufferCounter = 0f;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            StopJump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        HandleMoveInput();
    }

    void FixedUpdate()
    {
        if (isDead && GameManager.instance.checkGameover())
        {
            return;
        }


        if (isDashing)
        {
            return;
        }

        Move();
    }


    /* -------------------------------------------------------------------*/
    void Die()
    {
        isDead = true;
        GameManager.instance.GameResult();
    }

    void Fall()
    {
        if (playerRb.position.y < fallLimit)
        {
            playerLife--;
            if (playerLife < 0)
            {
                Die();
            }
            else
            {
                GameManager.instance.UpdateLife(playerLife);
                transform.position = spawnPosition;
                StartCoroutine(Respawn());
            }
        }
    }

    IEnumerator Respawn()
    {
        isInvicible = true;
        gameObject.tag = "InvinciblePlayer";
        float invinvibleTimeCounter = 0f;
        respawnPlatform.SetActive(true);
        while (invinvibleTimeCounter < invicibletime)
        {
            playerRender.color = new Color(1f, 1f, 1f, 0.5f);
            yield return new WaitForSeconds(0.2f);
            playerRender.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.2f);
            invinvibleTimeCounter += 0.4f;
        }

        isInvicible = false;
        gameObject.tag = "Player";

        playerRender.color = playerColor;
        respawnPlatform.SetActive(false);

    }

    // 대쉬 함수
    IEnumerator Dash()
    {
        // 대쉬 사용 불가로 설정하고 상태 true
        canDash = false;
        isDashing = true;
        animator.SetBool("isDashing", isDashing);

        // 대쉬 도중엔 중력 무시
        float prevGravity = playerRb.gravityScale;
        playerRb.gravityScale = 0f;

        // 순간 속도를 초기화하고 대쉬 속도 부여
        playerRb.velocity = Vector2.zero;
        playerRb.velocity = new Vector2(facingDirection * dashForce, 0f);

        // 트레일 이펙트 켜기
        playerTrail.emitting = true;

        // 대쉬 지속 시간 동안 대기
        yield return new WaitForSeconds(dashTime);

        // 대쉬 종료: 중력 복구, 트레일 끄기, 상태 갱신
        playerRb.gravityScale = prevGravity; // 원래 중력값 복원
        if (playerTrail != null) playerTrail.emitting = false;
        isDashing = false;
        animator.SetBool("isDashing", isDashing);

        // 쿨타임 동안 대기 후 다시 대쉬 가능
        yield return new WaitForSeconds(dashCoolTime);
        canDash = true;
    }

    // 점프 시작 함수
    void StartJump()
    {
        Debug.Log("Start Jump");
        playerRb.velocity = new Vector2(playerRb.velocity.x, JumpForce);

    }

    void StopJump()
    {
        if (playerRb.velocity.y > 0f)
        {
            Debug.Log("Stop Jump");

            // 기존 y속도의 절반으로 감소시켜 상승을 빠르게 멈추게 함
            playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y * 0.5f);
        }
        coyoteTimeCounter = 0f;
    }

    // 더블 점프
    void DoubleJump()
    {
        Debug.Log("Double Jump");
        animator.SetTrigger("isDoubleJump");

        playerRb.velocity = new Vector2(playerRb.velocity.x, DoubleJumpForce);
        canDoubleJump = false;
    }

    // 이동 입력 처리 함수
    void HandleMoveInput()
    {
        // -1, 0, 1 값 중 하나
        moveDirection = Input.GetAxisRaw("Horizontal");
        animator.SetInteger("moveDirection", (int)moveDirection);

        if (moveDirection != 0)
        {
            if (isSpriteLeft)
            {
                // 스프라이트 플레이어 방향에 따라 반전
                playerRender.flipX = moveDirection > 0f;

                // 바라보는 방향을 업데이트
                facingDirection = moveDirection;
            }
            else
            {
                playerRender.flipX = moveDirection < 0f;
                facingDirection = moveDirection;
            }
        }
    }

    // 캐릭터 이동 함수
    public void Move()
    {
        // x축 속도를 moveDirection * moveSpeed 로 설정, y속도는 현재 y속도 유지
        playerRb.velocity = new Vector2(moveDirection * moveSpeed, playerRb.velocity.y);
    }
}
