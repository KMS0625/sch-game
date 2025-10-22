using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D col;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어가 밟는 순간 바로 사라짐
        if (collision.gameObject.CompareTag("Player"))
        {
            BreakImmediately();
        }
    }

    void BreakImmediately()
    {
        // 시각적으로 사라지기
        sr.enabled = false;

        // Collider 비활성화 → 더 이상 밟을 수 없음
        col.enabled = false;

        // 1초 뒤에 완전히 제거 (메모리 정리용)
        Destroy(gameObject, 1f);
    }
}

//금 가있게