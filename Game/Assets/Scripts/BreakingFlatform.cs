using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    public float disappearTime = 1f;
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
            StartCoroutine(Break());
    }

    private System.Collections.IEnumerator Break()
    {
        yield return new WaitForSeconds(disappearTime);
        sr.enabled = false;
        col.enabled = false;
        Destroy(gameObject, 0.5f);
    }
}
