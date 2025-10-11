using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker
{
    private Rigidbody2D rb;
    private BoxCollider2D bc;

    public GroundChecker(Rigidbody2D rigidbody, BoxCollider2D boxCollider)
    {
        rb = rigidbody;
        bc = boxCollider;
    }
    public void checkGround(ref bool isGrounded)
    {
        float halfHeight = bc.size.y * rb.transform.localScale.y * 0.5f;
        float halfWidth = bc.size.x * rb.transform.localScale.x * 0.5f;
        float raycastDistance = halfHeight + 0.1f;

        Vector2 rightRayStart = rb.position + new Vector2(halfWidth, 0);
        Vector2 leftRayStart = rb.position + new Vector2(-halfWidth, 0);

        // 레이캐스트 수행
        RaycastHit2D rightHit = Physics2D.Raycast(rightRayStart, Vector2.down, raycastDistance, LayerMask.GetMask("Platform"));
        RaycastHit2D leftHit = Physics2D.Raycast(leftRayStart, Vector2.down, raycastDistance, LayerMask.GetMask("Platform"));

        // 디버그 선 그리기
        Debug.DrawRay(rightRayStart, Vector2.down * raycastDistance, Color.red, 0.1f);
        Debug.DrawRay(leftRayStart, Vector2.down * raycastDistance, Color.blue, 0.1f);

        if (rightHit.collider || leftHit.collider)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
