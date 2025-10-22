using UnityEngine;

public class RespawnPlatform : MonoBehaviour
{
    public float breakDelay = 0.5f;
    public float respawnDelay = 3f;

    private SpriteRenderer sr;
    private Collider2D col;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(BreakAndRespawn());
        }
    }

    private System.Collections.IEnumerator BreakAndRespawn()
    {
        // 부서짐
        sr.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        // 재생성
        sr.enabled = true;
        col.enabled = true;
    }
}

//불투명하게