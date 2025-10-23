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
        Bounds bounds = bc.bounds;

        float rightX = bounds.max.x;
        float leftX = bounds.min.x;

        float bottomY = bounds.min.y;

        float raycastDistance = 0.1f;

        Vector2 rightRayStart = new Vector2(rightX, bottomY);
        Vector2 leftRayStart = new Vector2(leftX, bottomY);
        LayerMask platformLayer = LayerMask.GetMask("Platform");

        RaycastHit2D rightHit = Physics2D.Raycast(rightRayStart, Vector2.down, raycastDistance, platformLayer);
        RaycastHit2D leftHit = Physics2D.Raycast(leftRayStart, Vector2.down, raycastDistance, platformLayer);

        Debug.DrawRay(rightRayStart, Vector2.down * raycastDistance, Color.red, 0.1f);
        Debug.DrawRay(leftRayStart, Vector2.down * raycastDistance, Color.blue, 0.1f);
        if (rightHit.collider != null || leftHit.collider != null) // != null 체크가 명확합니다.
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}